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
using System.Text;
using SnCore.Tools.Web;
using SnCore.Services;
using SnCore.WebServices;
using System.Collections.Generic;
using SnCore.SiteMap;

public partial class TagWordsManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            listboxSelectType.DataSource = Enum.GetNames(typeof(TransitTagWordQueryOptions));
            listboxSelectType.DataBind();
            listboxSelectType.Items.FindByValue("New").Selected = true;
            listboxSelectType_SelectedIndexChanged(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("System Preferences", Request, "SystemPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Tag Words", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    public void PromoteOrExclude(bool promote, object sender, EventArgs e)
    {
        List<string> promoted = new List<string>();
        List<string> demoted = new List<string>();
        List<string> excluded = new List<string>();
        List<string> included = new List<string>();

        List<TransitTagWord> words = new List<TransitTagWord>();
        foreach (DataGridItem item in gridManage.Items)
        {
            switch (item.ItemType)
            {
                case ListItemType.AlternatingItem:
                case ListItemType.SelectedItem:
                case ListItemType.Item:
                case ListItemType.EditItem:
                    bool item_checked = ((CheckBox)item.Cells[(int)Cells.checkbox].Controls[1]).Checked;
                    int item_id = int.Parse(item.Cells[(int)Cells.id].Text);

                    TransitTagWord tw = SessionManager.TagWordService.GetTagWordById(
                        SessionManager.Ticket, item_id);
                    bool item_promote = ((promote && item_checked) || (!promote && !item_checked));
                    if (item_promote)
                    {
                        tw.Promoted = !tw.Promoted;
                        if (tw.Promoted) tw.Excluded = false; // can't promote an excluded word
                        if (tw.Promoted) promoted.Add(Render(tw.Word)); else demoted.Add(Render(tw.Word));
                        words.Add(tw);
                    }
                    else if (listboxSelectType.SelectedValue == "New")
                    {
                        // don't flip promoted or excluded words
                        tw.Excluded = !tw.Excluded;
                        if (tw.Excluded) tw.Promoted = false; // can't promote an excluded word
                        if (tw.Excluded) excluded.Add(Render(tw.Word)); else included.Add(Render(tw.Word));
                        words.Add(tw);
                    }
                    break;
            }
        }

        foreach (TransitTagWord word in words)
        {
            SessionManager.TagWordService.CreateOrUpdateTagWord(SessionManager.Ticket, word);
        }

        StringBuilder sb = new StringBuilder();
        sb.Append(string.Format("{0} item{1} processed.<BR>", words.Count, words.Count != 1 ? "s" : string.Empty));
        if (promoted.Count > 0) sb.AppendFormat("Promoted: {0}<BR>\n", string.Join(", ", promoted.ToArray()));
        if (demoted.Count > 0) sb.AppendFormat("Demoted: {0}<BR>\n", string.Join(", ", demoted.ToArray()));
        if (included.Count > 0) sb.AppendFormat("Included: {0}<BR>\n", string.Join(", ", included.ToArray()));
        if (excluded.Count > 0) sb.AppendFormat("Excluded: {0}<BR>\n", string.Join(", ", excluded.ToArray()));
        noticeManage.HtmlEncode = false;
        noticeManage.Info = sb.ToString();

        GetData(sender, e);
    }

    public void buttonPromote_Click(object sender, EventArgs e)
    {
        PromoteOrExclude(true, sender, e);
    }

    public void buttonExclude_Click(object sender, EventArgs e)
    {
        PromoteOrExclude(false, sender, e);
    }

    public void listboxSelectType_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetData(sender, e);
    }

    public void GetData(object sender, EventArgs e)
    {
        TransitTagWordQueryOptions options = (TransitTagWordQueryOptions)
            Enum.Parse(typeof(TransitTagWordQueryOptions), listboxSelectType.SelectedValue);

        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.TagWordService.GetTagWordsCount(
            SessionManager.Ticket, options);
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        TransitTagWordQueryOptions options = (TransitTagWordQueryOptions)
            Enum.Parse(typeof(TransitTagWordQueryOptions), listboxSelectType.SelectedValue);
        ServiceQueryOptions serviceoptions = new ServiceQueryOptions();
        serviceoptions.PageSize = gridManage.PageSize;
        serviceoptions.PageNumber = gridManage.CurrentPageIndex;
        gridManage.DataSource = SessionManager.TagWordService.GetTagWords(
            SessionManager.Ticket, options, serviceoptions);
    }

    private enum Cells
    {
        id = 0,
        image,
        checkbox,
        name
    };
}
