using System;

namespace Wilco.Web.UI.WebControls
{
	/// <summary>
	/// Represents a suggestion for the auto completion.
	/// </summary>
	public sealed class AutoCompleteSuggestion
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
		/// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.AutoCompleteSuggestion"/> class.
		/// </summary>
		public AutoCompleteSuggestion(string text)
		{
			this.text = text;
		}
	}
}