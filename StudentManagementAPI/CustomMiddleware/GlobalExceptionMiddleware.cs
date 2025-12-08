
namespace StudentManagementAPI.CustomMiddleware
{
    public class GlobalExceptionMiddleware 
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Unhandled exception occured.");

                context.Response.StatusCode = 500;

                context.Response.ContentType = "application/json";

                var response = new
                {
                    message = "Unhandled exceptions occured, something went wrong!",
                    details = ex.Message
                };

                await context.Response.WriteAsJsonAsync(response);


            }
        }
    }
}
