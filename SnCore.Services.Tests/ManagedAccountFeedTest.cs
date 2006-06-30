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

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountFeedTest : NHibernateTest
    {
        public ManagedAccountFeedTest()
        {

        }

        [Test]
        public void TestRSSContentSaltShaker()
        {
            TestRSSContentDescription("http://www.saltshaker.net/feed");
        }

        [Test]
        public void TestRSSContentChesaholics()
        {
            TestRSSContentDescription("http://cheesaholics.blogs.com/cheesaholics_anonymous/rss.xml");
        }

        private void TestRSSContentDescription(string url)
        {
            AccountFeed feed = new AccountFeed();
            feed.FeedUrl = url;

            IList deleted = (IList)feed.AccountFeedItems;
            List<AccountFeedItem> updated = new List<AccountFeedItem>();

            ManagedAccountFeed m_feed = new ManagedAccountFeed(Session, feed);

            HttpWebRequest request = m_feed.GetFeedHttpRequest();
            RssFeed rssfeed = RssFeed.Read(request);

            Assert.IsTrue(m_feed.Update(rssfeed, deleted, updated), "RSS feed was not updated.");

            foreach (AccountFeedItem item in updated)
            {
                Console.WriteLine(item.Title);
                Console.WriteLine(item.Description);
                Assert.IsFalse(item.Description.EndsWith("..."), "Only partial description vs. full content retrieved.");
                Console.WriteLine();
            }
        }

        [Test]
        public void TestRSSBlankProperties()
        {
            AccountFeed feed = new AccountFeed();
            feed.FeedUrl = "http://www.saltshaker.net/feed";

            IList deleted = (IList)feed.AccountFeedItems;
            List<AccountFeedItem> updated = new List<AccountFeedItem>();

            ManagedAccountFeed m_feed = new ManagedAccountFeed(Session, feed);

            HttpWebRequest request = m_feed.GetFeedHttpRequest();
            RssFeed rssfeed = RssFeed.Read(request);

            Assert.IsTrue(m_feed.Update(rssfeed, deleted, updated), "RSS feed was not updated.");

            Console.WriteLine("Name: " + feed.Name);
            Console.WriteLine("Description: " + feed.Description);
            Console.WriteLine("LinkUrl: " + feed.LinkUrl);

            Assert.IsFalse(string.IsNullOrEmpty(feed.Name), "Feed name was not updated.");
            Assert.IsFalse(string.IsNullOrEmpty(feed.Description), "Feed description was not updated.");
            Assert.IsFalse(string.IsNullOrEmpty(feed.LinkUrl), "Feed link url was not updated.");
        }

        [Test]
        public void TestRSSRelativeUri()
        {
            string raw =
                "<img src='http://absolute.url.com/image.gif'>" +
                "<img src='relative/path/image.gif'>" +
                "<a href='http://absolute.url.com/page.html'>link</a>" +
                "<a href='relative/path/page.html'>link</a>";

            string rebased = Renderer.CleanHtml(raw, new Uri("http://absolute.rebased.com/"));

            Console.WriteLine(rebased);
        }
    }
}
