using Football_League.Shared.APIResponses;

namespace Football_League.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionHandlingMiddleware> logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (ArgumentException ex)
            {
                logger.LogWarning(ex, "Handled bad request: {Message}", ex.Message);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new BaseAPIResponse(false, ex.Message));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new BaseAPIResponse(false, "An unexpected error has occurred."));
            }
        }
    }
}
