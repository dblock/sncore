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

            TransitAccountEmailMessage message = new TransitAccountEmailMessage();
            message.Body = GetContent();
            message.Subject = inputSubject.Text;
            message.DeleteSent = true;
            foreach (string address in inputEmailAddress.Text.Split("\n".ToCharArray()))
            {
                try
                {
                    message.MailTo = new MailAddress(address.Trim()).ToString();
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Error adding \"{0}\".\n{1}", address.Trim(), ex.Message), ex);
                }

                AccountService.SendAccountMailMessage(SessionManager.Ticket, message);
            }

            Redirect(Url);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
