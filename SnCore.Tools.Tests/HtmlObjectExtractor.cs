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
                "</object>",
                "<object width=\"500\" height=\"580\" align=\"middle\">" +
                " <param value=\"ids=72157594501048788&amp;names=la mar cebicheria&amp;userName=foodite&amp;userId=69594707@N00&amp;titles=on&amp;source=sets\" name=\"FlashVars\" />" +
                " <param value=\"http://www.db798.com/pictobrowser.swf\" name=\"PictoBrowser\" />" +
                " <param value=\"noscale\" name=\"scale\" />" +
                " <param value=\"#ffffff\" name=\"bgcolor\" />" +
                " <embed width=\"500\" height=\"580\" align=\"middle\" name=\"PictoBrowser\" bgcolor=\"#ffffff\" scale=\"noscale\" quality=\"best\" loop=\"false\" " +
                " flashvars=\"ids=72157594501048788&amp;names=la mar cebicheria&amp;userName=foodite&amp;userId=69594707@N00&amp;titles=on&amp;source=sets\" " +
                " src=\"http://www.db798.com/pictobrowser.swf\">" +
                "</embed></object>",
                "<object height=\"200\" width=\"242\"><param name=\"movie\" value=\"http://www.youtube.com/v/M8BJTRUcStM\" />" +
                "<embed src=\"http://www.youtube.com/v/M8BJTRUcStM\" type=\"application/x-shockwave-flash\" height=\"200\" width=\"342\">" +
                "</embed></object>",
                "<p><embed src='http://apps.rockyou.com/rockyou.swf?instanceid=61671486&#038;ver=102906' quality='high'  salign='lt' width='426' height='320' wmode='transparent' name='rockyou' type='application/x-shockwave-flash' pluginspage=' http://www.macromedia.com/go/getflashplayer'/><br><a target='_BLANK' href=' http://www.rockyou.com/slideshow-create.php?refid=61671486'><img title='RockYou slideshow' src='http://apps.rockyou.com/images/logo-mini.gif ' border='0'></a> | <a target='_BLANK' alt='Comment, Add to Favorite' href='http://www.rockyou.com/show_my_gallery.php?instanceid=61671486'>View  Show</a> | <a target='_BLANK' href='http://www.rockyou.com/slideshow-create.php?refid=61671486'>Create  Your Own</a></p>" +
                "<p>It was very informative and thought provoking, he was very comfortable talking about the  subject of the slaughter and processing of the animals.  He discussed his choice of outsourcing to a small local facility where the process is not a huge production line on a tight schedule and is more humane, where the life and death of the animal can be acknowledged at the time of the slaughter, its existence being more than just profitability.  Deep ponderous thoughts&#8230;</p>" +
                "<p><embed src='http://apps.rockyou.com/rockyou.swf?instanceid=61744910&#038;ver=102906' quality='high'  salign='lt' width='426' height='320' wmode='transparent' name='rockyou' type='application/x-shockwave-flash' pluginspage=' http://www.macromedia.com/go/getflashplayer'/><br><a target='_BLANK' href=' http://www.rockyou.com/slideshow-create.php?refid=61744910'><img title='RockYou slideshow' src='http://apps.rockyou.com/images/logo-mini.gif ' border='0'></a> | <a target='_BLANK' alt='Comment, Add to Favorite' href='http://www.rockyou.com/show_my_gallery.php?instanceid=61744910'>View  Show</a> | <a target='_BLANK' href='http://www.rockyou.com/slideshow-create.php?refid=61744910'>Create  Your Own</a></p>" +
                "<p>It felt rather subdued as we all got on the bus and came back to the shop where a beautiful spread awaited us.  I&#8217;m sure my eyes lit up - ahhh, lunch!  Stacks of crusty buns, steaming plates of sliced beef (one was corned beef and the other was knuckle which was something like brisket) with bowls of au jus, cheese, sliced tomato and onion.  On the other table was a bowl of lovely tossed greens, tasty condiments and a big platter of home-made cookies.</p>" +
                "<p><img src=\"http://i11.photobucket.com/albums/a154/foodhoe/mar07/marinsunfarm/msf_spread.jpg\" alt=\"spread\" height=\"272\" width=\"448\" /></p>"
            };

            foreach (string test in tests)
            {
                List<HtmlGenericControl> embed = HtmlObjectExtractor.Extract(test);
                foreach (HtmlGenericControl control in embed)
                {
                    Console.WriteLine("Type: {0}", HtmlObjectExtractor.GetType(control));
                    Console.WriteLine(HtmlGenericCollector.GetHtml(control));
                }
            }
        }
    }
}
