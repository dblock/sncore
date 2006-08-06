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
using Wilco.Web.UI;
using SnCore.Services;
using System.Collections.Generic;

public partial class AccountProfilesViewControl : Control
{
    public int AccountId
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<int>(ViewState, "AccountId", 0);
        }
        set
        {
            ViewState["AccountId"] = value;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                object[] args = { AccountId };
                List<TransitAccountProfile> profiles = SessionManager.GetCachedCollection<TransitAccountProfile>(
                    AccountService, "GetAccountProfilesById", args);
                foreach (TransitAccountProfile profile in profiles)
                {
                    labelAboutMe.Text = RenderEx(profile.AboutSelf);
                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        panelAboutMe.Visible = (labelAboutMe.Text.Length > 0);
        base.OnPreRender(e);
    }

}
