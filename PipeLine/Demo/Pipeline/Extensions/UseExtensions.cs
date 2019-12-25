using System;
using System.Threading.Tasks;

namespace Pipeline.Extensions
{
    public static class UseExtensions
    {
        public static IApplicationBuilder<TContext> Use<TContext>(this IApplicationBuilder<TContext> app, Func<TContext, Func<Task>, Task> middleware)
             where TContext : IRequestContext
        {
            return app.Use(next =>
            {
                return context =>
                {
                    Func<Task> simpleNext = () => next(context);
                    return middleware(context, simpleNext);
                };
            });
        }
    }
}
