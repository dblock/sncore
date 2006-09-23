using System;
using NUnit.Framework;
using System.Collections;
using System.Web.UI.HtmlControls;
using SnCore.Tools.Web.Html;
using System.Collections.Generic;
using System.Diagnostics;
using SnCore.Tools;
using SnCore.Tools.Web;

namespace SnCore.Tools.Tests
{
    [TestFixture]
    public class HtmlAbsoluteLinksWriterTest
    {
        public HtmlAbsoluteLinksWriterTest()
        {

        }

        public class TestDataItem
        {
            public string _input;
            public string _output;

            public TestDataItem(string input, string output)
            {
                _input = input;
                _output = output;
            }
        }
        
        [Test]
        public void RewriteRelativeUrls()
        {
            string baseuri = "http://www.rewritten.com/";

            TestDataItem[] testdata = 
            {
                new TestDataItem("http://www.server.com/", "http://www.server.com/"),
                new TestDataItem("http://www.server.com/xyz.html", "http://www.server.com/xyz.html"),
                new TestDataItem("/", string.Format("{0}", baseuri)),
                new TestDataItem("", string.Format("{0}", baseuri)),
                new TestDataItem("/x.html", string.Format("{0}x.html", baseuri))
            };

            foreach (TestDataItem test in testdata)
            {
                string input = string.Format("<a href=\"{0}\">link</a>", test._input);
                string expected_output = string.Format("<html><a href=\"{0}\">link</a></html>", test._output);
                string actual_output = HtmlAbsoluteLinksWriter.Rewrite(input, new Uri(baseuri, UriKind.Absolute));
                Console.WriteLine("{0} => {1}", input, actual_output);
                Assert.AreEqual(expected_output, actual_output);
            }
        }
    }
}
