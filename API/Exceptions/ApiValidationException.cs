namespace API.Exceptions
{
    public class ApiValidationException(int statusCode,List<string> errors)
    {
        public int StatusCode { get; set; } = statusCode;
        public List<string> Errors { get; set; } = errors;
    }
}
