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
using System.Text;
using System.IO;

public partial class PlaceFriendsQueueView : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            queue.OnGetDataSource += new EventHandler(queue_OnGetDataSource);

            if (!IsPostBack)
            {
                GetData(sender, e);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void GetData(object sender, EventArgs e)
    {
        queue.CurrentPageIndex = 0;
        queue.VirtualItemCount = PlaceService.GetFriendsPlaceQueueItemsCount(SessionManager.Ticket);
        queue_OnGetDataSource(this, null);
        queue.DataBind();
    }

    void queue_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = queue.CurrentPageIndex;
            options.PageSize = queue.PageSize;
            queue.DataSource = PlaceService.GetFriendsPlaceQueueItems(SessionManager.Ticket, options);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public string RenderAccounts(TransitAccount[] accounts)
    {
        HtmlTable table = new HtmlTable();
        table.Border = 0;
        table.BorderColor = "White";
        HtmlTableRow row = new HtmlTableRow();
        table.Rows.Add(row);
        foreach (TransitAccount ta in accounts)
        {
            HtmlTableCell cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl(string.Format(
                "<div><a href='AccountView.aspx?id={0}'>" +
                "<img border=0 style='width: 50%;' src='AccountPictureThumbnail.aspx?id={1}'></a></div>" +
                "<div class=sncore_link><a href='AccountView.aspx?id={0}'>{2}</a>", ta.Id, ta.PictureId, Render(ta.Name))));
            row.Cells.Add(cell);            
            if (row.Cells.Count % 4 == 0)
            {
                row = new HtmlTableRow();
                table.Rows.Add(row);
            }
        }

        StringBuilder sb = new StringBuilder();
        StringWriter sw = new StringWriter(sb);
        table.RenderControl(new HtmlTextWriter(sw));
        return sb.ToString();
    }
}
