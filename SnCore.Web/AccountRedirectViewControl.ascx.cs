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
using SnCore.Services;
using Wilco.Web.UI;

public partial class AccountRedirectViewControl : Control
{
    public int AccountId
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<int>(ViewState, "AccountId", SessionManager.AccountId);
        }
        set
        {
            ViewState["AccountId"] = value;
        }
    }

    public string TargetUri
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<string>(ViewState, "TargetUri", string.Empty);
        }
        set
        {
            ViewState["TargetUri"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TransitAccountRedirect t_instance = SessionManager.GetInstance<TransitAccountRedirect, int, string>(
                AccountId, TargetUri, SessionManager.AccountService.GetAccountRedirectByTargetUri);

            if (t_instance != null)
            {
                linkRedirect.Visible = true;
                linkRedirect.NavigateUrl = t_instance.SourceUri;
                linkRedirect.Text = string.Format("{0}/{1}", SessionManager.WebsiteUrl, t_instance.SourceUri);
            }
            else
            {
                linkRedirect.Visible = false;
            }
        }
    }
}
