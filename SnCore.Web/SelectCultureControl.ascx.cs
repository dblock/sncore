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
using SnCore.WebControls;
using SnCore.Services;
using System.Globalization;

public partial class SelectCultureControl : Control
{
    protected override void OnLoad(EventArgs e)
    {
        listCultures.DataSource = SessionManager.GetCollection<TransitCultureInfo>(
            SessionManager.SystemService.GetInstalledCultures);
        listCultures.DataBind();
        base.OnLoad(e);
    }

    public void listCultures_ItemCommand(object source, CommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "SelectCulture":
                // update account culture
                if (SessionManager.IsLoggedIn)
                {
                    TransitAccount t_account = SessionManager.Account;
                    t_account.LCID = int.Parse(e.CommandArgument.ToString());
                    SessionManager.CreateOrUpdate<TransitAccount>(
                        t_account, SessionManager.AccountService.CreateOrUpdateAccount);
                }
                // overwrite the cookie
                Response.Cookies.Add(new HttpCookie(SessionManager.sSnCoreCulture, e.CommandArgument.ToString()));
                Response.Redirect(Request.Url.PathAndQuery);
                break;
        }
    }
}
