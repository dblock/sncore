using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SnCore.Tools.Web.Html
{
    public class CssAbsoluteLinksWriter
    {
        private Uri mBaseUri = null;

        private string RewriteUri(Match m)
        {
            string uri = m.Groups["url"].Value;
            return string.Format("url({0})", new Uri(mBaseUri, uri));
        }

        public CssAbsoluteLinksWriter(Uri baseuri)
        {
            mBaseUri = baseuri;
        }

        public string Rewrite(string css)
        {
            MatchEvaluator m = new MatchEvaluator(RewriteUri);
            return Regex.Replace(css, @"url\((?<url>.*)\)", m, RegexOptions.IgnoreCase);
        }

        public static string Rewrite(string css, Uri baseuri)
        {
            CssAbsoluteLinksWriter w = new CssAbsoluteLinksWriter(baseuri);
            return w.Rewrite(css);
        }
    }
}
