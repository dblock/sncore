using System;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

using Wilco.Collections.Generic;

namespace Wilco.Web.UI.WebControls
{
	/// <summary>
	/// Represents a dummy data source.
	/// </summary>
	public class DummyDataSource : ICollection, IEnumerable
	{
		private int dataItemCount;

		/// <summary>
		/// Gets the number of items.
		/// </summary>
		public int Count
		{
			get
			{
				return this.dataItemCount;
			}
		}

		/// <summary>
		/// Gets whether access to the collection is synchronized.
		/// </summary>
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets the sync. root.
		/// </summary>
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.DummyDataSource"/> class.
		/// </summary>
		/// <param name="dataItemCount"></param>
		public DummyDataSource(int dataItemCount)
		{
			this.dataItemCount = dataItemCount;
		}

		/// <summary>
		/// Copies the contents to the specified array.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		public void CopyTo(Array array, int index)
		{
			IEnumerator enumerator = this.GetEnumerator();
			while (enumerator.MoveNext())
			{
				array.SetValue(enumerator.Current, index++);
			}
		}

		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			return new DummyDataSource.DummyDataSourceEnumerator(this.dataItemCount);
		}

		/// <summary>
		/// Represents a dummy data source enumerator.
		/// </summary>
		private class DummyDataSourceEnumerator : IEnumerator
		{
			private int count;
			private int index;

			/// <summary>
			/// Gets the current object.
			/// </summary>
			public object Current
			{
				get
				{
					return null;
				}
			}

			/// <summary>
			/// CTor.
			/// </summary>
			/// <param name="count"></param>
			public DummyDataSourceEnumerator(int count)
			{
				this.count = count;
				this.index = -1;
			}

			/// <summary>
			/// Moves to the next position.
			/// </summary>
			/// <returns></returns>
			public bool MoveNext()
			{
				this.index++;
				return (this.index < this.count);
			}

			/// <summary>
			/// Resets the enumerator.
			/// </summary>
			public void Reset()
			{
				this.index = -1;
			}
		}
	}
}