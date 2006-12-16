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
using Wilco.Web.UI;
using SnCore.WebServices;
using SnCore.Services;
using SnCore.Tools.Web;

public partial class AccountLicenseViewControl : Control
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
        if (!IsPostBack)
        {
            object[] args = { SessionManager.Ticket, AccountId };
            TransitAccountLicense tal = SessionManager.GetCachedItem<TransitAccountLicense>(
                SessionManager.LicenseService, "GetAccountLicenseByAccountId", args);

            panelLicense.Visible = (tal != null);

            if (tal != null && !string.IsNullOrEmpty(tal.LicenseUrl))
            {
                licenseImage.Src = tal.ImageUrl;
                licenseLink.HRef = tal.LicenseUrl;
                licenseImage.Alt = Renderer.Render(tal.Name);
            }
        }
    }
}
