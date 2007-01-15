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
using SnCore.SiteMap;

public partial class AccountPictureView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        picturesView.OnGetDataSource += new EventHandler(picturesView_OnGetDataSource);
        if (!IsPostBack)
        {
            mPictureId = RequestId;
            GetPictureData(sender, e);
            GetPicturesData(sender, e);

            TransitAccountPicture p = AccountPicture;
            TransitAccount a = Account;
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("People", Request, "AccountsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(a.Name, Request, string.Format("AccountView.aspx?id={0}", a.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Pictures", Request, string.Format("AccountPicturesView.aspx?id={0}", a.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode(p.Name, Request.Url));
            StackSiteMap(sitemapdata);
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
                    SessionManager.AccountService, "GetAccountPictureById", sp_args);
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
                object[] as_args = { SessionManager.Ticket, AccountPicture.AccountId };
                mAccount = SessionManager.GetCachedItem<TransitAccount>(
                    SessionManager.AccountService, "GetAccountById", as_args);
            }
            return mAccount;
        }
    }

    void GetPicturesData(object sender, EventArgs e)
    {
        AccountPicturesQueryOptions ap = new AccountPicturesQueryOptions();
        ap.Hidden = false;
        object[] p_args = { SessionManager.Ticket, Account.Id, ap };
        picturesView.CurrentPageIndex = 0;
        picturesView.VirtualItemCount = SessionManager.GetCachedCollectionCount<TransitAccountPicture>(
            SessionManager.AccountService, "GetAccountPicturesCount", p_args);
        picturesView_OnGetDataSource(sender, e);
        picturesView.DataBind();
    }

    void GetPictureData(object sender, EventArgs e)
    {
        TransitAccountPicture p = AccountPicture;

        downloadPicture.HRef = inputPicture.Src = string.Format("AccountPicture.aspx?id={0}", p.Id);
        inputName.Text = Renderer.Render(p.Name);
        inputDescription.Text = Renderer.Render(p.Description);
        inputCreated.Text = Adjust(p.Created).ToString("d");
        inputCounter.Text = p.Counter.Total.ToString();

        TransitAccount l = Account;

        labelAccountName.Text = this.Title = string.Format("{0}: {1}",
            Renderer.Render(l.Name), string.IsNullOrEmpty(p.Name) ? "Untitled" : Renderer.Render(p.Name));

        labelAccountName.Text = this.Title = string.Format("{0}: {1}",
            Renderer.Render(l.Name), string.IsNullOrEmpty(p.Name) ? "Untitled" : Renderer.Render(p.Name));

        linkBack.NavigateUrl = string.Format("AccountView.aspx?id={0}", l.Id);
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

        panelNavigator.Visible = (p.Index >= 0);

        discussionComments.ReturnUrl = string.Format("AccountPictureView.aspx?id={0}", PictureId);
        discussionComments.DiscussionId = SessionManager.DiscussionService.GetOrCreateAccountPictureDiscussionId(
            SessionManager.Ticket, PictureId);
        discussionComments.DataBind();
    }

    public void picturesView_ItemCommand(object source, CommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Picture":
                mPictureId = int.Parse(e.CommandArgument.ToString());
                GetPictureData(source, e);
                break;
        }
    }

    void picturesView_OnGetDataSource(object sender, EventArgs e)
    {
        AccountPicturesQueryOptions ap = new AccountPicturesQueryOptions();
        ap.Hidden = false;
        ServiceQueryOptions options = new ServiceQueryOptions(picturesView.PageSize, picturesView.CurrentPageIndex);
        object[] args = { SessionManager.Ticket, Account.Id, ap, options };
        picturesView.DataSource = SessionManager.GetCachedCollection<TransitAccountPicture>(
            SessionManager.AccountService, "GetAccountPictures", args);
    }

}
