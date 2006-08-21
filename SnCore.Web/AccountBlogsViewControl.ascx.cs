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
using SnCore.Services;

public partial class AccountBlogsViewControl : Control
{
    public int AccountId
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<int>(ViewState, "AccountId", 0);
        }
        set
        {
            ViewState["AccountId"] = value;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            accountBlogs.OnGetDataSource += new EventHandler(accountBlogs_OnGetDataSource);
            if (!IsPostBack)
            {
                accountBlogs_OnGetDataSource(sender, e);
                accountBlogs.DataBind();
                this.Visible = accountBlogs.Items.Count > 0;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void accountBlogs_OnGetDataSource(object sender, EventArgs e)
    {
        object[] args = { AccountId };
        accountBlogs.DataSource = SessionManager.GetCachedCollection<TransitAccountBlog>(
            BlogService, "GetAccountBlogsById", args);
    }
}
