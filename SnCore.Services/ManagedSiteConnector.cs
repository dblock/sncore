using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using System.Web;
using System.Web.Security;
using SnCore.Tools.Web;
using NHibernate.Expression;
using System.Net;
using System.Net.Mail;

namespace SnCore.Services
{
    public abstract class ManagedSiteConnector
    {
        const string sSnCoreAuthCookieName = "SnCore.authcookie";

        public static Uri GetBaseUri(ISession session)
        {
            string baseuri = ManagedConfiguration.GetValue(session, "SnCore.WebSite.Url", "http://localhost/SnCoreWeb");
            if (!baseuri.EndsWith("/")) baseuri = baseuri + "/";
            return new Uri(baseuri);
        }

        public static Cookie GetAdminAuthCookie(ISession session)
        {
#if DEBUG
            if (! ContentPage.EnableRemoteContent)
            {
                return null;
            }
#endif
            return new Cookie(sSnCoreAuthCookieName, ManagedAccount.GetAdminTicket(session));
        }

        public static string GetContentAsAdmin(ISession session, string relativeuri)
        {
            Uri baseuri = GetBaseUri(session);
            Uri pageuri = new Uri(baseuri, relativeuri);
            return ContentPage.GetContent(pageuri, baseuri, string.Empty, false, GetAdminAuthCookie(session));
        }

        public static string GetContentAsUser(ISession session, string relativeuri)
        {
            Uri baseuri = GetBaseUri(session);
            Uri pageuri = new Uri(baseuri, relativeuri);
            return ContentPage.GetContent(pageuri, baseuri, string.Empty, false, null);
        }

        public static string GetAdminEmailAddress(ISession session)
        {
            return new MailAddress(ManagedConfiguration.GetValue(
                session, "SnCore.Admin.EmailAddress", "admin@localhost.com"), ManagedConfiguration.GetValue(
                session, "SnCore.Admin.Name", "Admin")).ToString();
        }

        public static void SendAccountEmailMessageUriAsAdmin(
            ISession session, 
            string mailto,
            string relativeuri)
        {
            AccountEmailMessage message = new AccountEmailMessage();
            message.Account = ManagedAccount.GetAdminAccount(session);
            message.Body = GetContentAsAdmin(session, relativeuri);
            message.Subject = ContentPage.GetContentSubject(message.Body);
            message.MailTo = mailto;
            // hide e-mail
            message.MailFrom = GetAdminEmailAddress(session);
            message.Sent = false;
            message.DeleteSent = true;
            message.Created = message.Modified = DateTime.UtcNow;
            session.Save(message);
            session.Flush();
        }
    }
}
