using System;
using System.Globalization;
using System.IO;
using Sgml;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;

namespace SnCore.Tools.Web.Html
{
    /// <summary>
    /// This class extracts all links from an HTML body.
    /// </summary>
    public class HtmlUriExtractor : Sgml.SgmlReader
    {
        private Uri mRoot = null;
        private List<Uri> mUris = new List<Uri>();

        public List<Uri> Uris
        {
            get
            {
                return mUris;
            }
            set
            {
                mUris = value;
            }
        }

        public HtmlUriExtractor(TextReader reader)
            : this(reader, null)
        {

        }

        public HtmlUriExtractor(TextReader reader, Uri root)
            : base()
        {
            base.InputStream = reader;
            base.DocType = "HTML";
            base.WhitespaceHandling = WhitespaceHandling.All;
        }

        public HtmlUriExtractor(string content)
            : this(content, null)
        {

        }

        public HtmlUriExtractor(string content, Uri root)
            : base()
        {
            mRoot = root;
            base.InputStream = new StringReader(content);
            base.DocType = "HTML";
            base.WhitespaceHandling = WhitespaceHandling.All;
        }

        public override bool Read()
        {
            bool status = base.Read();
            if (status)
            {
                switch (base.NodeType)
                {
                    case XmlNodeType.Element:
                        if ((base.Name.ToLower() == "a") || (base.Name.ToLower() == "link"))
                        {
                            Uri uri = null;
                            string href = base.GetAttribute("href");

                            if (mRoot != null)
                            {
                                if (Uri.TryCreate(mRoot, href, out uri))
                                    mUris.Add(uri);
                            }
                            else
                            {
                                if (Uri.TryCreate(href, UriKind.RelativeOrAbsolute, out uri))
                                    mUris.Add(uri);
                            }
                        }
                        break;
                }
            }
            return status;
        }

        public static List<Uri> Extract(string html)
        {
            return Extract(html, null);
        }

        public static List<Uri> Extract(string html, Uri root)
        {
            HtmlUriExtractor ex = new HtmlUriExtractor(html, root);
            while (!ex.EOF) ex.Read();
            return ex.Uris;
        }

        public static string TryCreate(Uri baseuri, string relativeuri, string defaultvalue)
        {
            try
            {
                Uri result = null;
                if (Uri.TryCreate(baseuri, relativeuri, out result))
                    return result.ToString();
            }
            catch (UriFormatException)
            {
                // TryCreate chokes on "mailto:foo (at) bar.com"
            }

            return defaultvalue;
        }
    }
}
