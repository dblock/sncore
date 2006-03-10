using System;
using System.Web.UI;

namespace Wilco.Web.UI.WebControls.WebParts
{
	/// <summary>
	/// Represents a control which does not notify its parents about any control replacements.
	/// </summary>
	internal sealed class NonParentingControl : Control
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.WebParts.NonParentingControl"/> class.
		/// </summary>
		public NonParentingControl()
		{
			//
		}

		/// <summary>
		/// The control was added to another control.
		/// </summary>
		/// <param name="control"></param>
		/// <param name="index"></param>
		protected override void AddedControl(Control control, int index)
		{
			//
		}

		/// <summary>
		/// The control was removed from another control.
		/// </summary>
		/// <param name="control"></param>
		protected override void RemovedControl(Control control)
		{
			//
		}
	}
}