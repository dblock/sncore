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

public partial class AccountGroupPictureEdit : AuthenticatedPage
{
    public int AccountGroupId
    {
        get
        {
            return GetId("pid");
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TransitAccountGroup p = SessionManager.GroupService.GetAccountGroupById(
                SessionManager.Ticket, AccountGroupId);
            linkBack.NavigateUrl = string.Format("AccountGroupPicturesManage.aspx?id={0}", p.Id);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Groups", Request, "AccountGroupsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(p.Name, Request, string.Format("AccountGroupView.aspx?id={0}", p.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Pictures", Request, string.Format("AccountGroupPicturesManage.aspx?id={0}", p.Id)));

            if (RequestId > 0)
            {
                TransitAccountGroupPicture t = SessionManager.GroupService.GetAccountGroupPictureById(SessionManager.Ticket, RequestId);
                inputName.Text = t.Name;
                inputDescription.Text = t.Description;
                imageFull.ImageUrl = string.Format("AccountGroupPicture.aspx?id={0}&CacheDuration=0", t.Id);
                sitemapdata.Add(new SiteMapDataAttributeNode(t.Name, Request.Url));
            }
            else
            {
                sitemapdata.Add(new SiteMapDataAttributeNode("New Picture", Request.Url));
            }

            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(manageAdd);
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitAccountGroupPicture t = new TransitAccountGroupPicture();
        t.Name = inputName.Text;
        t.Description = inputDescription.Text;
        t.AccountGroupId = AccountGroupId;
        t.Id = RequestId;
        SessionManager.CreateOrUpdate<TransitAccountGroupPicture>(
            t, SessionManager.GroupService.CreateOrUpdateAccountGroupPicture);
        Redirect(string.Format("AccountGroupPicturesManage.aspx?id={0}", AccountGroupId));
    }
}
