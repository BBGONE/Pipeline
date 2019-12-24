using System;
using System.Threading.Tasks;

namespace Pipeline.Middleware
{
    public interface IMiddleware
    {
        Task InvokeAsync(RequestContext context, RequestDelegate next);
    }

    public interface IMiddlewareFactory
    {
        IMiddleware Create(Type middlewareType);

        void Release(IMiddleware middleware);
    }
}
