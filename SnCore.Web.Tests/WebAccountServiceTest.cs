using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web;
using System.Net;
using SnCore.Tools.Web.Html;
using SnCore.Web.Tests.SnCore.WebServices.WebAccountService;
using System.IO;

namespace SnCore.Web.Tests
{
    [TestFixture]
    public class WebAccountTests
    {
        public const string sSnCoreAuthCookieName = "SnCore.authcookie";

        [Test]
        public void TestLogin()
        {
            WebAccountService service = new WebAccountService();
            string ticket = service.Login("admin@localhost.com", "password");
            Assert.IsFalse(string.IsNullOrEmpty(ticket));
        }

        [Test]
        public void TestHttpFetchWithLogin()
        {
            WebAccountService service = new WebAccountService();
            string ticket = service.Login("admin@localhost.com", "password");
            Cookie cookie = new Cookie(sSnCoreAuthCookieName, ticket, "/", "localhost");

            CookieContainer cookies = new CookieContainer();
            cookies.Add(cookie);

            HttpWebRequest request = (HttpWebRequest) HttpWebRequest.Create("http://localhost/SnCoreWeb/AccountPreferencesManage.aspx");
            request.CookieContainer = cookies;
            request.Method = "GET";
            request.AllowAutoRedirect = false;

            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            Stream s = response.GetResponseStream();
            StreamReader sr = new StreamReader(s);
            string data = sr.ReadToEnd();
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK, string.Format("Response code was {0}", response.StatusCode));
            Assert.IsFalse(string.IsNullOrEmpty(data));
        }
    }
}