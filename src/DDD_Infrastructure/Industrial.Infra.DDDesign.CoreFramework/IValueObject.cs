namespace Industrial.Infra.DDDesign.CoreFramework
{
    public interface IValueObject
    {
        IEnumerable<object> GetAtomicValues();
    }
}