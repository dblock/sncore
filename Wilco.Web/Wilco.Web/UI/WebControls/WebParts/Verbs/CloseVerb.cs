using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Wilco.Web.UI.WebControls.WebParts
{
	/// <summary>
	/// Represents a verb which lets a user remove a webpart panel.
	/// </summary>
	public class CloseVerb : HyperLink
	{
		/// <summary>
		/// Gets or sets the message which will be displayed as a confirmation for deleting a webpart panel.
		/// </summary>
		[
		Description("Gets or sets the message which will be displayed as a confirmation for deleting a webpart panel."),
		Localizable(true),
		PersistenceMode(PersistenceMode.Attribute)
		]
		public string DeleteConfirmMessage
		{
			get
			{
				return ViewStateUtility.GetViewStateValue<string>(this.ViewState, "DeleteConfirmMessage", String.Empty);
			}
			set
			{
				this.ViewState["DeleteConfirmMessage"] = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.WebParts.MinimizeRestoreVerb"/> class.
		/// </summary>
		public CloseVerb()
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

			if (this.DesignMode || (this.Page == null))
				return;

			this.RegisterClientScript();
		}

		/// <summary>
		/// Registers the client script.
		/// </summary>
		private void RegisterClientScript()
		{
			// Get the reference to the panel's body which should be minimized/restored.
			Control parentControl = this.NamingContainer;
			while ((parentControl != null) && !(parentControl is WebPartPanel))
			{
				parentControl = parentControl.NamingContainer;
			}

			if (parentControl == null)
				throw new InvalidOperationException("Verbs can only be used in webpart panels.");

			WebPartPanel panel = (WebPartPanel)parentControl;
			if (panel.AllowDelete)
			{
				this.Attributes["onclick"] = String.Format("RemoveWebPart('{0}', '{1}');", panel.ContainerClientID, this.DeleteConfirmMessage);
			}

			this.Style[HtmlTextWriterStyle.Cursor] = "pointer";
		}
	}
}