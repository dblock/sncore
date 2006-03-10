using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SnCore.WebControls
{
    public class Pager : Control
    {
        private PagedDataSource mPagedDataSource = null;

        private TableCell mNavigator = null;
        private TableCell mNumbers = null;
        private ArrayList mPageButtons = null;
        private LinkButton mNextPageButton = null;
        private LinkButton mPrevPageButton = null;
        private LinkButton mNextSectionButton = null;
        private LinkButton mPrevSectionButton = null;
        private string mCssClass = null;
        private int mPageCounters = 10;
        private int mColumnSpan = 1;
        private int mFirst = -1;
        private string mNextPageText = "Next";
        private string mPrevPageText = "Prev";
        private HorizontalAlign mHorizontalAlign = HorizontalAlign.NotSet;

        public event DataGridPageChangedEventHandler PageIndexChanged;

        public Pager()
        {

        }

        public Pager(PagedDataSource pagedDataSource, DataListPagerStyle pagerStyle)
            : this(pagedDataSource)
        {
            NextPageText = pagerStyle.NextPageText;
            PrevPageText = pagerStyle.PrevPageText;
            CssClass = pagerStyle.CssClass;
            HorizontalAlign = pagerStyle.HorizontalAlign;
        }

        public Pager(PagedDataSource pagedDataSource)
        {
            PagedDataSource = pagedDataSource;
        }

        public Pager(PagedDataSource pagedDataSource, int columnSpan)
            : this(pagedDataSource)
        {
            ColumnSpan = columnSpan;
        }

        public Pager(PagedDataSource pagedDataSource, int columnSpan, DataGridPagerStyle pagerStyle)
            : this(pagedDataSource, columnSpan)
        {
            NextPageText = pagerStyle.NextPageText;
            PrevPageText = pagerStyle.PrevPageText;
            CssClass = pagerStyle.CssClass;
            HorizontalAlign = pagerStyle.HorizontalAlign;
        }

        public ArrayList PageButtons
        {
            get
            {
                if (mPageButtons == null)
                {
                    mPageButtons = new ArrayList();
                }
                return mPageButtons;
            }
            set
            {
                mPageButtons = value;
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

        public int ColumnSpan
        {
            get
            {
                return mColumnSpan;
            }
            set
            {
                mColumnSpan = value;
            }
        }

        private HorizontalAlign HorizontalAlign
        {
            get
            {
                return mHorizontalAlign;
            }
            set
            {
                mHorizontalAlign = value;
            }
        }

        private string PrevPageText
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

        private string NextPageText
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

        public LinkButton PrevPageButton
        {
            get
            {
                if (mPrevPageButton == null)
                {
                    mPrevPageButton = new LinkButton();
                    mPrevPageButton.Text = PrevPageText;
                    mPrevPageButton.CommandName = DataGrid.PageCommandName;
                    mPrevPageButton.Command += new CommandEventHandler(PageCommand_Click);
                    mPrevPageButton.CausesValidation = false;
                }
                return mPrevPageButton;
            }
        }

        public LinkButton NextPageButton
        {
            get
            {
                if (mNextPageButton == null)
                {
                    mNextPageButton = new LinkButton();
                    mNextPageButton.Text = NextPageText;
                    mNextPageButton.CommandName = DataGrid.PageCommandName;
                    mNextPageButton.Command += new CommandEventHandler(PageCommand_Click);
                    mNextPageButton.CausesValidation = false;
                }
                return mNextPageButton;
            }
        }

        public TableCell Navigator
        {
            get
            {
                if (mNavigator == null)
                {
                    mNavigator = new TableCell();
                    mNavigator.HorizontalAlign = HorizontalAlign;

                    if (PagedDataSource.PageCount > 1)
                    {
                        Table NavigatorTable = new Table();
                        TableRow NavigatorRow = new TableRow();

                        mNavigator.ColumnSpan = ColumnSpan;
                        mNavigator.Controls.Add(NavigatorTable);

                        NavigatorTable.CssClass = "GridPager";
                        NavigatorTable.Rows.Add(NavigatorRow);

                        NavigatorRow.Cells.Add(Numbers);
                    }
                }
                return mNavigator;
            }
            set
            {
                mNavigator = value;
            }
        }

        public string CssClass
        {
            get
            {
                return mCssClass;
            }
            set
            {
                mCssClass = value;
            }
        }

        protected TableCell Numbers
        {
            get
            {
                if (mNumbers == null)
                {
                    mNumbers = new TableCell();

                    mNumbers.HorizontalAlign = HorizontalAlign.Center;
                    mNumbers.CssClass = CssClass;

                    if (PagedDataSource.PageCount > 1)
                    {
                        mNumbers.Controls.Add(PrevPageButton);
                        mNumbers.Controls.Add(new LiteralControl("&nbsp;|"));
                        mNumbers.Controls.Add(PrevSectionButton);

                        for (int i = 0; i < PagedDataSource.PageCount; i++)
                        {
                            LinkButton button = new LinkButton();
                            button.Text = "&nbsp;" + (i + 1).ToString() + "&nbsp;";
                            button.CommandName = DataGrid.PageCommandName;
                            button.CommandArgument = i.ToString();
                            button.Command += new CommandEventHandler(PageCommand_Click);
                            button.CausesValidation = false;
                            PageButtons.Add(button);
                            mNumbers.Controls.Add(button);
                        }

                        mNumbers.Controls.Add(NextSectionButton);
                        mNumbers.Controls.Add(new LiteralControl("|&nbsp;"));
                        mNumbers.Controls.Add(NextPageButton);
                    }
                }
                return mNumbers;
            }
        }

        public void PageCommand_Click(object s, CommandEventArgs e)
        {
            First = Convert.ToInt32(e.CommandArgument) - PageCounters / 2;
            PageIndexChangedCommand(s, e);
        }

        //protected override void CreateChildControls()
        //{
        //    base.CreateChildControls();
        //    Controls.Add(Navigator);
        //}

        public void PreRenderCommands()
        {
            PrevPageButton.Enabled = (!PagedDataSource.IsFirstPage);
            PrevPageButton.CommandArgument = (PagedDataSource.CurrentPageIndex - 1).ToString();

            NextPageButton.Enabled = (!PagedDataSource.IsLastPage);
            NextPageButton.CommandArgument = (PagedDataSource.CurrentPageIndex + 1).ToString();

            NextSectionButton.Visible = Last < PagedDataSource.PageCount;
            NextSectionButton.CommandArgument = PagedDataSource.CurrentPageIndex.ToString();

            PrevSectionButton.Visible = First > 0;
            PrevSectionButton.CommandArgument = PagedDataSource.CurrentPageIndex.ToString();

            foreach (LinkButton Button in PageButtons)
            {
                int TargetPage = Convert.ToInt32(Button.CommandArgument);
                Button.Enabled = (PagedDataSource.CurrentPageIndex != TargetPage);
                Button.Visible = TargetPage >= First && TargetPage <= Last;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            PreRenderCommands();
            base.OnPreRender(e);
        }

        public int PageCounters
        {
            get
            {
                return mPageCounters;
            }
            set
            {
                mPageCounters = value;
            }
        }

        public int First
        {
            get
            {
                if (mFirst == -1)
                {
                    object ViewStateCounter = ViewState[UniqueID + ":First"];
                    if (ViewStateCounter != null)
                    {
                        mFirst = (int)ViewStateCounter;
                    }
                    else
                    {
                        mFirst = PagedDataSource.CurrentPageIndex - PageCounters / 2;
                    }
                    mFirst = (mFirst >= 0) ? mFirst : 0;
                }
                return mFirst;
            }
            set
            {
                mFirst = value;

                if (mFirst >= PagedDataSource.PageCount)
                {
                    mFirst = PagedDataSource.PageCount - PageCounters / 2;
                }

                if (mFirst < 0)
                {
                    mFirst = 0;
                }

                ViewState[UniqueID + ":First"] = mFirst;
            }
        }

        public int Last
        {
            get
            {
                int LastCounter = First + PageCounters;
                return LastCounter < PagedDataSource.PageCount ? LastCounter : PagedDataSource.PageCount;
            }
        }

        public LinkButton PrevSectionButton
        {
            get
            {
                if (mPrevSectionButton == null)
                {
                    mPrevSectionButton = new LinkButton();
                    mPrevSectionButton.Text = "&nbsp;...&nbsp;";
                    mPrevSectionButton.CommandName = DataGrid.PageCommandName;
                    mPrevSectionButton.Command += new CommandEventHandler(PrevSectionButton_Command);
                    mPrevSectionButton.CausesValidation = false;
                }
                return mPrevSectionButton;
            }
        }

        public LinkButton NextSectionButton
        {
            get
            {
                if (mNextSectionButton == null)
                {
                    mNextSectionButton = new LinkButton();
                    mNextSectionButton.Text = "&nbsp;...&nbsp;";
                    mNextSectionButton.CommandName = DataGrid.PageCommandName;
                    mNextSectionButton.Command += new CommandEventHandler(NextSectionButton_Command);
                    mNextSectionButton.CausesValidation = false;
                }
                return mNextSectionButton;
            }
        }

        public void NextSectionButton_Command(object s, CommandEventArgs e)
        {
            First = First + PageCounters;
            PageIndexChangedCommand(s, e);
        }

        public void PrevSectionButton_Command(object s, CommandEventArgs e)
        {
            First = First - PageCounters;
            PageIndexChangedCommand(s, e);
        }

        public void PageIndexChangedCommand(object s, CommandEventArgs e)
        {
            if (PageIndexChanged != null)
            {
                PageIndexChanged(this, new DataGridPageChangedEventArgs(s, Convert.ToInt32(e.CommandArgument)));
            }
            PagedDataSource.CurrentPageIndex = Convert.ToInt32(e.CommandArgument);
            PreRenderCommands();
        }
    }
}