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

public partial class AccountEventPictureEdit : AuthenticatedPage
{
    public int AccountEventId
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
            TransitAccountEvent p = SessionManager.EventService.GetAccountEventById(
                SessionManager.Ticket, AccountEventId, SessionManager.UtcOffset.Ticks);

            linkBack.NavigateUrl = string.Format("AccountEventPicturesManage.aspx?id={0}", p.Id);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Events", Request, "AccountEventsManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(p.Name, Request, string.Format("AccountEventView.aspx?id={0}", p.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Pictures", Request, string.Format("AccountEventPicturesManage.aspx?id={0}", p.Id)));

            if (RequestId > 0)
            {
                TransitAccountEventPicture t = SessionManager.EventService.GetAccountEventPictureById(SessionManager.Ticket, RequestId);
                inputName.Text = t.Name;
                inputDescription.Text = t.Description;
                imageFull.ImageUrl = string.Format("AccountEventPicture.aspx?id={0}&CacheDuration=0", t.Id);
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
        TransitAccountEventPicture t = new TransitAccountEventPicture();
        t.Name = inputName.Text;
        t.Description = inputDescription.Text;
        t.AccountEventId = AccountEventId;
        t.Id = RequestId;
        SessionManager.CreateOrUpdate<TransitAccountEventPicture>(
            t, SessionManager.EventService.CreateOrUpdateAccountEventPicture);
        Redirect(string.Format("AccountEventPicturesManage.aspx?id={0}", AccountEventId));
    }
}
