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
    public class HtmlObjectExtractorTest
    {
        [Test]
        public void ExtractSome()
        {
            string[] tests = {
                "<html> bla bla bla" +
                " <object width='400' height='326' type='application/x-shockwave-flash' data='http://video.google.com/googleplayer.swf?docId=-6954414057124045339'>" +
                "  <param name='allowScriptAccess' value='never' />" +
                "  <param name='movie' value='http://video.google.com/googleplayer.swf?docId=-6954414057124045339'/>" +
                "  <param name='quality' value='best'/>" +
                "  <param name='bgcolor' value='#ffffff' />" +
                "  <param name='scale' value='noScale' />" +
                "  <param name='wmode' value='window'/>" +
                " </object>" +
                " nonsense" +
                " <object width='400' height='326' type='application/xyz' data='http://video.google.com/googleplayer.swf?docId=-6954414057124045339'>" +
                "  <param name='p1' value='v1'/>" +
                "  nonsense" +
                " </object>" +
                "bla bla bla</html>",
                "<object width='425' height='350'>" +
                " <param name='movie' value='http://www.youtube.com/v/ZfbTO0GlONU'></param>" +
                " <param name='wmode' value='transparent'></param>" +
                " <embed src='http://www.youtube.com/v/ZfbTO0GlONU' type='application/x-shockwave-flash' wmode='transparent' width='425' height='350'></embed>" +
                "</object>"
            };

            foreach (string test in tests)
            {
                List<HtmlGenericControl> embed = HtmlObjectExtractor.Extract(test);
                foreach (HtmlGenericControl control in embed)
                {
                    Console.WriteLine("Type: {0}", HtmlObjectExtractor.GetType(control));
                    Console.WriteLine(HtmlObjectExtractor.GetHtml(control));
                }
            }
        }
    }
}
