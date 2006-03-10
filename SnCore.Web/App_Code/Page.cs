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
using System.Reflection;
using System.Text.RegularExpressions;
using SnCore.WebServices;
using SnCore.BackEndServices;

public class Page : System.Web.UI.Page
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

    public string AdjustToRFC822(object dt)
    {
        return AdjustToRFC822((DateTime)dt);
    }

    public string AdjustToRFC822(DateTime dt)
    {
        return SessionManager.AdjustToRFC822(dt);
    }

    public DateTime Adjust(DateTime dt)
    {
        return SessionManager.Adjust(dt);
    }

    public DateTime Adjust(object dt)
    {
        return Adjust((DateTime)dt);
    }

    public string GetValue(object s, string defaultvalue)
    {
        return SessionManager.GetValue(s, defaultvalue);
    }

    public string GetValue(string s, string defaultvalue)
    {
        return SessionManager.GetValue(s, defaultvalue);
    }

    protected WebAccountService AccountService
    {
        get
        {
            return SessionManager.AccountService;
        }
    }

    protected WebSocialService SocialService
    {
        get
        {
            return SessionManager.SocialService;
        }
    }

    protected WebLocationService LocationService
    {
        get
        {
            return SessionManager.LocationService;
        }
    }

    protected WebSystemService SystemService
    {
        get
        {
            return SessionManager.SystemService;
        }
    }

    protected WebDiscussionService DiscussionService
    {
        get
        {
            return SessionManager.DiscussionService;
        }
    }

    protected WebStoryService StoryService
    {
        get
        {
            return SessionManager.StoryService;
        }
    }

    protected WebBugService BugService
    {
        get
        {
            return SessionManager.BugService;
        }
    }

    public WebTagWordService TagWordService
    {
        get
        {
            return SessionManager.TagWordService;
        }
    }

    public WebSyndicationService SyndicationService
    {
        get
        {
            return SessionManager.SyndicationService;
        }
    }

    public WebBackEndService BackEndService
    {
        get
        {
            return SessionManager.BackEndService;
        }
    }

    public WebPlaceService PlaceService
    {
        get
        {
            return SessionManager.PlaceService;
        }
    }

    protected SessionManager SessionManager
    {
        get
        {
            if (mSessionManager == null)
            {
                mSessionManager = new SessionManager(this);
            }
            return mSessionManager;
        }
    }

    public Page()
    {

    }

    public void Redirect(string url)
    {
        Response.Redirect(url);
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

    public void ReportException(Exception ex)
    {
        if (Master == null) throw ex;
        object notice = Master.FindControl("noticeMenu");
        if (notice == null) throw ex;
        notice.GetType().GetProperty("Exception").SetValue(notice, ex, null);
    }

    public void ReportInfo(string message, bool htmlencode)
    {
        if (Master == null) throw new Exception(message);
        object notice = Master.FindControl("noticeMenu");
        if (notice == null) throw new Exception(message);
        notice.GetType().GetProperty("Info").SetValue(notice, message, null);
        notice.GetType().GetProperty("HtmlEncode").SetValue(notice, htmlencode, null);
    }

    public void ReportInfo(string message)
    {
        ReportInfo(message, false);
    }

    public void ReportWarning(string message)
    {
        if (Master == null) throw new Exception(message);
        object notice = Master.FindControl("noticeMenu");
        if (notice == null) throw new Exception(message);
        notice.GetType().GetProperty("Warning").SetValue(notice, message, null);
    }

    public void SetDefaultButton(Button button)
    {
        PageManager.SetDefaultButton(button, this.Controls);
    }
 }

public class AuthenticatedPage : Page
{
    public AuthenticatedPage()
    {

    }

    protected override void OnLoad(EventArgs e)
    {
        if (string.IsNullOrEmpty(SessionManager.Ticket))
        {
            Redirect("AccountLogin.aspx?ReturnUrl=" + Renderer.UrlEncode(Request.Url.PathAndQuery));
        }

        base.OnLoad(e);
    }
}

