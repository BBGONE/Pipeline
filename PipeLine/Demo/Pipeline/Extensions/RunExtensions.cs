using System;

namespace Pipeline.Extensions
{
    public static class RunExtensions
    {
        public static void Run<TContext>(this ApplicationBuilder<TContext> app, RequestDelegate<TContext> handler)
            where TContext : IRequestContext
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            app.Use(_ => handler);
        }
    }
}
