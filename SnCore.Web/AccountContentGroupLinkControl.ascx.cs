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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                if (!string.IsNullOrEmpty(ConfigurationName))
                {
                    int id = int.Parse(SessionManager.GetCachedConfiguration(ConfigurationName, "0"));

                    if (id <= 0)
                    {
                        throw new Exception(string.Format("Invalid id {0} for configuration {1}", id, ConfigurationName)); 
                    }

                    object[] args = { SessionManager.Ticket, id };
                    TransitAccountContentGroup group = SessionManager.GetCachedItem<TransitAccountContentGroup>(
                        SessionManager.ContentService, "GetAccountContentGroupById", args);

                    linkContentGroup.Text = string.Format("{0}{1}", 
                        ShowLinkPrefix ? "&#187; " : string.Empty,
                        Render(LowerCase ? group.Name.ToLower() : group.Name));

                    linkContentGroup.ToolTip = Render(group.Description);
                    linkContentGroup.NavigateUrl = string.Format("AccountContentGroupView.aspx?id={0}", id);
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
