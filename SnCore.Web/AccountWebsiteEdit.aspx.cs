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

public partial class AccountWebsiteEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Websites", Request, "AccountWebsitesManage.aspx"));

            DomainClass cs = SessionManager.GetDomainClass("AccountWebsite");
            inputName.MaxLength = cs["Name"].MaxLengthInChars;
            inputUrl.MaxLength = cs["Url"].MaxLengthInChars;

            int id = RequestId;

            if (id > 0)
            {
                TransitAccountWebsite tw = SessionManager.AccountService.GetAccountWebsiteById(SessionManager.Ticket, id);
                inputName.Text = tw.Name;
                inputUrl.Text = tw.Url;
                inputDescription.Text = tw.Description;
                sitemapdata.Add(new SiteMapDataAttributeNode(tw.Name, Request.Url));
            }
            else
            {
                sitemapdata.Add(new SiteMapDataAttributeNode("New Website", Request.Url));
            }

            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(manageAdd);
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitAccountWebsite tw = new TransitAccountWebsite();
        tw.Name = inputName.Text;

        if (!Uri.IsWellFormedUriString(inputUrl.Text, UriKind.Absolute))
            inputUrl.Text = "http://" + inputUrl.Text;

        tw.Url = inputUrl.Text;
        tw.Description = inputDescription.Text;
        tw.Id = RequestId;
        SessionManager.CreateOrUpdate<TransitAccountWebsite>(
            tw, SessionManager.AccountService.CreateOrUpdateAccountWebsite);
        Redirect("AccountWebsitesManage.aspx");

    }
}
