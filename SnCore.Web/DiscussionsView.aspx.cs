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

public partial class DiscussionsView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(search);
            if (!IsPostBack)
            {
                gridView.DataSource = DiscussionService.GetDiscussions();
                gridView.DataBind();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    protected void search_Click(object sender, EventArgs e)
    {
        try
        {
            Redirect("SearchDiscussionPosts.aspx?q=" + Renderer.UrlEncode(inputSearch.Text));
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
