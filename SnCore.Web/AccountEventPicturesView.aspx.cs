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

public partial class AccountEventPicturesView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (RequestId > 0)
                {
                    TransitAccountEvent a = EventService.GetAccountEventById(SessionManager.Ticket, RequestId, SessionManager.UtcOffset);
                    linkAccountEvent.Text = Renderer.Render(a.Name);
                    this.Title = string.Format("{0} Pictures", Renderer.Render(a.Name));
                    linkAccountEvent.NavigateUrl = "AccountEventView.aspx?id=" + a.Id;
                    listView.DataSource = EventService.GetAccountEventPicturesById(RequestId, null);
                    listView.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
