using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Wilco.Web.UI.WebControls.WebParts
{
	/// <summary>
	/// Represents a collection of webpart zones.
	/// </summary>
	public sealed class WebPartZoneCollection : ReadOnlyCollection<WebPartZone>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.WebParts.WebPartZoneCollection"/> class.
		/// </summary>
		public WebPartZoneCollection()
			: base(new List<WebPartZone>())
		{
			//
		}

		/// <summary>
		/// Gets the webpart zone with the specified ID.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public WebPartZone this[string id]
		{
			get
			{
				foreach (WebPartZone zone in this.Items)
				{
					if (string.Compare(zone.ID, id, true, CultureInfo.InvariantCulture) == 0)
					{
						return zone;
					}
				}

				return null;
			}
		}

		/// <summary>
		/// Adds the webpart zone.
		/// </summary>
		/// <param name="zone">The zone to add.</param>
		internal void Add(WebPartZone zone)
		{
			this.Items.Add(zone);
		}
	}
}