using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Collections.Specialized;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;

namespace SnCore.Tools.Web
{
    public class Renderer
    {
        public Renderer()
        {

        }

        public static string Render(object message)
        {
            if (message == null) return string.Empty;
            return Render(message.ToString());
        }

        /// hack
        private static bool IsHtml(string message)
        {
            if (string.IsNullOrEmpty(message))
                return false;
            if (message.IndexOf("<") < 0)
                return false;
            if (message.IndexOf(">") < 0)
                return false;

            return true;
        }

        public static string Render(string message)
        {
            if (message == null) return string.Empty;
            string result = HttpUtility.HtmlEncode(message);

            if (! IsHtml(message))
            {
                result = result.Replace("\n", "<br/>\n");
            }

            return result;
        }

        public static string RenderEx(object message)
        {
            if (message == null) return string.Empty;
            return RenderEx(message.ToString());
        }

        public static string RenderEx(string message)
        {
            string result = CleanHtml(message);

            if (!IsHtml(message))
            {
                result = result.Replace("\n", "<br/>\n");
            }

            result = RenderMarkups(result);
            result = RenderHref(result);
            return result;
        }

        public static Regex HtmlExpression = new Regex(@"<[^>]*>", RegexOptions.IgnoreCase);
        public static Regex BrExpression = new Regex(@"<br[\/ ]*>", RegexOptions.IgnoreCase);

        public static string RemoveHtml(object message)
        {
            if (message == null) return string.Empty;
            return RemoveHtml(message.ToString());
        }

        public static string RemoveHtml(string message)
        {
            message = BrExpression.Replace(message, "\n");
            message = HtmlExpression.Replace(message, string.Empty);
            message = message.Trim('\n');
            return message;
        }

        public static string CleanHtml(object message)
        {
            if (message == null) return string.Empty;
            return CleanHtml(message.ToString());
        }

        public static string CleanHtml(string html)
        {
            return CleanHtml(html, null, null);
        }

