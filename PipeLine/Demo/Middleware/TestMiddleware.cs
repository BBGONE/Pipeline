using Pipeline;
using System;
using System.Threading.Tasks;


namespace DemoPipeline.Middleware
{
    public class MyTestMiddlewareOptions
    {
        public string Display { get; set; } = "MyTestMiddleware";
    }

    public class TestMiddleware
    {
        private readonly RequestDelegate<RequestContext> _next;
        private readonly MyTestMiddlewareOptions _options;

        public TestMiddleware(RequestDelegate<RequestContext> next, MyTestMiddlewareOptions options)
        {
            _next = next;
            _options = options;
        }

        public async Task Invoke(RequestContext ctx)
        {
            try
            {
                ctx.AddLogItem($"Exit {_options.Display} {ctx.Request.Name}");
            }
            catch (Exception)
            {
            }

            await _next(ctx);
        }
    }
}
