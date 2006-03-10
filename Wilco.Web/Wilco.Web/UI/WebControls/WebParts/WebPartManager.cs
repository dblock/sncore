using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web.UI;

namespace Wilco.Web.UI.WebControls.WebParts
{
	/// <summary>
	/// Represents a webpart manager.
	/// </summary>
	[Bindable(false)]
	[NonVisualControl(true)]
	[ParseChildren(true)]
	[PersistChildren(false)]
	public class WebPartManager : Control, INamingContainer
	{
		private IDictionary<string, WebPartZone> zoneIDs;
		private IDictionary<string, WebPartPanel> webPartPanelIDs;
		private WebPartZoneCollection zones;
		private bool pageLoadComplete;
		private static readonly object EventWebPartPanelCommand = new object();
		private static readonly object EventWebPartPanelFrameStateChanged = new object();
		private static readonly object EventWebPartPanelMoved = new object();
		private static readonly object EventWebPartPanelRemoved = new object();

		/// <summary>
		/// Occurs when a webpart panel command is activated.
		/// </summary>
		[Description("Occurs when a webpart panel command is activated.")]
		public event WebPartPanelEventHandler WebPartPanelCommand
		{
			add
			{
				this.Events.AddHandler(WebPartManager.EventWebPartPanelCommand, value);
			}
			remove
			{
				this.Events.RemoveHandler(WebPartManager.EventWebPartPanelCommand, value);
			}
		}

