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
using Microsoft.Web.UI;

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

    public WebBackEndService BackEndService
    {
        get
        {
            return SessionManager.BackEndService;
        }
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

    public void ReportException(Exception ex)
    {
        object notice = FindControl("noticeMenu");
        notice.GetType().GetProperty("Exception").SetValue(notice, ex, null);
        UpdatePanel panel = (UpdatePanel) FindControl("panelNoticeMenu");
        panel.Update();
    }
}

