using System;

namespace Wilco.Web.UI.WebControls.WebParts
{
	/// <summary>
	/// Represents a method which handles the 
	/// <see cref="Wilco.Web.UI.WebControls.WebParts.WebPartPanel.Moved"/> event.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void WebPartPanelMovedEventHandler(object sender, WebPartPanelMovedEventArgs e);

	/// <summary>
	/// Represents event data.
	/// </summary>
	[Serializable]
	public sealed class WebPartPanelMovedEventArgs : WebPartPanelEventArgs
	{
		private string previousZoneID;
		private int previousZoneIndex;

		/// <summary>
		/// Gets the ID of the zone from which the panel was moved.
		/// </summary>
		public string PreviousZoneID
		{
			get
			{
				return this.previousZoneID;
			}
		}

		/// <summary>
		/// Gets the previous zone index.
		/// </summary>
		public int PreviousZoneIndex
		{
			get
			{
				return this.previousZoneIndex;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.WebParts.WebPartPanelMovedEventArgs"/> class.
		/// </summary>
		/// <param name="commandName"></param>
		/// <param name="panel"></param>
		/// <param name="previousZoneID"></param>
		/// <param name="previousZoneIndex"></param>
		public WebPartPanelMovedEventArgs(string commandName, WebPartPanel panel, string previousZoneID, int previousZoneIndex) : base(commandName, panel)
		{
			this.previousZoneID = previousZoneID;
			this.previousZoneIndex = previousZoneIndex;
		}
	}
}