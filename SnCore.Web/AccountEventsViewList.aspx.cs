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

public partial class AccountEventsViewList : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                int pagenumber = int.Parse(Request["PageNumber"]);
                int pagecount = int.Parse(Request["PageCount"]);
                int pagesize = int.Parse(Request["PageSize"]);

                TransitAccountEventQueryOptions options = new TransitAccountEventQueryOptions();
                ServiceQueryOptions serviceoptions = new ServiceQueryOptions();
                serviceoptions.PageSize = pagesize;
                serviceoptions.PageNumber = pagenumber;
                gridManage.DataSource = SessionManager.EventService.GetAllAccountEvents(
                    SessionManager.Ticket, SessionManager.UtcOffset, options, serviceoptions);
                gridManage.DataBind();

                if (pagenumber > 0)
                    linkPrev.HRef = string.Format("AccountEventsViewList.aspx?PageNumber={0}&PageSize={1}&PageCount={2}",
                        pagenumber - 1, pagesize, pagecount);

                if (pagenumber < pagecount - 1)
                    linkNext.HRef = string.Format("AccountEventsViewList.aspx?PageNumber={0}&PageSize={1}&PageCount={2}",
                        pagenumber + 1, pagesize, pagecount);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
