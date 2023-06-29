namespace API.Errors
{
    public record ApiException(int StatusCode, string Message = null, string Details = null);
}
