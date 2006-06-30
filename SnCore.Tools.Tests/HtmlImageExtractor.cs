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
    public class HtmlImageExtractorTest
    {
        public HtmlImageExtractorTest()
        {

        }

        [Test]
        public void ExtractBasic()
        {
            string html = "<html><img src='http://www.server.com/image.gif'></html>";
            List<HtmlImage> images = HtmlImageExtractor.Extract(html);
            Assert.AreEqual(images.Count, 1);
            Assert.AreEqual("http://www.server.com/image.gif", images[0].Src);
            Assert.IsTrue(string.IsNullOrEmpty(images[0].Alt));
            Assert.AreEqual(-1, images[0].Width);
            Assert.AreEqual(-1, images[0].Height);
        }

        [Test]
        public void ExtractAllProperties()
        {
            string html = "<html><img src='http://www.server.com/image.gif' alt='altvalue' width=10 height=20></html>";
            List<HtmlImage> images = HtmlImageExtractor.Extract(html);
            Assert.AreEqual(images.Count, 1);
            Assert.AreEqual("http://www.server.com/image.gif", images[0].Src);
            Assert.AreEqual("altvalue", images[0].Alt);
            Assert.AreEqual(10, images[0].Width);
            Assert.AreEqual(20, images[0].Height);
        }

        [Test]
        public void ExtractMultipleImages()
        {
            string html = "<html><img src='http://www.server.com/image.gif' alt='altvalue' width=10 height=20>" +
                "<img src='http://www.server.com/anotherimage.gif'></html>";
            List<HtmlImage> images = HtmlImageExtractor.Extract(html);
            Assert.AreEqual(images.Count, 2);
        }

        [Test]
        public void ExtractRelativeImages()
        {
            string html = "<html><img src='image.gif' alt='altvalue' width=10 height=20>" +
                "<img src='http://www.two.com/image.gif'></html>";
            List<HtmlImage> images = HtmlImageExtractor.Extract(html, new Uri("http://www.one.com"));
            Assert.AreEqual(images.Count, 2);
            Assert.AreEqual("http://www.one.com/image.gif", images[0].Src);
            Assert.AreEqual("http://www.two.com/image.gif", images[1].Src);
        }
    }
}
