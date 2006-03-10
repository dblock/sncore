using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Wilco.Collections.Generic
{
	/// <summary>
	/// Represents a generic tree.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Tree<T> : IEnumerable<T> where T : class
	{
		private List<TreeNode<T>> nodes;

		/// <summary>
		/// Gets the number of nodes at the root of this (sub)tree.
		/// </summary>
		public int Count
		{
			get
			{
				return this.nodes.Count;
			}
		}

		/// <summary>
		/// Gets the node at the specified index.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public TreeNode<T> this[int index]
		{
			get
			{
				return this.nodes[index];
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Wilco.Collections.Generic.Tree&lt;T&gt;"/> class.
		/// </summary>
		public Tree()
		{
			this.nodes = new List<TreeNode<T>>();
		}

		/// <summary>
		/// Adds an item.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public TreeNode<T> Add(T item)
		{
			TreeNode<T> node = new TreeNode<T>(item);
			this.nodes.Add(node);
			return node;
		}

		/// <summary>
		/// Removes an item.
		/// </summary>
		/// <param name="item"></param>
		public void Remove(T item)
		{
			foreach (TreeNode<T> node in this.nodes)
			{
				if (node.Value == item)
				{
					this.nodes.Remove(node);
					break;
				}
			}
		}

		/// <summary>
		/// Finds a node recursively.
		/// </summary>
		/// <param name="match"></param>
		/// <returns></returns>
		public TreeNode<T> FindRecursive(Predicate<T> match)
		{
			TreeNode<T> result = null;
			foreach (TreeNode<T> item in this.nodes)
			{
				if (match(item.Value))
					return item;

				result = item.Nodes.FindRecursive(match);
				if (result != null)
					return result;
			}

			return null;
		}

		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<T> GetEnumerator()
		{
			return new TreeEnumerator<T>(this);
		}

		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns></returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}

		/// <summary>
		/// Represents a tree enumerator.
		/// </summary>
		/// <typeparam name="TItem"></typeparam>
		public struct TreeEnumerator<TItem> : IEnumerator<TItem> where TItem : class
		{
			private Tree<TItem> tree;
			private int currentIndex;

			/// <summary>
			/// Gets the current value.
			/// </summary>
			public TItem Current
			{
				get
				{
					return this.tree[currentIndex].Value;
				}
			}

			/// <summary>
			/// Gets the current value.
			/// </summary>
			object IEnumerator.Current
			{
				get
				{
					return ((IEnumerator<TItem>)this).Current;
				}
			}

			/// <summary>
			/// CTor.
			/// </summary>
			/// <param name="tree"></param>
			internal TreeEnumerator(Tree<TItem> tree)
			{
				this.tree = tree;
				this.currentIndex = -1;
			}

			/// <summary>
			/// Disposes the enumerator.
			/// </summary>
			public void Dispose()
			{
				//
			}

			/// <summary>
			/// Moves to the next item in the tree.
			/// </summary>
			/// <returns></returns>
			public bool MoveNext()
			{
				return (++this.currentIndex < this.tree.Count);
			}

			/// <summary>
			/// Resets the enumerator.
			/// </summary>
			public void Reset()
			{
				this.currentIndex = -1;
			}
		}
	}
}