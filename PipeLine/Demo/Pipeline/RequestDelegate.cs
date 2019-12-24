using System;
using System.Threading.Tasks;

namespace Pipeline
{
    public delegate Task RequestDelegate(RequestContext ctx);

    public class MiddlewareComponentNode
    {
        public RequestDelegate Next;
        public RequestDelegate Process;
        public Func<RequestDelegate, RequestDelegate> Component;
    }
}
