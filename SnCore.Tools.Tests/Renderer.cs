using System;
using System.Collections.Generic;
using System.Text;
using SnCore.Tools.Web;
using NUnit.Framework;
using System.Text.RegularExpressions;

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
        public void ReplaceNoEmail()
        {
            TestDataItem[] testdata = 
            {
                new TestDataItem("foo", "foo"),
                new TestDataItem("<!-- NOEMAIL-START --><!-- NOEMAIL-END -->", string.Empty),
                new TestDataItem("<!-- NOEMAIL-START -->foo<!-- NOEMAIL-END -->", string.Empty),
                new TestDataItem("<!-- NOEMAIL-START -->foo<!-- NOEMAIL-END -->bar", "bar"),
                new TestDataItem("bar<!-- NOEMAIL-START -->foo<!-- NOEMAIL-END -->", "bar"),
                new TestDataItem("left<!-- NOEMAIL-START -->s1<!-- NOEMAIL-END -->middle<!-- NOEMAIL-START -->s2<!-- NOEMAIL-END -->right", "leftmiddleright"),
            };

            foreach (TestDataItem test in testdata)
            {
                string output = Regex.Replace(test._input, @"\<!-- NOEMAIL-START --\>.*?\<!-- NOEMAIL-END --\>", string.Empty,
                    RegexOptions.IgnoreCase | RegexOptions.Singleline);                
                Console.WriteLine("{0} => {1}", test._input, output);
                Assert.AreEqual(test._output, output);
            }
        }

        [Test]
        public void ReplaceOnClick()
        {        
            TestDataItem[] testdata = 
            {
                new TestDataItem("<a href=\"Default.aspx\" id=\"ctl00_menuHome_menuItem\" " +
                    "onclick=\"document.getElementById('ctl00_menuHome_labelItem').attributes['class'].value = 'sncore_menu_item_activated';;\">" +
                    "<span id=\"ctl00_menuHome_labelItem\" class=\"sncore_menu_item\">home</span></a>", 
                    "<a href=\"Default.aspx\" id=\"ctl00_menuHome_menuItem\"><span id=\"ctl00_menuHome_labelItem\" class=\"sncore_menu_item\">home</span></a>"),
                new TestDataItem(
                    "<a href=\"Default.aspx\" id=\"ctl00_menuHome_menuItem\" onclick=\"document.getElementById('ctl00_menuHome_labelItem').attributes['class'].value = 'sncore_menu_item_activated';;\"><span id=\"ctl00_menuHome_labelItem\" class=\"sncore_menu_item\">home</span></a><a href=\"AccountsView.aspx\" id=\"ctl00_menuPeople_menuItem\" onclick=\"document.getElementById('ctl00_menuPeople_labelItem').attributes['class'].value = 'sncore_menu_item_activated';;\"><span id=\"ctl00_menuPeople_labelItem\" class=\"sncore_menu_item\">people</span></a><a href=\"AccountFeedItemsView.aspx\" id=\"ctl00_menuFeeds_menuItem\" onclick=\"document.getElementById('ctl00_menuFeeds_labelItem').attributes['class'].value = 'sncore_menu_item_activated';;\"><span id=\"ctl00_menuFeeds_labelItem\" class=\"sncore_menu_item\">feeds</span></a><a href=\"PlacesView.aspx\" id=\"ctl00_menuPlaces_menuItem\" onclick=\"document.getElementById('ctl00_menuPlaces_labelItem').attributes['class'].value = 'sncore_menu_item_activated';;\"><span id=\"ctl00_menuPlaces_labelItem\" class=\"sncore_menu_item_selected\">places</span></a><a href=\"AccountEventsToday.aspx\" id=\"ctl00_menuEvents_menuItem\" onclick=\"document.getElementById('ctl00_menuEvents_labelItem').attributes['class'].value = 'sncore_menu_item_activated';;\"><span id=\"ctl00_menuEvents_labelItem\" class=\"sncore_menu_item\">events</span></a><a href=\"AccountStoriesView.aspx\" id=\"ctl00_menuStories_menuItem\" onclick=\"document.getElementById('ctl00_menuStories_labelItem').attributes['class'].value = 'sncore_menu_item_activated';;\"><span id=\"ctl00_menuStories_labelItem\" class=\"sncore_menu_item\">stories</span></a><a href=\"DiscussionTopOfThreadsView.aspx\" id=\"ctl00_menuDiscuss_menuItem\" onclick=\"document.getElementById('ctl00_menuDiscuss_labelItem').attributes['class'].value = 'sncore_menu_item_activated';;\"><span id=\"ctl00_menuDiscuss_labelItem\" class=\"sncore_menu_item\">forums</span></a><a href=\"AccountPreferencesManage.aspx\" id=\"ctl00_menuMe_menuItem\" onclick=\"document.getElementById('ctl00_menuMe_labelItem').attributes['class'].value = 'sncore_menu_item_activated';;\"><span id=\"ctl00_menuMe_labelItem\" class=\"sncore_menu_item\">me me</span></a><a href=\"AccountInvitationsManage.aspx\" id=\"ctl00_menuInvite_menuItem\" onclick=\"document.getElementById('ctl00_menuInvite_labelItem').attributes['class'].value = 'sncore_menu_item_activated';;\"><span id=\"ctl00_menuInvite_labelItem\" class=\"sncore_menu_item\">invite</span></a><a id=\"ctl00_menuLogout\" class=\"sncore_menu_item\" href=\"javascript:__doPostBack('ctl00$menuLogout','')\">logout</a><a href=\"Help.aspx\" id=\"ctl00_menuHelp_menuItem\" onclick=\"document.getElementById('ctl00_menuHelp_labelItem').attributes['class'].value = 'sncore_menu_item_activated';;\"><span id=\"ctl00_menuHelp_labelItem\" class=\"sncore_menu_item\">help</span></a>",
                    "<a href=\"Default.aspx\" id=\"ctl00_menuHome_menuItem\"><span id=\"ctl00_menuHome_labelItem\" class=\"sncore_menu_item\">home</span></a><a href=\"AccountsView.aspx\" id=\"ctl00_menuPeople_menuItem\"><span id=\"ctl00_menuPeople_labelItem\" class=\"sncore_menu_item\">people</span></a><a href=\"AccountFeedItemsView.aspx\" id=\"ctl00_menuFeeds_menuItem\"><span id=\"ctl00_menuFeeds_labelItem\" class=\"sncore_menu_item\">feeds</span></a><a href=\"PlacesView.aspx\" id=\"ctl00_menuPlaces_menuItem\"><span id=\"ctl00_menuPlaces_labelItem\" class=\"sncore_menu_item_selected\">places</span></a><a href=\"AccountEventsToday.aspx\" id=\"ctl00_menuEvents_menuItem\"><span id=\"ctl00_menuEvents_labelItem\" class=\"sncore_menu_item\">events</span></a><a href=\"AccountStoriesView.aspx\" id=\"ctl00_menuStories_menuItem\"><span id=\"ctl00_menuStories_labelItem\" class=\"sncore_menu_item\">stories</span></a><a href=\"DiscussionTopOfThreadsView.aspx\" id=\"ctl00_menuDiscuss_menuItem\"><span id=\"ctl00_menuDiscuss_labelItem\" class=\"sncore_menu_item\">forums</span></a><a href=\"AccountPreferencesManage.aspx\" id=\"ctl00_menuMe_menuItem\"><span id=\"ctl00_menuMe_labelItem\" class=\"sncore_menu_item\">me me</span></a><a href=\"AccountInvitationsManage.aspx\" id=\"ctl00_menuInvite_menuItem\"><span id=\"ctl00_menuInvite_labelItem\" class=\"sncore_menu_item\">invite</span></a><a id=\"ctl00_menuLogout\" class=\"sncore_menu_item\" href=\"javascript:__doPostBack('ctl00$menuLogout','')\">logout</a><a href=\"Help.aspx\" id=\"ctl00_menuHelp_menuItem\"><span id=\"ctl00_menuHelp_labelItem\" class=\"sncore_menu_item\">help</span></a>")
            };

            foreach (TestDataItem test in testdata)
            {
                string output = Regex.Replace(test._input, @".?onclick=""[0-9a-zA-Z\._\';\ =\(\)\[\]]*""", string.Empty,
                    RegexOptions.IgnoreCase | RegexOptions.Singleline);                
                Console.WriteLine("{0}\n\t{1}", test._input, output);
                Assert.AreEqual(test._output, output);
            }

        }

        [Test]
        public void ReplaceJavascript()
        {
            TestDataItem[] testdata = 
            {
                new TestDataItem("<a href=\"javascript:__doPostBack('ctl00$menuLogout','')\">logout</a>", "")
            };

            foreach (TestDataItem test in testdata)
            {
                string output = Regex.Replace(test._input, @".?href=""javascript:[0-9a-zA-Z$\._\';,\ =\(\)\[\]]*""", string.Empty,
                    RegexOptions.IgnoreCase | RegexOptions.Singleline);
                Console.WriteLine("{0}\n\t{1}", test._input, output);
                Assert.AreEqual(test._output, output);
            }

        }

    }
}