		/// <summary>
		/// Occurs when a webpart panel's frame state is changed.
		/// </summary>
		[Description("Occurs when a webpart panel's frame state is changed.")]
		public event WebPartPanelEventHandler WebPartPanelFrameStateChanged
		{
			add
			{
				this.Events.AddHandler(WebPartManager.EventWebPartPanelFrameStateChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(WebPartManager.EventWebPartPanelFrameStateChanged, value);
			}
		}

		/// <summary>
		/// Occurs when a webpart panel is moved.
		/// </summary>
		[Description("Occurs when a webpart panel is moved.")]
		public event WebPartPanelMovedEventHandler WebPartPanelMoved
		{
			add
			{
				this.Events.AddHandler(WebPartManager.EventWebPartPanelMoved, value);
			}
			remove
			{
				this.Events.RemoveHandler(WebPartManager.EventWebPartPanelMoved, value);
			}
		}

		/// <summary>
		/// Occurs when a webpart panel is removed.
		/// </summary>
		[Description("Occurs when a webpart panel is removed.")]
		public event WebPartPanelEventHandler WebPartPanelRemoved
		{
			add
			{
				this.Events.AddHandler(WebPartManager.EventWebPartPanelRemoved, value);
			}
			remove
			{
				this.Events.RemoveHandler(WebPartManager.EventWebPartPanelRemoved, value);
			}
		}

		/// <summary>
		/// Gets or sets the CSS class of the drag object.
		/// </summary>
		[
		Category("Appearance"), 
		Description("Gets or sets the CSS class of the drag object."),
		PersistenceMode(PersistenceMode.Attribute)
		]
		public string DragObjectCssClass
		{
			get
			{
				return ViewStateUtility.GetViewStateValue<string>(this.ViewState, "DragObjectCssClass", String.Empty);
			}
			set
			{
				this.ViewState["DragObjectCssClass"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the CSS class of the splitter object.
		/// </summary>
		[
		Category("Appearance"), 
		Description("Gets or sets the CSS class of the splitter object."),
		PersistenceMode(PersistenceMode.Attribute)
		]
		public string SplitterObjectCssClass
		{
			get
			{
				return ViewStateUtility.GetViewStateValue<string>(this.ViewState, "SplitterObjectCssClass", String.Empty);
			}
			set
			{
				this.ViewState["SplitterObjectCssClass"] = value;
			}
		}

		/// <summary>
		/// Gets the collection of child controls.
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
		/// Gets the collection of zones.
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public WebPartZoneCollection Zones
		{
			get
			{
				return this.zones;
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
				if (this.HasControls())
				{
					return new WebPartPanelCollection(this.Controls);
				}

				return new WebPartPanelCollection();
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.WebParts.WebPartManager"/> class.
		/// </summary>
		public WebPartManager()
		{
			this.zones = new WebPartZoneCollection();
			this.zoneIDs = new Dictionary<string, WebPartZone>();
			this.webPartPanelIDs = new Dictionary<string, WebPartPanel>();
		}

		/// <summary>
		/// Checks whether the client supports client side drag and drop.
		/// </summary>
		/// <param name="page"></param>
		/// <returns></returns>
		public static bool SupportsClientSideDragDrop(Page page)
		{
			if (page == null)
				return false;

			return page.Request.Browser.IsBrowser("IE");
		}

		/// <summary>
		/// Gets the current webpart manager on the specified page.
		/// </summary>
		/// <param name="page"></param>
		/// <returns></returns>
		public static WebPartManager GetCurrentWebPartManager(Page page)
		{
			if (page == null)
				throw new ArgumentNullException("page");

			return (page.Items[typeof(WebPartManager)] as WebPartManager);
		}

		/// <summary>
		/// Creates a generic webpart panel for the specified control.
		/// </summary>
		/// <param name="childControl"></param>
		/// <returns></returns>
		public WebPartPanel CreateWebPartPanel(Control childControl)
		{
			WebPartPanel panel = new WebPartPanel();
			panel.PanelTemplate = new GenericTemplate(new TemplateEventHandler(delegate(object sender, TemplateEventArgs e)
			{
				e.Container.Controls.Add(childControl);
			}));
			panel.ID = childControl.ID;
			panel.Caption = "Generic panel for " + childControl.ID;

			// TODO: Implement some logic to determine what caption to display.
			return panel;
		}

		/// <summary>
		/// Programmatically adds a webpart panel to a zone at the specified index.
		/// </summary>
		/// <param name="panel"></param>
		/// <param name="zone"></param>
		/// <param name="zoneIndex"></param>
		public void AddWebPartPanel(WebPartPanel panel, WebPartZone zone, int zoneIndex)
		{
			((WebPartManagerControlCollection)this.Controls).AddWebPartPanel(panel);

			// Only use the move procedure to set the panel's zone ID and zone index if it isn't 
			// already set. It could be set if this is a postback, and the panel was added in a 
			// previous request. If we would re-add the panel, the panel would be placed at the 
			// original specified position.
			if (panel.ZoneID == null && panel.ZoneIndex < 0)
				this.MoveWebPartPanel(panel, zone, zoneIndex);
		}

		/// <summary>
		/// Moves a webpart panel to the specified zone at the specified index.
		/// </summary>
		/// <param name="panel"></param>
		/// <param name="zone"></param>
		/// <param name="zoneIndex"></param>
		public void MoveWebPartPanel(WebPartPanel panel, WebPartZone zone, int zoneIndex)
		{
			WebPartPanelCollection panels = zone.WebPartPanels;
			if (zoneIndex < 0)
				throw new ArgumentOutOfRangeException("zoneIndex");

			// Update the indices of the webpart panels which are placed beneath the moved panel.
			for (int i = zoneIndex; i < panels.Count; i++)
				panels[i].ZoneIndex++;

			panel.ZoneID = zone.ID;
			panel.ZoneIndex = zoneIndex;
		}

		/// <summary>
		/// Removes a webpart panel from the page.
		/// </summary>
		/// <param name="panel">The panel to remove.</param>
		public void RemoveWebPartPanel(WebPartPanel panel)
		{
			panel.IsRemoved = true;
		}

		/// <summary>
		/// Creates the control collection.
		/// </summary>
		/// <returns></returns>
		protected override ControlCollection CreateControlCollection()
		{
			return new WebPartManager.WebPartManagerControlCollection(this);
		}

		/// <summary>
		/// Gets the webparts for the specified zone.
		/// </summary>
		/// <param name="zone"></param>
		/// <returns></returns>
		internal WebPartPanelCollection GetWebPartsForZone(WebPartZone zone)
		{
			WebPartPanelCollection panels = new WebPartPanelCollection();

			// Get the webpart panel's in the zone sorted by the the zone index.
			SortedList<WebPartPanel, object> sortedPanels = new SortedList<WebPartPanel, object>(new WebPartManager.WebPartPanelZoneIndexComparer());
			foreach (WebPartPanel panel in this.Controls)
			{
				if (panel.ZoneID == zone.ID && !panel.IsRemoved)
				{
					sortedPanels.Add(panel, null);
				}
			}

			// Re-organize the zone indices, starting from 0.
			for (int i = 0; i < sortedPanels.Keys.Count; i++)
			{
				sortedPanels.Keys[i].ZoneIndex = i;
				panels.Add(sortedPanels.Keys[i]);
			}

			return panels;
		}

		/// <summary>
		/// Registers a webpart zone.
		/// </summary>
		/// <param name="zone"></param>
		internal void RegisterZone(WebPartZone zone)
		{
			if (this.pageLoadComplete)
				throw new InvalidOperationException("Zones must be registered before the completion of the page initialization.");

			if (String.IsNullOrEmpty(zone.ID))
				throw new ArgumentException("The zone does not have an ID.");

			if (this.zoneIDs.ContainsKey(zone.ID))
				throw new ArgumentException("A zone with the same ID was already added.");

			if (this.zones.Contains(zone))
				throw new ArgumentException("The zone was already registered.");

			this.zoneIDs.Add(zone.ID, zone);
			this.zones.Add(zone);

			((WebPartManagerControlCollection)this.Controls).AddWebPartPanelsFromZone(zone);
		}

		/// <summary>
		/// Registers a webpart panel.
		/// </summary>
		/// <param name="panel"></param>
		internal void RegisterWebPartPanel(WebPartPanel panel)
		{
			if (this.pageLoadComplete)
				throw new InvalidOperationException("Webpart panels must be registered before the completion of the page load.");

			if (String.IsNullOrEmpty(panel.ID))
				throw new ArgumentException("The webpart panel does not have an ID.");

			if (this.webPartPanelIDs.ContainsKey(panel.ID))
				throw new ArgumentException("A webpart panel with the same ID already exists.");

			this.webPartPanelIDs.Add(panel.ID, panel);
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

			// Register the webpart manager.
			Type managerType = typeof(WebPartManager);
			WebPartManager manager = this.Page.Items[managerType] as WebPartManager;
			if (manager != null)
			{
				throw new InvalidOperationException("A page can only have one webpart manager.");
			}

			this.Page.Items[managerType] = this;
			this.Page.LoadComplete += new EventHandler(this.Page_LoadComplete);
		}

		/// <summary>
		/// Raises the <see cref="System.Web.UI.Control.PreRender"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			this.RegisterClientScript();
		}

		/// <summary>
		/// Renders the control.
		/// </summary>
		/// <param name="writer"></param>
		protected override void Render(HtmlTextWriter writer)
		{
			// There is nothing to render.
		}

		/// <summary>
		/// Registers the client-side script.
		/// </summary>
		private void RegisterClientScript()
		{
			ClientScriptManager csm = this.Page.ClientScript;

			// Register a hidden field which will hold the changes made on the clientside.
			csm.RegisterHiddenField("webPartLayoutChanges", String.Empty);

			// Register the webpart manager script.
			csm.RegisterClientScriptResource(this.GetType(), "Wilco.Web.Resources.WebPartManager.js");

			// Register the initialization script.
			StringBuilder script = new StringBuilder();
			script.Append("var webPartPageFormName = document.forms[0].id;");
			script.AppendFormat("WebPartSetupLayoutFlags('{0}', '{1}');\r\n", this.DragObjectCssClass, this.SplitterObjectCssClass);
			csm.RegisterStartupScript(this.GetType(), "WebPartManager_Init", script.ToString(), true);

			StringBuilder zoneArray = new StringBuilder();
			for (int i = 0; i < this.zones.Count; i++)
			{
				if (i > 0)
					zoneArray.Append(',');
				zoneArray.AppendFormat("new Array('{0}', '{1}', '{2}')", this.zones[i].ClientID, this.zones[i].CssClass, this.zones[i].DragCssClass);
			}

			csm.RegisterArrayDeclaration("webPartZones", zoneArray.ToString());
		}

		/// <summary>
		/// Determines whether the event for the server control is passed up the page's
		/// UI server control hierarchy.
		/// </summary>
		/// <param name="source">The source of the event.</param>
		/// <param name="args">An <see cref="System.EventArgs"/> object that contains the event data.</param>
		/// <returns>true if the event has been canceled; otherwise, false. The default is false.</returns>
		protected override bool OnBubbleEvent(object source, EventArgs args)
		{
			bool cancel = false;

			WebPartPanelEventArgs webPartPanelArgs = args as WebPartPanelEventArgs;
			if (webPartPanelArgs != null)
			{
				switch (webPartPanelArgs.CommandName)
				{
					case "delete":
						this.OnWebPartPanelRemoved(webPartPanelArgs);
						break;
					case "minimize":
						this.OnWebPartPanelFrameStateChanged(webPartPanelArgs);
						break;
					case "move":
						WebPartPanelMovedEventArgs moveArgs = webPartPanelArgs as WebPartPanelMovedEventArgs;
						if (moveArgs == null)
							throw new InvalidOperationException("Commands with the command name 'move' should be be of the type 'WebPartPanelMovedEventArgs'.");
						this.OnWebPartPanelMoved(moveArgs);
						break;
					default:
						this.OnWebPartPanelCommand(webPartPanelArgs);
						break;
				}

				cancel = true;
			}

			return cancel;
		}

		/// <summary>
		/// Raises the <see cref="Wilco.Web.UI.WebControls.WebParts.WebPartManager.WebPartPanelCommand"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnWebPartPanelCommand(WebPartPanelEventArgs e)
		{
			WebPartPanelEventHandler handler = this.Events[WebPartManager.EventWebPartPanelCommand] as WebPartPanelEventHandler;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		/// <summary>
		/// Raises the <see cref="Wilco.Web.UI.WebControls.WebParts.WebPartManager.WebPartPanelFrameStateChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnWebPartPanelFrameStateChanged(WebPartPanelEventArgs e)
		{
			WebPartPanelEventHandler handler = this.Events[WebPartManager.EventWebPartPanelFrameStateChanged] as WebPartPanelEventHandler;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		/// <summary>
		/// Raises the <see cref="Wilco.Web.UI.WebControls.WebParts.WebPartManager.WebPartPanelMoved"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnWebPartPanelMoved(WebPartPanelMovedEventArgs e)
		{
			WebPartPanelMovedEventHandler handler = this.Events[WebPartManager.EventWebPartPanelMoved] as WebPartPanelMovedEventHandler;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		/// <summary>
		/// Raises the <see cref="Wilco.Web.UI.WebControls.WebParts.WebPartManager.WebPartPanelRemoved"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnWebPartPanelRemoved(WebPartPanelEventArgs e)
		{
			WebPartPanelEventHandler handler = this.Events[WebPartManager.EventWebPartPanelRemoved] as WebPartPanelEventHandler;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		/// <summary>
		/// Handles the page's init complete event by setting a flag that the page load has been completed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Page_LoadComplete(object sender, EventArgs e)
		{
			this.pageLoadComplete = true;
		}

		#region Inner classes.
		/// <summary>
		/// Represents a collection of controls for a webpart manager.
		/// </summary>
		private class WebPartManagerControlCollection : ControlCollection
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.WebParts.WebPartManager.WebPartManagerControlCollection"/> class.
			/// </summary>
			/// <param name="manager"></param>
			public WebPartManagerControlCollection(WebPartManager manager)
				: base(manager)
			{
				//
			}

			/// <summary>
			/// Adds a child control to the collection.
			/// </summary>
			/// <param name="child"></param>
			public override void Add(Control child)
			{
				throw new InvalidOperationException("Can not add controls to a read-only control collection.");
			}

			/// <summary>
			/// Adds a webpart panel to the collection.
			/// </summary>
			/// <param name="panel"></param>
			internal void AddWebPartPanel(WebPartPanel panel)
			{
				base.Add(panel);
			}

			/// <summary>
			/// Adds the webpart panels in the specified zone to the current collection.
			/// </summary>
			/// <param name="zone"></param>
			internal void AddWebPartPanelsFromZone(WebPartZone zone)
			{
				WebPartPanelCollection panels = zone.GetInitialWebPartPanels();
				foreach (WebPartPanel panel in panels)
				{
					base.Add(panel);
				}
			}
		}

		/// <summary>
		/// Represents a comparer which can compare webpart panel zone indices.
		/// </summary>
		private class WebPartPanelZoneIndexComparer : Comparer<WebPartPanel>
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.WebParts.WebPartManager.WebPartPanelZoneIndexComparer"/> class.
			/// </summary>
			public WebPartPanelZoneIndexComparer()
			{
				//
			}

			/// <summary>
			/// Compares 2 webpart panels.
			/// </summary>
			/// <param name="left"></param>
			/// <param name="right"></param>
			/// <returns></returns>
			public override int Compare(WebPartPanel left, WebPartPanel right)
			{
				return left.ZoneIndex - right.ZoneIndex;
			}
		}
		#endregion
	}
}