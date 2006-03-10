using System;
using System.Collections.Generic;

namespace SnCore.Tools.Collections
{
    /// <summary>
    /// From http://www.codeproject.com/csharp/trees.asp.
    /// </summary>
    public class NodeCollection<Element> : IEnumerable<Node<Element>> where Element : class
    {
        List<Node<Element>> mList = new List<Node<Element>>();
        Node<Element> mOwner = null;

        internal NodeCollection(Node<Element> owner)
        {
            if (null == owner) throw new ArgumentNullException("owner");
            mOwner = owner;
        }

        public void Add(Node<Element> rhs)
        {
            if (mOwner.DoesShareHierarchyWith(rhs))
                throw new InvalidOperationException("Cannot add an ancestor or descendant.");
            mList.Add(rhs);
            rhs.Parent = mOwner;
        }
        
        public void Remove(Node<Element> rhs)
        {
            mList.Remove(rhs);
            rhs.Parent = null;
        }
        
        public bool Contains(Node<Element> rhs)
        {
            return mList.Contains(rhs);
        }
        
        public void Clear()
        {
            foreach (Node<Element> n in this)
                n.Parent = null;
            mList.Clear();
        }
        
        public int Count
        {
            get
            {
                return mList.Count;
            }
        }

        public Node<Element> Owner
        {
            get
            {
                return mOwner;
            }
        }

        public Node<Element> this[int index]
        {
            get
            {
                return mList[index];
            }
        }

        public IEnumerator<Node<Element>> GetEnumerator()
        {
            return mList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        { 
            return this.GetEnumerator(); 
        }

    } // class NodeCollection
    
    public class Node<Element> where Element : class
    {
        NodeCollection<Element> mChildren = null;
        Node<Element> mParent = null;
        Element mData = null;

        public Node(Element nodedata)
        {
            mChildren = new NodeCollection<Element>(this);
            mData = nodedata;
        }

        public Node<Element> Parent
        {
            get
            {
                return mParent;
            }
            internal set
            {
                mParent = value;
            }
        }

        public NodeCollection<Element> Children
        {
            get
            {
                return mChildren;
            }
        }

        public Node<Element> Root
        {
            get
            {
                if (null == mParent) return this;
                return mParent.Root;
            }
        }

        public bool IsAncestorOf(Node<Element> rhs)
        {
            if (mChildren.Contains(rhs)) return true;
            foreach (Node<Element> kid in mChildren)
                if (kid.IsAncestorOf(rhs)) return true;
            return false;
        }

        public bool IsDescendantOf(Node<Element> rhs)
        {
            if (null == mParent) return false;
            if (rhs == mParent) return true;
            return mParent.IsDescendantOf(rhs);
        }

        public bool DoesShareHierarchyWith(Node<Element> rhs)
        {
            if (rhs == this) return true;
            if (this.IsAncestorOf(rhs)) return true;
            if (this.IsDescendantOf(rhs)) return true;
            return false;
        }

        public Element Data
        {
            get
            {
                return mData;
            }
        }

        public IEnumerator<Node<Element>> GetDepthFirstNodeEnumerator()
        {
            yield return this;
            foreach (Node<Element> kid in mChildren)
            {
                IEnumerator<Node<Element>> kidenumerator = kid.GetDepthFirstNodeEnumerator();
                while (kidenumerator.MoveNext())
                    yield return kidenumerator.Current;
            }
        }

        public IEnumerator<Element> GetDepthFirstEnumerator()
        {
            yield return mData;
            foreach (Node<Element> kid in mChildren)
            {
                IEnumerator<Element> kidenumerator = kid.GetDepthFirstEnumerator();
                while (kidenumerator.MoveNext())
                    yield return kidenumerator.Current;
            }
        }

        public IEnumerator<Element> GetBreadthFirstEnumerator()
        {
            Queue<Node<Element>> todo = new Queue<Node<Element>>();
            todo.Enqueue(this);
            while (0 < todo.Count)
            {
                Node<Element> n = todo.Dequeue();
                foreach (Node<Element> kid in n.mChildren)
                    todo.Enqueue(kid);
                yield return n.mData;
            }
        }

