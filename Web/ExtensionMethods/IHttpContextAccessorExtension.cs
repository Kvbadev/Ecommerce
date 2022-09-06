namespace Web.ExtensionMethods;

public static class IHttpContextAccessorExtension
{
    public static string GetBearerToken(this IHttpContextAccessor contextAccessor)
    {
        string? bearer = contextAccessor.HttpContext?.Request.Headers["Authorization"];
        return (bearer != null && bearer != string.Empty) ? bearer.Replace("Bearer ", "") : string.Empty;
    }
}