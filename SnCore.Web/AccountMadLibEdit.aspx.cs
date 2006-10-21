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

public partial class AccountMadLibEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(save);
            if (!IsPostBack)
            {
                if (RequestId > 0)
                {
                    TransitMadLib t = MadLibService.GetMadLibById(RequestId);
                    inputTemplate.Text = t.Template;
                    inputName.Text = t.Name;
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
            TransitMadLib t = new TransitMadLib();
            t.Id = RequestId;
            t.Template = inputTemplate.Text;
            t.Name = inputName.Text;
            MadLibService.CreateOrUpdateMadLib(SessionManager.Ticket, t);
            Redirect("AccountMadLibsManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
