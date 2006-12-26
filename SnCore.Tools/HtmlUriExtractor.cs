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
            : base()
        {
            base.InputStream = reader;
            base.DocType = "HTML";
            base.WhitespaceHandling = WhitespaceHandling.All;
        }

        public HtmlUriExtractor(string content)
            : base()
        {
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
                            if (Uri.TryCreate(href, UriKind.RelativeOrAbsolute, out uri))
                                mUris.Add(uri);
                        }
                        break;
                }
            }
            return status;
        }

        public static List<Uri> Extract(string html)
        {
            HtmlUriExtractor ex = new HtmlUriExtractor(html);
            while (!ex.EOF) ex.Read();
            return ex.Uris;
        }
    }
}
