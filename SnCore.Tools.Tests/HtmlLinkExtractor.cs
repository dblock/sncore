using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using SnCore.Tools.Web.Html;
using System.IO;

namespace SnCore.Tools.Tests
{
    [TestFixture]
    public class HtmlLinkExtractorTest
    {
        [Test]
        public void ExtractSome()
        {
            string[] tests = {
                "<html> bla bla bla" +
                " <LINK type=\"html/icon\" REL='SHORTCUT ICON' HREF='/groups/img/3/favicon.ico'>" +
                " <link rel=\"alternate\" type=\"application/rss+xml\" title=\"RSS\" href=\"http://groups.google.com/group/dotnetopenid/feed/rss_v2_0_msgs.xml\">" +
                " <link rel=\"alternate\" type=\"application/atom+xml\" title=\"Atom\" href=\"http://groups.google.com/group/dotnetopenid/feed/atom_v1_0_msgs.xml\">" + 
                "</html>",
                "<a href='mailto:a bunch of crap %20 %20'></a>"
            };

            foreach (string test in tests)
            {
                List<HtmlLinkControl> links = HtmlLinkExtractor.Extract(test, new Uri("http://www.google.com"));
                Console.WriteLine("Links: {0}", links.Count);
                foreach (HtmlLinkControl link in links)
                {
                    Console.WriteLine("Link: title='{0}' type='{1}' rel='{2}", link.Title, link.Type, link.Rel);
                    Console.WriteLine("\t{0}", link.Href);
                    Console.WriteLine(HtmlGenericCollector.GetHtml(link));
                }
            }
        }
    }
}
