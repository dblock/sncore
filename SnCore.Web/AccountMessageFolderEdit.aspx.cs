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
using SnCore.SiteMap;

public partial class AccountMessageFolderEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                int id = RequestId;

                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode("Messages", Request, "AccountMessageFoldersManage.aspx"));

                if (id > 0)
                {
                    TransitAccountMessageFolder tw = SessionManager.AccountService.GetAccountMessageFolderById(SessionManager.Ticket, id);
                    inputName.Text = Renderer.Render(tw.Name);
                    sitemapdata.Add(new SiteMapDataAttributeNode(tw.Name, Request.Url));
                }
                else
                {
                    sitemapdata.Add(new SiteMapDataAttributeNode("New Folder", Request.Url));
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
            TransitAccountMessageFolder tw = (RequestId > 0) 
                ? SessionManager.AccountService.GetAccountMessageFolderById(SessionManager.Ticket, RequestId) 
                : new TransitAccountMessageFolder();
            
            tw.Name = inputName.Text;
            if (RequestId == 0) tw.AccountMessageFolderParentId = GetId("pid");
            SessionManager.AccountService.AddAccountMessageFolder(SessionManager.Ticket, tw);
            Redirect("AccountMessageFoldersManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }

    }
}
