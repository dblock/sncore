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

public partial class DiscussionThreadMove : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                TransitDiscussionThread tt = DiscussionService.GetDiscussionThreadById(
                    SessionManager.Ticket, RequestId);

                TransitDiscussion td = DiscussionService.GetDiscussionById(tt.DiscussionId);

                if (td.Personal)
                {
                    throw new Exception("You can only move posts for public discussions.");
                }

                TransitDiscussionPost tp = DiscussionService.GetDiscussionThreadPost(
                    SessionManager.Ticket, tt.Id);

                this.Title = Renderer.Render(td.Name);
                linkDiscussion.Text = Renderer.Render(td.Name);
                linkThread.Text = Renderer.Render(tp.Subject);

                listDiscussions.DataSource = DiscussionService.GetDiscussions(null);
                listDiscussions.DataBind();

                listDiscussions.Items.FindByValue(tt.DiscussionId.ToString()).Selected = true;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void move_Click(object sender, EventArgs args)
    {
        try
        {
            DiscussionService.MoveDiscussionThread(
                SessionManager.Ticket,
                RequestId,
                int.Parse(listDiscussions.SelectedValue));

            ReportInfo("Discussion thread moved.");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
