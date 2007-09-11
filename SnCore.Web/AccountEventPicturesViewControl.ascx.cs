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

public partial class AccountEventPicturesViewControl : Control
{
    public int AccountEventId
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<int>(ViewState, "AccountEventId", 0);
        }
        set
        {
            ViewState["AccountEventId"] = value;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        picturesView.OnGetDataSource += new EventHandler(picturesView_OnGetDataSource);

        if (!IsPostBack)
        {
            GetPicturesData(sender, e);
            linkUpload.NavigateUrl = string.Format("AccountEventPicturesManage.aspx?id={0}", AccountEventId);
        }
    }

    void GetPicturesData(object sender, EventArgs e)
    {
        picturesView.CurrentPageIndex = 0;
        picturesView.VirtualItemCount = SessionManager.GetCount<TransitAccountEventPicture, int>(
            AccountEventId, SessionManager.EventService.GetAccountEventPicturesCount);
        picturesView_OnGetDataSource(sender, e);
        picturesView.DataBind();
        noPictures.Visible = (picturesView.Items.Count == 0);
    }

    void picturesView_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions(picturesView.PageSize, picturesView.CurrentPageIndex);
        picturesView.DataSource = SessionManager.GetCollection<TransitAccountEventPicture, int>(
            AccountEventId, options, SessionManager.EventService.GetAccountEventPictures);
    }

    public static string GetCommentCount(int count)
    {
        if (count == 0) return string.Empty;
        return string.Format("{0} comment{1}", count, count == 1 ? string.Empty : "s");
    }
}
