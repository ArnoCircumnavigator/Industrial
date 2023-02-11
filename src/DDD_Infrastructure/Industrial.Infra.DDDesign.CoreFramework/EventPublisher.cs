using System.Reflection;

namespace Industrial.Infra.DDDesign.CoreFramework
{
    public sealed class EventPublisher : IEventPublisher
    {
        public void Publish(IDomainEvent @event)
        {
            foreach (var subscriberType in DomainInitializer.Current.GetSubscriberTypesList(@event.GetType()))
            {
                foreach (var result in HandleEvent(@event, subscriberType))
                {
                    if (result != null)
                    {
                        @event.Results.Add(result);
                    }
                }
            }
        }
        public void Publish(IEnumerable<IDomainEvent> @events)
        {
            foreach (var e in @events)
            {
                Publish(e);
            }
        }

        public void Publish(params IDomainEvent[] events)
        {
            foreach (var e in events)
            {
                Publish(e);
            }
        }

        private static IEnumerable<object> HandleEvent(IDomainEvent evnt, Type subscriberType)
        {
            IList<object> results = new List<object>();

            var eventHandlers = DomainInitializer.Current.GetEventHandlers(subscriberType).Where(eventHandler => IsEventHandler(eventHandler, evnt.GetType()));
            foreach (var eventHandler in eventHandlers)
            {
                ExecuteEventHandler(eventHandler, GetSubscriber(subscriberType), evnt, ref results);
            }

            return results;
        }
        private static void ExecuteEventHandler(MethodInfo eventHandler, object eventSource, object evnt, ref IList<object> results)
        {
            var result = eventHandler.Invoke(eventSource, new object[] { evnt });
            if (result != null)
            {
                results.Add(result);
            }
        }
        private static bool IsEventHandler(MethodInfo method, Type eventType)
        {
            var parameters = method.GetParameters();
            return parameters.Count() == 1 && parameters[0].ParameterType.IsAssignableFrom(eventType);
        }
        private static object GetSubscriber(Type subscriberType)
        {
            if (DependencyResolver.IsTypeRegistered(subscriberType))
            {
                return DependencyResolver.Resolve(subscriberType);
            }
            else
            {
                return CreateSubscriber(subscriberType);
            }
        }
        private static object CreateSubscriber(Type subscriberType)
        {
            var constructor = subscriberType.GetConstructors()[0];
            var parameterValues = new List<object>();
            foreach (var parameterInfo in constructor.GetParameters())
            {
                parameterValues.Add(GetValueForType(parameterInfo.ParameterType));
            }
            return constructor.Invoke(parameterValues.ToArray());
        }
        private static object GetValueForType(Type targetType)
        {
            if (targetType.IsInterface)
            {
                return DependencyResolver.Resolve(targetType);
            }
            else
            {
                return DefaultValueForType(targetType);
            }
        }
        private static object DefaultValueForType(Type targetType)
        {
            return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
        }
    }
}

