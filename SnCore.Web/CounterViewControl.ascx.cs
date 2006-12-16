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
    private string mUri = null;

    public TransitCounter Counter
    {
        get
        {
            if (mCounter == null)
            {
                object[] args = { Uri };
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

    public string Uri
    {
        get
        {
            if (string.IsNullOrEmpty(mUri))
            {
                mUri = Request.Url.ToString();
            }
            return mUri;
        }
        set
        {
            mUri = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TransitCounter tc = Counter;
            labelCounter.Text = string.Format("{0} since {1}", tc.Total, base.Adjust(tc.Timestamp).ToString("d"));
        }
    }
}
