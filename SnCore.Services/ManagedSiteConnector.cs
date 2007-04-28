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

        public static Cookie GetUserAuthCookie(ISession session, int user_id)
        {
#if DEBUG
            if (!ContentPage.EnableRemoteContent)
            {
                return null;
            }
#endif
            return new Cookie(sSnCoreAuthCookieName, ManagedAccount.GetUserTicket(session, user_id));
        }

        public static string GetContentAsAdmin(ISession session, string relativeuri)
        {
            Uri baseuri = GetBaseUri(session);
            Uri pageuri = new Uri(baseuri, relativeuri);
            ContentPageParameters p = new ContentPageParameters();
            p.UserAgent = ManagedConfiguration.GetValue(session, "SnCore.Web.UserAgent", "SnCore/1.0");
            p.AuthCookie = GetAdminAuthCookie(session);
            p.BaseUri = baseuri;
            p.HasOnline = false;
            return ContentPage.GetContent(pageuri, p);
        }

        public static string GetHttpContentAsUser(ISession session, int user_id, string relativeuri)
        {
            ContentPageParameters p = new ContentPageParameters();
            p.UserAgent = ManagedConfiguration.GetValue(session, "SnCore.Web.UserAgent", "SnCore/1.0");
            p.AuthCookie = GetUserAuthCookie(session, user_id);
            p.BaseUri = GetBaseUri(session);
            p.HasOnline = false;
            Uri pageuri = new Uri(p.BaseUri, relativeuri);
            return ContentPage.GetHttpContent(pageuri, p);
        }

        public static string GetContentAsUser(ISession session, int user_id, string relativeuri)
        {
            ContentPageParameters p = new ContentPageParameters();
            p.UserAgent = ManagedConfiguration.GetValue(session, "SnCore.Web.UserAgent", "SnCore/1.0");
            p.AuthCookie = GetUserAuthCookie(session, user_id);
            p.BaseUri = GetBaseUri(session);
            p.HasOnline = false;
            Uri pageuri = new Uri(p.BaseUri, relativeuri);
            return ContentPage.GetContent(pageuri, p);
        }

        public static string GetAdminEmailAddress(ISession session)
        {
            return new MailAddress(ManagedConfiguration.GetValue(
                session, "SnCore.Admin.EmailAddress", "admin@localhost.com"), ManagedConfiguration.GetValue(
                session, "SnCore.Admin.Name", "Admin")).ToString();
        }

        public static bool TrySendAccountEmailMessageUriAsAdmin(
            ISession session,
            ManagedAccount recepient,
            string relativeuri)
        {
            string sendto = string.Empty;
            
            if (! recepient.TryGetActiveEmailAddress(out sendto, ManagedAccount.GetAdminSecurityContext(session)))
                return false;

            MailAddress address = new MailAddress(sendto, recepient.Name);

            return TrySendAccountEmailMessageUriAsAdmin(
                session, address.ToString(), relativeuri);
        }

        public static bool TrySendAccountEmailMessageUriAsAdmin(
            ISession session,
            string mailto,
            string relativeuri)
        {
            try
            {
                SendAccountEmailMessageUriAsAdmin(session, mailto, relativeuri);
                return true;
            }
            catch
            {
                return false;
            }
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
