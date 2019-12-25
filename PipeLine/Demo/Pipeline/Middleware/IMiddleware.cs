using System;
using System.Threading.Tasks;

namespace Pipeline.Middleware
{
    public interface IMiddleware<TContext>
    {
        Task InvokeAsync(TContext context, RequestDelegate<TContext> next);
    }

    public interface IMiddlewareFactory<TContext>
    {
        IMiddleware<TContext> Create(Type middlewareType);

        void Release(IMiddleware<TContext> middleware);
    }
}
