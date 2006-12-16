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
using System.Collections.Generic;
using SnCore.Tools.Web;

public partial class AccountFeaturedViewControl : Control
{
    private TransitFeature mFeature = null;
    private TransitAccount mAccount = null;

    public void Page_Load()
    {
        try
        {
            if (!IsPostBack)
            {
                panelFeatured.Visible = (Feature != null);

                if (Feature != null && Account != null)
                {
                    linkFeature2.HRef = linkFeature3.HRef = string.Format("AccountView.aspx?id={0}", Feature.DataRowId);
                    labelFeatureName.Text = Render(Account.Name);
                    labelFeatureDescription.Text = Render(Account.City);
                    imgFeature.Src = string.Format("AccountPictureThumbnail.aspx?id={0}", Account.PictureId);
                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public TransitFeature Feature
    {
        get
        {
            if (mFeature == null)
            {
                object[] args = { "Account" };
                mFeature = SessionManager.GetCachedItem<TransitFeature>(
                    SessionManager.SystemService, "GetLatestFeature", args);
            }

            return mFeature;
        }
    }

    public TransitAccount Account
    {
        get
        {
            if (mAccount == null)
            {
                object[] args = { Feature.DataRowId };
                mAccount = SessionManager.GetCachedItem<TransitAccount>(
                    SessionManager.AccountService, "GetAccountById", args);
            }

            return mAccount;
        }
    }
}
