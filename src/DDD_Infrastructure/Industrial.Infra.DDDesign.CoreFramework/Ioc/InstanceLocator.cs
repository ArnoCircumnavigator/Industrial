namespace Industrial.Infra.DDDesign.CoreFramework
{
    public sealed class InstanceLocator : IInstanceLocator
    {
        private static IInstanceLocator currentLocator;
        private InstanceLocator()
        {

        }
        public static IInstanceLocator Current
        {
            get
            {
                return currentLocator;
            }
        }
        public static void SetLocator(IInstanceLocator locator)
        {
            currentLocator = locator;
        }

        public T GetInstance<T>()
        {
            return currentLocator.GetInstance<T>();
        }

        public object GetInstance(Type type)
        {
            return currentLocator.GetInstance(type);
        }

        public bool IsTypeRegistered<T>()
        {
            return currentLocator.IsTypeRegistered<T>();
        }

        public bool IsTypeRegistered(Type type)
        {
            return (currentLocator.IsTypeRegistered(type));
        }

        public void RegisterType<T>()
        {
            currentLocator.RegisterType<T>();
        }

        public void RegisterType(Type type)
        {
            currentLocator.RegisterType(type);
        }
    }
}
