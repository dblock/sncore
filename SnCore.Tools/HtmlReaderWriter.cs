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
using System.Drawing;

namespace SnCore.Tools.Web.Html
{
    /// <summary>
    /// This class skips all nodes which has some kind of prefix. This trick does the job 
    /// to clean up MS Word/Outlook HTML markups.
    /// </summary>
    public class HtmlReader : Sgml.SgmlReader
    {
        public HtmlReader(TextReader reader)
            : base()
        {
            base.InputStream = reader;
            base.DocType = "HTML";
            base.WhitespaceHandling = WhitespaceHandling.All;
        }

        public HtmlReader(string content)
            : base()
        {
            base.InputStream = new StringReader(content);
            base.DocType = "HTML";
            base.WhitespaceHandling = WhitespaceHandling.All;
        }
    }

    public class HtmlWriterOptions
    {
        public HtmlWriterOptions()
        {

        }

        /// <summary>
        /// If set to true, it will filter the output by using tag and attribute filtering,
        /// space reduce etc
        /// </summary>
        public bool FilterOutput = true;

        /// <summary>
        /// If true, it will reduce consecutive &nbsp; with one instance
        /// </summary>
        public bool ReduceConsecutiveSpace = false;

        /// <summary>
        /// Decode &nbsp; to regular spaces.
        /// </summary>
        public bool DecodeSpace = false;

        /// <summary>
        /// Replace windows quotes.
        /// </summary>
        public bool ReplaceQuotes = false;

        /// <summary>
        /// Allow CDATA blocs.
        /// </summary>
        public bool AllowCDATA = false;

        /// <summary>
        /// Allow HTML comments.
        /// </summary>
        public bool AllowComments = true;

        /// <summary>
        /// Set the tag names in lower case which are allowed to go to output
        /// </summary>
        public string[] AllowedTags = new string[] { "p", "b", "i", "u", "em", "big", "small", "strike",
			        "div", "img", "span", "blockquote", "code", "pre", "br", "hr", "table", "tr", "td", "th", "h1", "h2", "h3",
			        "ul", "ol", "li", "del", "ins", "strong", "a", "font", "dd", "dt", "object", "param", "embed", "link" };

        /// <summary>
        /// New lines \r\n are replaced with space which saves space and makes the
        /// output compact
        /// </summary>
        public bool RemoveNewlines = false;
        /// <summary>
        /// Specify which attributes are allowed. Any other attribute will be discarded
        /// </summary>
        public string[] AllowedAttributes = new string[] { "href", "target", "border", "src", "valign", "align", "width", 
            "height", "color", "size", "class", "style", "type", "name", "value", "rel" };

        /// <summary>
        /// Base href to adjust links
        /// </summary>
        public Uri BaseHref = null;

        /// <summary>
        /// Base href to adjust images
        /// </summary>
        public Uri RewriteImgSrc = null;

        /// <summary>
        /// Target image size, set to zero if needs stripping
        /// </summary>
        public Nullable<Size> RewriteImgSize;
    }

    /// <summary>
    /// Extends XmlTextWriter to provide Html writing feature which is not as strict as Xml
    /// writing. For example, Xml Writer encodes content passed to WriteString which encodes special markups like
    /// &nbsp to &amp;bsp. So, WriteString is bypassed by calling WriteRaw.
    /// </summary>
    public class HtmlWriter : XmlTextWriter
    {
        private string LastStartElement = string.Empty;
        private HtmlWriterOptions mOptions = null;

        public HtmlWriterOptions Options
        {
            get
            {
                if (mOptions == null)
                {
                    mOptions = new HtmlWriterOptions();
                }

                return mOptions;
            }
        }

        public HtmlWriter(TextWriter writer)
            : base(writer)
        {

        }

        public HtmlWriter(TextWriter writer, HtmlWriterOptions options)
            : base(writer)
        {
            mOptions = options;
        }

        public HtmlWriter(StringBuilder builder, HtmlWriterOptions options)
            : base(new StringWriter(builder))
        {
            mOptions = options;
        }

        public HtmlWriter(StringBuilder builder)
            : base(new StringWriter(builder))
        {

        }

        public HtmlWriter(Stream stream, Encoding enc, HtmlWriterOptions options)
            : base(stream, enc)
        {
            mOptions = options;
        }

        public HtmlWriter(Stream stream, Encoding enc)
            : base(stream, enc)
        {

        }

        public override void WriteProcessingInstruction(string name, string text)
        {
            
        }

        /// <summary>
        /// The reason why we are overriding this method is, we do not want the output to be
        /// encoded for texts inside attribute and inside node elements. For example, all the &nbsp;
        /// gets converted to &amp;nbsp in output. But this does not 
        /// apply to HTML. In HTML, we need to have &nbsp; as it is.
        /// </summary>
        /// <param name="text"></param>
        public override void WriteString(string text)
        {
            // Change all non-breaking space to normal space
            // text = text.Replace(" ", "&nbsp;");
            /// When you are reading RSS feed and writing Html, this line helps remove
            /// those CDATA tags
            //text = text.Replace("<![CDATA[", "");
            //text = text.Replace("]]>", "");

            // Do some encoding of our own because we are going to use WriteRaw which won't
            // do any of the necessary encoding
            //text = text.Replace("<", "&lt;");
            //text = text.Replace(">", "&gt;");
            //text = text.Replace("'", "&apos;");
            //text = text.Replace("\"", "&quote;");

            if (Options.FilterOutput)
            {
                // text = text.Trim();

                // We want to replace consecutive spaces to one space in order to save horizontal
                // width
                if (Options.ReduceConsecutiveSpace) text = text.Replace("&nbsp;&nbsp;", "&nbsp;");
                if (Options.DecodeSpace) text = text.Replace("&nbsp;", " ");
                if (Options.RemoveNewlines) text = text.Replace(Environment.NewLine, " ");
                
                // typical word quotes
                if (Options.ReplaceQuotes)
                {
                    text = text.Replace("“", "\"");
                    text = text.Replace("”", "\"");
                    text = text.Replace("’", "'");
                }

                base.WriteRaw(text);
            }
            else
            {
                base.WriteRaw(text);
            }
        }

