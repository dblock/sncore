using System;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace Wilco.Web.UI.WebControls
{
	/// <summary>
	/// Represents a repeater item with support for hierarchical data.
	/// </summary>
	public class HierarchicalRepeaterItem : RepeaterItem
	{
		private HierarchicalRepeater owner;
		private Division childPanel;

		/// <summary>
		/// Gets the number of child items.
		/// </summary>
		internal int ItemCount
		{
			get
			{
				return ViewStateUtility.GetViewStateValue<int>(this.ViewState, "_!ItemCount", -1);
			}
			set
			{
				this.ViewState["_!ItemCount"] = value;
			}
		}

		/// <summary>
		/// Gets the child panel.
		/// </summary>
		public Division ChildPanel
		{
			get
			{
				this.EnsureChildControls();
				return this.childPanel;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.HierarchicalRepeaterItem"/> class.
		/// </summary>
		/// <param name="owner"></param>
		/// <param name="itemIndex"></param>
		/// <param name="itemType"></param>
		public HierarchicalRepeaterItem(HierarchicalRepeater owner, int itemIndex, ListItemType itemType)
			: base(itemIndex, itemType)
		{
			this.owner = owner;
		}

		/// <summary>
		/// Creates the child controls.
		/// </summary>
		protected override void CreateChildControls()
		{
			base.CreateChildControls();
			this.childPanel = new Division();
			this.Controls.Add(this.childPanel);
			if (this.ItemCount > -1 && this.childPanel.Visible)
			{
				this.owner.CreateChildHierarchy(this);
			}
		}

		/// <summary>
		/// Raises the <see cref="Control.Load"/> event and ensures that the child control hierarchy is loaded.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			if (this.ItemCount > 0 && this.ChildPanel.Controls.Count == 0)
			{
				this.owner.CreateChildHierarchy(this);
			}

			base.OnLoad(e);
		}

		/// <summary>
		/// Bubbles an event to the parent control.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="e"></param>
		/// <returns></returns>
		protected override bool OnBubbleEvent(object source, EventArgs e)
		{
			if (e is RepeaterCommandEventArgs)
			{
				base.RaiseBubbleEvent(source, e);
				return true;
			}
			return base.OnBubbleEvent(source, e);
		}

		/// <summary>
		/// Renders the child controls.
		/// </summary>
		/// <param name="writer"></param>
		protected override void RenderChildren(HtmlTextWriter writer)
		{
			if (this.ChildPanel.Controls.Count == 0)
				this.ChildPanel.Visible = false;

			base.RenderChildren(writer);
		}
	}
}