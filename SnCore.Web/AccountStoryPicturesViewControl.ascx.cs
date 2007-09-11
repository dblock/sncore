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
using Wilco.Web.UI;
using SnCore.WebServices;

public partial class AccountStoryPicturesViewControl : Control
{
    public int AccountStoryId
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<int>(ViewState, "AccountStoryId", 0);
        }
        set
        {
            ViewState["AccountStoryId"] = value;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        picturesView.OnGetDataSource += new EventHandler(picturesView_OnGetDataSource);

        if (!IsPostBack)
        {
            GetPicturesData(sender, e);
        }
    }

    void GetPicturesData(object sender, EventArgs e)
    {
        picturesView.CurrentPageIndex = 0;
        picturesView.VirtualItemCount = SessionManager.GetCount<TransitAccountStoryPicture, int>(
            AccountStoryId, SessionManager.StoryService.GetAccountStoryPicturesCount);
        picturesView_OnGetDataSource(sender, e);
        picturesView.DataBind();
    }

    void picturesView_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions(picturesView.PageSize, picturesView.CurrentPageIndex);
        picturesView.DataSource = SessionManager.GetCollection<TransitAccountStoryPicture, int>(
            AccountStoryId, options, SessionManager.StoryService.GetAccountStoryPictures);
    }

    public static string GetCommentCount(int count)
    {
        if (count == 0) return string.Empty;
        return string.Format("{0} comment{1}", count, count == 1 ? string.Empty : "s");
    }
}
