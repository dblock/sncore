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
    public struct HtmlPageInfo
    {
        public string Title;
    };

    /// <summary>
    /// This class extracts all images from an HTML body.
    /// </summary>
    public class HtmlPageInfoExtractor : HtmlUrlBasedExtractor
    {
        private string mTitle;

        public string Title
        {
            get
            {
                return mTitle;
            }
        }

        public HtmlPageInfoExtractor(TextReader reader) 
            : this(reader, null)
        {

        }

        public HtmlPageInfoExtractor(string content)
            : this(new StringReader(content), null)
        {

        }

        public HtmlPageInfoExtractor(string content, Uri basehref)
            : this(new StringReader(content), basehref)
        {

        }

        private static string[] tags = { "title" };

        public HtmlPageInfoExtractor(TextReader reader, Uri basehref)
            : base(tags, reader, basehref)
        {

        }

        protected override void OnTagProcessed(HtmlGenericControl tag)
        {
            switch (tag.TagName.ToLower())
            {
                case "title":
                    base.Read();
                    mTitle = this.Value;
                    break;
            }
        }

        public static HtmlPageInfo Extract(string html)
        {
            return Extract(html, null);
        }

        public static HtmlPageInfo Extract(string html, Uri basehref)
        {
            Html.HtmlPageInfoExtractor ex = new Html.HtmlPageInfoExtractor(html, basehref);
            while (!ex.EOF) ex.Read();
            HtmlPageInfo result = new HtmlPageInfo();
            result.Title = ex.Title;
            return result;
        }
    }
}
