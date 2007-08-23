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

public partial class AccountGroupFeaturedViewControl : Control
{
    private TransitFeature mFeature = null;
    private TransitAccountGroup mAccountGroup = null;

    public void Page_Load()
    {
        if (!IsPostBack)
        {
            panelFeatured.Visible = (Feature != null);
            if (Feature != null && AccountGroup != null)
            {
                linkFeature2.HRef = linkFeature3.HRef = string.Format("AccountGroupView.aspx?id={0}", Feature.DataRowId);
                labelFeatureName.Text = Renderer.Render(AccountGroup.Name);
                labelFeatureDescription.Text = Renderer.GetSummary(AccountGroup.Description);
                imgFeature.Src = string.Format("AccountGroupPictureThumbnail.aspx?id={0}", AccountGroup.PictureId);
            }
        }
    }

    public TransitFeature Feature
    {
        get
        {
            if (mFeature == null)
            {
                mFeature = SessionManager.GetInstance<TransitFeature, string>(
                    "AccountGroup", SessionManager.ObjectService.GetLatestFeature);
            }

            return mFeature;
        }
    }

    public TransitAccountGroup AccountGroup
    {
        get
        {
            if (mAccountGroup == null)
            {
                mAccountGroup = SessionManager.GetInstance<TransitAccountGroup, int>(
                    Feature.DataRowId, SessionManager.GroupService.GetAccountGroupById);
            }

            return mAccountGroup;
        }
    }
}
