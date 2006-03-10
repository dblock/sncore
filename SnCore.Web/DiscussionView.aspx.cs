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

public partial class DiscussionView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                linkRss.NavigateUrl = string.Format("DiscussionRss.aspx?id={0}", RequestId);
                discussionMain.DiscussionId = RequestId;
                discussionMain.DataBind();
                TransitDiscussion td = DiscussionService.GetDiscussionById(RequestId);
                this.Title = linkDiscussion.Text = Renderer.Render(td.Name);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
