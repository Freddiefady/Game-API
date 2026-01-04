namespace GameStore.Middleware
{
    public class ApiKeyMiddleware(RequestDelegate next)
    {
        private const string ApiKeyName = "APIKEY";

        public async Task InvokeAsync(HttpContext context)
        {
            if (! context.Request.Headers.TryGetValue(ApiKeyName,  out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Api Key was not provided. (Using ApiKayMiddleware)");
                return;
            }

            var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = appSettings.GetValue<string>(ApiKeyName);

            if (!apiKey.Equals(extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized Client.");
                return;
            }

            await next(context);
        }
    }
}
