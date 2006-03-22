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

public partial class AccountWebsiteEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(manageAdd);
            if (!IsPostBack)
            {
                int id = RequestId;

                if (id > 0)
                {
                    TransitAccountWebsite tw = AccountService.GetAccountWebsiteById(SessionManager.Ticket, id);
                    inputName.Text = Renderer.Render(tw.Name);
                    inputUrl.Text = Renderer.Render(tw.Url);
                    inputDescription.Text = tw.Description;
                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void save_Click(object sender, EventArgs e)
    {
        try
        {
            TransitAccountWebsite tw = new TransitAccountWebsite();
            tw.Name = inputName.Text;

            if (!Uri.IsWellFormedUriString(inputUrl.Text, UriKind.Absolute))
                inputUrl.Text = "http://" + inputUrl.Text;

            tw.Url = inputUrl.Text;
            tw.Description = inputDescription.Text;
            tw.Id = RequestId;
            AccountService.AddAccountWebsite(SessionManager.Ticket, tw);
            Redirect("AccountWebsitesManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }

    }
}
