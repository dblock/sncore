using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SnCore.WebControls
{
    public enum PagedGridSortDirection
    {
        Ascending,
        Descending
    }

    public class PagedGrid : DataGrid
    {
        private int mFirst = -1;
        private ArrayList mPagers = null;
        private PagedDataSource mPagedDataSource = null;

        public event EventHandler OnGetDataSource = null;

        public PagedGrid()
        {
            
        }

        public int First
        {
            get
            {
                if (mFirst == -1)
                {
                    object ViewStateFirst = ViewState["First"];
                    if (ViewStateFirst != null)
                    {
                        mFirst = (int)ViewStateFirst;
                    }
                }
                return mFirst;
            }
            set
            {
                mFirst = value;
                ViewState["First"] = value;
            }
        }

        public string SortExpression
        {
            get
            {
                object result = ViewState["SortExpression"];
                return (result == null) ? string.Empty : (string)result;
            }
            set
            {
                ViewState["SortExpression"] = value;
            }
        }

        public PagedGridSortDirection SortDirection
        {
            get
            {                
                object result = ViewState["SortDirection"];
                return (result == null) 
                    ? PagedGridSortDirection.Ascending 
                    : (PagedGridSortDirection) Enum.Parse(typeof(PagedGridSortDirection), result.ToString());
            }
            set
            {
                ViewState["SortDirection"] = value.ToString();
            }
        }

        public ArrayList Pagers
        {
            get
            {
                if (mPagers == null)
                {
                    mPagers = new ArrayList();
                }
                return mPagers;
            }
            set
            {
                mPagers = value;
            }
        }

        public PagedDataSource PagedDataSource
        {
            get
            {
                return mPagedDataSource;
            }
            set
            {
                mPagedDataSource = value;
            }
        }

        protected override void OnPageIndexChanged(DataGridPageChangedEventArgs e)
        {
            if (OnGetDataSource != null)
            {
                CurrentPageIndex = PagedDataSource.CurrentPageIndex;
                OnGetDataSource(this, e);
                DataBind();
            }

            base.OnPageIndexChanged(e);
        }

        protected override void InitializePager(DataGridItem item, int columnSpan, PagedDataSource pagedDataSource)
        {
            PagedDataSource = pagedDataSource;
            Pager Pager = new Pager(pagedDataSource, columnSpan, PagerStyle);
            Pager.PageIndexChanged += new DataGridPageChangedEventHandler(Pager_PageIndexChanged);
            Pager.First = First;
            item.Cells.Add(Pager.Navigator);
            Pagers.Add(Pager);
        }

        protected override void OnPreRender(EventArgs e)
        {
            foreach (Pager Pager in Pagers)
            {
                Pager.First = First;
                Pager.PreRenderCommands();
            }
            base.OnPreRender(e);
        }

        private void Pager_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            First = ((Pager)source).First;
        }

        public int FindHeaderColumnIndex(string HeaderValue)
        {
            for (int Result = 0; Result < Columns.Count; Result++)
            {
                DataGridColumn DataColumn = Columns[Result];
                if (DataColumn.HeaderText == HeaderValue)
                {
                    return Result;
                }
            }
            return -1;
        }

        public int FindButtonColumnIndex(string CommandName)
        {
            for (int Result = 0; Result < Columns.Count; Result++)
            {
                DataGridColumn DataColumn = Columns[Result];
                if (DataColumn is ButtonColumn)
                {
                    ButtonColumn DataButtonColumn = (ButtonColumn)DataColumn;
                    if (DataButtonColumn.CommandName == CommandName)
                    {
                        return Result;
                    }
                }
            }
            return -1;
        }

        public int FindBoundColumnIndex(string BoundDataField)
        {
            for (int Result = 0; Result < Columns.Count; Result++)
            {
                DataGridColumn DataColumn = Columns[Result];
                if (DataColumn is BoundColumn)
                {
                    BoundColumn DataBoundColumn = (BoundColumn)DataColumn;
                    if (DataBoundColumn.DataField == BoundDataField)
                    {
                        return Result;
                    }
                }
            }
            return -1;
        }

        protected override void OnItemDataBound(DataGridItemEventArgs e)
        {
            switch (e.Item.ItemType)
            {
                case ListItemType.AlternatingItem:
                case ListItemType.Item:
                case ListItemType.SelectedItem:
                case ListItemType.EditItem:
                    int DeleteButtonColumnIndex = FindButtonColumnIndex("Delete");
                    if (DeleteButtonColumnIndex >= 0)
                    {
                        LinkButton DeleteButton = (LinkButton)e.Item.Cells[DeleteButtonColumnIndex].Controls[0];
                        DeleteButton.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
                    }
                    break;
            }
            base.OnItemDataBound(e);
        }

        protected override void OnSortCommand(DataGridSortCommandEventArgs e)
        {
            if (OnGetDataSource != null)
            {
                if (SortExpression == e.SortExpression)
                {
                    SortDirection = 
                        (SortDirection == PagedGridSortDirection.Ascending)
                            ? PagedGridSortDirection.Descending 
                            : PagedGridSortDirection.Ascending;
                }
                SortExpression = e.SortExpression;
                CurrentPageIndex = 0;
                OnGetDataSource(this, e);
                DataBind();
            }

            base.OnSortCommand(e);
        }
    }
}