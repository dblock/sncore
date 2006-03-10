using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Wilco.Web.UI.WebControls
{
    /// <summary>
    /// Represents a row selector field which can be used inside a <see cref="System.Web.UI.WebControls.GridView"/>.
    /// </summary>
    [DefaultEvent("SelectedIndexChanged")]
    public class RowSelectorField : DataControlField
    {
        private static readonly object EventSelectedIndexChanged = new object();

        private EventHandlerList events;
        private bool allowSelectAll;
        private bool autoPostBack;
        private string dataValueField;
        private ListSelectionMode selectionMode;
        private SelectAllCheckBox selectAll;

        /// <summary>
        /// Occurs when the selection inside the column changed.
        /// </summary>
        [
        Description("Occurs when the selection inside the column changed.")
        ]
        public event EventHandler SelectedIndexChanged
        {
            add
            {
                this.Events.AddHandler(RowSelectorField.EventSelectedIndexChanged, value);
            }
            remove
            {
                this.Events.RemoveHandler(RowSelectorField.EventSelectedIndexChanged, value);
            }
        }

        /// <summary>
        /// Gets the events.
        /// </summary>
        protected EventHandlerList Events
        {
            get
            {
                if (this.events == null)
                {
                    this.events = new EventHandlerList();
                }
                return this.events;
            }
        }

        /// <summary>
        /// Gets or sets whether all rows can be (de)selected at once.
        /// </summary>
        [
        DefaultValue(true),
        Description("Gets or sets whether all rows can be (de)selected at once.")
        ]
        public bool AllowSelectAll
        {
            get
            {
                return this.allowSelectAll;
            }
            set
            {
                this.allowSelectAll = value;
            }
        }

        /// <summary>
        /// Gets or sets whether the field should postback when the selection changes.
        /// </summary>
        [
        DefaultValue(false),
        Description("Gets or sets whether the field should postback when the selection changes.")
        ]
        public bool AutoPostBack
        {
            get
            {
                return this.autoPostBack;
            }
            set
            {
                this.autoPostBack = value;
            }
        }

        /// <summary>
        /// Gets or sets the data value field.
        /// </summary>
        /// <remarks>
        /// This field is ignored when the <see cref="SelectionMode"/> property is set to <value>Single</value>.
        /// </remarks>
        [
        DefaultValue(""),
        Description("Gets or sets the data value field.")
        ]
        public string DataValueField
        {
            get
            {
                if (this.dataValueField == null)
                    return String.Empty;

                return this.dataValueField;
            }
            set
            {
                this.dataValueField = value;
            }
        }

        /// <summary>
        /// Gets or sets the indices of the rows that are selected.
        /// </summary>
        [
        Description("Gets or sets the indices of the rows that are selected."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public virtual int[] SelectedIndices
        {
            get
            {
                GridView owner = this.Control as GridView;
                if (owner == null)
                    return new int[0];

                int columnIndex = owner.Columns.IndexOf(this);
                if (columnIndex < 0)
                    return new int[0];

                if (this.SelectionMode == ListSelectionMode.Multiple)
                {
                    List<int> selectedIndices = new List<int>();
                    foreach (GridViewRow row in owner.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox cb = (CheckBox)row.Cells[columnIndex].Controls[0];
                            if (cb.Checked)
                            {
                                selectedIndices.Add(row.RowIndex);
                            }
                        }
                    }
                    return selectedIndices.ToArray();
                }
                else
                {
                    foreach (GridViewRow row in owner.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            GridViewRadioButton rb = (GridViewRadioButton)row.Cells[columnIndex].Controls[0];
                            if (rb.Checked)
                            {
                                return new int[1] { row.RowIndex };
                            }
                        }
                    }
                }

                return new int[0];
            }
            set
            {
                GridView owner = this.Control as GridView;
                if (owner == null)
                    return;

                int columnIndex = owner.Columns.IndexOf(this);
                if (columnIndex < 0)
                    return;

                if (this.SelectionMode == ListSelectionMode.Single && value.Length > 1)
                    throw new ArgumentException("value");

                bool selected;
                foreach (GridViewRow row in owner.Rows)
                {
                    selected = false;
                    for (int i = 0; i < value.Length; i++)
                    {
                        if (value[i] == row.RowIndex)
                        {
                            selected = true;
                            break;
                        }
                    }

                    if (this.SelectionMode == ListSelectionMode.Multiple)
                    {
                        CheckBox cb = (CheckBox)row.Cells[columnIndex].Controls[0];
                        cb.Checked = selected;
                    }
                    else
                    {
                        GridViewRadioButton rb = (GridViewRadioButton)row.Cells[columnIndex].Controls[0];
                        rb.Checked = selected;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the selection mode.
        /// </summary>
        [
        DefaultValue((int)ListSelectionMode.Single),
        Description("Gets or sets the selection mode.")
        ]
        public ListSelectionMode SelectionMode
        {
            get
            {
                return this.selectionMode;
            }
            set
            {
                this.selectionMode = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.RowSelectorField"/> class.
        /// </summary>
        public RowSelectorField()
        {
            this.allowSelectAll = true;
            this.selectionMode = ListSelectionMode.Single;
        }

        /// <summary>
        /// Creates an empty row selector field.
        /// </summary>
        /// <returns></returns>
        protected override DataControlField CreateField()
        {
            return new RowSelectorField();
        }

        /// <summary>
        /// Adds text or controls to a cell's control collection.
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="cellType"></param>
        /// <param name="rowState"></param>
        /// <param name="rowIndex"></param>
        public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
        {
            base.InitializeCell(cell, cellType, rowState, rowIndex);

            if (cellType == DataControlCellType.DataCell)
            {
                this.InitializeDataCell(cell);
            }
            else if (cellType == DataControlCellType.Header)
            {
                this.InitializeHeader(cell);
            }
        }

        /// <summary>
        /// Initializes the header.
        /// </summary>
        /// <param name="cell"></param>
        protected virtual void InitializeHeader(DataControlFieldCell cell)
        {
            if (this.AllowSelectAll && this.SelectionMode == ListSelectionMode.Multiple)
            {
                string text = cell.Text;

                this.selectAll = new SelectAllCheckBox();
                cell.Controls.Add(this.selectAll);
                this.selectAll.AutoPostBack = this.AutoPostBack;

                LiteralControl headerLabel = new LiteralControl(text);
                cell.Controls.Add(headerLabel);
            }
        }

        /// <summary>
        /// Initializes the data cell.
        /// </summary>
        /// <param name="cell"></param>
        protected virtual void InitializeDataCell(DataControlFieldCell cell)
        {
            Control selector = null;
            if (this.SelectionMode == ListSelectionMode.Multiple)
            {
                CheckBox cb = new CheckBox();
                this.selectAll.RegisterParticipant(cb);
                cb.AutoPostBack = this.AutoPostBack;
                cb.CheckedChanged += new EventHandler(this.selector_CheckedChanged);
                cb.DataBinding += new EventHandler(this.selector_DataBinding);
                selector = cb;
            }
            else
            {
                GridViewRadioButton rb = new GridViewRadioButton();
                rb.AutoPostBack = this.AutoPostBack;
                rb.ServerChange += new EventHandler(this.selector_CheckedChanged);
                rb.Name = "RowSelector";
                rb.DataBinding += new EventHandler(this.selector_DataBinding);
                selector = rb;
            }

            cell.Controls.Add(selector);
        }

        /// <summary>
        /// Raises the <see cref="Wilco.Web.UI.WebControls.RowSelectorField.SelectedIndexChanged"/> event.
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnSelectedIndexChanged(EventArgs args)
        {
            EventHandler handler = this.Events[RowSelectorField.EventSelectedIndexChanged] as EventHandler;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void selector_DataBinding(object sender, EventArgs e)
        {
            if (this.SelectionMode == ListSelectionMode.Multiple)
            {
                CheckBox cb = (CheckBox)sender;
                object dataItem = DataBinder.GetDataItem(cb.NamingContainer);

                if (dataItem != null)
                {
                    if (this.DataValueField.Length > 0)
                        cb.Checked = Convert.ToBoolean(DataBinder.GetPropertyValue(dataItem, this.DataValueField));
                }
            }
        }

        private void selector_CheckedChanged(object sender, EventArgs e)
        {
            this.OnSelectedIndexChanged(e);
        }
    }
}