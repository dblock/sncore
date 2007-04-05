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
    /// This class extracts all images from an HTML body.
    /// </summary>
    public class HtmlImageExtractor : HtmlUrlBasedExtractor
    {
        private List<HtmlImage> mImages = new List<HtmlImage>();

        public List<HtmlImage> Images
        {
            get
            {
                return mImages;
            }
            set
            {
                mImages = value;
            }
        }

        public HtmlImageExtractor(TextReader reader) 
            : this(reader, null)
        {

        }

        public HtmlImageExtractor(string content)
            : this(new StringReader(content), null)
        {

        }

        public HtmlImageExtractor(string content, Uri basehref)
            : this(new StringReader(content), basehref)
        {

        }

        private static string[] tags = { "img" };

        public HtmlImageExtractor(TextReader reader, Uri basehref)
            : base(tags, reader, basehref)
        {

        }

        protected override void OnTagProcessed(HtmlGenericControl tag)
        {
            HtmlImage image = new HtmlImage();
            image.Src = tag.Attributes["src"];
            if (BaseHref != null)
            {
                image.Src = HtmlUriExtractor.TryCreate(BaseHref, image.Src);
            }
            image.Alt = tag.Attributes["alt"];

            int width = 0;
            if (int.TryParse(tag.Attributes["width"], out width))
                image.Width = width;

            int height = 0;
            if (int.TryParse(tag.Attributes["height"], out height))
                image.Height = height;

            mImages.Add(image);
        }

        public static List<HtmlImage> Extract(string html)
        {
            return Extract(html, null);
        }

        public static List<HtmlImage> Extract(string html, Uri basehref)
        {
            Html.HtmlImageExtractor ex = new Html.HtmlImageExtractor(html, basehref);
            while (!ex.EOF) ex.Read();
            return ex.Images;
        }
    }
}
