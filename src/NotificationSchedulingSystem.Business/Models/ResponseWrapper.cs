namespace NotificationSchedulingSystem.Business.Models;

public class ResponseWrapper<TModel> where TModel : class
{
    public TModel? Result { get; set; }

    public ICollection<Error> Errors { get; set; }

    public ResponseWrapper()
    {
        // Prevent nulls in the response
        Errors = new List<Error>();
    }
}

public class Error
{
    public string? Message { get; set; }
}