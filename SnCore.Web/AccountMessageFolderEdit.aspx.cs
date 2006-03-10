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

public partial class AccountMessageFolderEdit : AuthenticatedPage
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
                    TransitAccountMessageFolder tw = AccountService.GetAccountMessageFolderById(SessionManager.Ticket, id);
                    inputName.Text = Renderer.Render(tw.Name);
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
            TransitAccountMessageFolder tw = new TransitAccountMessageFolder();
            tw.Name = inputName.Text;
            tw.Id = RequestId;
            tw.AccountMessageFolderParentId = GetId("pid");
            AccountService.AddAccountMessageFolder(SessionManager.Ticket, tw);
            Redirect("AccountMessageFoldersManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }

    }
}
