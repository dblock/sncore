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
	/// Represents a repeater with support for hierarchical data.
	/// </summary>
	[ParseChildren(true), PersistChildren(false)]
	public class HierarchicalRepeater : Repeater
	{
		private Tree<object> data;
		private string dataKeyField;
		private string dataParentKeyField;
		private string dataKeyNullValue;
		private Style childContainerStyle;
		private bool useDataSource;
		private HierarchicalRepeaterItem nextExpectedControl;

		/// <summary>
		/// Gets the data key field.
		/// </summary>
		public string DataKeyField
		{
			get
			{
				return this.dataKeyField;
			}
			set
			{
				this.dataKeyField = value;
			}
		}

		/// <summary>
		/// Gets the data parent key field.
		/// </summary>
		public string DataParentKeyField
		{
			get
			{
				return this.dataParentKeyField;
			}
			set
			{
				this.dataParentKeyField = value;
			}
		}

		/// <summary>
		/// Gets or sets the value of a key that is not set.
		/// </summary>
		[DefaultValue("")]
		public string DataKeyNullValue
		{
			get
			{
				return this.dataKeyNullValue;
			}
			set
			{
				this.dataKeyNullValue = value;
			}
		}

		/// <summary>
		/// Gets the child container style.
		/// </summary>
		[
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		NotifyParentProperty(true),
		PersistenceMode(PersistenceMode.InnerProperty)
		]
		public Style ChildContainerStyle
		{
			get
			{
				if (this.childContainerStyle == null)
				{
					this.childContainerStyle = new Style();
					if (this.IsTrackingViewState)
					{
						((IStateManager)this.childContainerStyle).TrackViewState();
					}
				}

				return this.childContainerStyle;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.HierarchicalRepeater"/> class.
		/// </summary>
		public HierarchicalRepeater()
		{
			this.dataKeyNullValue = String.Empty;
		}

		/// <summary>
		/// Creates the control hierarchy.
		/// </summary>
		/// <param name="useDataSource"></param>
		protected override void CreateControlHierarchy(bool useDataSource)
		{
			this.useDataSource = useDataSource;
			base.CreateControlHierarchy(useDataSource);
		}

		/// <summary>
		/// Gets the data from the data source. Only the root data will be returned.
		/// </summary>
		/// <returns></returns>
		protected override IEnumerable GetData()
		{
			if (this.data == null)
			{
				this.data = new Tree<object>();
			}

			IEnumerable data = base.GetData();

			string dataKey;
			string parentDataKey;
			TreeNode<object> node;
			foreach (object dataObject in data)
			{
				parentDataKey = this.GetFieldValue(dataObject, this.DataParentKeyField);
				if (parentDataKey == this.DataKeyNullValue)
				{
					dataKey = this.GetFieldValue(dataObject, this.DataKeyField);
					node = this.data.Add(dataObject);
					this.ProcessChildData(data, dataObject, node, dataKey);
				}
			}

			return this.data;
		}

		/// <summary>
		/// Builds the in-memory tree of data recursively.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="dataObject"></param>
		/// <param name="node"></param>
		/// <param name="parentDataKey"></param>
		private void ProcessChildData(IEnumerable data, object dataObject, TreeNode<object> node, string parentDataKey)
		{
			string dataKey;
			string childParentDataKey;
			TreeNode<object> childNode;
			foreach (object childDataObject in data)
			{
				childParentDataKey = this.GetFieldValue(childDataObject, this.DataParentKeyField);
				if (childParentDataKey == parentDataKey)
				{
					dataKey = this.GetFieldValue(childDataObject, this.DataKeyField);
					childNode = node.Nodes.Add(childDataObject);
					this.ProcessChildData(data, childDataObject, childNode, dataKey);
				}
			}
		}

		/// <summary>
		/// Gets the property value of the data object.
		/// </summary>
		/// <param name="dataObject"></param>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		private string GetFieldValue(object dataObject, string propertyName)
		{
			object key = DataBinder.GetPropertyValue(dataObject, propertyName);
			if (key != null)
				return key.ToString();
			
			return String.Empty;
		}

		/// <summary>
		/// Creates a hierarchical repeater item.
		/// </summary>
		/// <param name="itemIndex"></param>
		/// <param name="itemType"></param>
		/// <returns></returns>
		protected override RepeaterItem CreateItem(int itemIndex, ListItemType itemType)
		{
			HierarchicalRepeaterItem item = new HierarchicalRepeaterItem(this, itemIndex, itemType);
			if (this.useDataSource)
			{
				// We need a way to create our child hiearchy _after_ this control was added to the 
				// control hierarchy. It seems that one of the only ways to achieve this is by 
				// overriding AddedControl and see if the added control is this repeater item.
				this.nextExpectedControl = item;
			}
			return item;
		}

		/// <summary>
		/// Called after a control was added.
		/// </summary>
		/// <param name="control"></param>
		/// <param name="index"></param>
		protected override void AddedControl(Control control, int index)
		{
			base.AddedControl(control, index);
			if (control == this.nextExpectedControl)
			{
				switch (this.nextExpectedControl.ItemType)
				{
					case ListItemType.AlternatingItem:
					case ListItemType.EditItem:
					case ListItemType.Item:
					case ListItemType.SelectedItem:
						this.CreateChildHierarchy(this.nextExpectedControl);
						break;
				}
			}
		}

		/// <summary>
		/// Creates the child hierarchy.
		/// </summary>
		/// <param name="parent"></param>
		protected internal virtual void CreateChildHierarchy(HierarchicalRepeaterItem parent)
		{
			IEnumerable enumerable = null;
			if (this.useDataSource)
			{
				if (parent.ItemCount > -1)
					return;

				TreeNode<object> node = this.data.FindRecursive(new Predicate<object>(delegate(object target)
				{
					return (parent.DataItem == target);
				}));
				
				if (node != null)
				{
					enumerable = node.Nodes;
				}
			}
			else
			{
				if (parent.ItemCount > -1 && parent.ChildPanel.Controls.Count == 0)
				{
					enumerable = new DummyDataSource(parent.ItemCount);
				}
			}

			if (enumerable != null)
			{
				bool isSeparated = (this.SeparatorTemplate != null);
				int itemLength = 0;

				foreach (object dataObject in enumerable)
				{
					if (isSeparated && (itemLength > 0))
					{
						this.CreateItem(parent, itemLength - 1, ListItemType.Separator, useDataSource, null);
					}
					ListItemType type = ((itemLength % 2) == 0) ? ListItemType.Item : ListItemType.AlternatingItem;
					HierarchicalRepeaterItem childItem = (HierarchicalRepeaterItem)this.CreateItem(parent, itemLength, type, useDataSource, dataObject);
					itemLength++;
				}
			}
		}

		/// <summary>
		/// Creates an item.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="itemIndex"></param>
		/// <param name="itemType"></param>
		/// <param name="dataBind"></param>
		/// <param name="dataItem"></param>
		/// <returns></returns>
		private RepeaterItem CreateItem(HierarchicalRepeaterItem parent, int itemIndex, ListItemType itemType, bool dataBind, object dataItem)
		{
			HierarchicalRepeaterItem item = (HierarchicalRepeaterItem)this.CreateItem(itemIndex, itemType);
			RepeaterItemEventArgs args = new RepeaterItemEventArgs(item);
			this.InitializeItem(item);
			if (dataBind)
			{
				item.DataItem = dataItem;
			}
			this.OnItemCreated(args);
			
			parent.ChildPanel.Controls.Add(item);
			if (!this.ChildContainerStyle.IsEmpty)
			{
				parent.ChildPanel.ApplyStyle(this.ChildContainerStyle);
			}

			if (this.useDataSource && (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item))
			{
				if (parent.ItemCount == -1)
					parent.ItemCount = 1;
				else
					parent.ItemCount++;
				
				this.CreateChildHierarchy(item);
			}
			
			if (dataBind)
			{
				item.DataBind();
				this.OnItemDataBound(args);
			}
			return item;
		}
	}
}