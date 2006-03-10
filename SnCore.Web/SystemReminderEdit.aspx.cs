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

public partial class SystemReminderEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(manageAdd);
            if (!IsPostBack)
            {
                inputDataObject.DataSource = SystemService.GetDataObjects();
                inputDataObject.DataBind();

                inputDataObject_SelectedIndexChanged(sender, e);

                if (RequestId > 0)
                {
                    TransitReminder t = SystemService.GetReminderById(RequestId);
                    inputBody.Text = t.Body;
                    inputSubject.Text = t.Subject;

                    ListItem deltaHoursItem = inputDeltaHours.Items.FindByValue(t.DeltaHours.ToString());
                    if (deltaHoursItem == null)
                    {
                        deltaHoursItem = new ListItem(string.Format("Every {0} Hours", t.DeltaHours), t.DeltaHours.ToString());
                        inputDeltaHours.Items.Add(deltaHoursItem);
                    }

                    inputDataObjectField.Items.FindByValue(t.DataObjectField).Selected = true;

                    inputDeltaHours.ClearSelection();
                    deltaHoursItem.Selected = true;
                    inputEnabled.Checked = t.Enabled;
                    inputRecurrent.Checked = t.Recurrent;
                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void inputDataObject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            inputDataObjectField.DataSource = SystemService.GetDataObjectFieldsById(
                int.Parse(inputDataObject.SelectedValue));
            inputDataObjectField.DataBind();
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
            TransitReminder t = new TransitReminder();
            t.Id = RequestId;
            t.Subject = inputSubject.Text;
            t.Body = inputBody.Text;
            t.DeltaHours = int.Parse(inputDeltaHours.SelectedValue);
            t.Enabled = inputEnabled.Checked;
            t.Recurrent = inputRecurrent.Checked;
            t.DataObject_Id = int.Parse(inputDataObject.SelectedValue);
            t.DataObjectField = inputDataObjectField.SelectedValue;
            t.LastRun = DateTime.UtcNow;
            SystemService.CreateOrUpdateReminder(SessionManager.Ticket, t);
            Redirect("SystemRemindersManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
