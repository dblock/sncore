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
    public class HtmlImageExtractor : Sgml.SgmlReader
    {
        private Uri mBaseHref = null;
        private List<HtmlImage> mImages = new List<HtmlImage>();

        public Uri BaseHref
        {
            get
            {
                return mBaseHref;
            }
        }

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

        public HtmlImageExtractor(TextReader reader) : this(reader, null)
        {

        }

        public HtmlImageExtractor(TextReader reader, Uri basehref)
            : base()
        {
            mBaseHref = basehref;
            base.InputStream = reader;
            base.DocType = "HTML";
            base.WhitespaceHandling = WhitespaceHandling.All;
        }

        public HtmlImageExtractor(string content) : this(content, null)
        {

        }

        public HtmlImageExtractor(string content, Uri basehref)
            : base()
        {
            mBaseHref = basehref;
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
                        if (base.Name.ToLower() == "img")
                        {
                            HtmlImage image = new HtmlImage();
                            image.Src = base.GetAttribute("src");
                            if (BaseHref != null) image.Src = new Uri(BaseHref, image.Src).ToString();
                            image.Alt = base.GetAttribute("alt");
                            
                            int width = 0; 
                            if (int.TryParse(base.GetAttribute("width"), out width))
                                image.Width = width;

                            int height = 0;
                            if (int.TryParse(base.GetAttribute("height"), out height))
                                image.Height = height;

                            mImages.Add(image);
                        }
                        break;
                }
            }
            return status;
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
