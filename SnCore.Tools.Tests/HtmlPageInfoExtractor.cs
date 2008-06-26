using System;
using NUnit.Framework;
using System.Collections;
using System.Web.UI.HtmlControls;
using SnCore.Tools.Web.Html;
using System.Collections.Generic;
using System.Diagnostics;

namespace SnCore.Tools.Tests
{
    [TestFixture]
    public class HtmlPageInfoExtractorTest
    {
        public HtmlPageInfoExtractorTest()
        {

        }

        public class ExtractBasic_TestData
        {
            public string _html;
            public string _title;

            public ExtractBasic_TestData(string html, string title)
            {
                _html = html;
                _title = title;
            }
        };

        [Test]
        public void ExtractBasic()
        {
            ExtractBasic_TestData[] tests = 
            {
                new ExtractBasic_TestData("<html><title>test</title><img src='http://www.server.com/image.gif'></html>", "test"),
                new ExtractBasic_TestData("<html><title>", null),
                new ExtractBasic_TestData("<html><title>test", "test"),
                new ExtractBasic_TestData("<html><title>test\nrow</title><img src='http://www.server.com/image.gif'></html>", "test\nrow"),
            };

            foreach (ExtractBasic_TestData test in tests)
            {
                HtmlPageInfo info = HtmlPageInfoExtractor.Extract(test._html);
                Console.WriteLine("Title: {0}", info.Title);
                Assert.AreEqual(test._title, info.Title);
            }
        }
    }
}
