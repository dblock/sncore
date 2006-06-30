using System;
using System.Collections.Generic;
using System.Text;
using SnCore.Tools.Web;
using NUnit.Framework;

namespace SnCore.Tools.Tests
{
    [TestFixture]
    public class RendererTest
    {
        [Test]
        public void RenderHref()
        {
            string[] testdata = 
            {
                "http://www.server.com/",
                "http://www.server.com/?name1=value1&name2=value2",
                "http://www.flickr.com/photos/78363915@N00/70320821/in/set-952847/" // foodcandy bug #219
            };

            foreach (string s in testdata)
            {
                string output = Renderer.RenderHref(s);
                Assert.AreEqual(
                    string.Format("<a target=\"_blank\" href=\"{0}\">{1}</a>",
                        s, s.IndexOf("?") >= 0 ? s.Substring(0, s.IndexOf("?")) + " ..." : s),
                    output);
            }
        }
    }
}
