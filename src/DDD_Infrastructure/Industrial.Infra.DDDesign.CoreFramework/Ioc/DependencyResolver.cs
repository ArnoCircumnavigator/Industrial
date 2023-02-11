namespace Industrial.Infra.DDDesign.CoreFramework
{
    public static class DependencyResolver
    {
        private static readonly IInstanceLocator instanceLocator = InstanceLocator.Current;

        public static T Resolve<T>()
        {
            return instanceLocator.GetInstance<T>();
        }
        public static object Resolve(Type type)
        {
            return instanceLocator.GetInstance(type);
        }
        public static void RegisterType<T>()
        {
            instanceLocator.RegisterType<T>();
        }
        public static void RegisterType(Type type)
        {
            instanceLocator.RegisterType(type);
        }
        public static bool IsTypeRegistered<T>()
        {
            return instanceLocator.IsTypeRegistered<T>();
        }
        public static bool IsTypeRegistered(Type type)
        {
            return instanceLocator.IsTypeRegistered(type);
        }
    }
}