namespace ElectronicLearn.Web.Middlewares
{
    public class NotFoundMiddleware
    {
        private readonly RequestDelegate _next;
        public NotFoundMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
            {
                context.Request.Path = "/Error/404";
                await _next(context);
            }
        }
    }
}
