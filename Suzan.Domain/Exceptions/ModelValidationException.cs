namespace Suzan.Domain.Exceptions;

public class ModelValidationException : Exception
{
    public ModelValidationExceptionErrors Errors { get; set; }

    public int StatusCode => Errors.Status;

    public ModelValidationException(string title, int status)
    {
        Errors = new ModelValidationExceptionErrors(title, status);
    }

    public ModelValidationException(string title, int status, string propertyName, string error) : this(title, status)
    {
        Errors.AddError(propertyName, error);
    }

    public ModelValidationException Append(string propertyName, string error)
    {
        Errors.AddError(propertyName, error);
        return this;
    }

    public ModelValidationException Append(string propertyName, params string[] errors)
    {
        foreach (var error in errors)
        {
            Errors.AddError(propertyName, error);
        }
        return this;
    }
}

public class ModelValidationExceptionErrors
{
    public string Title { get; set; }
    public int Status { get; set; }
    public Dictionary<string, List<string>> Errors { get; } = new();

    public ModelValidationExceptionErrors(string title, int status = 200)
    {
        Title = title;
        Status = status;
    }

    public void AddError(string propertyName, string error)
    {
        if (!Errors.ContainsKey(propertyName))
        {
            Errors[propertyName] = new List<string>();
        }
        Errors[propertyName].Add(error);
    }
}
