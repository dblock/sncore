using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net;
using System.IO;
using System.Net.Mail;
using SnCore.Tools.Web;
using System.Text;
using SnCore.Services;
using SnCore.SiteMap;
using SnCore.Tools;
using System.Collections.Generic;

public partial class TellAFriend : AuthenticatedPage
{
    public string GetContent()
    {
        Uri uri = new Uri(Request.Url, Url);
        string baseuri = SessionManager.WebsiteUrl;
        if (!baseuri.EndsWith("/")) baseuri = baseuri + "/";
        ContentPageParameters p = new ContentPageParameters();
        p.BaseUri = new Uri(baseuri);
        p.HasOnline = true;
        p.Note = inputNote.Text;
        p.UserAgent = SessionManager.GetCachedConfiguration("SnCore.Web.UserAgent", "SnCore/1.0");
        return ContentPage.GetContent(uri, p);
    }

    public string Url
    {
        get
        {
            string result = Request.QueryString["Url"];
            if (string.IsNullOrEmpty(result)) throw new Exception("Missing Url");
            if (!Uri.IsWellFormedUriString(result, UriKind.Relative)) throw new Exception("Invalid Url");
            return result;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
            if (!IsPostBack)
            {
                string subject = HttpUtility.HtmlDecode(Request.Params["Subject"]);
                linkPage.NavigateUrl = linkCancel.NavigateUrl = Url;
                Title = inputSubject.Text = string.Format("Check out {0}", subject);

                if (!SessionManager.HasVerifiedEmailAddress())
                {
                    ReportWarning("You don't have any verified e-mail addresses.\n" +
                        "You must add/confirm a valid e-mail address before using this feature.");

                    send.Enabled = false;
                }

                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode(string.Format("Tell a Friend > {0}", subject), Request.Url));
                StackSiteMap(sitemapdata);
            }
    }

    public void send_Click(object Sender, EventArgs e)
    {
            if (string.IsNullOrEmpty(inputEmailAddress.Text))
            {
                throw new Exception("Missing E-Mail");
            }

            
            TransitAccountEmailMessage message = new TransitAccountEmailMessage();
            message.Body = GetContent();
            message.Subject = inputSubject.Text;
            message.DeleteSent = true;
            ExceptionCollection exceptions = new ExceptionCollection();
            List<string> invalidemails = new List<string>();
            foreach (string address in inputEmailAddress.Text.Split("\n".ToCharArray()))
            {
                if (string.IsNullOrEmpty(address) || string.IsNullOrEmpty(address.Trim()))
                    continue;

                try
                {
                    message.MailTo = new MailAddress(address.Trim()).ToString();
                    SessionManager.AccountService.CreateOrUpdateAccountEmailMessage(
                        SessionManager.Ticket, message);
                }
                catch (Exception ex)
                {
                    invalidemails.Add(address);
                    exceptions.Add(new Exception(string.Format("Error sending message to \"{0}\".\n{1}", 
                        address.Trim(), ex.Message), ex));
                }
            }

            inputEmailAddress.Text = string.Join("\n", invalidemails.ToArray());
            exceptions.Throw();
            Redirect(Url);
    }
}
