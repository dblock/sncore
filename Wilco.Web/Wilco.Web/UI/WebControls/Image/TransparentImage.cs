using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Wilco.Web.UI.WebControls
{
    /// <summary>
    /// Represents a transparent image.
    /// </summary>
    public class TransparentImage : Image
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.TransparentImage"/> class.
        /// </summary>
        public TransparentImage()
        {
            //
        }

        /// <summary>
        /// Adds the attributes to render.
        /// </summary>
        /// <param name="writer"></param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            bool supportsPNG = !this.Page.Request.Browser.IsBrowser("IE");
            if (supportsPNG)
            {
                base.AddAttributesToRender(writer);
                return;
            }

            string imgUrl = this.ImageUrl;
            this.ImageUrl = this.Page.ClientScript.GetWebResourceUrl(this.GetType(), "Wilco.Web.Resources.Spacer.gif");
            this.Style.Add(HtmlTextWriterStyle.Filter, String.Format("progid:DXImageTransform.Microsoft.AlphaImageLoader(src='{0}')", this.ResolveClientUrl(imgUrl)));
            base.AddAttributesToRender(writer);
            this.ImageUrl = imgUrl;
        }
    }
}