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
using SnCore.Data.Hibernate;

public partial class AccountMessageFolderEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int id = RequestId;

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Messages", Request, "AccountMessageFoldersManage.aspx"));

            DomainClass cs = SessionManager.GetDomainClass("AccountMessageFolder");
            inputName.MaxLength = cs["Name"].MaxLengthInChars;

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

    public void save_Click(object sender, EventArgs e)
    {
        TransitAccountMessageFolder tw = (RequestId > 0)
            ? SessionManager.AccountService.GetAccountMessageFolderById(SessionManager.Ticket, RequestId)
            : new TransitAccountMessageFolder();

        tw.Name = inputName.Text;
        if (RequestId == 0) tw.AccountMessageFolderParentId = GetId("pid");
        SessionManager.CreateOrUpdate<TransitAccountMessageFolder>(
            tw, SessionManager.AccountService.CreateOrUpdateAccountMessageFolder);
        Redirect("AccountMessageFoldersManage.aspx");

    }
}
