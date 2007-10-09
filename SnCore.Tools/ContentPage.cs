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
    public class ContentPageParameters
    {
        public Cookie AuthCookie;
        public Nullable<DateTime> IfModifiedSince;
        public Uri BaseUri;
        public string Note;
        public bool HasOnline = true;
        public string UserAgent;
    }

    public class ContentPage
    {
#if DEBUG
        public static bool EnableRemoteContent = true;
#endif
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

        public static string GetContent(Uri uri)
        {
            return GetContent(uri, null);
        }

        public static string GetHttpContent(Uri uri)
        {
            return GetHttpContent(uri, null);
        }

        public static string GetHttpContent(Uri uri, ContentPageParameters parameters)
        {
#if DEBUG
            if (!EnableRemoteContent)
            {
                return string.Format("Content of {0}.", uri);
            }
#endif

            string content = string.Empty;

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);

            if (parameters != null)
            {
                if (parameters.AuthCookie != null)
                {
                    request.CookieContainer = new CookieContainer();
                    request.CookieContainer.Add(uri, parameters.AuthCookie);
                }

                if (parameters.IfModifiedSince.HasValue)
                {
                    request.IfModifiedSince = parameters.IfModifiedSince.Value;
                }

                request.UserAgent = parameters.UserAgent;
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

        public static string GetContent(Uri uri, ContentPageParameters parameters)
        {
            string content = GetHttpContent(uri, parameters);

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

            content = HtmlAbsoluteLinksWriter.Rewrite(content, parameters != null ? parameters.BaseUri : null);

            StringBuilder scontent = new StringBuilder(content);

            if (parameters != null && parameters.HasOnline)
            {
                scontent.Insert(0, string.Format("<p style=\"margin: 10px; font-size: 8pt;\">Do not hit 'reply' to this email! <a href=\"{0}\">Can't see message? click here for online version &#187;&#187;</a></p>\n", uri.ToString()));
            }

            // insert additional note
            if (parameters != null && ! string.IsNullOrEmpty(parameters.Note))
            {
                scontent.Insert(0, string.Format("<p style=\"margin: 10px;\">{0}</p>\n", Renderer.Render(parameters.Note)));
            }

            // hack: insert stylesheet
            scontent.Insert(0, GetCss(parameters != null ? parameters.BaseUri : null));
            return scontent.ToString();
        }
    }
}