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
    public class HtmlLinkControl : HtmlGenericControl
    {
        public string Rel
        {
            get
            {
                return Attributes["rel"];
            }
            set
            {
                Attributes["rel"] = value;
            }
        }

        public string Href
        {
            get
            {
                return Attributes["href"];
            }
            set
            {
                Attributes["href"] = value;
            }
        }

        public string Title
        {
            get
            {
                return Attributes["title"];
            }
            set
            {
                Attributes["title"] = value;
            }
        }

        public string Type
        {
            get
            {
                return Attributes["type"];
            }
            set
            {
                Attributes["type"] = value;
            }
        }

        public HtmlLinkControl()
            : base("link")
        {

        }

        public HtmlLinkControl(HtmlGenericControl copy)
            : base("link")
        {
            foreach (string key in copy.Attributes.Keys)
            {
                Attributes.Add(key, copy.Attributes[key]);
            }
        }
    }

    public class HtmlLinkExtractor : HtmlUrlBasedExtractor
    {
        private List<HtmlLinkControl> mLinks = new List<HtmlLinkControl>();

        public List<HtmlLinkControl> Links
        {
            get
            {
                return mLinks;
            }
            set
            {
                mLinks = value;
            }
        }

        public HtmlLinkExtractor(TextReader reader)
            : this(reader, null)
        {

        }

        private static string[] tags = { "link" };

        public HtmlLinkExtractor(string content)
            : this(content, null)
        {

        }

        public HtmlLinkExtractor(string content, Uri root)
            : base(tags, content, root)
        {

        }

        public HtmlLinkExtractor(TextReader reader, Uri root)
            : base(tags, reader, root)
        {

        }

        protected override void OnTagProcessed(HtmlGenericControl tag)
        {
            HtmlLinkControl link = new HtmlLinkControl(tag);

            if (string.IsNullOrEmpty(link.Href) ||
                string.IsNullOrEmpty(link.Rel) ||
                string.IsNullOrEmpty(link.Type))
            {
                return;
            }

            if (string.IsNullOrEmpty(link.Title))
                link.Title = "Untitled";

            mLinks.Add(link);
        }

        public static List<HtmlLinkControl> Extract(string html)
        {
            return Extract(html, null);
        }

        public static List<HtmlLinkControl> Extract(string html, Uri root)
        {
            HtmlLinkExtractor ex = new HtmlLinkExtractor(html, root);
            while (!ex.EOF) ex.Read();
            return ex.Links;
        }
    }
}
