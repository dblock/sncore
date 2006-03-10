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

public partial class AccountsNewViewControl : Control
{
    private int mCount = 2;

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

    public void Page_Load()
    {
        try
        {
            if (!IsPostBack)
            {                
                List<TransitAccount> items = (List<TransitAccount>)
                    Cache[string.Format("accounts:{0}", ClientID)];

                if (items == null)
                {
                    items = SocialService.GetNewAccounts(Count);
                    Cache.Insert(string.Format("accounts:{0}", ClientID),
                        items, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
                }

                accounts.RepeatColumns = Count;
                accounts.DataSource = items;
                accounts.DataBind();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
