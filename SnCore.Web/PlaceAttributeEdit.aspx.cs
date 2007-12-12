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
using System.Collections.Generic;
using SnCore.SiteMap;
using SnCore.Data.Hibernate;
using SnCore.WebControls;

public partial class PlaceAttributeEdit : AuthenticatedPage
{
    private TransitPlace mPlace = null;

    public int PlaceId
    {
        get
        {
            return GetId("aid");
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DomainClass cs = SessionManager.GetDomainClass("PlaceAttribute");
            inputUrl.MaxLength = cs["Url"].MaxLengthInChars;

            linkBack.NavigateUrl = string.Format("PlaceAttributesManage.aspx?id={0}", PlaceId);

            List<TransitAttribute> attributes = SessionManager.ObjectService.GetAttributes(
                SessionManager.Ticket, null);

            if (RequestId == 0)
            {
                attributes.Insert(0, new TransitAttribute());
            }

            listAttributes.DataSource = attributes;
            listAttributes.DataBind();

            TransitPlace p = Place;
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Places", Request, "PlacesView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(p.Name, Request, string.Format("PlaceView.aspx?id={0}", p.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Attributes", Request, string.Format("PlaceAttributesManage.aspx?id={0}", p.Id)));

            if (RequestId > 0)
            {
                TransitPlaceAttribute attribute = SessionManager.PlaceService.GetPlaceAttributeById(
                    SessionManager.Ticket, RequestId);
                inputValue.Text = Renderer.Render(attribute.Value);
                inputUrl.Text = Renderer.Render(attribute.Url);
                inputDefaultUrl.Text = Renderer.Render(attribute.Attribute.DefaultUrl);
                inputDefaultValue.Text = Renderer.Render(attribute.Attribute.DefaultValue);
                ListItemManager.TrySelect(listAttributes, attribute.AttributeId);
                listAttributes.Enabled = false;
                previewImage.ImageUrl = string.Format("SystemAttribute.aspx?id={0}", attribute.AttributeId);
                previewImage.Visible = true;
                sitemapdata.Add(new SiteMapDataAttributeNode(attribute.Attribute.Name, Request.Url));
            }
            else
            {
                sitemapdata.Add(new SiteMapDataAttributeNode("New Attribute", Request.Url));
            }

            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(manageAdd);
    }

    public TransitPlace Place
    {
        get
        {
            if (mPlace == null)
            {
                mPlace = SessionManager.PlaceService.GetPlaceById(SessionManager.Ticket, PlaceId);
            }

            return mPlace;
        }
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitPlaceAttribute attribute = new TransitPlaceAttribute();

        attribute.Url = inputUrl.Text;
        attribute.Value = inputValue.Text;
        attribute.Id = RequestId;
        attribute.AttributeId = int.Parse(listAttributes.SelectedValue);
        attribute.PlaceId = PlaceId;
        SessionManager.CreateOrUpdate<TransitPlaceAttribute>(
            attribute, SessionManager.PlaceService.CreateOrUpdatePlaceAttribute);
        Redirect(linkBack.NavigateUrl);

    }

    public void listAttributes_SelectedItemChanged(object sender, EventArgs e)
    {
        int id = 0;
        if (int.TryParse(listAttributes.SelectedValue, out id))
        {
            TransitAttribute attribute = SessionManager.ObjectService.GetAttributeById(
                SessionManager.Ticket, id);
            inputDefaultUrl.Text = Renderer.Render(attribute.DefaultUrl);
            inputDefaultValue.Text = Renderer.Render(attribute.DefaultValue);
            previewImage.ImageUrl = string.Format("SystemAttribute.aspx?id={0}", attribute.Id);
            previewImage.Visible = true;
        }
        else
        {
            previewImage.Visible = false;
            inputDefaultUrl.Text = string.Empty;
            inputDefaultValue.Text = string.Empty;
        }
    }
}
