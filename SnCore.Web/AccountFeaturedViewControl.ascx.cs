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

public partial class AccountFeaturedViewControl : Control
{
    private TransitFeature mFeature = null;
    private TransitAccount mAccount = null;

    public void Page_Load()
    {
        try
        {
            if (!IsPostBack)
            {
                panelFeatured.Visible = (Feature != null);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public TransitFeature Feature
    {
        get
        {
            if (mFeature == null)
            {
                mFeature = (TransitFeature)Cache["feature:account"];
                if (mFeature == null)
                {
                    mFeature = SystemService.GetLatestFeature("Account");
                    if (mFeature == null)
                        return null;

                    Cache.Insert("feature:account", mFeature, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
                }
            }

            return mFeature;
        }
    }

    public TransitAccount Account
    {
        get
        {
            if (mAccount == null)
            {
                mAccount = (TransitAccount) Cache[string.Format("account:{0}", Feature.DataRowId)];
                if (mAccount == null)
                {
                    mAccount = AccountService.GetAccountById(Feature.DataRowId);
                    if (mAccount == null)
                        return null;

                    Cache.Insert(string.Format("account:{0}", Feature.DataRowId), 
                        mAccount, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
                }
            }

            return mAccount;
        }
    }

    public string GetSummary(string summary)
    {
        string result = Renderer.RemoveHtml(summary);
        if (result.Length > 256) result = result.Substring(0, 256) + " ...";
        return result;
    }

    public string GetDescription(int id)
    {
        TransitAccountProfile a = (TransitAccountProfile)Cache[string.Format("accountprofile:{0}", id)];
        if (a == null)
        {
            List<TransitAccountProfile> aa = AccountService.GetAccountProfilesById(id);

            if (aa == null || aa.Count == 0)
                return string.Empty;

            a = aa[0];
            Cache.Insert(string.Format("accountprofile:{0}", id),
                a, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
        }

        return a.AboutSelf;
    }
}
