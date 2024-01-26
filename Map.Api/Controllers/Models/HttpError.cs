namespace Map.API.Controllers.Models;

public class HttpError
{
    public record Error(string Code, string Message);
}