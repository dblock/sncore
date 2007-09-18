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
    private string mDetail = string.Empty;
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
            return mHtmlEncode;
        }
        set
        {
            mHtmlEncode = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string Style
    {
        get
        {
            return mStyle;
        }
        set
        {
            mStyle = value;
        }
    }

    public string CssClass
    {
        get
        {
            return mCssClass;
        }
        set
        {
            mCssClass = value;
        }
    }

    public Exception Exception
    {
        set
        {
            Kind = NoticeKind.Error;

            string detail = value.Message;
            string message = value.Message.Split('\n')[0];

            if (value.InnerException != null)
            {
                detail = detail + "\n" + value.InnerException.Message;
            }

            HtmlEncode = false;

            string reportbugurl = string.Format("mailto:{0}?subject={1}&body={2}",
                   SessionManager.GetCachedConfiguration(
                        "SnCore.Admin.EmailAddress", "admin@localhost.com"),
                        Renderer.UrlEncode(Request.Url.PathAndQuery),
                        Renderer.UrlEncode(detail));

            Message = string.Format("{0}<br><small>This may be a bug. If you believe you should not be getting this error, " +
                "please <a href={1}>click here</a> to report it.</small>", message, reportbugurl);

            Detail = detail;

            StringBuilder s = new StringBuilder();
            s.AppendFormat("User-raised exception from {0}: {1}\n{2}", value.Source, value.Message, value.StackTrace);
            if (Request != null && !string.IsNullOrEmpty(Request.RawUrl)) s.AppendFormat("\nUrl: {0}", Request.RawUrl);
            if (Request != null && Request.UrlReferrer != null) s.AppendFormat("\nReferrer: {0}", Request.UrlReferrer);
            if (Request != null && !string.IsNullOrEmpty(Request.UserAgent)) s.AppendFormat("\nUser-agent: {0}", Request.UserAgent);

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
            return mMessage;
        }
        set
        {
            mMessage = value;
            panelNotice.Visible = !string.IsNullOrEmpty(value);
            labelMessage.Text = HtmlEncode ? Renderer.Render(Message) : Message;
        }
    }

    protected string Detail
    {
        get
        {
            return mDetail;
        }
        set
        {
            mDetail = value;
            panelNotice.Visible = !string.IsNullOrEmpty(value);
            labelDetail.Text = HtmlEncode ? Renderer.Render(Detail) : Detail;
        }
    }

    public NoticeKind Kind
    {
        get
        {
            return mNoticeKind;
        }
        set
        {
            mNoticeKind = value;
            panelNotice.CssClass = string.Format("{0}_{1}", CssClass, value.ToString().ToLower());
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
