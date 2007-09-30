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

public partial class SnCoreMasterPage : MasterPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Page.Title = string.Format("{0} - {1}", 
                SessionManager.WebsiteTitle, Page.Title);

            siteMapPathRoot.Text = string.Format("{0} &gt;",
                Renderer.Render(SessionManager.WebsiteName));

            if (SessionManager.IsLoggedIn)
            {
                siteMapPathAccountName.Text = string.Format("{0} &gt;",
                    Renderer.Render(SessionManager.Account.Name));
            }
        }
    }
}
