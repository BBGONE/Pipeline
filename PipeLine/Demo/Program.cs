using Pipeline;
using System;
using System.Threading.Tasks;

namespace DemoPipeline
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ApplicationBuilder app = new ApplicationBuilder();

            Configuration.Configure(app);

            RequestDelegate _pipeline = app.Build();

            Task RunPipeline(RequestDelegate pipeline, int testId, bool isGoUseWhenRoute, bool isGoMapWhenRoute, bool isThrowException, bool isThrowArgumentException = false)
            {
                RequestContext context = new RequestContext();
                context.Request.IsGoUseWhenRoute = isGoUseWhenRoute;
                context.Request.IsGoMapWhenRoute = isGoMapWhenRoute;
                context.Request.IsThrowException = isThrowException;
                context.Request.IsThrowArgumentException = isThrowArgumentException;
                context.Request.Name = $"Run# {testId}";

                return pipeline(context);
            }

            await RunPipeline(_pipeline, 1, false, true, true);
            Console.WriteLine();
            await RunPipeline(_pipeline, 2, false, true, false);
            Console.WriteLine();
            await RunPipeline(_pipeline, 3, true, false, false);
            Console.WriteLine();
            await RunPipeline(_pipeline, 4, false, false, false, true);

            Console.ReadKey();
        }
    }
}
