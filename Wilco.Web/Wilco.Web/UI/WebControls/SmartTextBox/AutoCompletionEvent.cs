using System;

namespace Wilco.Web.UI.WebControls
{
	/// <summary>
	/// Represents the method that handles auto complete events.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate AutoCompleteSuggestion[] AutoCompleteEventHandler(object sender, AutoCompleteEventArgs e);

	/// <summary>
	/// Represents the event arguments for auto complete events.
	/// </summary>
	[Serializable]
	public sealed class AutoCompleteEventArgs : EventArgs
	{
		private string text;

		/// <summary>
		/// Gets the text.
		/// </summary>
		/// <value></value>
		public string Text
		{
			get
			{
				return this.text;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.AutoCompleteEventArgs"/> class.
		/// </summary>
		/// <param name="text"></param>
		public AutoCompleteEventArgs(string text)
		{
			this.text = text;
		}
	}
}