using System;
using System.Web.UI;

namespace Wilco.Web.UI.WebControls
{
	/// <summary>
	/// Provides a delegate which represents template related events.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void TemplateEventHandler(object sender, TemplateEventArgs e);

	/// <summary>
	/// Represents event data for template related event handlers.
	/// </summary>
	[Serializable]
	public sealed class TemplateEventArgs : EventArgs
	{
		private Control container;

		/// <summary>
		/// Gets the container.
		/// </summary>
		public Control Container
		{
			get
			{
				return this.container;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.TemplateEventArgs"/> class.
		/// </summary>
		/// <param name="container"></param>
		public TemplateEventArgs(Control container)
		{
			this.container = container;
		}
	}
}