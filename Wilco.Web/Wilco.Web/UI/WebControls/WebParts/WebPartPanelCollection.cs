using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Web.UI;

namespace Wilco.Web.UI.WebControls.WebParts
{
	/// <summary>
	/// Represents a collection of webpart panels.
	/// </summary>
	public sealed class WebPartPanelCollection : ReadOnlyCollection<WebPartPanel>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.WebParts.WebPartPanelCollection"/> class.
		/// </summary>
		public WebPartPanelCollection()
			: base(new List<WebPartPanel>())
		{
			//
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.WebParts.WebPartPanelCollection"/> class.
		/// </summary>
		/// <param name="webPartPanels">A collection of controls which represents the webpart panels.</param>
		public WebPartPanelCollection(ControlCollection webPartPanels)
			: base(new List<WebPartPanel>())
		{
			WebPartPanel panel;
			foreach (Control c in webPartPanels)
			{
				if (c == null)
					throw new ArgumentNullException("webPartPanels");

				panel = c as WebPartPanel;
				if (panel == null)
					throw new ArgumentException("The control is not of the type WebPartPanel.");

				this.Items.Add(panel);
			}
		}

		/// <summary>
		/// Gets the webpart panel with the specified ID.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public WebPartPanel this[string id]
		{
			get
			{
				foreach (WebPartPanel panel in this.Items)
				{
					if (string.Compare(panel.ID, id, true, CultureInfo.InvariantCulture) == 0)
					{
						return panel;
					}
				}

				return null;
			}
		}

		/// <summary>
		/// Adds a webpart panel.
		/// </summary>
		/// <param name="panel"></param>
		internal void Add(WebPartPanel panel)
		{
			this.Items.Add(panel);
		}
	}
}