using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net;
using SnCore.Tools.Web;
using System.IO;
using System.Text;
using SnCore.Tools.Web.Html;
using System.Text.RegularExpressions;

namespace SnCore.Tools.Web
{
    public class ContentPage
    {
        public static string GetContentSubject(string body)
        {
            int title_start = body.IndexOf("<title>", 0, StringComparison.OrdinalIgnoreCase);
            if (title_start < 0)
                return string.Empty;

            title_start = title_start + "<title>".Length;

            int title_end = body.IndexOf("</title>", title_start, StringComparison.OrdinalIgnoreCase);
            if (title_end < 0)
                return string.Empty;

            return body.Substring(title_start, title_end - title_start).Trim();
        }

        public static string GetContent(Uri uri, Uri baseuri)
        {
            return GetContent(uri, baseuri, string.Empty);
        }

        public static string GetHttpContent(Uri uri)
        {
            return GetHttpContent(uri, null);
        }

        public static string GetHttpContent(Uri uri, Cookie authcookie)
        {
            string content = string.Empty;

            HttpWebRequest request = (HttpWebRequest) HttpWebRequest.Create(uri);
            
            if (authcookie != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(uri, authcookie);
            }

            WebResponse response = request.GetResponse();
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                content = sr.ReadToEnd();
                sr.Close();
            }

            return content;
        }

        public static string GetCss(Uri baseuri)
        {
            StringBuilder css = new StringBuilder();
            css.AppendLine("<style type=\"text/css\">");
            css.AppendLine("<!--");
            css.AppendLine(CssAbsoluteLinksWriter.Rewrite(GetHttpContent(new Uri(baseuri, "Style.css")), baseuri));
            css.AppendLine("-->");
            css.AppendLine("</style>");
            return css.ToString();
        }

        public static string GetContent(Uri uri, Uri baseuri, string note)
        {
            return GetContent(uri, baseuri, note, true, null);
        }

        public static string GetContent(Uri uri, Uri baseuri, string note, bool hasonline, Cookie authcookie)
        {
            string content = GetHttpContent(uri, authcookie);

            string[] expressions = 
            { 
                @"\<!-- NOEMAIL-START --\>.*?\<!-- NOEMAIL-END --\>",
                @"\<script.*?\<\/script\>",
                @"\<style.*?\<\/style\>",
                @"\<link.*?\<\/link\>",
                @"\<link.*?\/\>",
                @"href=""javascript:[0-9a-zA-Z$\._\';,\ =\(\)\[\]]*""",
                @".?onclick=""[0-9a-zA-Z\._\';,\ =\(\)\[\]]*""",
            };

            foreach (string r in expressions)
            {
                content = Regex.Replace(content, r, string.Empty,
                    RegexOptions.IgnoreCase | RegexOptions.Singleline);
            }

            content = HtmlAbsoluteLinksWriter.Rewrite(content, baseuri);

            StringBuilder scontent = new StringBuilder(content);

            if (hasonline)
            {
                scontent.Insert(0, string.Format("<p style=\"margin: 10px;\"><a href=\"{0}\">can't see message? click here for online version &#187;&#187;</a></p>\n", uri.ToString()));
            }

            // insert additional note
            if (!string.IsNullOrEmpty(note))
            {
                scontent.Insert(0, string.Format("<p style=\"margin: 10px;\">{0}</p>\n", Renderer.Render(note)));
            }

            // hack: insert stylesheet
            scontent.Insert(0, GetCss(baseuri));
            return scontent.ToString();
        }
    }
}