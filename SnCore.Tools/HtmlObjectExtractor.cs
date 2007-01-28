using System;
using System.Globalization;
using System.IO;
using Sgml;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using SnCore.Tools.Drawing;
using System.Drawing;

namespace SnCore.Tools.Web.Html
{
    /// <summary>
    /// This class extracts all images from an HTML body.
    /// </summary>
    public class HtmlObjectExtractor : Sgml.SgmlReader
    {
        private Stack<HtmlControl> mObjectControls = new Stack<HtmlControl>();
        private Uri mBaseHref = null;
        private List<HtmlGenericControl> mEmbeds = new List<HtmlGenericControl>();
        private bool mInsideObject = false;

        public Uri BaseHref
        {
            get
            {
                return mBaseHref;
            }
        }

        public List<HtmlGenericControl> Embeds
        {
            get
            {
                return mEmbeds;
            }
            set
            {
                mEmbeds = value;
            }
        }

        public HtmlObjectExtractor(TextReader reader)
            : this(reader, null)
        {

        }

        public HtmlObjectExtractor(TextReader reader, Uri basehref)
            : base()
        {
            mBaseHref = basehref;
            base.InputStream = reader;
            base.DocType = "HTML";
            base.WhitespaceHandling = WhitespaceHandling.All;
        }

        public HtmlObjectExtractor(string content)
            : this(content, null)
        {

        }

        public HtmlObjectExtractor(string content, Uri basehref)
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
                    case XmlNodeType.EndElement:

                        if (mObjectControls.Count < 1)
                            break;

                        if (base.Name.ToLower() == "object")
                        {
                            mInsideObject = false;
                            break;
                        }

                        HtmlControl top = mObjectControls.Pop();
                        break;

                    case XmlNodeType.Element:

                        bool fObject = false;

                        if (!mInsideObject)
                        {
                            mInsideObject = fObject = (base.Name.ToLower() == "object");
                        }

                        if (!mInsideObject)
                            break;

                        HtmlGenericControl embed = new HtmlGenericControl(base.Name);

                        for (int i = 0; i < AttributeCount; i++)
                        {
                            string name = GetAttributeName(i);
                            string value = GetAttribute(i);

                            switch (name)
                            {
                                case "src":
                                    if (BaseHref != null) value = new Uri(BaseHref, value).ToString();
                                    break;
                            }

                            embed.Attributes.Add(name, value);
                        }

                        // width and height
                        try
                        {
                            Size size = new Size(int.Parse(GetAttribute("width")), int.Parse(GetAttribute("height")));
                            size = ThumbnailBitmap.GetNewSize(size, new Size(200, 300));
                            embed.Attributes["width"] = size.Width.ToString();
                            embed.Attributes["height"] = size.Height.ToString();
                        }
                        catch
                        {
                        }

                        if (fObject)
                        {
                            mEmbeds.Add(embed);
                        }
                        else
                        {
                            mObjectControls.Peek().Controls.Add(embed);
                        }

                        mObjectControls.Push(embed);
                        break;
                }
            }
            return status;
        }

        public static List<HtmlGenericControl> Extract(string html)
        {
            return Extract(html, null);
        }

        public static List<HtmlGenericControl> Extract(string html, Uri basehref)
        {
            Html.HtmlObjectExtractor ex = new Html.HtmlObjectExtractor(html, basehref);
            while (!ex.EOF) ex.Read();
            return ex.Embeds;
        }

        public static string GetHtml(HtmlGenericControl control)
        {
            StringBuilder s = new StringBuilder();
            HtmlTextWriter tw = new HtmlTextWriter(new StringWriter(s));
            control.RenderControl(tw);
            tw.Flush();
            return s.ToString();
        }

        public static string GetType(HtmlGenericControl control)
        {
            string type = control.Attributes["type"];
            if (!string.IsNullOrEmpty(type))
            {
                return type;
            }

            foreach (HtmlGenericControl child in control.Controls)
            {
                type = GetType(child);
                if (!string.IsNullOrEmpty(type))
                    break;
            }

            return type;
        }
    }
}
