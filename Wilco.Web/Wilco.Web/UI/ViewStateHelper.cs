using System;
using System.Web.UI;

namespace Wilco.Web.UI
{
	/// <summary>
	/// Represents a helper class for viewstate related functionality.
	/// </summary>
	public static class ViewStateUtility
	{
		/// <summary>
		/// Gets the viewstate value for the specified key.
		/// </summary>
		/// <typeparam name="T">The type of the value.</typeparam>
		/// <param name="state">The viewstate.</param>
		/// <param name="key">The key.</param>
		/// <returns>The value if it is not null; the default value otherwise.</returns>
		public static T GetViewStateValue<T>(StateBag state, string key)
		{
			object savedState = state[key];
			if (savedState != null)
				return (T)savedState;
			return default(T);
		}

		/// <summary>
		/// Gets the viewstate value for the specified key.
		/// </summary>
		/// <typeparam name="T">The type of the value.</typeparam>
		/// <param name="state">The viewstate.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The default value which will be returned if the value is null.</param>
		/// <returns>The value if it is not null; the default value otherwise.</returns>
		public static T GetViewStateValue<T>(StateBag state, string key, T defaultValue)
		{
			object savedState = state[key];
			if (savedState != null)
				return (T)savedState;
			return defaultValue;
		}
	}
}