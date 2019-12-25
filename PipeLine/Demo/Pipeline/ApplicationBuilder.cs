using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pipeline
{
    public class ApplicationBuilder<TContext> : IApplicationBuilder<TContext>
        where TContext: IRequestContext
    {
        public ApplicationBuilder()
        {
            Properties = new Dictionary<string, object>();
        }

        public IServiceProvider ApplicationServices { get; set; }
     
        // Gets a key/value collection that can be used to share data between middleware.
        public IDictionary<string, object> Properties { get; }

        public ApplicationBuilder<TContext> New()
        {
            return new ApplicationBuilder<TContext>();
        }

        public RequestDelegate<TContext> Build()
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

        protected virtual async Task OnError(Exception ex, TContext ctx)
        {
            ctx.CaptureException(ex);
            ctx.AddLogItem($"Error: {ex.Message}");
            ctx.Response.StatusCode = 500;
            await Task.CompletedTask;
        }

        private RequestDelegate<TContext> GetNextFunc(LinkedListNode<MiddlewareComponentNode<TContext>> node)
        {
            if (node.Next == null)
            {
                // no more middleware components left in the list 
                return ctx =>
                {
                    ctx.AddLogItem("Nothing to process the request StatusCode = 404");
                    // consider a 404 status since no other middleware processed the request
                    ctx.Response.StatusCode = 404;
                    return Task.CompletedTask;
                };
            }
            else
            {
                return node.Next.Value.Process;
            }
        }

        RequestDelegate<TContext> GetCatchError(RequestDelegate<TContext> next)
        {
            RequestDelegate<TContext> catchErrorDelegate = async ctx =>
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

        public ApplicationBuilder<TContext> Use(Func<RequestDelegate<TContext>, RequestDelegate<TContext>> component)
        {
            var node = new MiddlewareComponentNode<TContext>
            {
                Component = component
            };

            _components.AddLast(node);
            return this;
        }

        LinkedList<MiddlewareComponentNode<TContext>> _components = new LinkedList<MiddlewareComponentNode<TContext>>();
    }
}
