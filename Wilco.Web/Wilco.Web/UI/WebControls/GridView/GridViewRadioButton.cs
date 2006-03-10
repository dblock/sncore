using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Wilco.Web.UI.WebControls
{
    /// <summary>
    /// Represents a gridview radio button.
    /// </summary>
    public class GridViewRadioButton : HtmlInputRadioButton
    {
        private bool autoPostBack;

        /// <summary>
        /// Gets or sets whether the field should postback when the selection changes.
        /// </summary>
        [
        DefaultValue(false),
        Description("Gets or sets whether the field should postback when the selection changes.")
        ]
        public bool AutoPostBack
        {
            get
            {
                return this.autoPostBack;
            }
            set
            {
                this.autoPostBack = value;
            }
        }

        /// <summary>
        /// Gets the rendered name attribute value.
        /// </summary>
        public virtual string RenderedNameAttribute
        {
            get
            {
                DataControlFieldCell cell = this.Parent as DataControlFieldCell;
                GridView gridView = cell.NamingContainer.NamingContainer as GridView;
                return String.Format("{0};{1};{2}", gridView.UniqueID, gridView.Columns.IndexOf(cell.ContainingField), this.Name);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.GridViewRadioButton"/> class.
        /// </summary>
        public GridViewRadioButton()
        {
            //
        }

        /// <summary>
        /// Processes the postback data for the <see cref="Wilco.Web.UI.WebControls.GridViewRadioButton"/> control.
        /// </summary>
        /// <param name="postDataKey"></param>
        /// <param name="postCollection"></param>
        /// <returns></returns>
        protected override bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            string value = postCollection[this.RenderedNameAttribute];
            bool isChecked = false;
            if (value != null && value == this.Value)
            {
                if (!this.Checked)
                {
                    if (this.Page != null)
                    {
                        this.Page.ClientScript.ValidateEvent(this.Value, this.RenderedNameAttribute);
                    }
                    this.Checked = true;
                    isChecked = true;
                }
                return isChecked;
            }
            if (this.Checked)
            {
                this.Checked = false;
            }
            return isChecked;
        }

        /// <summary>
        /// Renders the control's attributes.
        /// </summary>
        /// <param name="writer"></param>
        protected override void RenderAttributes(HtmlTextWriter writer)
        {
            if (this.Page != null)
            {
                this.Page.ClientScript.RegisterForEventValidation(this.Value, this.RenderedNameAttribute);
            }
            writer.WriteAttribute("value", this.Value);
            this.Attributes.Remove("value");

            writer.WriteAttribute("name", this.RenderedNameAttribute);
            base.Attributes.Remove("name");
            bool addTypeAttribute = false;
            if (!string.IsNullOrEmpty(this.Type))
            {
                writer.WriteAttribute("type", this.Type);
                this.Attributes.Remove("type");
                addTypeAttribute = true;
            }
            if (this.ID != null)
            {
                writer.WriteAttribute("id", this.ClientID);
            }
            if (this.AutoPostBack)
            {
                string onClick = this.Attributes["onclick"];
                if (onClick != null)
                {
                    onClick += ";" + this.Page.ClientScript.GetPostBackEventReference(this, null);
                    this.Attributes.Remove("onclick");
                }
                else
                {
                    onClick = this.Page.ClientScript.GetPostBackEventReference(this, null);
                }
                writer.WriteAttribute("onclick", onClick);
            }
            this.Attributes.Render(writer);
            if (addTypeAttribute && base.DesignMode)
            {
                base.Attributes.Add("type", this.Type);
            }
            writer.Write(" /");
        }
    }
}