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
using SnCore.WebServices;
using SnCore.Services;
using SnCore.SiteMap;

public partial class AccountAttributesManage : AuthenticatedPage
{
    private TransitAccount mAccount = null;
    public TransitAccount Account
    {
        get
        {
            if (mAccount == null)
            {
                mAccount = SessionManager.AccountService.GetAccountById(RequestId);
            }

            return mAccount;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        linkNew.NavigateUrl = string.Format("AccountAttributeEdit.aspx?aid={0}", RequestId);
        accountLink.HRef = string.Format("AccountView.aspx?id={0}", RequestId);
        accountImage.Src = string.Format("AccountPictureThumbnail.aspx?id={0}", Account.PictureId);
        accountName.Text = Render(Account.Name);

        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("People", Request, "AccountsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(Account.Name, Request, string.Format("AccountView.aspx?id={0}", Account.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Attributes", Request.Url));

            GetData(sender, e);

            StackSiteMap(sitemapdata);
        }
    }

    public void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.AccountService.GetAccountAttributesCountById(RequestId);
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    private enum Cells
    {
        id = 0
    };

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageSize = gridManage.PageSize;
        options.PageNumber = gridManage.CurrentPageIndex;
        gridManage.DataSource = SessionManager.AccountService.GetAccountAttributesById(
            RequestId, options);
    }

    public void gridManage_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Delete":
                int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                SessionManager.AccountService.DeleteAccountAttribute(SessionManager.Ticket, id);
                ReportInfo("Account attribute deleted.");
                gridManage.CurrentPageIndex = 0;
                gridManage_OnGetDataSource(sender, e);
                gridManage.DataBind();
                break;
        }
    }
}
