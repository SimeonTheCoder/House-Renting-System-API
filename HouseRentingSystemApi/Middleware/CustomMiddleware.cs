using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace HouseRentingSystemApi.Middleware
{
    public class StopwatchMiddleware
    {
        private readonly RequestDelegate next;

        public StopwatchMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Stopwatch stopwatch = new();

            stopwatch.Start();

            await this.next(context);

            stopwatch.Stop();

            long time = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"The request took {time} ms");
        }
    }
}
