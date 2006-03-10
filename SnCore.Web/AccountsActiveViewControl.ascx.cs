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

public partial class AccountsActiveViewControl : Control
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
                    Cache[string.Format("activeaccounts:{0}", ClientID)];

                if (items == null)
                {
                    items = SocialService.GetActiveAccounts(Count);
                    Cache.Insert(string.Format("activeaccounts:{0}", ClientID),
                        items, null, DateTime.Now.AddMinutes(5), TimeSpan.Zero);
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
