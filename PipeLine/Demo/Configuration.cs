using DemoPipeline.Middleware;
using Pipeline;
using Pipeline.Extensions;
using System;
using System.Threading.Tasks;

namespace DemoPipeline
{
    public class Configuration
    {
        public static void Configure(ApplicationBuilder app)
        {
            app.Use(next =>
            {
                const string middleware_name = "middleware 1";

                return async ctx =>
                {
                    ctx.AddLogItem($"Enter {middleware_name} {ctx.Request.Name}");
                    await next(ctx);
                    ctx.AddLogItem($"Exit {middleware_name} {ctx.Request.Name}");
                };
            });

            app.Use(next =>
            {
                const string middleware_name = "middleware 2";

                return async ctx =>
                {
                    ctx.AddLogItem($"Enter {middleware_name} {ctx.Request.Name}");
                    try
                    {
                        await next(ctx);
                    }
                    catch (ArgumentException ex)
                    {
                        ctx.AddLogItem($"Catched in {middleware_name} {ctx.Request.Name}: {ex.Message}");
                    }
                    ctx.AddLogItem($"Exit {middleware_name} {ctx.Request.Name}");

                };
            });

            app.UseWhen(context => context.Request?.IsGoUseWhenRoute, appBuilder =>
            {
                const string middleware_name = "middleware UseWhen";

                appBuilder.Use(async (ctx, next) =>
                {
                    ctx.AddLogItem($"Enter {middleware_name} {ctx.Request.Name}");
                    await next();
                    ctx.AddLogItem($"Exit {middleware_name} {ctx.Request.Name}");
                });
            });

            app.Use(async (ctx, next) =>
            {
                const string middleware_name = "middleware 3";

                if (ctx.Request?.IsThrowException)
                {
                    throw new Exception($"Exception in {middleware_name} {ctx.Request.Name}");
                }
                if (ctx.Request?.IsThrowArgumentException)
                {
                    throw new ArgumentException($"ArgumentException in {middleware_name} {ctx.Request.Name}");
                }
                ctx.AddLogItem($"Enter {middleware_name} {ctx.Request.Name}");
                await next();
                ctx.AddLogItem($"Exit {middleware_name} {ctx.Request.Name}");
            });

            app.MapWhen(context => context.Request?.IsGoMapWhenRoute, appBuilder =>
            {
                const string middleware_name = "middleware MapWhen";

                appBuilder.Use(async (ctx, next) =>
                {
                    ctx.AddLogItem($"Enter {middleware_name} {ctx.Request.Name}");
                    await next();
                    ctx.AddLogItem($"Exit {middleware_name} {ctx.Request.Name}");
                });

                appBuilder.Run(ctx =>
                {
                    ctx.AddLogItem($"Running {middleware_name} application code {ctx.Request.Name}");
                    return Task.CompletedTask;
                });
            });

            app.UseMiddleware<TestMiddleware>(new MyTestMiddlewareOptions());

            app.Run(ctx =>
            {
                ctx.AddLogItem($"Running application code {ctx.Request.Name}");
                return Task.CompletedTask;
            });
        }
    }
}
