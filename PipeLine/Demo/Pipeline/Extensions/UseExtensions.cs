using System;
using System.Threading.Tasks;

namespace Pipeline.Extensions
{
    public static class UseExtensions
    {
        public static IApplicationBuilder Use(this IApplicationBuilder app, Func<RequestContext, Func<Task>, Task> middleware)
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
