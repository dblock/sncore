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

public partial class AccountStoryPictureEdit : AuthenticatedPage
{
    public int AccountStoryId
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
            TransitAccountStory p = SessionManager.StoryService.GetAccountStoryById(SessionManager.Ticket, AccountStoryId);
            linkBack.NavigateUrl = string.Format("AccountStoryPicturesManage.aspx?id={0}", p.Id);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Stories", Request, "AccountStoriesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(p.Name, Request, string.Format("AccountStoryView.aspx?id={0}", p.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Pictures", Request, string.Format("AccountStoryPicturesManage.aspx?id={0}", p.Id)));

            if (RequestId > 0)
            {
                TransitAccountStoryPicture t = SessionManager.StoryService.GetAccountStoryPictureById(SessionManager.Ticket, RequestId);
                inputName.Text = t.Name;
                imageFull.ImageUrl = string.Format("AccountStoryPicture.aspx?id={0}&CacheDuration=0", t.Id);
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
        TransitAccountStoryPicture t = new TransitAccountStoryPicture();
        t.Name = inputName.Text;
        t.AccountStoryId = AccountStoryId;
        t.Id = RequestId;
        SessionManager.CreateOrUpdate<TransitAccountStoryPicture>(
            t, SessionManager.StoryService.CreateOrUpdateAccountStoryPicture);
        Redirect(string.Format("AccountStoryPicturesManage.aspx?id={0}", AccountStoryId));
    }
}
