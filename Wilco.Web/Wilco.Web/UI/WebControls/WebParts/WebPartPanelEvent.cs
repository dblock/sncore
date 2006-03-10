using System;

namespace Wilco.Web.UI.WebControls.WebParts
{
	/// <summary>
	/// Represents a method which handles the 
	/// <see cref="Wilco.Web.UI.WebControls.WebParts.WebPartPanel.FrameStateChanged"/> event.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void WebPartPanelEventHandler(object sender, WebPartPanelEventArgs e);

	/// <summary>
	/// Represents event data.
	/// </summary>
	[Serializable]
	public class WebPartPanelEventArgs : EventArgs
	{
		private string commandName;
		private WebPartPanel panel;

		/// <summary>
		/// Gets the command name.
		/// </summary>
		public string CommandName
		{
			get
			{
				return this.commandName;
			}
		}

		/// <summary>
		/// Gets the webpart panel.
		/// </summary>
		public WebPartPanel Panel
		{
			get
			{
				return this.panel;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.WebParts.WebPartPanelEventArgs"/> class.
		/// </summary>
		/// <param name="commandName">The command name.</param>
		/// <param name="panel"></param>
		public WebPartPanelEventArgs(string commandName, WebPartPanel panel)
		{
			this.commandName = commandName;
			this.panel = panel;
		}
	}
}