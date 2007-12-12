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
using SnCore.WebServices;
using SnCore.BackEndServices;
using System.Threading;

public class Control : System.Web.UI.UserControl
{
    private SessionManager mSessionManager = null;

    public string GetValue(object s, string defaultvalue)
    {
        return SessionManager.GetValue(s, defaultvalue);
    }

    public string GetValue(string s, string defaultvalue)
    {
        return SessionManager.GetValue(s, defaultvalue);
    }

    public DateTime Adjust(DateTime dt)
    {
        return SessionManager.Adjust(dt);
    }

    public DateTime ToUTC(DateTime dt)
    {
        return SessionManager.ToUTC(dt);
    }

    public DateTime Adjust(object dt)
    {
        return Adjust((DateTime)dt);
    }

    public string AdjustToRFC822(object dt)
    {
        return AdjustToRFC822((DateTime)dt);
    }

    public string AdjustToRFC822(DateTime dt)
    {
        return SessionManager.AdjustToRFC822(dt);
    }

    public SessionManager SessionManager
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
                    mSessionManager = new SessionManager(Page);
                }
            }
            return mSessionManager;
        }
    }

    public string Render(object o)
    {
        return Renderer.Render(o);
    }

    public string RenderEx(object o)
    {
        return SessionManager.RenderMarkups(Renderer.RenderEx(o));
    }

    public string UrlEncode(object o)
    {
        return Renderer.UrlEncode(o);
    }

    public string UrlDecode(object o)
    {
        return Renderer.UrlDecode(o);
    }

    public Control()
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

    public object FindNoticeMenuControl()
    {
        System.Web.UI.MasterPage master = Page.Master;
        object notice = null;

        while (notice == null && master != null)
        {
            notice = master.FindControl("noticeMenu");
            master = master.Master;
        }

        return notice;
    }

    public void ReportException(Exception ex)
    {
        RethrowException(ex);
        object notice = FindNoticeMenuControl();
        if (notice == null) throw ex;
        notice.GetType().GetProperty("Exception").SetValue(notice, ex, null);
    }

    public void ReportInfo(string message)
    {
        ReportInfo(message, true);
    }

    public void ReportInfo(string message, bool htmlencode)
    {
        object notice = FindNoticeMenuControl();
        if (notice == null) throw new Exception(message);
        notice.GetType().GetProperty("HtmlEncode").SetValue(notice, htmlencode, null);
        notice.GetType().GetProperty("Info").SetValue(notice, message, null);
    }

    public string GetSummary(string summary)
    {
        return Renderer.GetSummary(SessionManager.RenderMarkups(summary));
    }

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

    protected override void OnInit(EventArgs e)
    {
        try
        {
            base.OnInit(e);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    protected override void OnLoad(EventArgs e)
    {
        try
        {
            base.OnLoad(e);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        try
        {
            base.OnPreRender(e);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
