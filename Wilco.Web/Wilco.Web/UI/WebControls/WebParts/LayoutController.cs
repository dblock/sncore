// TODO: Implement a generic layout controller, which can be used as a replacement for the drag 'n drop feature.

using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Wilco.Web.UI.WebControls.WebParts
{
	/// <summary>
	/// Represents a layout controller for the webpart manager. This control 
	/// will allow a user to control the layout using input controls, instead of 
	/// using drag&amp;drop.
	/// </summary>
	public class LayoutController : CompositeControl
	{
		private WebPartManager webPartManager;
		private DropDownList panels;
		private DropDownList zones;
		private TextBox zoneIndex;
		private Button move;

		/// <summary>
		/// Gets the webpart manager.
		/// </summary>
		protected WebPartManager WebPartManager
		{
			get
			{
				if (this.webPartManager == null)
				{
					this.webPartManager = WebPartManager.GetCurrentWebPartManager(this.Page);
				}

				return this.webPartManager;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.WebParts.LayoutController"/> class.
		/// </summary>
		public LayoutController()
		{
			//
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
		}

		/// <summary>
		/// Raises the <see cref="System.Web.UI.Control.Load"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			this.EnsureChildControls();
			if (!this.Page.IsPostBack)
			{
				this.Bind();
			}
		}

		/// <summary>
		/// Binds the data.
		/// </summary>
		protected virtual void Bind()
		{
			this.panels.DataTextField = "Caption";
			this.panels.DataValueField = "ID";
			this.panels.DataSource = this.WebPartManager.WebPartPanels;
			this.panels.DataBind();

			this.zones.DataTextField = "Title";
			this.zones.DataValueField = "ID";
			this.zones.DataSource = this.WebPartManager.Zones;
			this.zones.DataBind();
		}

		/// <summary>
		/// Notifies server controls that use composition-based implementation to create any 
		/// child controls they contain in preparation for posting back or rendering.
		/// </summary>
		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			this.panels = new DropDownList();
			this.Controls.Add(this.panels);
			this.panels.ID = "panels";

			this.zones = new DropDownList();
			this.Controls.Add(this.zones);
			this.zones.ID = "zones";

			this.zoneIndex = new TextBox();
			this.Controls.Add(this.zoneIndex);
			this.zoneIndex.ID = "zoneIndex";

			this.move = new Button();
			this.Controls.Add(this.move);
			this.move.ID = "move";
			this.move.Text = "Move";
			this.move.Click += new EventHandler(this.move_Click);
		}

		/// <summary>
		/// Handles the move button's click event by moving the selected webpart panel to the specified position.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void move_Click(object sender, EventArgs e)
		{
			if (this.Page.IsValid)
			{
				this.WebPartManager.MoveWebPartPanel(this.WebPartManager.WebPartPanels[this.panels.SelectedValue],
					this.WebPartManager.Zones[this.zones.SelectedValue], Convert.ToInt32(this.zoneIndex.Text));
			}
		}
	}
}