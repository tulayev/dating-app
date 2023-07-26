namespace API.Middlewares
{
    public record ApiException(int StatusCode, string Message = null, string Details = null);
}
