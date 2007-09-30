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
using SnCore.SiteMap;
using AccountService;

public partial class AccountPictureView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            AccountService.TransitAccountPicture p = GetAccountPicture();
            AccountService.TransitAccount a = GetAccount();

            GetPictureData(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("People", Request, "AccountsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(a.Name, Request, string.Format("AccountView.aspx?id={0}", a.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Pictures", Request, string.Format("AccountPicturesView.aspx?id={0}", a.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode(p.Name, Request.Url));
            StackSiteMap(sitemapdata);
        }

    }

    AccountService.TransitAccountPicture GetAccountPicture()
    {
        return SessionManager.GetInstance<TransitAccountPicture, AccountService.ServiceQueryOptions, int>(
                            RequestId, SessionManager.AccountService.GetAccountPictureById);
    }

    public AccountService.TransitAccount GetAccount()
    {
        return SessionManager.GetInstance<TransitAccount, AccountService.ServiceQueryOptions, int>(
            GetAccountPicture().AccountId, SessionManager.AccountService.GetAccountById);
    }

    void GetPictureData(object sender, EventArgs e)
    {
        TransitAccountPicture p = GetAccountPicture();

        inputPicture.Src = string.Format("AccountPictureThumbnail.aspx?id={0}",
            p.Id);

        inputName.Text = Renderer.Render(p.Name);
        inputDescription.Text = Renderer.Render(p.Description);
        inputCreated.Text = SessionManager.Adjust(p.Created).ToString("d");
        inputCounter.Text = p.Counter.Total.ToString();

        TransitAccount l = GetAccount();

        labelAccountName.Text = this.Title = string.Format("{0}: {1}",
            Renderer.Render(l.Name), string.IsNullOrEmpty(p.Name) ? "Untitled" : Renderer.Render(p.Name));

        labelAccountName.Text = this.Title = string.Format("{0}: {1}",
            Renderer.Render(l.Name), string.IsNullOrEmpty(p.Name) ? "Untitled" : Renderer.Render(p.Name));

        //discussionComments.ReturnUrl = string.Format("AccountPictureView.aspx?id={0}", PictureId);
        //discussionComments.DiscussionId = SessionManager.GetCount<TransitDiscussion, string, int>(
        //    typeof(AccountPicture).Name, PictureId, SessionManager.DiscussionService.GetOrCreateDiscussionId);
        //discussionComments.DataBind();
    }

    public static string GetCommentCount(int count)
    {
        if (count == 0) return string.Empty;
        return string.Format("{0} comment{1}", count, count == 1 ? string.Empty : "s");
    }
}
