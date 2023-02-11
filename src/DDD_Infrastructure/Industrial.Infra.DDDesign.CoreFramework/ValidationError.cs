namespace Industrial.Infra.DDDesign.CoreFramework
{
    public class ValidationError : IValidationError
    {
        private readonly List<ValidationErrorItem> errorItemList = new();

        public bool IsValid
        {
            get
            {
                return errorItemList.Count == 0;
            }
        }
        public IEnumerable<ValidationErrorItem> GetErrors()
        {
            return errorItemList;
        }
        public IValidationError AddError(string errorKey)
        {
            errorItemList.Add(new ValidationErrorItem { ErrorKey = errorKey });
            return this;
        }
        public IValidationError AddError(string errorKey, params object[] parameters)
        {
            errorItemList.Add(new ValidationErrorItem { ErrorKey = errorKey, Parameters = new List<object>(parameters) });
            return this;
        }
        public IValidationError AddError(string errorKey, IList<object> parameters)
        {
            errorItemList.Add(new ValidationErrorItem { ErrorKey = errorKey, Parameters = new List<object>(parameters) });
            return this;
        }
    }
    public class ValidationErrorItem
    {
        public ValidationErrorItem() { Parameters = new List<object>(); }
        public string ErrorKey { get; set; } = string.Empty;
        public List<object> Parameters { get; set; }
    }
}
