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

public partial class CounterViewControl : Control
{
    private TransitCounter mCounter;

    public TransitCounter Counter
    {
        get
        {
            if (mCounter == null)
            {
                object[] args = { Request.Url.ToString() };
                mCounter = SessionManager.GetCachedItem<TransitCounter>(
                    SessionManager.StatsService, "GetCounterByUri", args);
            }

            return mCounter;
        }
        set
        {
            mCounter = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                TransitCounter tc = Counter;
                labelCounter.Text = string.Format("{0} since {1}", tc.Total, base.Adjust(tc.Timestamp).ToString("d"));
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
