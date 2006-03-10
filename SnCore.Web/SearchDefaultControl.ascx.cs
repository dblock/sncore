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
using System.Text;

public partial class SearchDefaultControl : Control
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PageManager.SetDefaultButton(search, Controls);

        StringBuilder sb = new StringBuilder();
        sb.Append("if (typeof(Page_ClientValidate) == 'function') { ");
        sb.Append("if (Page_ClientValidate() == false) { return false; }} ");
        sb.Append("document.getElementById('" + panelSearch.ClientID + "').style.cssText = 'display: none';");
        sb.Append("document.getElementById('" + panelSearching.ClientID + "').style.cssText = '';");
        search.Attributes.Add("onclick", sb.ToString());
    }

    protected void search_Click(object sender, EventArgs e)
    {
        try
        {
            Redirect("Search.aspx?q=" + Renderer.UrlEncode(inputSearch.Text));
        }
        catch (Exception ex) 
        { 
            ReportException(ex); 
        }
    }
}
