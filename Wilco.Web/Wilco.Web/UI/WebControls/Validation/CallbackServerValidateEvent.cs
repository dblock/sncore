using System;
using System.Web.UI.WebControls;

namespace Wilco.Web.UI.WebControls
{
	/// <summary>
	/// Represents the method which handles the <see cref="Wilco.Web.UI.WebControls.CallbackValidator.ServerValidate"/> event.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void CallbackServerValidateEventHandler(object sender, CallbackServerValidateEventArgs e);

	/// <summary>
	/// Represents event data.
	/// </summary>
	public class CallbackServerValidateEventArgs : ServerValidateEventArgs
	{
		private string errorMessage;

		/// <summary>
		/// Gets or sets the error message.
		/// </summary>
		public string ErrorMessage
		{
			get
			{
				return this.errorMessage;
			}
			set
			{
				this.errorMessage = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.CallbackServerValidateEventArgs"/> class.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="isValid"></param>
		/// <param name="errorMessage"></param>
		public CallbackServerValidateEventArgs(string value, bool isValid, string errorMessage) : base(value, isValid)
		{
			this.errorMessage = errorMessage;
		}
	}
}