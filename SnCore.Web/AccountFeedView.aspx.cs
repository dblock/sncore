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
using SnCore.Services;
using SnCore.WebServices;

public partial class AccountFeedView : Page
{
    private TransitFeature mAccountFeedFeature = null;

    public TransitFeature LatestAccountFeedFeature
    {
        get
        {
            if (mAccountFeedFeature == null)
            {
                mAccountFeedFeature = SystemService.FindLatestFeature(
                    "AccountFeed", RequestId);
            }
            return mAccountFeedFeature;
        }
    }

    public void Page_Load()
    {
        try
        {
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
            if (!IsPostBack)
            {
                TransitAccountFeed f = SyndicationService.GetAccountFeedById(SessionManager.Ticket, RequestId);
                labelFeed.Text = Renderer.Render(f.Name);
                labelFeed.NavigateUrl = f.LinkUrl;
                labelFeedDescription.Text = Renderer.Render(f.Description);
                labelFeedLastError.Text = Renderer.Render(f.LastError);

                TransitFeedType t = SyndicationService.GetFeedTypeByName(f.FeedType);
                gridManage.RepeatColumns = t.SpanColumns;
                gridManage.RepeatRows = t.SpanRows;

                TransitAccount a = AccountService.GetAccountById(f.AccountId);
                labelAccountName.Text = Renderer.Render(a.Name);
                linkAccount.HRef = string.Format("AccountView.aspx?id={0}", a.Id);
                imageAccount.Src = string.Format("AccountPictureThumbnail.aspx?id={0}", a.PictureId);

                this.Title = string.Format("{0}'s {1}", Renderer.Render(a.Name), Renderer.Render(f.Name));

                gridManage.VirtualItemCount = SyndicationService.GetAccountFeedItemsCountById(RequestId);
                gridManage_OnGetDataSource(this, null);
                gridManage.DataBind();

                if (SessionManager.IsAdministrator)
                {
                    linkFeature.Text = (LatestAccountFeedFeature != null)
                        ? string.Format("Feature &#187; Last on {0}", Adjust(LatestAccountFeedFeature.Created).ToString("d"))
                        : "Feature &#187; Never Featured";
                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = gridManage.CurrentPage;
            options.PageSize = gridManage.PageSize;
            gridManage.DataSource = SyndicationService.GetAccountFeedItemsById(RequestId, options);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public string GetComments(int count)
    {
        if (count == 0) return "post a comment";
        else if (count == 1) return "1 comment";
        else return string.Format("{0} comments", count);
    }

    protected override void OnPreRender(EventArgs e)
    {
        panelAdmin.Visible = SessionManager.IsAdministrator;
        if (panelAdmin.Visible)
        {
            linkDeleteFeatures.Visible = (LatestAccountFeedFeature != null);
        }
        base.OnPreRender(e);
    }

    public void feature_Click(object sender, EventArgs e)
    {
        try
        {
            if (!SessionManager.IsAdministrator)
            {
                // avoid round-trip
                throw new Exception("You must be an administrator to feature other users.");
            }

            TransitFeature t_feature = new TransitFeature();
            t_feature.DataObjectName = "AccountFeed";
            t_feature.DataRowId = RequestId;
            SystemService.CreateOrUpdateFeature(SessionManager.Ticket, t_feature);
            Redirect(Request.Url.PathAndQuery);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void deletefeature_Click(object sender, EventArgs e)
    {
        try
        {
            if (!SessionManager.IsAdministrator)
            {
                // avoid round-trip
                throw new Exception("You must be an administrator to feature feeds.");
            }

            TransitFeature t_feature = new TransitFeature();
            t_feature.DataObjectName = "AccountFeed";
            t_feature.DataRowId = RequestId;
            SystemService.DeleteAllFeatures(SessionManager.Ticket, t_feature);
            Redirect(Request.Url.PathAndQuery);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }


}
