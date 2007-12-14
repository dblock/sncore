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
    public class WebMiscTests
    {
        public void TestPage(Uri root, Uri uri)
        {
            Cookie cookie = null;
            List<Uri> links = null;
            double ts = 0;
            TestPage(root, uri, cookie, out links, out ts);
        }

        public void TestPage(Uri root, Uri uri, Cookie cookie, out List<Uri> links, out double ts)
        {
            DateTime start = DateTime.UtcNow;
            Console.Write("{0} (from {1}) ...", uri.PathAndQuery, root.PathAndQuery);

            CookieContainer cookies = null;
            if (cookie != null)
            {
                cookies = new CookieContainer();
                cookies.Add(cookie);
            }

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            if (cookies != null) request.CookieContainer = cookies;
            request.Method = "GET";
            request.AllowAutoRedirect = true;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream s = response.GetResponseStream();
            StreamReader sr = new StreamReader(s);

            string data = sr.ReadToEnd();

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK, string.Format("Response code was {0}", response.StatusCode));
            Assert.IsFalse(string.IsNullOrEmpty(data));

            ts = DateTime.UtcNow.Subtract(start).TotalSeconds;
            Console.Write("[{0} bytes][{1} sec.]", data.Length, ts.ToString("0.00"));

            Assert.IsTrue(data.Length > 0, "Downloaded file size is zero.");

            if (response.ContentType != "text/html; charset=utf-8")
            {
                Console.WriteLine("[ignore: {0}]", response.ContentType);
                links = new List<Uri>();
                return;
            }

            int error_start = data.IndexOf("<div id=\"ctl00_noticeMenu_panelNotice\" class=\"sncore_notice_error\"");
            if (error_start > 0)
            {
                int error_end = data.IndexOf("</div>", error_start);
                string error = data.Substring(error_start, error_end - error_start);
                Assert.IsTrue(error_start < 0, string.Format("{0}: error: {1}", uri, error));
            }

            links = HtmlUriExtractor.Extract(data, root);
            // Console.WriteLine("\tExtracted {0} links.", links.Count);

            Console.WriteLine("[done]");
        }

        [Test]
        public void TestDeepGuest()
        {
            TestDeep(null);
        }

        [Test]
        public void TestDeepAdmin()
        {
            TestDeep("admin@localhost.com", "password");
        }

        [Test]
        public void TestDeepUser()
        {
            TestDeep("user@localhost.com", "password");
        }

        public void TestDeep(string username, string password)
        {
            WebAccountService service = new WebAccountService();
            string ticket = service.Login(username, password);
            Cookie cookie = new Cookie(WebAccountTests.sSnCoreAuthCookieName, ticket, "/", "localhost");
            TestDeep(cookie);
        }

        public void TestDeep(Cookie cookie)
        {
            Uri root = new Uri("http://localhost/SnCoreWeb/Default.aspx");

            List<Uri> queue = new List<Uri>(2500);
            List<Uri> visited = new List<Uri>(5000);

            double totaltime = 0;
            int totalcount = 0;

            queue.Add(root);

            Dictionary<Uri, Uri> references = new Dictionary<Uri, Uri>();
            references.Add(root, new Uri("http://localhost/SnCoreWeb/"));

            while (queue.Count > 0)
            {
                Uri topofqueue = queue[0];
                List<Uri> links;
                double ts = 0;

                try
                {
                    TestPage(references[topofqueue], topofqueue, cookie, out links, out ts);
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine("{0}: {1}", topofqueue, ex.Message);
                    throw ex;
                }
                finally
                {
                    totalcount++;
                    totaltime += ts;
                    // add to visited links
                    visited.Add(topofqueue);
                    // remove from queue
                    queue.RemoveAt(0);
                }

                Console.Write("{0}/{1} [avg: {2}] => ", totalcount, queue.Count, (totaltime / totalcount).ToString("0.00"));

                int count = 0;
                foreach (Uri uri in links)
                {
                    Uri fulluri = uri;

                    if (!root.IsBaseOf(fulluri))
                    {
                        // Console.WriteLine("\tSkipping {0}.", uri);
                        continue;
                    }

                    if (references.ContainsKey(fulluri) || queue.Contains(fulluri) || visited.Contains(fulluri))
                        continue;

                    if (fulluri.PathAndQuery.Contains("/docs/html/"))
                        continue;

                    Assert.IsFalse(fulluri.ToString().Contains("\0"),
                        string.Format("Uri {0} in {1} contains non-ASCII character.", fulluri, topofqueue));

                    Assert.IsFalse(fulluri.ToString().Contains("<%"),
                        string.Format("Uri {0} in {1} contains non-executed ASP.NET code.", fulluri, topofqueue));

                    Assert.IsFalse(fulluri.ToString().Contains("id=0"),
                        string.Format("Uri {0} in {1} contains a link to a zero id.", fulluri, topofqueue));

                    references.Add(fulluri, topofqueue);

                    // Console.WriteLine("\tAdding {0}.", fulluri.OriginalString);
                    queue.Add(fulluri);
                    count++;
                }
            }
        }

        [Test]
        public void TestExtractUrls()
        {
            WebAccountService service = new WebAccountService();
            string ticket = service.Login("admin@localhost.com", "password");
            Cookie cookie = new Cookie(WebAccountTests.sSnCoreAuthCookieName, ticket, "/", "localhost");

            Uri root = new Uri("http://localhost/SnCoreWeb/");
            Uri uri = new Uri("http://localhost/SnCoreWeb/DiscussionThreadView.aspx?id=86&did=42&ReturnUrl=%2fSnCoreWeb%2fDiscussionTopOfThreadsView.aspx");

            List<Uri> links = new List<Uri>();
            double ts = 0;

            TestPage(root, uri, cookie, out links, out ts);

            Console.WriteLine("{0} links", links.Count);

            foreach(Uri link in links)
            {
                Console.WriteLine("[{0}]", link);
                Assert.IsTrue(link.Query.IndexOf("/") < 0);
            }
        }
    }
}