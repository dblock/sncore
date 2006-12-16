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
using SnCore.Tools.Web;
using SnCore.WebServices;
using SnCore.BackEndServices;
using SnCore.Services;
using System.Collections.Generic;

public partial class AccountFeedItemsNewViewControl : Control
{
    private int mItemsShown = 0;
    private int mCount = 5;

    public int Count
    {
        get
        {
            return mCount;
        }
        set
        {
            mCount = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            object[] args = { new ServiceQueryOptions(Count, 0) };
            FeedsView.DataSource = SessionManager.GetCachedCollection<TransitAccountFeedItem>(
                SessionManager.SyndicationService, "GetAccountFeedItems", args);
            FeedsView.DataBind();
        }
    }

    public string GetDescription(string description)
    {
        if (mItemsShown++ >= 1)
            return string.Empty;

        if (string.IsNullOrEmpty(description))
            return string.Empty;

        return base.GetSummary(description);
    }

    public string GetComments(int count)
    {
        if (count == 0) return "read";
        else if (count == 1) return "one comment";
        else return string.Format("{0} comments", count);
    }

}
