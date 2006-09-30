using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SnCore.Tools.Web;
using SnCore.WebServices;
using SnCore.Services;

public partial class AccountPictureView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            picturesView.OnGetDataSource += new EventHandler(picturesView_OnGetDataSource);
            if (!IsPostBack)
            {
                mPictureId = RequestId;
                GetPictureData(sender, e);
                GetPicturesData(sender, e);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }

    }

    private int mPictureId = 0;

    public int PictureId
    {
        get
        {
            if (mPictureId == 0)
            {
                mPictureId = RequestId;
            }
            return mPictureId;
        }
    }

    private TransitAccountPicture mAccountPicture = null;

    TransitAccountPicture AccountPicture
    {
        get
        {
            if (mAccountPicture == null)
            {
                object[] sp_args = { SessionManager.Ticket, PictureId };
                mAccountPicture = SessionManager.GetCachedItem<TransitAccountPicture>(
                    AccountService, "GetAccountPictureById", sp_args);
            }
            return mAccountPicture;
        }
    }

    private TransitAccount mAccount = null;

    public TransitAccount Account
    {
        get
        {
            if (mAccount == null)
            {
                object[] as_args = { AccountPicture.AccountId };
                mAccount = SessionManager.GetCachedItem<TransitAccount>(
                    AccountService, "GetAccountById", as_args);
            }
            return mAccount;
        }
    }

    void GetPicturesData(object sender, EventArgs e)
    {
        object[] p_args = { Account.Id };
        picturesView.CurrentPageIndex = 0;
        picturesView.VirtualItemCount = SessionManager.GetCachedCollectionCount(
            AccountService, "GetAccountPicturesCountById", p_args);
        picturesView_OnGetDataSource(sender, e);
        picturesView.DataBind();
    }

    void GetPictureData(object sender, EventArgs e)
    {
        TransitAccountPicture p = AccountPicture;

        inputPicture.Src = string.Format("AccountPicture.aspx?id={0}", p.Id);
        inputName.Text = Renderer.Render(p.Name);
        inputDescription.Text = Renderer.Render(p.Description);
        inputCreated.Text = Adjust(p.Created).ToString();

        TransitAccount l = Account;

        labelAccountName.Text = this.Title = string.Format("{0}: {1}",
            Renderer.Render(l.Name), string.IsNullOrEmpty(p.Name) ? "Untitled" : Renderer.Render(p.Name));

        labelAccountName.Text = this.Title = string.Format("{0}: {1}",
            Renderer.Render(l.Name), string.IsNullOrEmpty(p.Name) ? "Untitled" : Renderer.Render(p.Name));

        linkAccount.Text = Renderer.Render(l.Name);
        linkAccount.NavigateUrl = "AccountView.aspx?id=" + p.AccountId;
        linkPictures.NavigateUrl = "AccountPicturesView.aspx?id=" + p.AccountId;
        linkPicture.Text = Renderer.Render((p.Name != null && p.Name.Length > 0) ? p.Name : "Untitled");

        linkBack.NavigateUrl = linkAccount.NavigateUrl = string.Format("AccountView.aspx?id={0}", l.Id);
        linkBack.Text = string.Format("&#187; Back to {0}", Renderer.Render(l.Name));
        linkComments.Visible = p.CommentCount > 0;
        linkComments.Text = string.Format("&#187; {0} comment{1}",
            (p.CommentCount > 0) ? p.CommentCount.ToString() : "no",
            (p.CommentCount == 1) ? "" : "s");

        linkPrev.Enabled = p.PrevId > 0;
        linkPrev.CommandArgument = p.PrevId.ToString();
        linkNext.Enabled = p.NextId > 0;
        linkNext.CommandArgument = p.NextId.ToString();
        labelIndex.Text = string.Format("{0} / {1}", p.Index + 1, p.Count); 

        discussionComments.DiscussionId = DiscussionService.GetAccountPictureDiscussionId(PictureId);
        discussionComments.DataBind();
    }

    public void picturesView_ItemCommand(object source, CommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "Picture":
                    mPictureId = int.Parse(e.CommandArgument.ToString());
                    GetPictureData(source, e);
                    break;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void picturesView_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions options = new ServiceQueryOptions(picturesView.PageSize, picturesView.CurrentPageIndex);
            object[] args = { Account.Id, options };
            picturesView.DataSource = SessionManager.GetCachedCollection<TransitAccountPicture>(
                AccountService, "GetAccountPicturesById", args);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

}
