using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Wilco.Web.UI.WebControls.WebParts
{
	/// <summary>
	/// Represents a webpart panel.
	/// </summary>
	[
	ParseChildren(true),
	PersistChildren(false)
	]
	public class WebPartPanel : WebControl, INamingContainer, IPostBackDataHandler
	{
		private WebPartManager webPartManager;
		private WebPartZone zone;
		private string zoneID;
		private int zoneIndex = -1;
		private bool isMinimized;
		private bool isRemoved;
		private string newZoneID;
		private int newZoneIndex;
		private bool newIsRemoved;
		private bool newIsMinimized;
		private Style captionStyle;
		private Style headerStyle;
		private ITemplate panelTemplate;
		private Division verbs;
		private Control container;
		private static readonly object EventRemoved = new object();
		private static readonly object EventMoved = new object();
		private static readonly object EventFrameStateChanged = new object();

		/// <summary>
		/// Occurs when a webpart panel is removed.
		/// </summary>
		[Description("Occurs when a webpart panel is removed.")]
		public event WebPartPanelEventHandler Removed
		{
			add
			{
				this.Events.AddHandler(WebPartPanel.EventRemoved, value);
			}
			remove
			{
				this.Events.RemoveHandler(WebPartPanel.EventRemoved, value);
			}
		}

		/// <summary>
		/// Occurs when a webpart panel is moved.
		/// </summary>
		[Description("Occurs when a webpart panel is moved.")]
		public event WebPartPanelMovedEventHandler Moved
		{
			add
			{
				this.Events.AddHandler(WebPartPanel.EventMoved, value);
			}
			remove
			{
				this.Events.RemoveHandler(WebPartPanel.EventMoved, value);
			}
		}

		/// <summary>
		/// Occurs when a webpart panel's frame state is changed.
		/// </summary>
		[Description("Occurs when a webpart panel's frame state is changed.")]
		public event WebPartPanelEventHandler FrameStateChanged
		{
			add
			{
				this.Events.AddHandler(WebPartPanel.EventFrameStateChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(WebPartPanel.EventFrameStateChanged, value);
			}
		}

		/// <summary>
		/// Gets or sets whether the panel can be deleted.
		/// </summary>
		[
		DefaultValue(true),
		Description("Gets or sets whether the panel can be deleted."),
		PersistenceMode(PersistenceMode.Attribute)
		]
		public bool AllowDelete
		{
			get
			{
				return ViewStateUtility.GetViewStateValue<bool>(this.ViewState, "AllowDelete", true);
			}
			set
			{
				this.ViewState["AllowDelete"] = value;
			}
		}

		/// <summary>
		/// Gets or sets whether the panel can be minimized.
		/// </summary>
		[
		DefaultValue(true),
		Description("Gets or sets whether the panel can be minimized."),
		PersistenceMode(PersistenceMode.Attribute)
		]
		public bool AllowMinimize
		{
			get
			{
				return ViewStateUtility.GetViewStateValue<bool>(this.ViewState, "AllowMinimize", true);
			}
			set
			{
				this.ViewState["AllowMinimize"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the caption of the panel.
		/// </summary>
		[
		Category("Appearance"),
		Description("Gets or sets the caption of the panel."),
		Localizable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		PersistenceMode(PersistenceMode.Attribute)
		]
		public string Caption
		{
			get
			{
				return ViewStateUtility.GetViewStateValue<string>(this.ViewState, "Caption", String.Empty);
			}
			set
			{
				this.ViewState["Caption"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the drag caption of the panel.
		/// </summary>
		[
		Category("Appearance"),
		Description("Gets or sets the drag caption of the panel."),
		Localizable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		PersistenceMode(PersistenceMode.Attribute)
		]
		public string DragCaption
		{
			get
			{
				return ViewStateUtility.GetViewStateValue<string>(this.ViewState, "DragCaption", String.Empty);
			}
			set
			{
				this.ViewState["DragCaption"] = value;
			}
		}

		/// <summary>
		/// Gets the caption style.
		/// </summary>
		[
		Category("Styles"),
		Description("Gets the caption style."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		PersistenceMode(PersistenceMode.Attribute | PersistenceMode.InnerProperty)
		]
		public Style CaptionStyle
		{
			get
			{
				if (this.captionStyle == null)
				{
					this.captionStyle = new Style();
					if (this.IsTrackingViewState)
					{
						((IStateManager)this.captionStyle).TrackViewState();
					}
				}
				return this.captionStyle;
			}
		}

		/// <summary>
		/// Gets the header style.
		/// </summary>
		[
		Category("Styles"),
		Description("Gets the header style."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		PersistenceMode(PersistenceMode.Attribute | PersistenceMode.InnerProperty)
		]
		public Style HeaderStyle
		{
			get
			{
				if (this.headerStyle == null)
				{
					this.headerStyle = new Style();
					if (this.IsTrackingViewState)
					{
						((IStateManager)this.headerStyle).TrackViewState();
					}
				}
				return this.headerStyle;
			}
		}

		/// <summary>
		/// Gets the webpart panel's verbs.
		/// </summary>
		[
		Browsable(false),
		Description("Gets the webpart panel's verbs."),
		PersistenceMode(PersistenceMode.InnerProperty)
		]
		public Division Verbs
		{
			get
			{
				this.EnsureChildControls();
				return this.verbs;
			}
		}

		/// <summary>
		/// Gets or sets the template of the panel.
		/// </summary>
		[
		Browsable(false),
		Description("Gets or sets the template of the panel."),
		PersistenceMode(PersistenceMode.InnerProperty),
		TemplateInstance(TemplateInstance.Single)
		]
		public ITemplate PanelTemplate
		{
			get
			{
				return this.panelTemplate;
			}
			set
			{
				this.panelTemplate = value;

				// Create/recreate the new template.
				this.CreatePanelTemplate();
			}
		}

		/// <summary>
		/// Gets whether the webpart panel is minimized.
		/// </summary>
		[
		Description("Gets whether the webpart panel is minimized."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		]
		public bool IsMinimized
		{
			get
			{
				return this.isMinimized;
			}
			set
			{
				this.isMinimized = value;
			}
		}

		/// <summary>
		/// Gets whether the webpart panel is removed from the page.
		/// </summary>
		[
		Description("Gets whether the webpart panel is removed from the page."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		]
		public bool IsRemoved
		{
			get
			{
				return this.isRemoved;
			}
			internal set
			{
				this.isRemoved = value;
			}
		}

		/// <summary>
		/// Gets the ID of the zone in which the panel is originally placed.
		/// </summary>
		[
		Description("Gets the ID of the zone in which the panel is originally placed."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		]
		public string ZoneID
		{
			get
			{
				return this.zoneID;
			}
			internal set
			{
				this.zoneID = value;
				this.zone = null;
			}
		}

		/// <summary>
		/// Gets the zone index of the panel.
		/// </summary>
		[
		Description("Gets the zone index of the panel."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		]
		public int ZoneIndex
		{
			get
			{
				return this.zoneIndex;
			}
			internal set
			{
				this.zoneIndex = value;
			}
		}

		/// <summary>
		/// Gets the webpart panel's zone.
		/// </summary>
		protected WebPartZone Zone
		{
			get
			{
				if (this.zone == null)
				{
					WebPartManager manager = this.WebPartManager;
					if (manager != null)
					{
						this.zone = this.GetZoneByID(manager.Zones, this.zoneID);
					}
				}

				return this.zone;
			}
		}

		/// <summary>
		/// Gets the webpart manager.
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
		/// Overrides <see cref="Controls"/>.
		/// </summary>
		public override ControlCollection Controls
		{
			get
			{
				this.EnsureChildControls();
				return this.container.Controls;
			}
		}

		/// <summary>
		/// Gets the container's client ID.
		/// </summary>
		protected internal string ContainerClientID
		{
			get
			{
				return "box_" + this.ClientID;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.WebParts.WebPartPanel"/> class.
		/// </summary>
		public WebPartPanel() : base("div")
		{
			//
		}

		/// <summary>
		/// Notifies server controls that use composition-based implementation to create any 
		/// child controls they contain in preparation for posting back or rendering.
		/// </summary>
		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			this.verbs = new Division();
			base.Controls.Add(this.verbs);

			this.container = new Control();
			base.Controls.Add(this.container);
		}

		/// <summary>
		/// Creates the panel template.
		/// </summary>
		protected virtual void CreatePanelTemplate()
		{
			this.EnsureChildControls();

			this.container.Controls.Clear();
			if (this.panelTemplate != null)
			{
				this.panelTemplate.InstantiateIn(this.container);
			}
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

			this.webPartManager.RegisterWebPartPanel(this);
			
			this.Page.RegisterRequiresControlState(this);
		}

		/// <summary>
		/// Raises the <see cref="System.Web.UI.Control.PreRender"/> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> object that contains the event data.</param>
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			this.Page.RegisterRequiresPostBack(this);
		}

		/// <summary>
		/// Adds the attributes to render.
		/// </summary>
		/// <param name="writer"></param>
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			base.AddAttributesToRender(writer);

			writer.AddAttribute("WebPartID", this.ClientID);
			writer.AddAttribute("hasPers", "false");
			writer.AddAttribute("allowDelete", this.AllowDelete.ToString());
			writer.AddAttribute("allowMinimize", this.AllowMinimize.ToString());
		}

		/// <summary>
		/// Renders the control.
		/// </summary>
		/// <param name="writer"></param>
		protected override void Render(HtmlTextWriter writer)
		{
			this.verbs.Visible = false;
			base.Render(writer);
		}

		/// <summary>
		/// Raises the <see cref="Wilco.Web.UI.WebControls.WebParts.WebPartPanel.Removed"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnWebPartPanelRemoved(WebPartPanelEventArgs e)
		{
			WebPartPanelEventHandler handler = this.Events[WebPartPanel.EventRemoved] as WebPartPanelEventHandler;
			if (handler != null)
			{
				handler(this, e);
			}

			// Bubble the event.
			this.RaiseBubbleEvent(this, e);
		}

		/// <summary>
		/// Raises the <see cref="Wilco.Web.UI.WebControls.WebParts.WebPartPanel.Moved"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnWebPartPanelMoved(WebPartPanelMovedEventArgs e)
		{
			WebPartPanelMovedEventHandler handler = this.Events[WebPartPanel.EventMoved] as WebPartPanelMovedEventHandler;
			if (handler != null)
			{
				handler(this, e);
			}

			// Bubble the event.
			this.RaiseBubbleEvent(this, e);
		}

		/// <summary>
		/// Raises the <see cref="Wilco.Web.UI.WebControls.WebParts.WebPartPanel.FrameStateChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnFrameStateChanged(WebPartPanelEventArgs e)
		{
			WebPartPanelEventHandler handler = this.Events[WebPartPanel.EventFrameStateChanged] as WebPartPanelEventHandler;
			if (handler != null)
			{
				handler(this, e);
			}

			// Bubble the event.
			this.RaiseBubbleEvent(this, e);
		}

		#region IPostBackDataHandler members
		/// <summary>
		/// When implemented by a class, processes post back data for an ASP.NET server control.
		/// </summary>
		/// <param name="postDataKey">The key identifier for the control.</param>
		/// <param name="postCollection">The collection of all incoming name values.</param>
		/// <returns>true if the server control's state changes as a result of the post back; otherwise false.</returns>
		bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
		{
			string layoutChanges = postCollection["webPartLayoutChanges"];
			if (layoutChanges != null && layoutChanges.Length > 0)
			{
				string[] parts = layoutChanges.Split('|');
				string[] info;
				for (int i = parts.Length - 1; i >= 0; i--)
				{
					info = parts[i].Split(',');

					if (info[0] == this.ClientID)
					{
						this.newZoneID = this.ZoneID;
						this.newZoneIndex = this.ZoneIndex;
						this.newIsMinimized = this.IsMinimized;
						this.newIsRemoved = this.IsRemoved;

						for (int j = 1; j < info.Length; j++)
						{
							switch (info[j].ToLower())
							{
								case "zoneid":
									this.newZoneID = info[j + 1];
									break;
								case "partorder":
									this.newZoneIndex = Convert.ToInt32(info[j + 1]);
									break;
								case "framestate":
									this.newIsMinimized = info[j + 1] == "1";
									break;
								case "isincluded":
									this.newIsRemoved = true;
									break;
							}
						}

						if (this.newZoneID != this.ZoneID || this.newZoneIndex != this.ZoneIndex 
							|| this.newIsMinimized != this.isMinimized || this.newIsRemoved != this.IsRemoved)
						{
							return true;
						}
						break;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// When implemented by a class, signals the server control object to notify the ASP.NET application that the state of the control has changed.
		/// </summary>
		void IPostBackDataHandler.RaisePostDataChangedEvent()
		{
			WebPartManager manager = this.WebPartManager;
			
			if (this.newIsRemoved != this.IsRemoved)
			{
				manager.RemoveWebPartPanel(this);
				this.OnWebPartPanelRemoved(new WebPartPanelEventArgs("delete", this));

				// Don't raise the other events if the panel is removed.
				return;
			}

			if (this.newZoneID != this.ZoneID || this.newZoneIndex != this.ZoneIndex)
			{
				string previousZoneID = this.ZoneID;
				int previousZoneIndex = this.ZoneIndex;
				if (manager != null)
				{
					manager.MoveWebPartPanel(this, this.GetZoneByID(manager.Zones, this.newZoneID), this.newZoneIndex);
				}
				this.OnWebPartPanelMoved(new WebPartPanelMovedEventArgs("move", this, previousZoneID, previousZoneIndex));
			}

			if (this.newIsMinimized != this.IsMinimized)
			{
				this.IsMinimized = this.newIsMinimized;
				this.OnFrameStateChanged(new WebPartPanelEventArgs("minimize", this));
			}
		}

		/// <summary>
		/// Gets the zone by its ID.
		/// </summary>
		/// <param name="zones"></param>
		/// <param name="zoneID"></param>
		/// <returns></returns>
		private WebPartZone GetZoneByID(WebPartZoneCollection zones, string zoneID)
		{
			foreach (WebPartZone zone in zones)
			{
				if (zone.ID == zoneID)
					return zone;
			}

			throw new InvalidOperationException("The target zone to which the webpart panel was dragged does not exist.");
		}
		#endregion

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
			this.zoneID = (string)controlState[1];
			this.zoneIndex = (int)controlState[2];
			this.isRemoved = (bool)controlState[3];
			this.isMinimized = (bool)controlState[4];
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
			object[] controlState = new object[5] { baseControlState, this.ZoneID, this.ZoneIndex, this.isRemoved, this.isMinimized };
			return controlState;
		}
		#endregion
	}
}