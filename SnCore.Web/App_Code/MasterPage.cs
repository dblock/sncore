using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SnCore.Tools.Web;
using SnCore.Services;
using SnCore.WebServices;
using SnCore.BackEndServices;
using System.Threading;

public class MasterPage : System.Web.UI.MasterPage
{
    private SessionManager mSessionManager = null;

    public int RequestId
    {
        get
        {
            return GetId("id");
        }
    }

    public int GetId(string querystring)
    {
        string id = Request.QueryString[querystring];
        if (string.IsNullOrEmpty(id)) return 0;
        return int.Parse(id);
    }

    public DateTime Adjust(DateTime dt)
    {
        return SessionManager.Adjust(dt);
    }

    public DateTime Adjust(object dt)
    {
        return Adjust((DateTime)dt);
    }

    protected SessionManager SessionManager
    {
        get
        {
            if (mSessionManager == null)
            {
                if (Page is Page)
                {
                    mSessionManager = ((Page)Page).SessionManager;
                }
                else
                {
                    mSessionManager = new SessionManager(this);
                }
            }
            return mSessionManager;
        }
    }

    public MasterPage()
    {

    }

    public void Redirect(string url)
    {
        Response.Redirect(url);
    }

    private static void RethrowException(Exception ex)
    {
        if (ex is ThreadAbortException)
            throw ex;
    }

    public void ReportException(Exception ex)
    {
        RethrowException(ex);
        object notice = FindControl("noticeMenu");
        notice.GetType().GetProperty("Exception").SetValue(notice, ex, null);
    }

    public virtual void OnPagePreInit(EventArgs e)
    {

    }
}

