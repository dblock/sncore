using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SnCore.WebControls
{

    public sealed class DataListPagerStyle : TableItemStyle
    {
        private PagerMode mMode = PagerMode.NextPrev;
        private string mNextPageText = "&gt;";
        private int mPageButtonCount = 10;
        private PagerPosition mPosition = PagerPosition.Bottom;
        private string mPrevPageText = "&lt;";
        private bool mVisible = true;

        public int PagersCount
        {
            get
            {
                int result = 0;
                if (HasPagerOnBottom) result++;
                if (HasPagerOnTop) result++;
                return result;
            }
        }

        public bool HasPagerOnTop
        {
            get
            {
                return (Position == PagerPosition.Top) || (Position == PagerPosition.TopAndBottom);
            }
        }

        public bool HasPagerOnBottom
        {
            get
            {
                return (Position == PagerPosition.Bottom) || (Position == PagerPosition.TopAndBottom);
            }
        }

        public PagerMode PagerMode
        {
            get
            {
                return mMode;
            }
            set
            {
                mMode = value;
            }
        }

        public string NextPageText
        {
            get
            {
                return mNextPageText;
            }
            set
            {
                mNextPageText = value;
            }
        }

        public int PageButtonCount
        {
            get
            {
                return mPageButtonCount;
            }
            set
            {
                mPageButtonCount = value;
            }
        }

        public PagerPosition Position
        {
            get
            {
                return mPosition;
            }
            set
            {
                mPosition = value;
            }
        }

        public string PrevPageText
        {
            get
            {
                return mPrevPageText;
            }
            set
            {
                mPrevPageText = value;
            }
        }

        public bool Visible
        {
            get
            {
                return mVisible;
            }
            set
            {
                mVisible = value;
            }
        }
    }

    internal sealed class DummyDataSource : ICollection
    {
        private int dataItemCount;

        public DummyDataSource(int dataItemCount)
        {
            this.dataItemCount = dataItemCount;
        }

        public IEnumerator GetEnumerator()
        {
            return new DummyDataSourceEnumerator(dataItemCount);
        }

        public void CopyTo(System.Array arr, int index)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get
            {
                return dataItemCount;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public object SyncRoot
        {
            get
            {
                return this;
            }
        }

        private class DummyDataSourceEnumerator : Object, IEnumerator
        {
            private int count;
            private int index;

            public DummyDataSourceEnumerator(int count)
            {
                this.count = count;
                this.index = -1;
            }

            public object Current
            {
                get
                {
                    return null;
                }
            }

            public bool MoveNext()
            {
                if (index + 1 < count)
                {
                    index++;
                    return true;
                }

                return false;
            }

            public void Reset()
            {
                index = 0;
            }
        }
    }

    public class PagedList : DataList
    {
        private int mFirst = -1;
        private List<Pager> mPagers = new List<Pager>();
        private List<DataListItem> mPagerListItems = new List<DataListItem>();
        private PagedDataSource mPagedDataSource = null;
        private DataListPagerStyle mPagerStyle = new DataListPagerStyle();
        private bool mAllowCustomPaging = false;

        public event EventHandler OnGetDataSource = null;

        public PagedList()
        {

        }

        public bool AllowCustomPaging
        {
            get
            {
                return mAllowCustomPaging;
            }
            set
            {
                mAllowCustomPaging = value;
            }
        }

        private List<Pager> Pagers
        {
            get
            {
                return mPagers;
            }
        }

        public PagedDataSource PagedDataSource
        {
            get
            {
                if (mPagedDataSource == null)
                {
                    mPagedDataSource = new PagedDataSource();
                    mPagedDataSource.AllowPaging = true;
                    mPagedDataSource.AllowCustomPaging = AllowCustomPaging;
                    mPagedDataSource.CurrentPageIndex = CurrentPage;
                    mPagedDataSource.VirtualCount = VirtualItemCount;
                    mPagedDataSource.PageSize = PageSize;
                    mPagedDataSource.DataSource = (IEnumerable)base.DataSource;
                }
                return mPagedDataSource;
            }
        }

        public override object DataSource
        {
            get
            {
                return base.DataSource;
            }
            set
            {
                base.DataSource = value;
                PagedDataSource.DataSource = (IEnumerable)value;
            }
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

        public override void DataBind()
        {
            base.DataSource = PagedDataSource;
            base.DataBind();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            int pagerIndex = 0;
            writer.Write("<table>");
            if (PagerStyle.HasPagerOnTop)
            {
                writer.Write("<tr>");
                Pagers[pagerIndex++].Navigator.RenderControl(writer);
                writer.Write("</tr>");
            }
            writer.Write("<tr><td>");
            base.Render(writer);
            writer.Write("</td></tr>");
            if (PagerStyle.HasPagerOnBottom)
            {
                writer.Write("<tr>");
                Pagers[pagerIndex++].Navigator.RenderControl(writer);
                writer.Write("</tr>");
            }
            writer.Write("</table>");
        }

        protected void InitializePager(Pager pager)
        {
            pager.PagedDataSource = PagedDataSource;
            pager.PageIndexChanged += new DataGridPageChangedEventHandler(Pager_PageIndexChanged);
            pager.First = First;

            DataListItem item = new DataListItem(0, ListItemType.Pager);
            item.Controls.Add(pager.Navigator);
            Controls.Add(item);

            mPagerListItems.Add(item);
        }

        public void Pager_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
        {
            First = ((Pager)sender).First;
            PagedDataSource.CurrentPageIndex = e.NewPageIndex;
            CurrentPage = e.NewPageIndex;

            if (OnGetDataSource != null)
            {
                OnGetDataSource(this, e);
            }

            DataBind();
        }

        public DataListPagerStyle PagerStyle
        {
            get
            {
                return mPagerStyle;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            foreach (Pager pager in Pagers)
            {
                pager.PreRenderCommands();
            }

            base.OnPreRender(e);
        }

        protected override void CreateControlHierarchy(bool useDataSource)
        {
            if (useDataSource)
            {
                if (!AllowCustomPaging)
                {
                    VirtualItemCount = PagedDataSource.DataSourceCount;
                }
            }
            else
            {
                PagedDataSource.DataSource = new DummyDataSource(VirtualItemCount);                
            }
            
            ClearPagers();

            for (int i = 0; i < PagerStyle.PagersCount; i++)
            {
                Pager pager = new Pager(PagedDataSource, PagerStyle);
                mPagers.Add(pager);
                InitializePager(pager);
            }

            base.CreateControlHierarchy(useDataSource);
        }

        private void ClearPagers()
        {
            foreach (DataListItem item in mPagerListItems)
                Controls.Remove(item);

            mPagerListItems.Clear();

            mPagers.Clear();
        }

        public int PageSize
        {
            get
            {
                return Math.Max(RepeatColumns, 1) * RepeatRows;
            }
        }

        public int CurrentPage
        {
            get
            {
                object o = this.ViewState["CurrentPage"];
                return o == null ? 0 : (int)o;
            }
            set
            {
                this.ViewState["CurrentPage"] = value;
            }
        }

        public int VirtualItemCount
        {
            get
            {
                object o = this.ViewState["VirtualItemCount"];
                return o == null ? 0 : (int)o;
            }
            set
            {
                this.ViewState["VirtualItemCount"] = value;
                PagedDataSource.VirtualCount = value;
            }
        }

        public int RepeatRows
        {
            get
            {
                object o = this.ViewState["RepeatRows"];
                return o == null ? 10 : (int)o;
            }
            set
            {
                this.ViewState["RepeatRows"] = value;
            }
        }
    }
}