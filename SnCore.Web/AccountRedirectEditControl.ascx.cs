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
using SnCore.WebServices;
using Wilco.Web.UI;

public partial class AccountRedirectEditControl : Control
{
    public int RedirectId
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<int>(ViewState, "RedirectId", 0);
        }
        set
        {
            ViewState["RedirectId"] = value;
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
            TransitAccountRedirect redirect = SessionManager.AccountService.GetAccountRedirectByTargetUri(
                SessionManager.Ticket, TargetUri);

            if (redirect != null)
            {
                RedirectId = redirect.Id;
                inputSource.Text = redirect.SourceUri;
                linkSource.NavigateUrl = redirect.SourceUri;
            }
        }

        PageManager.SetDefaultButton(btnSave, Controls);
    }

    public void save(object sender, EventArgs e)
    {
        TransitAccountRedirect redirect = new TransitAccountRedirect();
        redirect.Id = RedirectId;
        redirect.AccountId = SessionManager.Account.Id;
        linkSource.NavigateUrl = redirect.SourceUri = inputSource.Text;
        redirect.TargetUri = TargetUri;

        if (string.IsNullOrEmpty(inputSource.Text) && (RedirectId > 0))
        {
            SessionManager.AccountService.DeleteAccountRedirect(SessionManager.Ticket, RedirectId);
            RedirectId = 0;
            ReportInfo("Redirect deleted, it may take a few minutes to de-activate.");
        }
        else
        {
            RedirectId = redirect.Id = SessionManager.AccountService.AddAccountRedirect(SessionManager.Ticket, redirect);
            ReportInfo("Redirect updated, it may take a few minutes to activate.");
        }

        panelPermanentRedirect.Update();
    }
}