        public static string CleanHtml(string html, Uri basehref, Uri rewriteimgsrc)
        {
            try
            {
                Html.HtmlReader r = new Html.HtmlReader(html);
                StringWriter sw = new StringWriter();
                Html.HtmlWriter w = new Html.HtmlWriter(sw);
                w.ReduceConsecutiveSpace = true;
                w.DecodeSpace = true;
                w.ReplaceQuotes = true;
                w.BaseHref = basehref;
                w.RewriteImgSrc = rewriteimgsrc;
                while (! r.EOF)
                {
                    w.WriteNode(r, true);
                }
                w.Close();
                return sw.ToString();
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string UrlEncode(string message)
        {
            if (message == null) return string.Empty;
            return HttpUtility.UrlEncode(message);
        }

        public static string UrlEncode(object message)
        {
            if (message == null) return string.Empty;
            return UrlEncode(message.ToString());
        }

        public static string UrlDecode(string message)
        {
            if (message == null) return string.Empty;
            return HttpUtility.UrlDecode(message);
        }

        public static string UrlDecode(object message)
        {
            if (message == null) return string.Empty;
            return UrlDecode(message.ToString());
        }

        public static NameValueCollection ParseQueryString(string qstring)
        {
            NameValueCollection outc = new NameValueCollection();

            if (!string.IsNullOrEmpty(qstring))
            {
                //simplify our task
                qstring = qstring + "&";


                Regex r = new Regex(@"(?<name>[^=&]+)=(?<value>[^&]+)&", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                IEnumerator _enum = r.Matches(qstring).GetEnumerator();
                while (_enum.MoveNext() && _enum.Current != null)
                {
                    outc.Add(((Match)_enum.Current).Result("${name}"),
                            ((Match)_enum.Current).Result("${value}"));
                }
            }

            return outc;
        }

        static string HrefHandler(Match ParameterMatch)
        {
            string result = ParameterMatch.Value;

            // quoted results within other tags are ignored
            if (result.StartsWith("\"") && result.EndsWith("\""))
                return result;

            string afterresult = string.Empty;
            const string punctutation = ".,;:";
            while ((result.Length > 0) && (punctutation.IndexOf(result[result.Length - 1]) >= 0))
            {
                afterresult = afterresult + result[result.Length - 1];
                result = result.Remove(result.Length - 1, 1);
            }

            string shortenedresult = result;
            
            int cut = shortenedresult.IndexOf("?");
            if (cut >= 0)
            {
                shortenedresult = shortenedresult.Substring(0, cut) + " ...";
            }

            result = RemoveMarkups(result);

            result = string.Format("<a target=\"_blank\" href=\"{0}\">{1}</a>{2}",
                result,
                shortenedresult,
                afterresult);

            return result;
        }

        public static Regex HrefExpression = new Regex(@"[\'\""]{0,1}[\w]+://[a-zA-Z0-9\/\,\(\)\.\?\&+\##\%~=:;_\-\@]*[\'\""]{0,1}", RegexOptions.IgnoreCase);

        public static string RenderHref(string RenderValue)
        {
            MatchEvaluator HrefHandlerDelegate = new MatchEvaluator(HrefHandler);
            return HrefExpression.Replace(RenderValue, HrefHandlerDelegate);
        }

        class NameValueMap : StringDictionary 
        {
            public NameValueMap()
            {
                Add("[small]", "<small>");
                Add("[/small]", "</small>");
                Add("[h1]", "<h1>");
                Add("[h2]", "<h2>");
                Add("[h3]", "<h3>");
                Add("[big]", "<big>");
                Add("[/h1]", "</h1>");
                Add("[/h2]", "</h2>");
                Add("[/h3]", "</h3>");
                Add("[/big]", "</big>");
                Add("[b]", "<b>");
                Add("[/b]", "</b>");
                Add("[em]", "<em>");
                Add("[/em]", "</em>");
                Add("[i]", "<em>");
                Add("[/i]", "</em>");
                Add("[red]", "<font color=\"red\">");
                Add("[/red]", "</font>");
                Add("[green]", "<font color=\"green\">");
                Add("[/green]", "</font>");
                Add("[blue]", "<font color=\"blue\">");
                Add("[/blue]", "</font>");
                Add("[img]", "<img border=\"0\" src=\"");
                Add("[/img]", "\">");
                Add("[image]", "<img border=\"0\" src=\"");
                Add("[/image]", "\"/>");
                Add("[center]", "<div style=\"text-align: center;\">");
                Add("[/center]", "</div>");
                Add("[quote]", "<div style=\"border: dotted 1px silver; color: Silver; padding: 5px; font-size: xx-small;\">");
                Add("[/quote]", "</div>");
            }
        };

        static NameValueMap sMarkupMap = new NameValueMap();

        static string MarkupHandler(Match ParameterMatch)
        {
            string word = ParameterMatch.Value;

            if (word.StartsWith("[[") && word.EndsWith("]]"))
                return word.Substring(1, word.Length - 2);

            string result = sMarkupMap[word];
            return string.IsNullOrEmpty(result) ? word : result;
        }

        static string MarkupClearHandler(Match ParameterMatch)
        {
            return string.Empty;
        }

        static Regex MarkupExpression = new Regex(@"[\[]+\/*\w*[\]]+", RegexOptions.IgnoreCase);

        public static string RenderMarkups(string RenderValue)
        {
            MatchEvaluator MarkupHandlerDelegate = new MatchEvaluator(MarkupHandler);
            return MarkupExpression.Replace(RenderValue, MarkupHandlerDelegate);
        }

        public static string RemoveMarkups(string RenderValue)
        {
            MatchEvaluator MarkupHandlerDelegate = new MatchEvaluator(MarkupClearHandler);
            return MarkupExpression.Replace(RenderValue, MarkupHandlerDelegate);
        }

        public static string SqlEncode(string Value)
        {
            return Value.Replace("'", "''");
        }

        public static string GetSummary(string summary)
        {
            return GetSummary(summary, 168, 368);
        }

        public static string GetSummary(string summary, int min, int max)
        {
            string result = RemoveHtml(RemoveMarkups(summary));
            if (result.Length > max)
            {
                result = result.Substring(0, max);
                int cut = result.LastIndexOf(".");
                if (cut < min) cut = result.LastIndexOf(" ");
                if (cut >= min)
                {
                    result = result.Substring(0, cut) + " ...";
                }
            }
            return result;
        }

        public static string GetLink(string uri, string text, int max)
        {
            return string.Format("<a href=\"{0}\" target=\"_blank\">{1}</a>", uri, 
                (max > 0 && text.Length > max) ? text.Substring(0, max) + " ..." : text);
        }

        public static string GetLink(string uri, string text)
        {
            return GetLink(uri, text, -1);
        }
    }
}