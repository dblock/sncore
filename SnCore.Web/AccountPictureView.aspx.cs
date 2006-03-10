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
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                TransitAccountPicture p = AccountService.GetAccountPictureById(SessionManager.Ticket, RequestId);
                inputPicture.Src = "AccountPicture.aspx?id=" + RequestId;
                inputName.Text = Renderer.Render(p.Name);
                inputDescription.Text = Renderer.Render(p.Description);
                inputCreated.Text = Adjust(p.Created).ToString();

                TransitAccount a = AccountService.GetAccountById(p.AccountId);

                linkAccount.Text = Renderer.Render(a.Name);
                linkAccount.NavigateUrl = "AccountView.aspx?id=" + p.AccountId;
                linkPictures.NavigateUrl = "AccountPicturesView.aspx?id=" + p.AccountId;
                linkPicture.Text = Renderer.Render((p.Name != null && p.Name.Length > 0) ? p.Name : "Untitled");

                linkComments.Visible = p.CommentCount > 0;
                linkComments.Text = string.Format("{0} comment{1}",
                    (p.CommentCount > 0) ? p.CommentCount.ToString() : "no",
                    (p.CommentCount == 1) ? "" : "s");

                discussionComments.DiscussionId = DiscussionService.GetPictureDiscussionId(RequestId);
                discussionComments.DataBind();

                this.Title = string.Format("{0}'s {1}", Renderer.Render(a.Name), Renderer.Render(p.Name));
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }

    }
}
