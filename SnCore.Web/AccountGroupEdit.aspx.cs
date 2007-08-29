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

public partial class AccountGroupEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Groups", Request, "AccountGroupsManage.aspx"));

            linkBack.NavigateUrl = ReturnUrl;

            int id = RequestId;

            if (id > 0)
            {
                TransitAccountGroup tw = SessionManager.GroupService.GetAccountGroupById(SessionManager.Ticket, id);
                inputName.Text = tw.Name;
                inputDescription.Text = tw.Description;
                inputPrivate.Checked = tw.IsPrivate;
                sitemapdata.Add(new SiteMapDataAttributeNode(tw.Name, Request.Url));
            }
            else
            {
                sitemapdata.Add(new SiteMapDataAttributeNode("New Group", Request.Url));
            }

            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(manageAdd);
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitAccountGroup tw = new TransitAccountGroup();
        tw.Name = inputName.Text;
        tw.Description = inputDescription.Text;
        tw.IsPrivate = inputPrivate.Checked;
        tw.Id = RequestId;
        SessionManager.CreateOrUpdate<TransitAccountGroup>(
            tw, SessionManager.GroupService.CreateOrUpdateAccountGroup);
        Redirect(ReturnUrl);
    }


    public string ReturnUrl
    {
        get
        {
            object o = Request.QueryString["ReturnUrl"];
            return (o == null ? "AccountGroupsManage.aspx" : o.ToString());
        }
    }
}
