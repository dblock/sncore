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
using SnCore.Tools.Web;

public partial class _Default : Page
{
    private HtmlMeta mMetaDescription = null;

    public _Default()
    {
        mIsMobileEnabled = true;
    }

    protected override void OnLoad(EventArgs e)
    {
        if (Header != null)
        {
            Header.Controls.Add(MetaDescription);
        }

        base.OnLoad(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            accountsNewMain.DataBind();

            websiteBlog.BlogId = int.Parse(SessionManager.GetCachedConfiguration(
                    "SnCore.Blog.Id", "0"));

            websiteBlog.Visible = (websiteBlog.BlogId > 0);
        }
    }

    public HtmlMeta MetaDescription
    {
        get
        {
            if (mMetaDescription == null)
            {
                mMetaDescription = new HtmlMeta();
                mMetaDescription.Name = "description";
                mMetaDescription.Content = SessionManager.GetCachedConfiguration(
                    "SnCore.Description", string.Empty);
            }
            return mMetaDescription;
        }
    }
}