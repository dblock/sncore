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
using Microsoft.Web.UI;

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

    protected WebContentService ContentService
    {
        get
        {
            return SessionManager.ContentService;
        }
    }

    protected WebTagWordService TagWordService
    {
        get
        {
            return SessionManager.TagWordService;
        }
    }

    protected WebAccountService AccountService
    {
        get
        {
            return SessionManager.AccountService;
        }
    }

    protected WebEventService EventService
    {
        get
        {
            return SessionManager.EventService;
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

    protected WebMadLibService MadLibService
    {
        get
        {
            return SessionManager.MadLibService;
        }
    }

    protected WebSystemService SystemService
    {
        get
        {
            return SessionManager.SystemService;
        }
    }

    public WebSyndicationService SyndicationService
    {
        get
        {
            return SessionManager.SyndicationService;
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

    public WebBlogService BlogService
    {
        get
        {
            return SessionManager.BlogService;
        }
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

    public void ReportException(Exception ex)
    {
        object notice = Page.Master.FindControl("noticeMenu");
        notice.GetType().GetProperty("Exception").SetValue(notice, ex, null);
    }

    public string GetSummary(string summary)
    {
        return Renderer.GetSummary(SessionManager.RenderMarkups(summary));
    }
}
