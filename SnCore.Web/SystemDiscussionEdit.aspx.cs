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

public partial class SystemDiscussionEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Discussions", Request, "SystemDiscussionsManage.aspx"));

            DomainClass cs = SessionManager.GetDomainClass("Discussion");
            inputName.MaxLength = cs["Name"].MaxLengthInChars;

            int id = RequestId;

            if (id > 0)
            {
                TransitDiscussion tw = SessionManager.DiscussionService.GetDiscussionById(
                    SessionManager.Ticket, id);
                inputName.Text = Renderer.Render(tw.Name);
                inputDescription.Text = Renderer.Render(tw.Description);
                sitemapdata.Add(new SiteMapDataAttributeNode(tw.Name, Request.Url));
            }
            else
            {
                sitemapdata.Add(new SiteMapDataAttributeNode("New Discussion", Request.Url));
            }

            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(manageAdd);
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitDiscussion tw = new TransitDiscussion();
        tw.Name = inputName.Text;
        tw.Description = inputDescription.Text;
        tw.Id = RequestId;
        SessionManager.CreateOrUpdate<TransitDiscussion>(
            tw, SessionManager.DiscussionService.CreateOrUpdateDiscussion);
        Redirect("SystemDiscussionsManage.aspx");

    }
}
