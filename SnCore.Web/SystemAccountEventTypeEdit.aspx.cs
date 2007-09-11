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
using SnCore.SiteMap;
using SnCore.Data.Hibernate;

public partial class SystemAccountEventTypeEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("System Preferences", Request, "SystemPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Event Types", Request, "SystemAccountEventTypesManage.aspx"));

            DomainClass cs = SessionManager.GetDomainClass("AccountEventType");
            inputName.MaxLength = cs["Name"].MaxLengthInChars;

            if (RequestId > 0)
            {
                TransitAccountEventType t = SessionManager.EventService.GetAccountEventTypeById(
                    SessionManager.Ticket, RequestId);
                inputName.Text = t.Name;
                inputDefaultType.Checked = t.DefaultType;
                sitemapdata.Add(new SiteMapDataAttributeNode(t.Name, Request.Url));
            }
            else
            {
                sitemapdata.Add(new SiteMapDataAttributeNode("New Type", Request.Url));
            }

            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(manageAdd);
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitAccountEventType t = new TransitAccountEventType();
        t.Name = inputName.Text;
        t.Id = RequestId;
        t.DefaultType = inputDefaultType.Checked;
        SessionManager.CreateOrUpdate<TransitAccountEventType>(
            t, SessionManager.EventService.CreateOrUpdateAccountEventType);
        Redirect("SystemAccountEventTypesManage.aspx");
    }
}
