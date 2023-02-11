namespace Industrial.Infra.DDDesign.CoreFramework
{
    public interface IDomainEvent
    {
        IList<object> Results { get; }
        T GetTypedResult<T>();
        IList<T> GetTypedResults<T>();
    }
}
