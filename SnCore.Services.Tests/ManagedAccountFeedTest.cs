using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using SnCore.Data.Tests;
using System.Collections;
using NHibernate.Expression;
using Rss;
using Atom.Core;
using System.Net;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Sgml;
using System.Xml;
using System.Text;
using SnCore.Tools.Web;
using System.Reflection;
using System.Web;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountFeedTest : ManagedCRUDTest<AccountFeed, TransitAccountFeed, ManagedAccountFeed>
    {
        private ManagedAccountTest _account = new ManagedAccountTest();
        private ManagedFeedTypeTest _type = new ManagedFeedTypeTest();

        [SetUp]
        public override void SetUp()
        {
            _account.SetUp();
            _type.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _type.TearDown();
            _account.TearDown();
        }

        public ManagedAccountFeedTest()
        {

        }

        public override TransitAccountFeed GetTransitInstance()
        {
            TransitAccountFeed t_instance = new TransitAccountFeed();
            t_instance.AccountId = _account.Instance.Id;
            t_instance.Description = GetNewString();
            t_instance.FeedType = _type.Instance.Instance.Name;
            t_instance.FeedUrl = GetNewUri();
            t_instance.LinkUrl = GetNewUri();
            t_instance.Name = GetNewString();
            return t_instance;
        }

        [Test]
        public void TestKnownFeeds()
        {
            string[] names = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            foreach (string name in names)
            {
                if (name.Contains(".rss.") || name.Contains(".atom."))
                {
                    Console.WriteLine(name);

                    Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
                    Assert.IsNotNull(s, "Missing xml resource.");

                    AccountFeed feed = new AccountFeed();
                    feed.FeedUrl = string.Format("http://{0}", name);

                    IList<AccountFeedItem> deleted = feed.AccountFeedItems;
                    List<AccountFeedItem> updated = new List<AccountFeedItem>();

                    ManagedAccountFeed m_feed = new ManagedAccountFeed(Session, feed);

                    if (!m_feed.Update(RssFeed.Read(Assembly.GetExecutingAssembly().GetManifestResourceStream(name)), deleted, updated))
                        if (!m_feed.Update(AtomFeed.Load(Assembly.GetExecutingAssembly().GetManifestResourceStream(name), new Uri("http://purl.org/atom/ns#")), deleted, updated))
                            if (!m_feed.Update(AtomFeed.Load(Assembly.GetExecutingAssembly().GetManifestResourceStream(name), new Uri("http://www.w3.org/2005/Atom")), deleted, updated))
                                throw new Exception("Invalid or empty RSS or ATOM feed.");

                    // feed must have all properties
                    Assert.IsFalse(string.IsNullOrEmpty(feed.Name), "Feed name was not updated.");
                    Assert.IsFalse(string.IsNullOrEmpty(feed.Description), "Feed description was not updated.");
                    Assert.IsFalse(string.IsNullOrEmpty(feed.LinkUrl), "Feed link url was not updated.");

                    foreach (AccountFeedItem item in updated)
                    {
                        Console.WriteLine(" {0} [{1}][{2}]", item.Title, item.Created, item.Updated);
                        // titles must be fully decoded
                        Assert.AreEqual(item.Title, HttpUtility.HtmlDecode(item.Title));
                    }
                }
            }
        }

        [Test]
        public void TestRSSRelativeUri()
        {
            string raw =
                "<img src='http://absolute.url.com/image.gif'>" +
                "<img src='relative/path/image.gif'>" +
                "<a href='http://absolute.url.com/page.html'>link</a>" +
                "<a href='relative/path/page.html'>link</a>";

            SnCore.Tools.Web.Html.HtmlWriterOptions options = new SnCore.Tools.Web.Html.HtmlWriterOptions();
            options.BaseHref = new Uri("http://absolute.rebased.com/");
            string rebased = Renderer.CleanHtml(raw, options);

            Console.WriteLine(rebased);
        }

        [Test]
        public void TestBasicXslTransform()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<?xml version=\"1.0\"?><xml><node>value</node></xml>");

            Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream("SnCore.Services.Tests.xsl.Identity.xsl");
            Assert.IsNotNull(s, "Missing identity xsl resource.");
            XmlDocument xsl = new XmlDocument();
            xsl.Load(new StreamReader(s));

            Console.WriteLine("xml: {0}", doc.OuterXml);
            Console.WriteLine("xsl: {0}", xsl.OuterXml);

            Stream ts = ManagedAccountFeed.Transform(doc, xsl.OuterXml);
            XmlDocument transformed = new XmlDocument();
            transformed.Load(ts);
            Console.WriteLine("transformed: {0}", transformed.OuterXml);
        }

        [Test]
        protected void TestRSSXslTransform()
        {
            Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream("SnCore.Services.Tests.xsl.Identity.xsl");
            Assert.IsNotNull(s, "Missing identity xsl resource.");
            string xsl = new StreamReader(s).ReadToEnd();

            FeedType feedtype = new FeedType();
            feedtype.Name = GetNewString();
            feedtype.Xsl = xsl;

            AccountFeed feed = new AccountFeed();
            feed.FeedType = feedtype;
            feed.FeedUrl = "http://link.brightcove.com/services/link/bcpid315932789/bclid78326389?action=rss";

            IList<AccountFeedItem> deleted = new List<AccountFeedItem>();
            List<AccountFeedItem> updated = new List<AccountFeedItem>();

            ManagedAccountFeed m_feed = new ManagedAccountFeed(Session, feed);
            m_feed.Update(RssFeed.Read(m_feed.GetFeedStream()), deleted, updated);
            foreach (AccountFeedItem item in updated)
            {
                Console.WriteLine(item.Title);
                Console.WriteLine(item.Description);
                Console.WriteLine(item.Link);
                Console.WriteLine("=====");
            }
        }

        [Test]
        protected void TestRSSXslTransformBrightcoveVideoFeed()
        {
            Console.WriteLine("Loading xsl ...");
            Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream("SnCore.Services.Tests.xsl.BrightcoveVideoFeed.xsl");
            Assert.IsNotNull(s, "Missing identity xsl resource.");
            string xsl = new StreamReader(s).ReadToEnd();

            FeedType feedtype = new FeedType();
            feedtype.Name = GetNewString();
            feedtype.Xsl = xsl;

            AccountFeed feed = new AccountFeed();
            feed.FeedType = feedtype;
            feed.FeedUrl = "http://link.brightcove.com/services/link/bcpid315932789/bclid78326389?action=rss";

            IList<AccountFeedItem> deleted = new List<AccountFeedItem>();
            List<AccountFeedItem> updated = new List<AccountFeedItem>();

            ManagedAccountFeed m_feed = new ManagedAccountFeed(Session, feed);
            Console.WriteLine("Fetching ...");
            Console.WriteLine(new StreamReader(m_feed.GetFeedStream()).ReadToEnd());

            Console.WriteLine("Reading ...");
            m_feed.Update(RssFeed.Read(m_feed.GetFeedStream()), deleted, updated);
            foreach (AccountFeedItem item in updated)
            {
                Console.WriteLine(item.Title);
                Console.WriteLine(item.Description);
                Console.WriteLine(item.Link);
                Console.WriteLine("=====");
            }
        }

    }
}
