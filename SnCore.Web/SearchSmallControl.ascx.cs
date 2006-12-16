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
using System.Web.Services.Protocols;
using SnCore.Tools.Web;

public partial class SearchSmallControl : Control
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PageManager.SetDefaultButton(search, Controls);
    }

    protected void search_Click(object sender, EventArgs e)
    {
        Redirect("Search.aspx?q=" + Renderer.UrlEncode(inputSearch.Text));
    }
}
