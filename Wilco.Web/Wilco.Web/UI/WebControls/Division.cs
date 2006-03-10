using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace Wilco.Web.UI.WebControls
{
	/// <summary>
	/// Represents a division which acts as a container and guarantees that 
	/// ID's of child controls are unique.
	/// </summary>
	[
	Designer(typeof(ContainerControlDesigner)),
	ParseChildren(false)
	]
	public class Division : WebControl, INamingContainer
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.Division"/> class.
		/// </summary>
		public Division() : base("div")
		{
			//
		}
	}
}