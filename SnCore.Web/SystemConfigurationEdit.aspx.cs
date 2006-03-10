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

public partial class SystemConfigurationEdit : AuthenticatedPage
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
                    TransitConfiguration tw = SystemService.GetConfigurationById(id);
                    inputName.Text = Renderer.Render(tw.Name);
                    inputValue.Text = Renderer.Render(tw.Value);
                    inputPassword.Checked = tw.Password;
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
            TransitConfiguration tw = new TransitConfiguration();
            tw.Name = inputName.Text;
            tw.Id = RequestId;
            tw.Value = inputValue.Text;
            tw.Password = inputPassword.Checked;
            SystemService.AddConfiguration(SessionManager.Ticket, tw);
            Page.Cache.Remove(string.Format("settings:{0}", tw.Name));
            Redirect("SystemConfigurationsManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }

    }
}
