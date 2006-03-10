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

public partial class TagWordsManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

            if (!IsPostBack)
            {
                listboxSelectType.DataSource = Enum.GetNames(typeof(TransitTagWordQueryOptions));
                listboxSelectType.DataBind();
                listboxSelectType.Items.FindByValue("New").Selected = true;
                listboxSelectType_SelectedIndexChanged(sender, e);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void buttonPromote_Click(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            StringBuilder sb = new StringBuilder();
            foreach (DataGridItem item in gridManage.Items)
            {
                switch (item.ItemType)
                {
                    case ListItemType.AlternatingItem:
                    case ListItemType.SelectedItem:
                    case ListItemType.Item:
                    case ListItemType.EditItem:
                        bool item_checked = ((CheckBox)item.Cells[(int)Cells.checkbox].Controls[1]).Checked;
                        if (item_checked)
                        {
                            int item_id = int.Parse(item.Cells[(int)Cells.id].Text);

                            TransitTagWord tw = TagWordService.GetTagWordById(item_id);
                            tw.Promoted = !tw.Promoted;
                            if (tw.Promoted) tw.Excluded = false; // can't promote an excluded word
                            TagWordService.CreateOrUpdateTagWord(SessionManager.Ticket, tw);
                            sb.Append(string.Format("Tag word \"{0}\" {1}.<br>", 
                                Renderer.Render(tw.Word), tw.Promoted ? "promoted" : "demoted"));
                            count++;
                        }
                        break;
                }
            }
            sb.Append(string.Format("{0} item{1} processed.", count, count != 1 ? "s" : string.Empty));
            noticeManage.Info = sb.ToString();
            noticeManage.HtmlEncode = false;
            gridManage_OnGetDataSource(sender, e);
            gridManage.DataBind();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void buttonExclude_Click(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            StringBuilder sb = new StringBuilder();
            foreach (DataGridItem item in gridManage.Items)
            {
                switch (item.ItemType)
                {
                    case ListItemType.AlternatingItem:
                    case ListItemType.SelectedItem:
                    case ListItemType.Item:
                    case ListItemType.EditItem:
                        bool item_checked = ((CheckBox)item.Cells[(int)Cells.checkbox].Controls[1]).Checked;
                        if (item_checked)
                        {
                            int item_id = int.Parse(item.Cells[(int)Cells.id].Text);

                            TransitTagWord tw = TagWordService.GetTagWordById(item_id);
                            tw.Excluded = !tw.Excluded;
                            if (tw.Excluded) tw.Promoted = false; // can't promote an excluded word
                            TagWordService.CreateOrUpdateTagWord(SessionManager.Ticket, tw);
                            sb.Append(string.Format("Tag word \"{0}\" {1}.<br>",
                                Renderer.Render(tw.Word), tw.Excluded ? "excluded" : "included"));
                            count++;
                        }
                        break;
                }
            }
            sb.Append(string.Format("{0} item{1} processed.", count, count != 1 ? "s" : string.Empty));
            noticeManage.Info = sb.ToString();
            noticeManage.HtmlEncode = false;
            gridManage_OnGetDataSource(sender, e);
            gridManage.DataBind();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void listboxSelectType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            gridManage_OnGetDataSource(sender, e);
            gridManage.DataBind();
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
            TransitTagWordQueryOptions options = (TransitTagWordQueryOptions)
                Enum.Parse(typeof(TransitTagWordQueryOptions), listboxSelectType.SelectedValue);
            gridManage.DataSource = TagWordService.GetTagWords(options, -1);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    private enum Cells
    {
        id = 0,
        image,
        checkbox,
        name
    };
}
