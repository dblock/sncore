// Copyright © 2005 by Omar Al Zabir. All rights are reserved.
// 
// If you like this code then feel free to go ahead and use it.
// The only thing I ask is that you don't remove or alter my copyright notice.
//
// Your use of this software is entirely at your own risk. I make no claims or
// warrantees about the reliability or fitness of this code for any particular purpose.
// If you make changes or additions to this code please mark your code as being yours.
// 
// website http://www.oazabir.com, email OmarAlZabir@gmail.com, msn oazabir@hotmail.com

using System;
using System.Globalization;
using System.IO;
using Sgml;
using System.Xml;
using System.Text;

namespace SnCore.Tools.Web.Html
{
    public class HtmlAbsoluteLinksWriter : XmlTextWriter
    {
        private string LastStartElement = string.Empty;
        public Uri BaseHref = null;

        public HtmlAbsoluteLinksWriter(TextWriter writer)
            : base(writer)
        {
        }
        public HtmlAbsoluteLinksWriter(StringBuilder builder)
            : base(new StringWriter(builder))
        {
        }
        public HtmlAbsoluteLinksWriter(Stream stream, Encoding enc)
            : base(stream, enc)
        {
        }

        public override void WriteString(string text)
        {
            base.WriteRaw(text);
        }

        public override void WriteAttributes(XmlReader reader, bool defattr)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            if ((reader.NodeType == XmlNodeType.Element) || (reader.NodeType == XmlNodeType.XmlDeclaration))
            {
                if (reader.MoveToFirstAttribute())
                {
                    this.WriteAttributes(reader, defattr);
                    reader.MoveToElement();
                }
            }
            else
            {
                if (reader.NodeType != XmlNodeType.Attribute)
                {
                    throw new XmlException("Xml_InvalidPosition");
                }
                do
                {
                    if (defattr || !reader.IsDefault)
                    {
                        string attributename = reader.LocalName.ToLower();

                        this.WriteStartAttribute(reader.Prefix, reader.LocalName, reader.NamespaceURI);

                        while (reader.ReadAttributeValue())
                        {
                            if (reader.NodeType == XmlNodeType.EntityReference)
                            {
                                this.WriteEntityRef(reader.Name);
                                continue;
                            }

                            string value = reader.Value;

                            if (BaseHref != null
                                && LastStartElement == "a"
                                && attributename == "href")
                            {
                                value = HtmlUriExtractor.TryCreate(
                                    BaseHref, reader.Value, value);
                            }

                            if (BaseHref != null
                                && (LastStartElement == "img" || LastStartElement == "embed")
                                && attributename == "src")
                            {
                                value = HtmlUriExtractor.TryCreate(
                                    BaseHref, reader.Value, value);
                            }

                            this.WriteString(value);
                        }

                        this.WriteEndAttribute();
                    }
                } while (reader.MoveToNextAttribute());
            }
        }

        public override void WriteStartElement(string prefix, string localName, string ns)
        {
            LastStartElement = localName.ToLower();
            base.WriteStartElement(prefix, localName, ns);
        }

        public static string Rewrite(string html, Uri baseuri)
        {
            HtmlReader r = new HtmlReader(html);
            StringWriter sw = new StringWriter();
            HtmlAbsoluteLinksWriter w = new HtmlAbsoluteLinksWriter(sw);
            w.BaseHref = baseuri;
            while (!r.EOF)
            {
                w.WriteNode(r, true);
            }
            w.Close();
            return sw.ToString();
        }
    }
}
