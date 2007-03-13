using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
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

public partial class AccountGroupPlaceAdd : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TransitPlace place = SessionManager.GetInstance<TransitPlace, int>(
                RequestId, SessionManager.PlaceService.GetPlaceById);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Places", Request, "PlacesView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(place.Name, Request, string.Format("PlaceView.aspx?id={0}", place.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Add to Group", Request.Url));
            StackSiteMap(sitemapdata);

            placeName.Text = Renderer.Render(place.Name);
            placeId.Text = "#" + place.Id.ToString();

            placeType.Text = Renderer.Render(place.Type);
            placeImage.ImageUrl = string.Format("PlacePictureThumbnail.aspx?id={0}", place.PictureId);

            linkBack.NavigateUrl = string.Format("PlaceView.aspx?id={0}", RequestId);

            List<TransitAccountGroupAccount> accounts = SessionManager.GroupService.GetAccountGroupAccountsByAccountId(
                SessionManager.Ticket, SessionManager.AccountId, null);

            if (accounts.Count == 0)
            {
                ReportWarning("You aren't a member of any groups.");
                listGroups.Enabled = false;
                return;
            }

            accounts.Insert(0, new TransitAccountGroupAccount());
            listGroups.DataSource = accounts;
            listGroups.DataBind();
        }
    }

    public void listGroups_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            TransitAccountGroupPlace t_instance = new TransitAccountGroupPlace();
            t_instance.AccountGroupId = int.Parse(listGroups.SelectedValue);
            t_instance.PlaceId = RequestId;
            SessionManager.GroupService.CreateOrUpdateAccountGroupPlace(SessionManager.Ticket, t_instance);
            Redirect(string.Format("AccountGroupView.aspx?id={0}", listGroups.SelectedValue));
        }
        catch (Exception ex)
        {
            listGroups.SelectedIndex = 0;
            ReportException(ex);
        }
    }
}