        public IEnumerator<Node<Element>> GetBreadthFirstNodeEnumerator()
        {
            Queue<Node<Element>> todo = new Queue<Node<Element>>();
            todo.Enqueue(this);
            while (0 < todo.Count)
            {
                Node<Element> n = todo.Dequeue();
                foreach (Node<Element> kid in n.mChildren)
                    todo.Enqueue(kid);
                yield return n;
            }
        }

    } // class Node

    public class Tree<Element> where Element : class
    {
        Node<Element> mRoot = new Node<Element>(null);
        
        public Tree()
        {

        }

        public IEnumerator<Element> GetDepthFirstEnumerator()
        {
            IEnumerator<Element> result = mRoot.GetDepthFirstEnumerator();
            result.MoveNext();
            return result;
        }

        public IEnumerator<Element> GetBreadthFirstEnumerator()
        {
            IEnumerator<Element> result = mRoot.GetBreadthFirstEnumerator();
            result.MoveNext();
            return result;
        }

        public IEnumerator<Node<Element>> GetDepthFirstNodeEnumerator()
        {
            IEnumerator<Node<Element>> result = mRoot.GetDepthFirstNodeEnumerator();
            result.MoveNext();
            return result;
        }

        public IEnumerator<Node<Element>> GetBreadthFirstNodeEnumerator()
        {
            IEnumerator<Node<Element>> result = mRoot.GetBreadthFirstNodeEnumerator();
            result.MoveNext();
            return result;
        }

        public int Count
        {
            get
            {
                int result = 0;

                IEnumerator<Element> enumerator = mRoot.GetDepthFirstEnumerator();
                while (enumerator.MoveNext())
                    result++;

                return result;
            }
        }

        public delegate bool IsParent(Element parent, Element child);
        public delegate bool IsEqual(Element left, Element right);
        public delegate bool HasParent(Element element);

        public List<Element> List
        {
            get
            {
                List<Element> result = new List<Element>();
                IEnumerator<Element> enumerator = this.GetDepthFirstEnumerator();
                while (enumerator.MoveNext())
                    result.Add(enumerator.Current);
                return result;
            }
        }

        public Tree(System.Collections.IEnumerable list, HasParent hasParent, IsParent isParent, IsEqual isEqual)
        {
            List<Element> copy = new List<Element>();
            System.Collections.IEnumerator enumerator = list.GetEnumerator();
            while (enumerator.MoveNext())
                copy.Add((Element) enumerator.Current);

            LoadFromCopy(copy, hasParent, isParent, isEqual);
        }

        /// <summary>
        /// Load from an unsorted list.
        /// </summary>
        /// <param name="list"></param>
        public Tree(IEnumerable<Element> list, HasParent hasParent, IsParent isParent, IsEqual isEqual)
        {
            List<Element> copy = new List<Element>();
            IEnumerator<Element> enumerator = list.GetEnumerator();
            while (enumerator.MoveNext())
                copy.Add(enumerator.Current);

            LoadFromCopy(copy, hasParent, isParent, isEqual);
        }

        void LoadFromCopy(List<Element> copy, HasParent hasParent, IsParent isParent, IsEqual isEqual)
        {
            while (copy.Count > 0)
            {
                int i = 0;
                while(i < copy.Count)
                {
                    bool fAdded = false;
                    Element f = copy[i];
                    if (! hasParent(f))
                    {
                        mRoot.Children.Add(new Node<Element>(f));
                        fAdded = true;
                    }
                    else
                    {
                        IEnumerator<Node<Element>> innerenumerator = GetDepthFirstNodeEnumerator();
                        while (innerenumerator.MoveNext())
                        {
                            if (isParent(innerenumerator.Current.Data, f))
                            {
                                innerenumerator.Current.Children.Add(new Node<Element>(f));
                                fAdded = true;
                                break;
                            }
                        }
                    }

                    if (fAdded)
                    {
                        copy.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }
    }
}
