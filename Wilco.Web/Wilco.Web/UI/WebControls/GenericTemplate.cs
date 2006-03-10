using System;
using System.Web.UI;

namespace Wilco.Web.UI.WebControls
{
	/// <summary>
	/// Represents a generic template, which uses delegates for building the template.
	/// </summary>
	public class GenericTemplate : Control, ITemplate
	{
		private TemplateEventHandler handler;

		/// <summary>
		/// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.GenericTemplate"/> class.
		/// </summary>
		public GenericTemplate(TemplateEventHandler handler)
		{
			this.handler = handler;
		}

		/// <summary>
		/// When implemented by a class, defines the System.Web.UI.Control object that
		/// child controls and templates belong to. These child controls are in turn
		/// defined within an inline template.
		/// </summary>
		/// <param name="container">
		/// The <see cref="Control"/> object to contain the instantiated controls from the inline
		/// template. 
		/// </param>
		public void InstantiateIn(Control container)
		{
			if (this.handler != null)
			{
				this.handler(this, new TemplateEventArgs(container));
			}
		}
	}
}