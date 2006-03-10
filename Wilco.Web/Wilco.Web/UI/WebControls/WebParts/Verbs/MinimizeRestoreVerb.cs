using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Wilco.Web.UI.WebControls.WebParts
{
	/// <summary>
	/// Represents a verb which lets a user minimize and restore the state of a webpart panel.
	/// </summary>
	public class MinimizeRestoreVerb : Control, IAttributeAccessor,INamingContainer
	{
		private StateBag attributesState;
		private System.Web.UI.AttributeCollection attributes;

		/// <summary>
		/// Gets the attributes of this control.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public System.Web.UI.AttributeCollection Attributes
		{
			get
			{
				if (this.attributes == null)
				{
					if (this.attributesState == null)
					{
						this.attributesState = new StateBag(true);
						if (this.IsTrackingViewState)
						{
							((IStateManager)this.attributesState).TrackViewState();
						}
					}
					this.attributes = new System.Web.UI.AttributeCollection(this.attributesState);
				}
				return this.attributes;
			}
		} 

		/// <summary>
		/// Gets or sets the tooltip which is displayed when a webpart panel can be minimized.
		/// </summary>
		[
		Description("Gets or sets the tooltip which is displayed when a webpart panel can be minimized."),
		Localizable(true),
		PersistenceMode(PersistenceMode.Attribute)
		]
		public string MinimizeToolTip
		{
			get
			{
				return ViewStateUtility.GetViewStateValue<string>(this.ViewState, "MinimizeToolTip", String.Empty);
			}
			set
			{
				this.ViewState["MinimizeToolTip"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the tooltip which is displayed when a webpart panel can be restored.
		/// </summary>
		[
		Description("Gets or sets the tooltip which is displayed when a webpart panel can be restored."),
		Localizable(true),
		PersistenceMode(PersistenceMode.Attribute)
		]
		public string RestoreToolTip
		{
			get
			{
				return ViewStateUtility.GetViewStateValue<string>(this.ViewState, "RestoreToolTip", String.Empty);
			}
			set
			{
				this.ViewState["RestoreToolTip"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the image url which is used when a webpart panel can be minimized.
		/// </summary>
		[
		Description("Gets or sets the url which is used when a webpart panel can be minimized."),
		Localizable(false),
		PersistenceMode(PersistenceMode.Attribute)
		]
		public string MinimizeImageUrl
		{
			get
			{
				return ViewStateUtility.GetViewStateValue<string>(this.ViewState, "MinimizeImageUrl", String.Empty);
			}
			set
			{
				this.ViewState["MinimizeImageUrl"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the image url which is used when a webpart panel can be restored.
		/// </summary>
		[
		Description("Gets or sets the url which is used when a webpart panel can be restored."),
		Localizable(false),
		PersistenceMode(PersistenceMode.Attribute)
		]
		public string RestoreImageUrl
		{
			get
			{
				return ViewStateUtility.GetViewStateValue<string>(this.ViewState, "RestoreImageUrl", String.Empty);
			}
			set
			{
				this.ViewState["RestoreImageUrl"] = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.WebParts.MinimizeRestoreVerb"/> class.
		/// </summary>
		public MinimizeRestoreVerb()
		{
			//
		}

		/// <summary>
		/// Raises the <see cref="System.Web.UI.Control.PreRender"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			if (this.DesignMode || (this.Page == null))
				return;

			this.RegisterClientScript();
		}

		/// <summary>
		/// Registers the client script.
		/// </summary>
		private void RegisterClientScript()
		{
			// Get the reference to the panel's body which should be minimized/restored.
			Control parentControl = this.NamingContainer;
			while ((parentControl != null) && !(parentControl is WebPartPanel))
			{
				parentControl = parentControl.NamingContainer;
			}

			if (parentControl == null)
				throw new InvalidOperationException("Verbs can only be used in webpart panels.");

			WebPartPanel panel = (WebPartPanel)parentControl;
			string bodyClientID = panel.ClientID;

			string minimizeImageUrl = this.ResolveUrl(this.MinimizeImageUrl);
			string restoreImageUrl = this.ResolveUrl(this.RestoreImageUrl);

			if (panel.AllowMinimize)
			{
				this.Attributes["onclick"] = String.Format("MinimizeRestore('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');",
					bodyClientID, this.ClientID, this.MinimizeToolTip, this.RestoreToolTip,
					minimizeImageUrl, restoreImageUrl);
			}

			if (panel.IsMinimized)
			{
				this.Attributes["title"] = this.RestoreToolTip;
				this.Attributes["src"] = restoreImageUrl;
			}
			else
			{
				this.Attributes["title"] = this.MinimizeToolTip;
				this.Attributes["src"] = minimizeImageUrl;
			}
		}

		/// <summary>
		/// Adds the attributes which should be rendered.
		/// </summary>
		/// <param name="writer"></param>
		protected virtual void AddAttributesToRender(HtmlTextWriter writer)
		{
			// Render the control properties.
			if (this.ID != null)
				writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID);

			writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "0px");

			// Render the attribute collection.
			if (this.attributesState == null)
				return;

			foreach (string key in this.Attributes.Keys)
			{
				writer.AddAttribute(key, this.Attributes[key]);
			}
		}

		/// <summary>
		/// Renders the begin tag of the control.
		/// </summary>
		/// <param name="writer"></param>
		protected virtual void RenderBeginTag(HtmlTextWriter writer)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
			writer.RenderBeginTag(HtmlTextWriterTag.A);
			this.AddAttributesToRender(writer);
			writer.RenderBeginTag(HtmlTextWriterTag.Img);
		}

		/// <summary>
		/// Renders the end tag of the control.
		/// </summary>
		/// <param name="writer"></param>
		protected virtual void RenderEndTag(HtmlTextWriter writer)
		{
			writer.RenderEndTag();
			writer.RenderEndTag();
		}

		/// <summary>
		/// Renders the control.
		/// </summary>
		/// <param name="writer"></param>
		protected override void Render(HtmlTextWriter writer)
		{
			this.RenderBeginTag(writer);
			base.Render(writer);
			this.RenderEndTag(writer);
		}

		#region IAttributeAccessor Members
		/// <summary>
		/// Gets an attribute.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		string IAttributeAccessor.GetAttribute(string key)
		{
			return this.Attributes[key];
		}

		/// <summary>
		/// Sets an attribute.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		void IAttributeAccessor.SetAttribute(string key, string value)
		{
			this.Attributes[key] = value;
		}
		#endregion
	}
}