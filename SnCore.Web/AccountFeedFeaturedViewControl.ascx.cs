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

public partial class AccountFeedFeaturedViewControl : Control
{
    private TransitFeature mFeature = null;
    private TransitAccountFeed mAccountFeed = null;

    public void Page_Load()
    {
        if (!IsPostBack)
        {
            panelFeatured.Visible = (Feature != null);

            if (Feature != null && AccountFeed != null)
            {
                linkFeature2.HRef = linkFeature3.HRef = string.Format("AccountFeedView.aspx?id={0}", Feature.DataRowId);
                labelFeatureName.Text = Render(AccountFeed.Name);
                imgFeature.Src = string.Format("AccountPictureThumbnail.aspx?id={0}", AccountFeed.AccountPictureId);
            }
        }
    }

    public TransitFeature Feature
    {
        get
        {
            if (mFeature == null)
            {
                object[] args = { "AccountFeed" };
                mFeature = SessionManager.GetCachedItem<TransitFeature>(
                    SessionManager.SystemService, "GetLatestFeature", args);
            }

            return mFeature;
        }
    }

    public TransitAccountFeed AccountFeed
    {
        get
        {
            if (mAccountFeed == null)
            {
                object[] args = { SessionManager.Ticket, Feature.DataRowId };
                mAccountFeed = SessionManager.GetCachedItem<TransitAccountFeed>(
                    SessionManager.SyndicationService, "GetAccountFeedById", args);
            }

            return mAccountFeed;
        }
    }
}
