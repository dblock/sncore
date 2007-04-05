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
    /// This class extracts all links from an HTML body and can rebase them.
    /// </summary>
    public class HtmlUrlBasedExtractor : HtmlGenericExtractor
    {
        private Uri mBaseHref = null;

        public Uri BaseHref
        {
            get
            {
                return mBaseHref;
            }
        }

        public HtmlUrlBasedExtractor(string[] tagnames, TextReader reader, Uri basehref)
            : base(tagnames, reader)
        {
            mBaseHref = basehref;
        }

        public HtmlUrlBasedExtractor(string[] tagnames, string content, Uri basehref)
            : base(tagnames, content)
        {
            mBaseHref = basehref;
        }

        protected override bool OnAttributeFound(ref string name, ref string value)
        {
            switch (name.ToLower())
            {
                case "src":
                case "href":
                    if (BaseHref != null)
                    {
                        value = HtmlUriExtractor.TryCreate(BaseHref, value);
                    }
                    break;
            }

            return base.OnAttributeFound(ref name, ref value);
        }
    }
}
