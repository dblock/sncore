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
    private string mUri = null;

    public TransitCounter GetCounter()
    {
        return SessionManager.GetInstance<TransitCounter, string>(
            Uri, SessionManager.StatsService.GetCounterByUri);
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
            TransitCounter tc = GetCounter();
            labelCounter.Text = string.Format("{0} since {1}", tc.Total, base.Adjust(tc.Created).ToString("d"));
        }
    }
}
