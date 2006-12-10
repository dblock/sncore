using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Services.Protocols;
using SnCore.Tools.Web;
using Wilco.Web.UI;
using System.Diagnostics;
using System.Text;

public partial class NoticeControl : Control
{
    private string mCssClass = "sncore_notice";
    private string mStyle = string.Empty;
    private bool mHtmlEncode = true;
    private string mMessage = string.Empty;
    private NoticeKind mNoticeKind = NoticeKind.Info;

    public enum NoticeKind
    {
        Info,
        Warning,
        Error,
        Question
    };

    public bool HtmlEncode
    {
        get
        {
            if (EnableViewState)
            {
                return ViewStateUtility.GetViewStateValue<bool>(
                        ViewState, "HtmlEncode", mHtmlEncode);
            }
            else
            {
                return mHtmlEncode;
            }
        }
        set
        {
            if (EnableViewState)
            {
                ViewState["HtmlEncode"] = value;
            }
            else
            {
                mHtmlEncode = value;
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string Style
    {
        get
        {
            if (EnableViewState)
            {
                return ViewStateUtility.GetViewStateValue<string>(
                    ViewState, "Style", mStyle);
            }

            return mStyle;
        }
        set
        {
            ViewState["Style"] = value;
        }
    }

    public string CssClass
    {
        get
        {
            if (EnableViewState)
            {
                return ViewStateUtility.GetViewStateValue<string>(
                    ViewState, "CssClass", mCssClass);
            }
            else
            {
                return mCssClass;
            }
        }
        set
        {
            ViewState["CssClass"] = value;
        }
    }

    public Exception Exception
    {
        set
        {
            Kind = NoticeKind.Error;
#if DEBUG
            string message = value.Message;
#else
            string message = value.Message.Split('\n')[0];
#endif

            string reportbugurl;
            if (SessionManager.IsLoggedIn)
            {
                reportbugurl =
                    string.Format("BugEdit.aspx?pid={0}&type=Bug&url={1}&message={2}",
                       SessionManager.GetCachedConfiguration(
                            "SnCore.Bug.ProjectId", "0"),
                    Renderer.UrlEncode(Request.Url.PathAndQuery),
                    Renderer.UrlEncode(message));
            }
            else
            {
                reportbugurl = string.Format("mailto:{0}?subject={1}&body={2}",
                       SessionManager.GetCachedConfiguration(
                            "SnCore.Admin.EmailAddress", "admin@localhost.com"),
                            Renderer.UrlEncode(Request.Url.PathAndQuery),
                            Renderer.UrlEncode(message));
            }

            HtmlEncode = false;

            Message = string.Format("{0}<br>This may be a bug. If you believe you should not be getting this error, " +
                "please <a href={1}>click here</a> to report it.", message, reportbugurl);

            StringBuilder s = new StringBuilder();
            s.AppendFormat("User-raised exception from {0}: {1}\n{2}", value.Source, value.Message, value.StackTrace);
            if (Request != null && ! string.IsNullOrEmpty(Request.RawUrl)) s.AppendFormat("\nUrl: {0}", Request.RawUrl);
            if (Request != null && Request.UrlReferrer != null) s.AppendFormat("\nReferrer: {0}", Request.UrlReferrer);
            if (Request != null && ! string.IsNullOrEmpty(Request.UserAgent)) s.AppendFormat("\nUser-agent: {0}", Request.UserAgent);

            SessionManager.EventLog.WriteEntry(s.ToString(), EventLogEntryType.Warning);

            if (value.InnerException != null)
            {
                SessionManager.EventLog.WriteEntry(string.Format("User-raised inner-exception from {0}: {1}\n{2}",
                    value.InnerException.Source, value.InnerException.Message, value.InnerException.StackTrace), 
                    EventLogEntryType.Warning);
            }
        }
    }

    protected string Message
    {
        get
        {
            if (EnableViewState)
            {
                return ViewStateUtility.GetViewStateValue<string>(
                    ViewState, "Message", mMessage);
            }
            else
            {
                return mMessage;
            }
        }
        set
        {
            if (EnableViewState)
            {
                ViewState["Message"] = value;
            }
            else
            {
                mMessage = value;
            }

            panelNotice.Visible = ! string.IsNullOrEmpty(value);
            labelMessage.Text = HtmlEncode ? Render(Message) : Message;
        }
    }

    public NoticeKind Kind
    {
        get
        {
            if (EnableViewState)
            {
                return ViewStateUtility.GetViewStateValue<NoticeKind>(
                    ViewState, "NoticeKind", mNoticeKind);
            }
            else
            {
                return mNoticeKind;
            }
        }
        set
        {
            if (EnableViewState)
            {
                ViewState["NoticeKind"] = value;
            }
            else
            {
                mNoticeKind = value;
            }

            panelNotice.CssClass = string.Format("{0}_{1}", CssClass, value.ToString().ToLower());
            imageMessage.ImageUrl = string.Format("images/site/{0}.gif", value.ToString().ToLower());
        }
    }

    public string Warning
    {
        set
        {
            Kind = NoticeKind.Warning;
            Message = value;
        }
    }

    public string Info
    {
        set
        {
            Kind = NoticeKind.Info;
            Message = value;
        }
    }

    public string Question
    {
        set
        {
            Kind = NoticeKind.Question;
            Message = value;
        }
    }

    public new string Error
    {
        set
        {
            Kind = NoticeKind.Error;
            Message = value;
        }
    }
}
