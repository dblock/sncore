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
        try
        {
            if (!IsPostBack)
            {
                List<TransitAccountFeedItem> items = (List<TransitAccountFeedItem>)
                    Cache[string.Format("feeditems:{0}", ClientID)];

                if (items == null)
                {
                    ServiceQueryOptions options = new ServiceQueryOptions();
                    options.PageSize = Count;
                    options.PageNumber = 0;
                    items = SyndicationService.GetAccountFeedItems(options);
                    Cache.Insert(string.Format("feeditems:{0}", ClientID),
                        items, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
                }

                FeedsView.DataSource = items;
                FeedsView.DataBind();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public string GetSummary(string summary)
    {
        string result = Renderer.RemoveHtml(summary);
        if (result.Length > 256) result = result.Substring(0, 256) + " ...";
        return result;
    }

    public string GetComments(int count)
    {
        if (count == 0) return "read and comment";
        else if (count == 1) return "read one comment";
        else return string.Format("read {0} comments", count);
    }

}
