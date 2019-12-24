using System;
using System.Dynamic;
using System.Runtime.ExceptionServices;

namespace Pipeline
{
    public class RequestContext
    {
        private ExceptionDispatchInfo _ExceptionInfo;

        public RequestContext() {
            Request = new ExpandoObject();
            Response = new ExpandoObject();
            _ExceptionInfo = null;
        }

        public void AddLogItem(string str) => Console.WriteLine(str);
        public dynamic Request { get; }
        public dynamic Response { get; }

        public IServiceProvider RequestServices { get; }
        public void CaptureException(Exception ex)
        {
            _ExceptionInfo = ExceptionDispatchInfo.Capture(ex);
        }
        public Exception ProcessingException { get { return _ExceptionInfo?.SourceException; } }
        public void ReThrow()
        {
            _ExceptionInfo?.Throw();
        }
    }
}
