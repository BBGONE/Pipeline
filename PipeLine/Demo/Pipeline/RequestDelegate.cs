using System;
using System.Threading.Tasks;

namespace Pipeline
{
    public delegate bool Predicate<TContext>(TContext ctx);

    public delegate Task RequestDelegate<TContext>(TContext ctx);

    public class MiddlewareComponentNode<TContext>
    {
        public RequestDelegate<TContext> Next;
        public RequestDelegate<TContext> Process;
        public Func<RequestDelegate<TContext>, RequestDelegate<TContext>> Component;
    }
}
