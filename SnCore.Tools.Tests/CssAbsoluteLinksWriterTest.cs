using System;
using NUnit.Framework;
using System.Collections;
using SnCore.Tools.Web.Html;
using System.Collections.Generic;
using System.Diagnostics;
using SnCore.Tools;
using SnCore.Tools.Web;

namespace SnCore.Tools.Tests
{
    [TestFixture]
    public class CssAbsoluteLinksWriterTest
    {
        public CssAbsoluteLinksWriterTest()
        {

        }

        public class TestDataItem
        {
            public string _input;
            public string _output;

            public TestDataItem(string io)
            {
                _input = _output = io;
            }
 
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
                new TestDataItem("body: background-color: red;"),
                new TestDataItem(
                    "background: url(images/site/head.gif) no-repeat;", 
                    string.Format("background: url({0}images/site/head.gif) no-repeat;", baseuri)),
                new TestDataItem(
                    "background: url(http://www.notrewritten.com/images/site/head.gif) no-repeat;")
            };

            foreach (TestDataItem test in testdata)
            {
                string output = CssAbsoluteLinksWriter.Rewrite(test._input, new Uri(baseuri, UriKind.Absolute));
                Console.WriteLine("{0} => {1}", test._input, output);
                Assert.AreEqual(test._output, output);
            }
        }
    }
}
