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
using SnCore.Services;
using SnCore.WebServices;

public partial class AccountPictureView : Page
{
    public void Page_Load()
    {
        try
        {
            if (!IsPostBack)
            {
                GetPictureData(RequestId);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }

    }

    void GetPictureData(int id)
    {
        object[] e_args = { SessionManager.Ticket, id };
        TransitAccountPicture p = SessionManager.GetCachedItem<TransitAccountPicture>(
            AccountService, "GetAccountPictureById", e_args);

        inputPicture.Src = string.Format("AccountPicture.aspx?id={0}", id);
        inputName.Text = Renderer.Render(p.Name);
        inputDescription.Text = Renderer.Render(p.Description);
        inputCreated.Text = Adjust(p.Created).ToString();

        object[] ae_args = { p.AccountId };
        TransitAccount l = SessionManager.GetCachedItem<TransitAccount>(
            AccountService, "GetAccountById", ae_args);

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

        if (!IsPostBack)
        {
            object[] p_args = { l.Id, null };
            picturesView.DataSource = SessionManager.GetCachedCollection<TransitAccountPicture>(
                AccountService, "GetAccountPicturesById", p_args);
            picturesView.DataBind();
        }

        object[] d_args = { id };
        discussionComments.DiscussionId = SessionManager.GetCachedCollectionCount(
            DiscussionService, "GetAccountPictureDiscussionId", d_args);

        discussionComments.DataBind();
    }

    public void picturesView_ItemCommand(object source, DataListCommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "Picture":
                    GetPictureData(int.Parse(e.CommandArgument.ToString()));
                    break;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
