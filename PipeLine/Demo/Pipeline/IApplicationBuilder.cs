using System;
using System.Collections.Generic;

namespace Pipeline
{
    public interface IApplicationBuilder
    {
        IServiceProvider ApplicationServices { get; set; }
        IDictionary<string, object> Properties { get; }

        RequestDelegate Build();
        ApplicationBuilder New();
        ApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> component);
    }
}