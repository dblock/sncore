using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

using Wilco.Web.UI.Design.WebControls.WebParts;

namespace Wilco.Web.UI.WebControls.WebParts
{
	/// <summary>
	/// Represents a webpart zone.
	/// </summary>
	[
	Designer(typeof(WebPartZoneDesigner)),
	ParseChildren(true),
	PersistChildren(false)
	]
	public class WebPartZone : CompositeControl
	{
		private Orientation orientation = Orientation.Vertical;
		private WebPartManager webPartManager;
		private Label emptyZoneMessage;
		private ITemplate zoneTemplate;
		private Style titleStyle;

		/// <summary>
		/// Gets or sets the CSS class to use when an item in the zone is being dragged.
		/// </summary>
		[
		Category("Appearance"), 
		Description("Gets or sets the CSS class to use when an item in the zone is being dragged."),
		PersistenceMode(PersistenceMode.Attribute)
		]
		public string DragCssClass
		{
			get
			{
				return ViewStateUtility.GetViewStateValue<string>(this.ViewState, "DragCssClass", String.Empty);
			}
			set
			{
				this.ViewState["DragCssClass"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the text which is displayed when the zone does not contain any webparts.
		/// </summary>
		[
		Category("Appearance"),
		DefaultValue(""),
		Description("Gets or sets the text which is displayed when the zone does not contain any webparts."),
		Localizable(true),
		PersistenceMode(PersistenceMode.Attribute)
		]
		public virtual string EmptyZoneText
		{
			get
			{
				this.EnsureChildControls();
				return this.emptyZoneMessage.Text;
			}
			set
			{
				this.EnsureChildControls();
				this.emptyZoneMessage.Text = value;
			}
		}

		/// <summary>
		/// Gets or sets the title of the zone.
		/// </summary>
		[
		Category("Appearance"),
		DefaultValue(""),
		Description("Gets or sets the title of the zone."),
		Localizable(true),
		PersistenceMode(PersistenceMode.Attribute)
		]
		public virtual string Title
		{
			get
			{
				return ViewStateUtility.GetViewStateValue<string>(this.ViewState, "Title", String.Empty);
			}
			set
			{
				this.ViewState["Title"] = value;
			}
		}

		/// <summary>
		/// Gets the title style.
		/// </summary>
		[
		Category("Styles"),
		Description("Gets the title style."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		PersistenceMode(PersistenceMode.Attribute | PersistenceMode.InnerProperty)
		]
		public Style TitleStyle
		{
			get
			{
				if (this.titleStyle == null)
				{
					this.titleStyle = new Style();
					if (this.IsTrackingViewState)
					{
						((IStateManager)this.titleStyle).TrackViewState();
					}
				}
				return this.titleStyle;
			}
		}

		/// <summary>
		/// Gets or sets the orientation of the webpart panels in this zone.
		/// </summary>
		[
		Category("Behavior"),
		Description("Gets or sets the orientation of the webpart panels in this zone."),
		DefaultValue(Orientation.Vertical),
		PersistenceMode(PersistenceMode.Attribute)
		]
		public Orientation Orientation
		{
			get
			{
				return this.orientation;
			}
			set
			{
				if (value < Orientation.Horizontal || value > Orientation.Vertical)
					throw new ArgumentOutOfRangeException("value");

				this.orientation = value;
			}
		}

		/// <summary>
		/// Gets the style of the empty zone text.
		/// </summary>
		[
		Category("Styles"),
		Description("Gets the style of the empty zone text."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		PersistenceMode(PersistenceMode.Attribute | PersistenceMode.InnerProperty),
		NotifyParentProperty(true)
		]
		public virtual Style EmptyZoneTextStyle
		{
			get
			{
				this.EnsureChildControls();
				return this.emptyZoneMessage.ControlStyle;
			}
		}

		/// <summary>
		/// Gets or sets the template of the zone.
		/// </summary>
		[
		Browsable(false),
		Description("Gets or sets the template of the zone."),
		PersistenceMode(PersistenceMode.InnerProperty),
		TemplateInstance(TemplateInstance.Single),
		TemplateContainer(typeof(WebPartZone))
		]
		public virtual ITemplate ZoneTemplate
		{
			get
			{
				return this.zoneTemplate;
			}
			set
			{
				this.zoneTemplate = value;

				if (this.DesignMode)
				{
					this.CreateDesignModeChildControls();
				}
			}
		}

		/// <summary>
		/// Overrides <see cref="System.Web.UI.Control.Controls"/> to hide the property from editors.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override ControlCollection Controls
		{
			get
			{
				return base.Controls;
			}
		}

		/// <summary>
		/// Gets the webpart panels.
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public WebPartPanelCollection WebPartPanels
		{
			get
			{
				if (base.DesignMode)
				{
					return new WebPartPanelCollection(this.Controls);
				}

				if (this.WebPartManager != null)
				{
					return this.WebPartManager.GetWebPartsForZone(this);
				}

				return new WebPartPanelCollection();
			}
		}

		/// <summary>
		/// Gets the webpart manager of the zone.
		/// </summary>
		protected WebPartManager WebPartManager
		{
			get
			{
				if (this.webPartManager == null)
				{
					if (this.DesignMode)
						return null;

					this.webPartManager = WebPartManager.GetCurrentWebPartManager(this.Page);
				}
				return this.webPartManager;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.WebParts.WebPartZone"/> class.
		/// </summary>
		public WebPartZone()
		{
			//
		}

		/// <summary>
		/// Gets the initial collection of webpart panels.
		/// </summary>
		/// <returns></returns>
		protected internal virtual WebPartPanelCollection GetInitialWebPartPanels()
		{
			WebPartPanelCollection panels = new WebPartPanelCollection();

			if (this.zoneTemplate != null)
			{
				// Create the control which contains the template.
				NonParentingControl templatedControl = new NonParentingControl();
				this.zoneTemplate.InstantiateIn(templatedControl);

				// Don't do anything with an empty template.
				if (!templatedControl.HasControls())
				{
					return panels;
				}

				WebPartPanel panel;
				int zoneIndexCounter = 0;
				foreach (Control childControl in templatedControl.Controls)
				{
					panel = this.CreateWebPartPanel(childControl);
					if (panel != null)
					{
						panel.ZoneIndex = (zoneIndexCounter++);
						panels.Add(panel);
					}
				}
			}

			return panels;
		}

		/// <summary>
		/// Creates a webpart panel for the specified control.
		/// </summary>
		/// <param name="childControl"></param>
		/// <returns></returns>
		private WebPartPanel CreateWebPartPanel(Control childControl)
		{
			WebPartPanel panel = childControl as WebPartPanel;

			// Create a webpart panel for the child control if it is not a webpart panel.
			if (panel == null && (!(childControl is LiteralControl)))
			{
				panel = this.WebPartManager.CreateWebPartPanel(childControl);
			}

			if (panel != null)
			{
				panel.ZoneID = this.ID;
			}

			return panel;
		}

		/// <summary>
		/// Creates a webpart panel renderer.
		/// </summary>
		/// <returns></returns>
		protected virtual WebPartPanelRenderer CreateWebPartPanelRenderer()
		{
			return new WebPartPanelRenderer(this);
		}

		/// <summary>
		/// Creates the child controls for the design mode.
		/// </summary>
		private void CreateDesignModeChildControls()
		{
			foreach (WebPartPanel panel in this.GetInitialWebPartPanels())
			{
				this.Controls.Add(panel);
			}
		}

		/// <summary>
		/// Notifies server controls that use composition-based implementation to create any 
		/// child controls they contain in preparation for posting back or rendering.
		/// </summary>
		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			this.emptyZoneMessage = new Label();
			this.emptyZoneMessage.ID = "message";
		}

		/// <summary>
		/// Creates the control collection.
		/// </summary>
		/// <returns></returns>
		protected override ControlCollection CreateControlCollection()
		{
			if (base.DesignMode)
			{
				return new ControlCollection(this);
			}
			return new EmptyControlCollection(this);
		}

		/// <summary>
		/// Raises the <see cref="System.Web.UI.Control.Init"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			if (this.DesignMode || (this.Page == null))
				return;

			if (this.WebPartManager == null)
				throw new InvalidOperationException("A webpart manager is required.");

			this.webPartManager.RegisterZone(this);
			this.Page.RegisterRequiresControlState(this);
		}

		/// <summary>
		/// Adds the attributes to render.
		/// </summary>
		/// <param name="writer"></param>
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			base.AddAttributesToRender(writer);

			writer.AddAttribute(HtmlTextWriterAttribute.Name, "webPartZone");
			writer.AddAttribute("zoneID", this.ID);

			if (WebPartManager.SupportsClientSideDragDrop(this.Page))
			{
				writer.AddAttribute("orientation", this.orientation.ToString());
				writer.AddAttribute("ondragenter", "MoveWebPartDragZoneEnter(this);");
				writer.AddAttribute("ondragover", "MoveWebPartStopEventBubble();");
			}
		}

		/// <summary>
		/// Renders the begin tag.
		/// </summary>
		/// <param name="writer"></param>
		public override void RenderBeginTag(HtmlTextWriter writer)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
			writer.RenderBeginTag(HtmlTextWriterTag.Table);

			// Render the zone's title.
			if (this.Title.Length > 0)
			{
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);

				this.TitleStyle.AddAttributesToRender(writer);
				writer.RenderBeginTag(HtmlTextWriterTag.Td);

				writer.Write(this.Title);

				writer.RenderEndTag();
				writer.RenderEndTag();
			}

			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);

			this.AddAttributesToRender(writer);
			writer.RenderBeginTag(HtmlTextWriterTag.Table);
		}

		/// <summary>
		/// Renders the end tag.
		/// </summary>
		/// <param name="writer"></param>
		public override void RenderEndTag(HtmlTextWriter writer)
		{
			writer.RenderEndTag();
			writer.RenderEndTag();
			writer.RenderEndTag();
			writer.RenderEndTag();
		}

		/// <summary>
		/// Renders the child controls.
		/// </summary>
		/// <param name="writer"></param>
		protected override void RenderChildren(HtmlTextWriter writer)
		{
			WebPartPanelRenderer renderer = this.CreateWebPartPanelRenderer();
			if (renderer == null)
				throw new InvalidOperationException("No webpart panel renderer specified.");

			WebPartPanelCollection panels = this.WebPartPanels;

			bool supportsClientDragDrop = WebPartManager.SupportsClientSideDragDrop(this.Page);

			bool horizontal = (this.orientation == Orientation.Horizontal);

			if (horizontal)
			{
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			}

			// Render the panels.
			foreach (WebPartPanel panel in panels)
			{
				if (!horizontal)
				{
					writer.RenderBeginTag(HtmlTextWriterTag.Tr);
				}

				writer.AddAttribute(HtmlTextWriterAttribute.Id, panel.ContainerClientID);
				writer.AddAttribute("relatedWebPart", panel.ClientID);
				if (supportsClientDragDrop)
				{
					writer.AddAttribute("orientation", this.Orientation.ToString());
					writer.AddAttribute("ondragenter", "MoveWebPartDragEnter(this);");
					writer.AddAttribute("ondragend", "MoveWebPart(this, dropLocation);");
					writer.AddAttribute("ondragover", "MoveWebPartDragOver(this, true);");
				}

				writer.RenderBeginTag(HtmlTextWriterTag.Td);

				renderer.RenderWebPartPanel(writer, panel);

				writer.RenderEndTag();

				if (!horizontal)
				{
					writer.RenderEndTag();
				}
			}

			if (!horizontal)
			{
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			}

			// Render the control which will be displayed when there are no panels in the zone.
			if (WebPartManager.SupportsClientSideDragDrop(this.Page))
			{
				writer.AddAttribute("ondragenter", "MoveWebPartDragEnter(this);");
				writer.AddAttribute("ondragover", "MoveWebPartDragOver(this, 'False');");
				writer.AddAttribute("orientation", this.Orientation.ToString());
			}

			writer.RenderBeginTag(HtmlTextWriterTag.Td);

			// Render the message which will be made visible when no panels are placed inside this zone.
			this.emptyZoneMessage.Attributes["name"] = "emptyWebPartZone";
			this.emptyZoneMessage.Attributes["webPartsInZone"] = this.WebPartPanels.Count.ToString();
			if (this.WebPartPanels.Count > 0)
				this.emptyZoneMessage.Style.Add(HtmlTextWriterStyle.Display, "none");

			this.emptyZoneMessage.RenderControl(writer);

			writer.RenderEndTag();
			writer.RenderEndTag();
		}

		#region Control state members.
		/// <summary>
		/// Restores control state information from a previous page request that was
		/// saved by the <see cref="System.Web.UI.Control.SaveControlState"/> method.
		/// </summary>
		/// <param name="savedState">
		/// An <see cref="System.Object"/> that represents the control state to be restored.
		/// </param>
		protected override void LoadControlState(object savedState)
		{
			object[] controlState = (object[])savedState;
			base.LoadControlState(controlState[0]);
			this.orientation = ((bool)controlState[1]) ? Orientation.Horizontal : Orientation.Vertical;
		}

		/// <summary>
		/// Saves any server control state changes that have occurred since the time the
		/// page was posted back to the server.
		/// </summary>
		/// <returns>
		/// Returns the server control's current state. If there is no state associated
		/// with the control, this method returns null.
		/// </returns>
		protected override object SaveControlState()
		{
			object baseControlState = base.SaveControlState();
			object[] controlState = new object[2] { baseControlState, (this.orientation == Orientation.Horizontal) };
			return controlState;
		}
		#endregion
	}
}