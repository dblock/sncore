using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;

namespace Janrain.OpenId
{
    
    public class LinkParser
    {
        private static RegexOptions flags = 
            RegexOptions.IgnoreCase | 
            RegexOptions.Compiled | 
            RegexOptions.Singleline | 
            RegexOptions.IgnorePatternWhitespace;

        private static Regex metaContentTypeRe = new Regex(@"<meta\s*http-equiv\s*=\s*['""]?content-type['""]?\s*content\s*=\s*['""]?([^'""]+?)['""\s]", flags);
        private static Regex removedRe = new Regex(@"<!--.*?-->|<!\[CDATA\[.*?\]\]>|<script\b[^>]*>.*?</script>", flags);

        private static string tagExpr = @"
# Starts with the tag name at a word boundary, where the tag name is
# not a namespace
<{0}\b(?!:)

# All of the stuff up to a "">"", hopefully attributes.
(?<attrs>[^>]*?)

(?: # Match a short tag
    />

|   # Match a full tag
    >

    (?<contents>.*?)

    # Closed by
    (?: # One of the specified close tags
        </?{1}\s*>

        # End of the string
    |   \Z

    )

)
";
        
        private static Regex tagMatcher(string tagName, params string[] closeTags)
        {
            string closers;
            if (closeTags.Length > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("(?:{0}", tagName);
                
                foreach (string closeTag in closeTags) 
                    sb.AppendFormat("|{0}", closeTag);
                
                sb.Append(")");
                closers = sb.ToString();
            }
            else
            {
                closers = tagName;
            }
            return new Regex(String.Format(tagExpr, tagName, closers), flags);
        }

        private static Regex htmlRe = tagMatcher("html");
        private static Regex headRe = tagMatcher(
                "head", new string[] { "body" });
        private static Regex linkRe = new Regex(@"<link\b", flags);
        private static Regex attrRe = new Regex(@"
# Must start with a sequence of word-characters, followed by an equals sign
(?<attrname>\w+)=

# Then either a quoted or unquoted attribute
(?:

# Match everything that's between matching quote marks
(?<qopen>[""'])(?<attrval>.*?)\k<qopen>

|

# If the value is not quoted, match up to whitespace
(?<attrval>(?:[^\s<>/]|/(?!>))+)
)

|

(?<endlink>[<>])
", flags);

        private static Regex entityRe = new Regex(
                @"&(?<entity>amp|lt|gt|quot);");

        private LinkParser() { } // Keep users from instantiating
        
        private static string MetaCharset ( byte[] data, 
                                            int length )
        {
            string asciiStr = ASCIIEncoding.ASCII.GetString(data, 0, length);
            Match m = metaContentTypeRe.Match(asciiStr);
            if (m.Success)
            {
                string ct = m.Groups[1].ToString();
                string[] ctParts = ct.Split(new char[] { ';' });
                foreach (string ctPart in ctParts)
                {
                    if (ctPart.StartsWith("charset="))
                        return ctPart.Substring(8);
                }
            }
            return null;
        }

        private static string ReplaceEntities ( string html )
        {
            string repl;
            Match m = entityRe.Match(html);
            while (m.Success)
            {
                switch (m.Groups["entity"].ToString())
                {
                    case ("amp"):
                        repl = "&";
                        break;
                    case ("lt"):
                        repl = "<";
                        break;
                    case ("gt"):
                        repl = ">";
                        break;
                    case ("quot"):
                        repl = @"""";
                        break;
                    default:
                        repl = null;
                        break;
                }
                html = html.Substring(0, m.Index) + repl +
                    html.Substring(m.Index + m.Length);
                m = entityRe.Match(html, m.Index + repl.Length );
            }

            return html;
        }

        private static object[] _ParseLinkAttrs(byte[] data, int length, string charset)
        {
            NameValueCollection attrs;
            ArrayList result = new ArrayList();
            string _charset = MetaCharset(data, length);
            if (_charset != null)
                charset = _charset;
            Encoding enc = Encoding.GetEncoding(charset);
            string html = enc.GetString(data, 0, length);
            html = removedRe.Replace(html, "");
            Match htmlMo = htmlRe.Match(html);
            if (!htmlMo.Success)
                return result.ToArray();

            Match headMo = headRe.Match(html, htmlMo.Index, htmlMo.Length);
            if (!headMo.Success)
                return result.ToArray();

            int start;
            string attrName, attrVal;
            for (Match linkMo = linkRe.Match(html, headMo.Index, headMo.Length); linkMo.Success; linkMo = linkMo.NextMatch())
            {
                start = linkMo.Index + linkMo.Length;
                attrs = new NameValueCollection();
                for (Match attrMo = attrRe.Match(html, start, headMo.Index + headMo.Length - start); attrMo.Success; attrMo = attrMo.NextMatch())
                {
                    if (attrMo.Groups["endlink"].Success)
                        break;
                    
                    attrName = attrMo.Groups["attrname"].Value;
                    attrVal = ReplaceEntities(attrMo.Groups["attrval"].Value);
                    attrs[attrName] = attrVal;
                }
                result.Add(attrs);
            }
            
            return result.ToArray();
        }

	public static NameValueCollection[] ParseLinkAttrs(byte[] data, int length, string charset)
	{
	    object[] result = _ParseLinkAttrs(data, length, charset);
	    NameValueCollection[] actual = new NameValueCollection[result.Length];
	    result.CopyTo(actual, 0);
	    return actual;
	}
    }

}
