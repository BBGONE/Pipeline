using System;
using System.Threading.Tasks;
using static Pipeline.Extensions.MapWhenExtensions;

namespace Pipeline.Middleware
{
    public class MapWhenMiddleware<TContext>
    {
        private readonly RequestDelegate<TContext> _next;
        private readonly MapWhenOptions<TContext> _options;

        public MapWhenMiddleware(RequestDelegate<TContext> next, MapWhenOptions<TContext> options)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _next = next;
            _options = options;
        }

        public async Task Invoke(TContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (_options.Predicate(context))
            {
                await _options.Branch(context);
            }
            else
            {
                await _next(context);
            }
        }
    }
}
