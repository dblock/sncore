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
using SnCore.WebControls;

public partial class SystemDiscussionEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            if (IsObjectBound)
            {
                int discussion_id = SessionManager.GetCount<TransitDiscussion, string, int>(
                    Type, ObjectId, SessionManager.DiscussionService.GetOrCreateDiscussionId);

                TransitDiscussion td = SessionManager.GetPrivateInstance<TransitDiscussion, int>(
                    discussion_id, SessionManager.DiscussionService.GetDiscussionById);

                sitemapdata.Add(new SiteMapDataAttributeNode(td.ParentObjectName, string.Format("{0}&ReturnUrl={1}",
                    td.ParentObjectUri, Renderer.UrlEncode(Request.Url.PathAndQuery))));
                sitemapdata.Add(new SiteMapDataAttributeNode("Discussions", Request.Url));
            }
            else
            {
                sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountManage.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode("Discussions", Request, "SystemDiscussionsManage.aspx"));
            }

            DomainClass cs = SessionManager.GetDomainClass("Discussion");
            inputName.MaxLength = cs["Name"].MaxLengthInChars;

            inputDefaultView.DataSource = DiscussionViewType.DefaultTypes;
            inputDefaultView.DataBind();

            linkBack.NavigateUrl = ReturnUrl;

            int id = RequestId;

            if (id > 0)
            {
                TransitDiscussion tw = SessionManager.DiscussionService.GetDiscussionById(
                    SessionManager.Ticket, id);
                inputName.Text = Renderer.Render(tw.Name);
                inputDescription.Text = Renderer.Render(tw.Description);
                ListItemManager.TrySelect(inputDefaultView, tw.DefaultView);
                ListItemManager.SelectAdd(inputDefaultViewRows, tw.DefaultViewRows);
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
        tw.DefaultView = inputDefaultView.SelectedValue;
        tw.DefaultViewRows = int.Parse(inputDefaultViewRows.SelectedValue);
        tw.ObjectId = ObjectId;
        tw.ParentObjectType = Type;
        tw.Personal = IsObjectBound;
        SessionManager.CreateOrUpdate<TransitDiscussion>(
            tw, SessionManager.DiscussionService.CreateOrUpdateDiscussion);
        Redirect(ReturnUrl);
    }

    public string ReturnUrl
    {
        get
        {
            object o = Request.QueryString["ReturnUrl"];
            return (o == null ? "SystemDiscussionsManage.aspx" : o.ToString());
        }
    }


    public string Type
    {
        get
        {
            return Request["Type"];
        }
    }

    public int ObjectId
    {
        get
        {
            return GetId("ObjectId");
        }
    }

    public bool IsObjectBound
    {
        get
        {
            return (!string.IsNullOrEmpty(Type)) && (ObjectId > 0);
        }
    }
}
