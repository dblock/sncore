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

public partial class MarketingCampaignEdit : AuthenticatedPage
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
                    TransitCampaign t = MarketingService.GetCampaignById(SessionManager.Ticket, RequestId);
                    inputActive.Checked = t.Active;
                    inputDescription.Text = t.Description;
                    inputName.Text = t.Name;
                    inputSenderEmail.Text = t.SenderEmailAddress;
                    inputSenderName.Text = t.SenderName;
                    inputUrl.Text = t.Url;
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
            TransitCampaign t = new TransitCampaign();
            t.Id = RequestId;
            t.Active = inputActive.Checked;
            t.Description = inputDescription.Text;
            t.Name = inputName.Text;
            t.SenderEmailAddress = inputSenderEmail.Text;
            t.SenderName = inputSenderName.Text;
            t.Url = inputUrl.Text;
            MarketingService.CreateOrUpdateCampaign(SessionManager.Ticket, t);
            Redirect("MarketingCampaignsManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
