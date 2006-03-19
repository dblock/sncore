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

public partial class _Default : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                labelDescription.Text = Renderer.Render(
                    SystemService.GetConfigurationByNameWithDefault(
                        "SnCore.Description", "SNCore description not set.").Value);
                
                accountsNewMain.DataBind();

                websiteBlog.BlogId = int.Parse(SystemService.GetConfigurationByNameWithDefault(
                        "SnCore.Blog.Id", "0").Value);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}