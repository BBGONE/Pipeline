using System;
using System.Collections.Generic;

namespace Pipeline
{
    public interface IApplicationBuilder<TContext>
         where TContext : IRequestContext
    {
        IServiceProvider ApplicationServices { get; set; }
        IDictionary<string, object> Properties { get; }

        RequestDelegate<TContext> Build();
        ApplicationBuilder<TContext> New();
        ApplicationBuilder<TContext> Use(Func<RequestDelegate<TContext>, RequestDelegate<TContext>> component);
    }
}