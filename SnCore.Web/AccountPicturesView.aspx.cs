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

public partial class AccountPicturesView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (RequestId > 0)
                {
                    TransitAccount a = AccountService.GetAccountById(RequestId);
                    linkAccount.Text = Renderer.Render(a.Name);
                    linkAccount.NavigateUrl = "AccountView.aspx?id=" + a.Id;
                    listView.DataSource = (RequestId != 0)
                        ? AccountService.GetAccountPicturesById(RequestId)
                        : AccountService.GetAccountPictures(SessionManager.Ticket);
                    listView.DataBind();
                    this.Title = string.Format("{0}'s Pictures", Renderer.Render(a.Name));
                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
