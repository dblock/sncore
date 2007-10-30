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
using System.Text;
using System.Collections.Generic;
using System.Threading;
using SnCore.SiteMap;
using SnCore.Services;
using SnCore.WebControls;

public class Page : System.Web.UI.Page
{
    private HtmlMeta mMetaDescription = null;
    protected SessionManager mSessionManager = null;

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

    protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
    {
        try
        {
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    protected override void OnPreInit(EventArgs e)
    {
        System.Web.UI.MasterPage master = Master;
        while (master != null)
        {
            if (master is MasterPage)
            {
                // ((MasterPage)master).OnPagePreInit(e);
            }

            master = master.Master;
        }

        base.OnPreInit(e);
    }

    protected override void OnLoad(EventArgs e)
    {
        try
        {
            if (Header != null)
            {
                Header.Controls.Add(MetaDescription);
            }

            if (IsPostBack && Header != null)
            {
                Title = (string)ViewState["Title"];
            }

            SiteMapDataAttribute[] attributes = (SiteMapDataAttribute[])this.GetType().GetCustomAttributes(typeof(SiteMapDataAttribute), true);
            if (attributes != null && attributes.Length > 0)
            {
                SiteMapDataAttribute attribute = attributes[0];
                StackSiteMap(attribute, Request.Url.PathAndQuery);
            }

            base.OnLoad(e);

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

    protected override void OnLoadComplete(EventArgs e)
    {
        if (!IsPostBack && Page.Header != null)
        {
            ViewState["Title"] = Title;
        }

        base.OnLoadComplete(e);
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

    public virtual SessionManager SessionManager
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
        Response.Redirect(url, true);
    }

    private static void RethrowException(Exception ex)
    {
        if (ex is ThreadAbortException)
            throw ex;
    }

    public object FindNoticeMenuControl()
    {
        System.Web.UI.MasterPage master = Master;
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
        //
        // BUGBUG: this doesn't work with a remote SOAP back-end
        //         the exception becomes a Exception that needs to be parsed
        //

        if (ex is ManagedAccount.AccessDeniedException
            && !SessionManager.IsLoggedIn)
        {
            RedirectToLogin();
            return;
        }

        RethrowException(ex);
        if (Master == null) throw ex;
        object notice = FindNoticeMenuControl();
        if (notice == null) throw ex;
        notice.GetType().GetProperty("Exception").SetValue(notice, ex, null);
    }

    public void ReportInfo(string message, bool htmlencode)
    {
        if (Master == null) throw new Exception(message);
        object notice = FindNoticeMenuControl();
        if (notice == null) throw new Exception(message);
        notice.GetType().GetProperty("HtmlEncode").SetValue(notice, htmlencode, null);
        notice.GetType().GetProperty("Info").SetValue(notice, message, null);
    }

    public HtmlMeta MetaDescription
    {
        get
        {
            if (mMetaDescription == null)
            {
                mMetaDescription = new HtmlMeta();
                mMetaDescription.Name = "description";
                mMetaDescription.Content = SessionManager.GetCachedConfiguration(
                    "SnCore.Description", string.Empty);
            }
            return mMetaDescription;
        }
    }

    public void ReportInfo(string message)
    {
        ReportInfo(message, false);
    }

    public void ReportWarning(string message)
    {
        if (Master == null) throw new Exception(message);
        object notice = FindNoticeMenuControl();
        if (notice == null) throw new Exception(message);
        notice.GetType().GetProperty("Warning").SetValue(notice, message, null);
    }

    public void RedirectToLogin(string uri)
    {
        Redirect(string.Format("AccountLogin.aspx?ReturnUrl={0}&AuthenticatedPage=true",
            Renderer.UrlEncode(uri)));
    }

    public void RedirectToLogin()
    {
        RedirectToLogin(Request.Url.PathAndQuery);
    }

    public SiteMapNode StackSiteMap(SiteMapDataAttribute attribute)
    {
        return StackSiteMap(attribute, Request.Url.PathAndQuery);
    }

    public SiteMapNode StackSiteMap(SiteMapDataAttribute attribute, string uri)
    {
        if (SiteMap.Provider is SiteMapDataProvider)
        {
            SiteMapDataProvider provider = (SiteMapDataProvider)SiteMap.Provider;
            return provider.Stack(attribute, uri);
        }
        else
        {
            throw new Exception("Site Map provider is not a data provider");
        }
    }

    public void SetDefaultButton(System.Web.UI.WebControls.Button button)
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
            RedirectToLogin();
        }

        base.OnLoad(e);
    }
}

