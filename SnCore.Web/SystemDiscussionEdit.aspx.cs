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

public partial class SystemDiscussionEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(manageAdd);
            if (!IsPostBack)
            {

                int id = RequestId;

                if (id > 0)
                {
                    TransitDiscussion tw = DiscussionService.GetDiscussionById(id);
                    inputName.Text = Renderer.Render(tw.Name);
                    inputDescription.Text = Renderer.Render(tw.Description);
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
            TransitDiscussion tw = new TransitDiscussion();
            tw.Name = inputName.Text;
            tw.Description = inputDescription.Text;
            tw.Id = RequestId;
            DiscussionService.AddDiscussion(SessionManager.Ticket, tw);
            Redirect("SystemDiscussionsManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }

    }
}
