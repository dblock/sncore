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

public partial class AccountPictureEdit : AuthenticatedPage
{
    public void Page_Load()
    {
        try
        {
            SetDefaultButton(manageAdd);
            if (!IsPostBack)
            {
                int id = RequestId;

                if (id > 0)
                {
                    TransitAccountPicture tw = AccountService.GetAccountPictureById(SessionManager.Ticket, id, null);
                    this.Title = inputName.Text = Renderer.Render(tw.Name);
                    inputDescription.Text = tw.Description;
                    inputPictureThumbnail.Src = string.Format("AccountPictureThumbnail.aspx?id={0}&CacheDuration=0", tw.Id);
                    inputHidden.Checked = tw.Hidden;

                    discussionComments.DiscussionId = DiscussionService.GetAccountPictureDiscussionId(id);
                    discussionComments.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void save_Click(object sender, EventArgs e)
    {
        try
        {
            TransitAccountPictureWithBitmap tw = new TransitAccountPictureWithBitmap();
            tw.Name = inputName.Text;
            tw.Description = inputDescription.Text;
            tw.Id = RequestId;
            tw.Hidden = inputHidden.Checked;
            AccountService.AddAccountPicture(SessionManager.Ticket, tw);
            Redirect("AccountPicturesManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }

    }
}
