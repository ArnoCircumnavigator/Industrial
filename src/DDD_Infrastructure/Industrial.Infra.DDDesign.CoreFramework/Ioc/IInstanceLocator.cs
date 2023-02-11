namespace Industrial.Infra.DDDesign.CoreFramework
{
    public interface IInstanceLocator
    {
        T GetInstance<T>();
        object GetInstance(Type type);
        bool IsTypeRegistered<T>();
        bool IsTypeRegistered(Type type);
        void RegisterType<T>();
        void RegisterType(Type type);
    }
}
