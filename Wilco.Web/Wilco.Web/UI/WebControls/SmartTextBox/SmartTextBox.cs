using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Wilco.Web.UI.WebControls
{
	/// <summary>
	/// Represents a textbox with an auto-completion ability.
	/// </summary>
	public class SmartTextBox : TextBox, ICallbackEventHandler
	{
		private ControlCollection hiddenControls;
		private HiddenField inputValue;
		private Division results;
        private string callbackResult;

		private static readonly object EventAutoCompletion = new object();

		/// <summary>
		/// Occurs when auto completion is being performed.
		/// </summary>
		/// <value></value>
		[Category("Action")]
		public event AutoCompleteEventHandler AutoCompletion
		{
			add
			{
				this.Events.AddHandler(EventAutoCompletion, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventAutoCompletion, value);
			}
		}

		/// <summary>
		/// Gets or sets whether auto-completion is enabled.
		/// </summary>
		/// <value></value>
		[
		Category("Behaviour"),
		DefaultValue(true),
		Description("Gets or sets whether auto-completion is enabled.")
		]
		public bool EnableAutoComplete
		{
			get
			{
				return ViewStateUtility.GetViewStateValue<bool>(this.ViewState, "EnableAutoComplete", true);
			}
			set
			{
				this.ViewState["EnableAutoComplete"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the CSS class used for the suggested results.
		/// </summary>
		/// <value></value>
		public string ResultCssClass
		{
			get
			{
				this.EnsureChildControls();
				return this.results.CssClass;
			}
			set
			{
				this.EnsureChildControls();
				this.results.CssClass = value;
			}
		}

		/// <summary>
		/// Gets or sets the CSS class used for a row in the suggested results.
		/// </summary>
		/// <value></value>
		public string ResultRowCssClass
		{
			get
			{
				return ViewStateUtility.GetViewStateValue<string>(this.ViewState, "ResultRowCssClass", String.Empty);
			}
			set
			{
				this.ViewState["ResultRowCssClass"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the CSS class used for a selected row in the suggested results.
		/// </summary>
		/// <value></value>
		public string SelectedResultRowCssClass
		{
			get
			{
				return ViewStateUtility.GetViewStateValue<string>(this.ViewState, "SelectedResultRowCssClass", String.Empty);
			}
			set
			{
				this.ViewState["SelectedResultRowCssClass"] = value;
			}
		}

		/// <summary>
		/// Gets or sets whether the suggestions should be encoded.
		/// </summary>
		/// <value></value>
		[
		Category("Behaviour"),
		DefaultValue(true),
		Description("Gets or sets whether the suggestions should be encoded.")
		]
		public bool EncodeSuggestions
		{
			get
			{
				return ViewStateUtility.GetViewStateValue<bool>(this.ViewState, "EncodeSuggestions", true);
			}
			set
			{
				this.ViewState["EncodeSuggestions"] = value;
			}
		}

		/// <summary>
		/// Gets the hidden controls.
		/// </summary>
		/// <value></value>
		private ControlCollection HiddenControls
		{
			get
			{
				if (this.hiddenControls == null)
				{
					this.hiddenControls = new ControlCollection(this);
				}
				return this.hiddenControls;
			}
		}

		/// <summary>
		/// Gets or sets the ID of the textbox.
		/// </summary>
		/// <value></value>
		public override string ID
		{
			get
			{
				return base.ID;
			}
			set
			{
				this.EnsureChildControls();
				base.ID = value;
				this.inputValue.ID = value + "HV";
				this.results.ID = value + "Results";
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.SmartTextBox"/> class.
		/// </summary>
		public SmartTextBox()
		{
			//
		}

		/// <summary>
		/// Notifies server controls that use composition-based implementation to create any 
		/// child controls they contain in preparation for posting back or rendering.
		/// </summary>
		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			// The input value stores the last known input of the textbox. This way we can check client-side if the 
			// text was actually changed and we should do another callback.
			this.inputValue = new HiddenField();
			this.HiddenControls.Add(this.inputValue);
			this.inputValue.ID = "smartTextBoxHV";

			// Create the results div.
			this.results = new Division();
			this.HiddenControls.Add(this.results);
			this.results.ID = "results";
			this.results.Style.Add(HtmlTextWriterStyle.Display, "none");
		}

		/// <summary>
		/// Raises the <see cref="System.Web.UI.Control.PreRender"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			if (this.Page.Request.Browser.SupportsCallback && this.EnableAutoComplete)
			{
				ClientScriptManager csm = this.Page.ClientScript;

				csm.RegisterClientScriptResource(this.GetType(), "Wilco.Web.Resources.SmartTextBox.js");

				string callbackFunction = csm.GetCallbackEventReference(this, String.Format("{0}.value", this.ClientID), "__smartTextBoxCallback", "'" + this.results.ClientID + "'");

				this.Attributes["onkeyup"] = String.Format("if (__smartTextBoxShouldRefresh()) {{ {0}; }}", callbackFunction);
				this.Attributes["onfocus"] = String.Format("__smartTextBoxUpdateFocus(true, '{0}', '{1}', '{2}', '{3}', '{4}');", this.ClientID, this.inputValue.ClientID, this.results.ClientID, this.ResultRowCssClass, this.SelectedResultRowCssClass);
				this.Attributes["onblur"] = String.Format("__smartTextBoxUpdateFocus(false, '{0}', '{1}', '{2}', '{3}', '{4}');", this.ClientID, this.inputValue.ClientID, this.results.ClientID, this.ResultRowCssClass, this.SelectedResultRowCssClass);
				this.Attributes["autocomplete"] = "off";

				this.results.Style.Add(HtmlTextWriterStyle.Position, "absolute");
			}
		}

		/// <summary>
		/// Renders the control.
		/// </summary>
		/// <param name="writer"></param>
		protected override void Render(HtmlTextWriter writer)
		{
			base.Render(writer);

			// Render the hidden controls.
			for (int i = 0; i < this.hiddenControls.Count; i++)
			{
				this.hiddenControls[i].RenderControl(writer);
			}
		}

		/// <summary>
		/// Executes the logic for the current callback.
		/// </summary>
		/// <param name="eventArgument"></param>
		public void RaiseCallbackEvent(string eventArgument)
		{
			// Create an array with the response.
			StringBuilder result = new StringBuilder();
			result.Append("new Array(");

			// Get the suggestions.
			AutoCompleteSuggestion[] suggestions = this.OnAutoCompletion(new AutoCompleteEventArgs(eventArgument));
			if (suggestions != null)
			{
				int lastElement = suggestions.Length - 1;
				for (int i = 0; i < suggestions.Length; i++)
				{
					this.AppendArrayElement(result, suggestions[i].Text, (i == lastElement));
				}
			}

			result.Append(");");

			this.callbackResult = result.ToString();
		}

        /// <summary>
        /// Gets the callback result.
        /// </summary>
        public string GetCallbackResult()
        {
            return this.callbackResult;
        }

		/// <summary>
		/// Raises the <see cref="Wilco.Web.UI.WebControls.SmartTextBox.AutoCompletion"/> event.
		/// </summary>
		/// <param name="e"></param>
		/// <returns>An array of suggestions, or null if no event handler was specified.</returns>
		protected virtual AutoCompleteSuggestion[] OnAutoCompletion(AutoCompleteEventArgs e)
		{
			AutoCompleteEventHandler handler = this.Events[EventAutoCompletion] as AutoCompleteEventHandler;
			if (handler != null)
			{
				return handler(this, e);
			}

			return null;
		}

		/// <summary>
		/// Appends an element to the specified array.
		/// </summary>
		/// <param name="array">The array.</param>
		/// <param name="element">The element to add to the array.</param>
		/// <param name="isLastElement">Whether this is the last element in the array.</param>
		private void AppendArrayElement(StringBuilder array, string element, bool isLastElement)
		{
			if (this.EncodeSuggestions)
				element = this.JSEncode(element);

			array.AppendFormat("'{0}'", element);
			if (!isLastElement)
				array.Append(",");
		}

		/// <summary>
		/// Encodes the specified string to make it javascript safe.
		/// </summary>
		/// <param name="element">The element to encode.</param>
		/// <returns>The encoded element.</returns>
		private string JSEncode(string element)
		{
			element = HttpUtility.HtmlAttributeEncode(element);
			element = element.Replace("\\", "\\\\");
			element = element.Replace("'", "\\'");
			return element;
		}
	}
}