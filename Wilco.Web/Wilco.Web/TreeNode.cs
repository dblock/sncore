using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Wilco.Collections.Generic
{
	/// <summary>
	/// Represents a generic tree node.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class TreeNode<T> where T : class
	{
		private Tree<T> nodes;
		private T value;

		/// <summary>
		/// Gets the child nodes.
		/// </summary>
		public Tree<T> Nodes
		{
			get
			{
				return this.nodes;
			}
		}

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		public T Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Wilco.Collections.Generic.TreeNode&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="value"></param>
		public TreeNode(T value)
		{
			this.nodes = new Tree<T>();
			this.value = value;
		}
	}
}