using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Amib.Threading;
using System.Reflection;
using System.Threading;

public class AsyncPage : Page, IHttpAsyncHandler
{
    class AsyncRequestState : IAsyncResult
    {
        public AsyncRequestState(HttpContext ctx,
                                 AsyncCallback cb,
                                 object extraData)
        {
            _ctx = ctx;
            _cb = cb;
            _extraData = extraData;
        }

        internal HttpContext _ctx;
        internal AsyncCallback _cb;
        internal object _extraData;
        private bool _isCompleted = false;
        private ManualResetEvent _callCompleteEvent = null;
        internal Exception _ex = null;

        internal void CompleteRequest()
        {
            _isCompleted = true;
            lock (this)
            {
                if (_callCompleteEvent != null)
                    _callCompleteEvent.Set();
            }
            // if a callback was registered, invoke it now
            if (_cb != null)
                _cb(this);
        }

        // IAsyncResult interface property implementations
        public object AsyncState
        { get { return (_extraData); } }
        public bool CompletedSynchronously
        { get { return (false); } }
        public bool IsCompleted
        { get { return (_isCompleted); } }
        public WaitHandle AsyncWaitHandle
        {
            get
            {
                lock (this)
                {
                    if (_callCompleteEvent == null)
                        _callCompleteEvent = new ManualResetEvent(false);

                    return _callCompleteEvent;
                }
            }
        }
    }

    static protected SmartThreadPool _threadPool;

    static AsyncPage()
    {
        STPStartInfo si = new STPStartInfo();
        si.MaxWorkerThreads = 25;
        si.MinWorkerThreads = 0;
        si.UseCallerCallContext = true;
        si.UseCallerHttpContext = true;
        _threadPool = new SmartThreadPool(si);
        _threadPool.Start();
    }

    public virtual new void ProcessRequest(HttpContext ctx)
    {
        // not used
    }

    public new bool IsReusable
    {
        get { return false; }
    }

    public IAsyncResult BeginProcessRequest(HttpContext ctx, AsyncCallback cb, object obj)
    {
        AsyncRequestState reqState = new AsyncRequestState(ctx, cb, obj);
        _threadPool.QueueWorkItem(new WorkItemCallback(ProcessRequest), reqState);
        return reqState;
    }

    public void EndProcessRequest(IAsyncResult ar)
    {
        AsyncRequestState reqState = ar as AsyncRequestState;
        if (reqState._ex != null)
        {
            reqState._ctx.Response.Write(reqState._ex.Message);
        }
    }

    object ProcessRequest(object state)
    {
        AsyncRequestState reqState = state as AsyncRequestState;

        try
        {
            // Synchronously call base class Page.ProcessRequest
            // as you are now on a thread pool thread. 

            HttpContext.Current = reqState._ctx;
            base.ProcessRequest(reqState._ctx);

            // Once complete, call CompleteRequest to finish
        }
        catch (Exception ex)
        {
            reqState._ex = ex;
            throw;
        }
        finally
        {
            reqState.CompleteRequest();
        }
        return null;
    }
}
