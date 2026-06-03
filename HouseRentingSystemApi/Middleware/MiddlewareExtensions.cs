namespace HouseRentingSystemApi.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseStopwatch(this IApplicationBuilder app)
        {
            return app.UseMiddleware<StopwatchMiddleware>();
        }
    }
}
