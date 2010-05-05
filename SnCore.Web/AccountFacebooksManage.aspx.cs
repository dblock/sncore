using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SnCore.WebServices;
using SnCore.Services;
using SnCore.Tools.Web;
using SnCore.SiteMap;

public partial class AccountFacebooksManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
        if (!IsPostBack)
        {
            string facebookmode = Request["facebook.login"];
            if (!string.IsNullOrEmpty(facebookmode))
            {
                FacebookPageManager facebook = new FacebookPageManager(SessionManager);
                SortedList<string, string> facebookCookies = facebook.GetFacebookCookies(HttpContext.Current.Request.Cookies);
                if (facebookCookies.Count > 0)
                {
                    List<String> keys = new List<String>(facebookCookies.Keys);
                    List<String> values = new List<String>(facebookCookies.Values);
                    SessionManager.AccountService.CreateAccountFacebook(SessionManager.Ticket, 
                        HttpContext.Current.Request.Cookies[facebook.FacebookAPIKey].Value, keys.ToArray(), values.ToArray());
                    Redirect(Request.Path);
                }
            }

            gridManage_OnGetDataSource(sender, e);
            gridManage.DataBind();

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Facebook", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        gridManage.DataSource = SessionManager.AccountService.GetAccountFacebooks(
            SessionManager.Ticket, SessionManager.AccountId, null);
    }


    private enum Cells
    {
        id = 0
    };


    public void gridManage_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
        switch (e.CommandName)
        {
            case "Delete":
                SessionManager.Delete<TransitAccountFacebook>(id, SessionManager.AccountService.DeleteAccountFacebook);
                Redirect("AccountFacebooksManage.aspx");
                break;
        }
    }
}
