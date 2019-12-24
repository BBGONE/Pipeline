using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pipeline
{
    public class ApplicationBuilder : IApplicationBuilder
    {
        public ApplicationBuilder()
        {
            Properties = new Dictionary<string, object>();
        }

        public IServiceProvider ApplicationServices { get; set; }
     
        // Gets a key/value collection that can be used to share data between middleware.
        public IDictionary<string, object> Properties { get; }

        public ApplicationBuilder New()
        {
            return new ApplicationBuilder();
        }

        public RequestDelegate Build()
        {
            var node = _components.Last;
            while (node != null)
            {
                node.Value.Next = GetNextFunc(node);
                node.Value.Process = node.Value.Component(node.Value.Next);
                node = node.Previous;
            }
            
            // catches unhandled exceptions
            return GetCatchError(_components.First.Value.Process);
        }

        protected virtual async Task OnError(Exception ex, RequestContext ctx)
        {
            ctx.CaptureException(ex);
            ctx.Response.StatusCode = 500;
            ctx.AddLogItem($"Error: {ex.Message}");
            await Task.CompletedTask;
        }

        private RequestDelegate GetNextFunc(LinkedListNode<MiddlewareComponentNode> node)
        {
            if (node.Next == null)
            {
                // no more middleware components left in the list 
                return ctx =>
                {
                    // consider a 404 status since no other middleware processed the request
                    ctx.Response.StatusCode = 404;
                    ctx.AddLogItem("Nothing to process the request StatusCode = 404");
                    return Task.CompletedTask;
                };
            }
            else
            {
                return node.Next.Value.Process;
            }
        }

        RequestDelegate GetCatchError(RequestDelegate next)
        {
            RequestDelegate catchErrorDelegate = async ctx =>
            {
                try
                {
                    await next(ctx);
                }
                catch (Exception ex)
                {
                    await OnError(ex, ctx);
                }
            };

            return catchErrorDelegate;
        }

        public ApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> component)
        {
            var node = new MiddlewareComponentNode
            {
                Component = component
            };

            _components.AddLast(node);
            return this;
        }

        LinkedList<MiddlewareComponentNode> _components = new LinkedList<MiddlewareComponentNode>();
    }
}
