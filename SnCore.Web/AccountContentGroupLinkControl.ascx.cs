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
using SnCore.WebServices;
using SnCore.Services;
using System.Web.Caching;

public partial class AccountContentGroupLinkControl : Control
{
    private string mConfigurationName = string.Empty;
    private bool mLowerCase = false;
    private bool mShowLinkPrefix = true;

    public bool ShowLinkPrefix
    {
        get
        {
            return mShowLinkPrefix;
        }
        set
        {
            mShowLinkPrefix = value;
        }
    }

    public string ConfigurationName
    {
        get
        {
            return mConfigurationName;
        }
        set
        {
            mConfigurationName = value;
        }
    }

    public bool LowerCase
    {
        get
        {
            return mLowerCase;
        }
        set
        {
            mLowerCase = value;
        }
    }

    protected TransitAccountContentGroup GetTransitAccountContentGroup()
    {
        if (string.IsNullOrEmpty(ConfigurationName))
            return null;

        string key = string.Format("cg:{0}", ConfigurationName.GetHashCode());

        TransitAccountContentGroup instance = (TransitAccountContentGroup) Cache[key];

        if (instance != null)
            return instance;

        int id = int.Parse(SessionManager.GetCachedConfiguration(ConfigurationName, "0"));

        if (id > 0)
        {
            instance = SessionManager.GetInstance<TransitAccountContentGroup, int>(
                id, SessionManager.ContentService.GetAccountContentGroupById);
        }

        if (instance != null)
        {
            Cache.Insert(key, instance, null, Cache.NoAbsoluteExpiration, SessionManager.DefaultCacheTimeSpan);
        }

        return instance;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                TransitAccountContentGroup group = GetTransitAccountContentGroup();

                if (group != null)
                {
                    linkContentGroup.Text = string.Format("{0}{1}",
                        ShowLinkPrefix ? "&#187; " : string.Empty,
                        Render(LowerCase ? group.Name.ToLower() : group.Name));

                    linkContentGroup.ToolTip = Render(group.Description);
                    linkContentGroup.NavigateUrl = string.Format("AccountContentGroupView.aspx?id={0}", group.Id);
                }
                else
                {
                    Visible = false;
                }
            }
            catch (Exception ex)
            {
                linkContentGroup.Text = string.Format("<!-- {0} -->", ex.Message);
            }
        }
    }
}
