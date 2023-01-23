namespace NETCOREAPI.Middlewares;

public class ApiKeyValidation
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiKeyValidation> _logger;
    public ApiKeyValidation(RequestDelegate next, ILogger<ApiKeyValidation> logger)
    {
        _next = next;
        _logger = logger;
    }


    public async Task Invoke(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue("x-api-key", out var apiKeyString))
        {
            context.Response.StatusCode = 401;
            _logger.LogInformation("x-api-key is missing");
            return;
        }

        var apiKey = apiKeyString.ToString();
        _logger.LogInformation($"x-api-key is {apiKey}");
        await _next(context);
    }
}

public static class ApiKeyValidationwareExtensions
{
    public static IApplicationBuilder UseAPIKeyValidation(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ApiKeyValidation>();
    }
}