        //public override void WriteWhitespace(string ws)
        //{
        //    if (!Options.FilterOutput) base.WriteWhitespace(ws);
        //}

        public override void WriteComment(string text)
        {
            if (!Options.FilterOutput || Options.AllowComments)
            {
                base.WriteComment(text);
            }
        }

        /// <summary>
        /// This method is overriden to filter out tags which are not allowed
        /// </summary>
        public override void WriteStartElement(string prefix, string localName, string ns)
        {
            if (Options.FilterOutput)
            {
                if (Array.IndexOf(Options.AllowedTags, localName.ToLower()) < 0)
                {
                    localName = "stripped";
                }
            }

            LastStartElement = localName.ToLower();
            base.WriteStartElement(prefix, localName, ns);
        }

        /// <summary>
        /// This method is overriden to filter out attributes which are not allowed
        /// </summary>
        public override void WriteAttributes(XmlReader reader, bool defattr)
        {
            if (Options.FilterOutput)
            {
                // The following code is copied from implementation of XmlWriter's
                // WriteAttributes method. 
                if (reader == null)
                {
                    throw new ArgumentNullException("reader");
                }
                if ((reader.NodeType == XmlNodeType.Element) || (reader.NodeType == XmlNodeType.XmlDeclaration))
                {
                    if (reader.MoveToFirstAttribute())
                    {
                        WriteAttributes(reader, defattr);
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

                            // Check if the attribute is allowed 
                            bool canWrite = true;

                            switch (LastStartElement)
                            {
                                case "embed":
                                    canWrite = true;
                                    break;
                                case "img":
                                    if (Options.RewriteImgSize.HasValue 
                                        && Options.RewriteImgSize.Value.Width <= 0 
                                        && Options.RewriteImgSize.Value.Height <= 0
                                        && (attributename == "width" || attributename == "height"))
                                    {
                                        canWrite = false;
                                    }
                                    break;
                                default:
                                    canWrite = (Array.IndexOf(Options.AllowedAttributes, attributename) >= 0);
                                    break;
                            }

                            // If allowed, write the attribute
                            if (canWrite)
                            {
                                WriteStartAttribute(reader.Prefix, reader.LocalName, reader.NamespaceURI);
                            }

                            while (reader.ReadAttributeValue())
                            {
                                if (reader.NodeType == XmlNodeType.EntityReference)
                                {
                                    if (canWrite)
                                    {
                                        WriteEntityRef(reader.Name);
                                    }

                                    continue;
                                }

                                if (canWrite)
                                {
                                    string value = reader.Value;

                                    if (Options.BaseHref != null 
                                        && LastStartElement == "a" 
                                        && attributename == "href")
                                    {
                                        value = HtmlUriExtractor.TryCreate(
                                            Options.BaseHref, reader.Value, value);
                                    }
                                    
                                    if (Options.BaseHref != null
                                        && (LastStartElement == "img" || LastStartElement == "embed")
                                        && attributename == "src")
                                    {
                                        value = HtmlUriExtractor.TryCreate(
                                            Options.BaseHref, reader.Value, value);
                                    }

                                    if (Options.RewriteImgSrc != null 
                                        && LastStartElement == "img" 
                                        && attributename == "src")
                                    {
                                        value = Options.RewriteImgSrc.ToString().Replace("{url}", 
                                            Renderer.UrlEncode(value));
                                    } 
                                    else if (Options.RewriteImgSize.HasValue
                                        && LastStartElement == "img" 
                                        && attributename == "width")
                                    {
                                        value = Options.RewriteImgSize.Value.Width.ToString();
                                    }
                                    else if (Options.RewriteImgSize.HasValue
                                        && LastStartElement == "img" 
                                        && attributename == "height")
                                    {
                                        value = Options.RewriteImgSize.Value.Height.ToString();
                                    }

                                    if (LastStartElement == "link" 
                                        && attributename == "rel")
                                    {
                                        switch (value.ToLower())
                                        {
                                            case "stylesheet":
                                                value = "stylesheet-stripped";
                                                break;
                                        }
                                    }

                                    WriteString(value);
                                }
                            }

                            if (canWrite)
                            {
                                WriteEndAttribute();
                            }
                        }
                    } while (reader.MoveToNextAttribute());
                }
            }
            else
            {
                base.WriteAttributes(reader, defattr);
            }
        }

        public override void WriteCData(string text)
        {
            if (! Options.FilterOutput || Options.AllowCDATA)
            {
                base.WriteCData(text);
            }
        }
    }

}
