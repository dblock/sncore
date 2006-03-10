using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Wilco.Web.UI.WebControls
{
	/// <summary>
	/// Represents a callback validator.
	/// </summary>
	public class CallbackValidator : BaseValidator, ICallbackEventHandler
	{
		private string errorMessageToUse;
        private string callbackResult;
		private static readonly object EventServerValidate = new object();

		/// <summary>
		/// Occurs when server-side validation occurs.
		/// </summary>
		[Description("Occurs when server-side validation occurs.")]
		public event CallbackServerValidateEventHandler ServerValidate
		{
			add
			{
				this.Events.AddHandler(CallbackValidator.EventServerValidate, value);
			}
			remove
			{
				this.Events.RemoveHandler(CallbackValidator.EventServerValidate, value);
			}
		}

		/// <summary>
		/// Gets or sets whether empty text should be validated.
		/// </summary>
		[
		DefaultValue(false),
		Description("Gets or sets whether empty text should be validated."),
		PersistenceMode(PersistenceMode.Attribute)
		]
		public bool ValidateEmptyText
		{
			get
			{
				return ViewStateUtility.GetViewStateValue<bool>(this.ViewState, "ValidateEmptyText", false);
			}
			set
			{
				this.ViewState["ValidateEmptyText"] = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.CallbackValidator"/> class.
		/// </summary>
		public CallbackValidator()
		{
			//
		}
		
		/// <summary>
		/// Raises the <see cref="System.Web.UI.Control.PreRender"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			ClientScriptManager csm = this.Page.ClientScript;
			csm.RegisterClientScriptResource(this.GetType(), "Wilco.Web.Resources.CallbackValidator.js");
		}

		/// <summary>
		/// Adds the attributes to render.
		/// </summary>
		/// <param name="writer"></param>
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			base.AddAttributesToRender(writer);
			if (this.EnableClientScript)
			{
				ClientScriptManager csm = this.Page.ClientScript;

				// Build the argument which will be passed to the server when the callback happens.
				string argument = String.Format("ValidatorGetValue({0}.controltovalidate)", this.ClientID);

				// The actual callback function.
				string callbackFunction = csm.GetCallbackEventReference(this, argument, "__callbackValidatorCallback", "'" + this.ClientID + "'");

				csm.RegisterExpandoAttribute(this.ClientID, "evaluationfunction", "CustomValidatorEvaluateIsValid", false);
				csm.RegisterExpandoAttribute(this.ClientID, "clientvalidationfunction", "__callbackValidatorValidate", false);
				csm.RegisterExpandoAttribute(this.ClientID, "callbackfunction", callbackFunction, false);
				
				if (this.ValidateEmptyText)
				{
					csm.RegisterExpandoAttribute(this.ClientID, "validateemptytext", "true", false);
				}
			}
		}

		/// <summary>
		/// Renders the control.
		/// </summary>
		/// <param name="writer"></param>
		protected override void Render(HtmlTextWriter writer)
		{
			if (this.errorMessageToUse != null)
				this.ErrorMessage = this.errorMessageToUse;
			
			base.Render(writer);
		}

		/// <summary>
		/// Evaluates whether the control's input is valid.
		/// </summary>
		/// <returns></returns>
		protected override bool EvaluateIsValid()
		{
			string validationValue = String.Empty;

			string controlToValidate = this.ControlToValidate;
			if (controlToValidate.Length > 0)
			{
				validationValue = base.GetControlValidationValue(controlToValidate);
				if (((validationValue == null) || (validationValue.Trim().Length == 0)) && !this.ValidateEmptyText)
				{
					return true;
				}
			}

			CallbackServerValidateEventArgs args = new CallbackServerValidateEventArgs(validationValue, true, this.ErrorMessage);
			this.OnServerValidate(args);
			return args.IsValid;
		}

		/// <summary>
		/// Executes the logic for the current callback.
		/// </summary>
		/// <param name="eventArgument"></param>
		public void RaiseCallbackEvent(string eventArgument)
		{
			CallbackServerValidateEventArgs args = new CallbackServerValidateEventArgs(eventArgument, true, this.ErrorMessage);
			this.OnServerValidate(args);

			if (args.IsValid)
				args.ErrorMessage = String.Empty;
			this.callbackResult = args.IsValid + "|" + args.ErrorMessage;
		}

        /// <summary>
        /// Gets the callback result.
        /// </summary>
        public string GetCallbackResult()
        {
            return this.callbackResult;
        }

		/// <summary>
		/// Raises the <see cref="Wilco.Web.UI.WebControls.CallbackValidator.ServerValidate"/> event.
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		protected virtual void OnServerValidate(CallbackServerValidateEventArgs e)
		{
			CallbackServerValidateEventHandler handler = this.Events[CallbackValidator.EventServerValidate] as CallbackServerValidateEventHandler;
			if (handler != null)
			{
				handler(this, e);
			}

			this.errorMessageToUse = e.ErrorMessage;
		}
	}
}