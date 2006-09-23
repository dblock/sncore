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

public partial class TellAFriend : AuthenticatedPage
{
    public string GetContent()
    {
        Uri uri = new Uri(Request.Url, Url);
        string baseuri = SessionManager.GetCachedConfiguration("SnCore.WebSite.Url", "http://localhost/SnCoreWeb");
        if (!baseuri.EndsWith("/")) baseuri = baseuri + "/";
        return ContentPage.GetContent(uri, new Uri(baseuri), inputNote.Text);
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
        try
        {
            if (!IsPostBack)
            {
                linkPage.NavigateUrl = linkCancel.NavigateUrl = Url;
                inputSubject.Text = string.Format("Check out {0}", HttpUtility.HtmlDecode(Request.Params["Subject"]));

                if (!AccountService.HasVerifiedEmail(SessionManager.Ticket))
                {
                    ReportWarning("You don't have any verified e-mail addresses.\n" +
                        "You must add/confirm a valid e-mail address before using this feature.");

                    send.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void send_Click(object Sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(inputEmailAddress.Text))
            {
                throw new Exception("Missing E-Mail");
            }

            SmtpClient smtp = new SmtpClient(
               SessionManager.GetCachedConfiguration("SnCore.Mail.Server", "localhost"),
               int.Parse(SessionManager.GetCachedConfiguration("SnCore.Mail.Port", "25")));
            smtp.DeliveryMethod = (SmtpDeliveryMethod)Enum.Parse(typeof(SmtpDeliveryMethod),
                SessionManager.GetCachedConfiguration("SnCore.Mail.Delivery", "Network"));

            string smtpusername = ConfigurationManager.AppSettings["smtp.username"];
            string smtppassword = ConfigurationManager.AppSettings["smtp.password"];

            if (!string.IsNullOrEmpty(smtpusername))
            {
                smtp.Credentials = new NetworkCredential(smtpusername, smtppassword);
            }
            else
            {
                smtp.UseDefaultCredentials = true;
            }

            MailMessage message = new MailMessage();
            message.Headers.Add("x-mimeole", string.Format("Produced By {0} {1}",
                SystemService.GetTitle(), SystemService.GetVersion()));
            message.Headers.Add("Content-class", "urn:content-classes:message");
            message.Headers.Add("Content-Type", "text/html; charset=\"utf-8\"");

            message.IsBodyHtml = true;
            message.Body = GetContent();
            message.From = new MailAddress(
                SessionManager.GetCachedConfiguration("SnCore.Admin.EmailAddress", "admin@localhost.com"),
                SessionManager.GetCachedConfiguration("SnCore.Admin.Name", "Admin"));
            message.ReplyTo = new MailAddress(
                AccountService.GetActiveEmailAddress(SessionManager.Ticket), 
                SessionManager.Account.Name);
            foreach (string address in inputEmailAddress.Text.Split("\n".ToCharArray()))
            {
                try
                {
                    message.To.Add(new MailAddress(address.Trim()));
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Error adding \"{0}\".\n{1}", address.Trim(), ex.Message), ex);
                }
            }
            message.Subject = inputSubject.Text;
            smtp.Send(message);

            Redirect(Url);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
