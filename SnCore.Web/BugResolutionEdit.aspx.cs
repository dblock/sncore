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
using SnCore.SiteMap;

public partial class BugResolutionEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("Bugs", Request, "BugProjectsManage.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode("Resolutions", Request, "BugResolutionsManage.aspx"));

                if (RequestId > 0)
                {
                    TransitBugResolution t = SessionManager.BugService.GetBugResolutionById(RequestId);
                    inputName.Text = t.Name;
                    sitemapdata.Add(new SiteMapDataAttributeNode(t.Name, Request.Url));
                }
                else
                {
                    sitemapdata.Add(new SiteMapDataAttributeNode("New Resolution", Request.Url));
                }

                StackSiteMap(sitemapdata);
            }

            SetDefaultButton(manageAdd);
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
            TransitBugResolution t = new TransitBugResolution();
            t.Name = inputName.Text;
            t.Id = RequestId;
            SessionManager.BugService.CreateOrUpdateBugResolution(SessionManager.Ticket, t);
            Redirect("BugResolutionsManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
