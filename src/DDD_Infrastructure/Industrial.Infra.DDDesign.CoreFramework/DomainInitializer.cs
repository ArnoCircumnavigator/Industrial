using System.Reflection;
using System.Runtime.CompilerServices;

namespace Industrial.Infra.DDDesign.CoreFramework
{
    public class DomainInitializer
    {
        private static readonly DomainInitializer current = new();
        private readonly Dictionary<ActorRoleMappingItem, Type> mappings = new();
        private readonly Dictionary<Type, List<Type>> eventSubscriberTypeMappings = new();
        private readonly Dictionary<Type, IEnumerable<MethodInfo>> subscriberEventHandlers = new();
        private List<Type> repositoryTypeList = new();

        public static DomainInitializer Current
        {
            get { return current; }
        }

        public void InitializeDomain(Assembly assembly)
        {
            ResolveRepositories(assembly);
            ResolveRoles(assembly);
        }

        public void ResolveRepositories(Assembly assembly)
        {
            repositoryTypeList = assembly.GetTypes().Where(type => type.IsInterface && type.GetInterfaces().Any(i => IsRepositoriesDefinition(i))).ToList();
        }
        /// <summary>
        /// 通过聚合根找到仓库
        /// 在DDD设计中一般来说，聚合根于仓库是一对一的
        /// </summary>
        /// <param name="aggregateRootType"></param>
        /// <returns></returns>
        public Type? GetRepositoryType(Type aggregateRootType)
        {
            foreach (Type? repositoryType in repositoryTypeList)
            {
                if (GetAggregateRootType(repositoryType) == aggregateRootType)
                {
                    return repositoryType;
                }
            }
            return null;
        }
        public void ResolveRoles(Assembly assembly)
        {
            IEnumerable<Type>? allRoleImplementationTypes = assembly.GetTypes().Where(type => type.IsClass && type.GetInterfaces().Any(i => IsRoleDefinition(i)));
            foreach (var roleImplementationType in allRoleImplementationTypes)
            {
                var actorType = GetActorType(roleImplementationType);
                foreach (var roleTypeDefinition in roleImplementationType.GetInterfaces().Where(i => IsRoleDefinition(i)))
                {
                    if (mappings.Keys.ToList().Exists(item => item.ActorType == actorType && item.RoleTypeDefinition == roleTypeDefinition))
                    {
                        throw new Exception($"Actor of type {actorType?.FullName} cannot act with role {roleTypeDefinition.FullName} twice");
                    }
                    mappings[new ActorRoleMappingItem { ActorType = actorType, RoleTypeDefinition = roleTypeDefinition }] = roleImplementationType;
                }
            }
        }
        public Type? GetActorRoleType(Type actorType, Type roleTypeDefinition)
        {
            return mappings.Where(item => item.Key.ActorType == actorType && item.Key.RoleTypeDefinition == roleTypeDefinition).Select(item => item.Value).SingleOrDefault();
        }

        public void ResolveDomainEvents(Assembly assembly)
        {
            foreach (Type? subscriberType in assembly.GetTypes().Where(type => type.IsClass && type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Count() == 0))
            {
                IEnumerable<MethodInfo> eventHandlers = subscriberType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Where(IsEventHandle);
                subscriberEventHandlers.Add(subscriberType, eventHandlers);
                foreach (var eventHandler in eventHandlers)
                {
                    Type eventType = eventHandler.GetParameters()[0].ParameterType;
                    if (!eventSubscriberTypeMappings.TryGetValue(eventType, out List<Type>? subscriberTypes))
                    {
                        subscriberTypes = new List<Type>();
                        eventSubscriberTypeMappings.Add(eventType, subscriberTypes);
                    }
                    if (!subscriberTypes.Exists(existingSubscriberType => existingSubscriberType == subscriberType))
                    {
                        subscriberTypes.Add(subscriberType);
                    }
                }
            }
        }
        public List<Type> GetSubscriberTypesList(Type eventType)
        {
            var subscriberTypesList = eventSubscriberTypeMappings.ToList().FindAll(pair => pair.Key.IsAssignableFrom(eventType)).Select(pair => pair.Value).ToList();

            List<Type> mergedSubscriberTyps = new List<Type>();
            foreach (List<Type>? subscriberTyps in subscriberTypesList)
            {
                foreach (Type? subscriberType in subscriberTyps)
                {
                    if (!mergedSubscriberTyps.Exists(mst => mst == subscriberType))
                    {
                        mergedSubscriberTyps.Add(subscriberType);
                    }
                }
            }

            return mergedSubscriberTyps;
        }

        private Type GetAggregateRootType(Type repositoryType)
        {
            return repositoryType.GetInterfaces().Single(i => IsRepositoriesDefinition(i)).GetGenericArguments()[0];
        }

        private static bool IsRepositoriesDefinition(Type type)
        {
            //是接口，是泛型，泛型定义是IRepository
            if (type.IsInterface && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IRepository<,>))
            {
                return true;
            }
            return false;
        }

        public IEnumerable<MethodInfo> GetEventHandlers(Type subscriberType)
        {
            if (!subscriberEventHandlers.TryGetValue(subscriberType, out IEnumerable<MethodInfo>? eventHandlers))
            {
                eventHandlers = new List<MethodInfo>();
            }
            return eventHandlers;
        }

        private static bool IsRoleDefinition(Type type)
        {
            if (type.IsInterface)
            {
                if (type.IsGenericType)
                {
                    if (type.GetGenericTypeDefinition() == typeof(IRole<>))
                    {
                        return false;
                    }
                    else if (type.GetInterfaces().Any(i => i.GetGenericTypeDefinition() == typeof(IRole<>)))
                    {
                        return true;
                    }
                }
                else if (type.GetInterfaces().Any(i => i.GetGenericTypeDefinition() == typeof(IRole<>)))
                {
                    return true;
                }
            }
            return false;
        }

        private Type GetActorType(Type roleImplementationType)
        {
            if (roleImplementationType == null || roleImplementationType == typeof(object) || roleImplementationType.BaseType == null)
                return roleImplementationType;

            Type baseType = roleImplementationType.BaseType;
            if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(Role<,>))
                return baseType.GetGenericArguments()[0];

            return GetActorType(baseType);
        }

        private static bool IsEventHandle(MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            return parameters.Length == 1 && typeof(IDomainEvent).IsAssignableFrom(parameters[0].ParameterType);
        }

        private class ActorRoleMappingItem
        {
            public Type? ActorType { get; set; }
            public Type? RoleTypeDefinition { get; set; }
        }
    }
}
