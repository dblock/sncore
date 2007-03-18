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
    public class HtmlGenericCollector : HtmlGenericExtractor
    {
        private List<HtmlGenericControl> mTags = new List<HtmlGenericControl>();

        public List<HtmlGenericControl> Tags
        {
            get
            {
                return mTags;
            }
            set
            {
                mTags = value;
            }
        }

        protected override void OnTagProcessed(HtmlGenericControl tag)
        {
            mTags.Add(tag);
            base.OnTagProcessed(tag);
        }

        public HtmlGenericCollector(string[] tagnames, TextReader reader)
            : base(tagnames, reader)
        {

        }

        public HtmlGenericCollector(string[] tagnames, string content)
            : base(tagnames, content)
        {

        }

        public static string GetHtml(HtmlGenericControl control)
        {
            StringBuilder s = new StringBuilder();
            HtmlTextWriter tw = new HtmlTextWriter(new StringWriter(s));
            control.RenderControl(tw);
            tw.Flush();
            return s.ToString();
        }
    };

    public class HtmlGenericExtractor : Sgml.SgmlReader
    {
        private List<string> mTagNames = new List<string>();

        public List<string> TagNames
        {
            get
            {
                return mTagNames;
            }
        }

        public HtmlGenericExtractor(string[] tagnames, TextReader reader)
            : base()
        {
            mTagNames.AddRange(tagnames);
            base.InputStream = reader;
            base.DocType = "HTML";
            base.WhitespaceHandling = WhitespaceHandling.All;
        }

        public HtmlGenericExtractor(string[] tagnames, string content)
            : this(tagnames, new StringReader(content))
        {

        }

        protected virtual bool OnTagFound(ref string name)
        {
            return mTagNames.Contains(name);
        }

        protected virtual bool OnAttributeFound(ref string name, ref string value)
        {
            return true;
        }

        protected virtual void OnTagProcessed(HtmlGenericControl tag)
        {

        }

        public override bool Read()
        {
            bool status = base.Read();
            if (status)
            {
                switch (base.NodeType)
                {
                    case XmlNodeType.Element:

                        string name = base.Name.ToLower();

                        if (!OnTagFound(ref name))
                            break;

                        HtmlGenericControl tag = new HtmlGenericControl(name);

                        for (int i = 0; i < AttributeCount; i++)
                        {
                            string attrname = GetAttributeName(i).ToLower();
                            string attrvalue = GetAttribute(i);

                            if (OnAttributeFound(ref attrname, ref attrvalue))
                            {
                                tag.Attributes.Add(attrname, attrvalue);
                            }
                        }

                        OnTagProcessed(tag);
                        break;
                }
            }
            return status;
        }
    }
}
