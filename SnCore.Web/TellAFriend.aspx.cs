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
        string rawcontent;
        Uri uri = new Uri(Request.Url, Url);
        WebRequest request = HttpWebRequest.Create(uri);
        WebResponse response = request.GetResponse();
        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
        {
            rawcontent = sr.ReadToEnd();
            sr.Close();
        }
        string baseuri = SessionManager.GetCachedConfiguration("SnCore.WebSite.Url", "http://localhost/SnCoreWeb");
        if (!baseuri.EndsWith("/")) baseuri = baseuri + "/";
        StringBuilder content = new StringBuilder(Renderer.CleanHtml(rawcontent.ToString(), new Uri(baseuri), null));

        // hack: lots of header stuff comes first
        // hack: atlas script has a CDATA section and is not cleaned up properly
        int headerstart = content.ToString().IndexOf("<table class=\"sncore_master_table\" align=\"center\">");
        if (headerstart >= 0) content.Remove(0, headerstart);
        int atlasscript = content.ToString().IndexOf("<stripped type=\"text/xml-script\">");
        if (atlasscript >= 0) content.Remove(atlasscript, content.Length - atlasscript);

        content.Insert(0, string.Format("<p style=\"margin: 10px;\"><a href=\"{0}\">&#187;&#187; online version</a></p>", uri.ToString()));

        // insert additional note
        if (! string.IsNullOrEmpty(inputNote.Text))
        {
            content.Insert(0, string.Format("<p style=\"margin: 10px;\">{0}</p>", Renderer.Render(inputNote.Text)));
        }

        // hack: insert stylesheet
        content.Insert(0, string.Format("<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}/Style.css\" />\n", baseuri));

        return content.ToString();
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
                        "You must add/confirm a valid e-mail address before posting stories.");

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
            smtp.UseDefaultCredentials = true;

            MailMessage message = new MailMessage();
            message.Headers.Add("x-mimeole", string.Format("Produced By {0} {1}",
                SystemService.GetTitle(), SystemService.GetVersion()));
            message.Headers.Add("Content-class", "urn:content-classes:message");
            message.IsBodyHtml = true;
            message.Body = GetContent();
            message.From = message.ReplyTo = new MailAddress(AccountService.GetActiveEmailAddress(SessionManager.Ticket));
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
