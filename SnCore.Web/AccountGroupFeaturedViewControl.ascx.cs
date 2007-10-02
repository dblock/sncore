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
using SnCore.WebServices;

public partial class AccountGroupFeaturedViewControl : Control
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetData(sender, e);
            //panelFeatured.Visible = (Feature != null);
            //if (Feature != null && AccountGroup != null)
            //{
            //    linkFeature2.HRef = linkFeature3.HRef = string.Format("AccountGroupView.aspx?id={0}", Feature.DataRowId);
            //    labelFeatureName.Text = Renderer.Render(AccountGroup.Name);
            //    labelFeatureDescription.Text = Renderer.GetSummary(AccountGroup.Description);
            //    imgFeature.Src = string.Format("AccountGroupPictureThumbnail.aspx?id={0}", AccountGroup.PictureId);
            //}
        }
    }

    public TransitAccountGroup GetAccountGroup(int id)
    {
        return SessionManager.GetInstance<TransitAccountGroup, int>(
            id, SessionManager.GroupService.GetAccountGroupById);
    }

    protected void GetData(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = 0;
        options.PageSize = gridManage.PageSize;
        gridManage.CurrentPageIndex = 0;
        gridManage.DataSource = SessionManager.GetCollection<TransitFeature, string>(
            "AccountGroup", options, SessionManager.ObjectService.GetFeatures);
        gridManage.DataBind();
    }
}
