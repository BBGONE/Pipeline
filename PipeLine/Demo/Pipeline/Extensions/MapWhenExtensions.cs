using Pipeline.Middleware;
using System;

namespace Pipeline.Extensions
{
    using Predicate = Func<RequestContext, bool>;

    public static class MapWhenExtensions
    {
        public class MapWhenOptions
        {
            private Predicate _predicate;

            /// <summary>
            /// The user callback that determines if the branch should be taken.
            /// </summary>
            public Predicate Predicate
            {
                get
                {
                    return _predicate;
                }
                set
                {
                    if (value == null)
                    {
                        throw new ArgumentNullException(nameof(value));
                    }

                    _predicate = value;
                }
            }

            /// <summary>
            /// The branch taken for a positive match.
            /// </summary>
            public RequestDelegate Branch { get; set; }
        }


        public static IApplicationBuilder MapWhen(this IApplicationBuilder app, Predicate predicate, Action<ApplicationBuilder> configuration)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            // create branch
            var branchBuilder = app.New();
            configuration(branchBuilder);
            var branch = branchBuilder.Build();

            // put middleware in pipeline
            var options = new MapWhenOptions
            {
                Predicate = predicate,
                Branch = branch,
            };

            return app.Use(next => new MapWhenMiddleware(next, options).Invoke);
        }
    }
}
