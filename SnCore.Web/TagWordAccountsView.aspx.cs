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
using SnCore.Services;
using SnCore.WebServices;
using SnCore.SiteMap;

public partial class TagWordAccountsView : Page
{
    public void Page_Load()
    {
        try
        {
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
            if (!IsPostBack)
            {
                TransitTagWord word = TagWordService.GetTagWordById(RequestId);
                tagSubtitle.Text = string.Format("Who is talking about <b>\"{0}\"</b>?", Renderer.Render(word.Word));
                this.Title = string.Format("Who is talking about \"{0}\"?", Renderer.Render(word.Word));
                gridManage_OnGetDataSource(this, null);
                gridManage.DataBind();

                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("People", Request, "AccountsView.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode("Tags", Request, "TagWordsView.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode(word.Word, Request.Url));
                StackSiteMap(sitemapdata);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            gridManage.DataSource = TagWordService.GetTagWordAccountsById(RequestId);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
