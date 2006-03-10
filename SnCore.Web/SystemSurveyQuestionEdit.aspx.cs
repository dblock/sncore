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
using SnCore.WebServices;
using SnCore.Services;

public partial class SystemSurveyQuestionEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(manageAdd);
            if (!IsPostBack)
            {
                linkBack.NavigateUrl = "SystemSurveyEdit.aspx?id=" + SurveyId.ToString();
                if (RequestId > 0)
                {
                    TransitSurveyQuestion tw = SystemService.GetSurveyQuestionById(RequestId);
                    inputQuestion.Text = Renderer.Render(tw.Question);
                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public int SurveyId
    {
        get
        {
            return GetId("sid");
        }
    }

    public void save_Click(object sender, EventArgs e)
    {
        try
        {
            TransitSurveyQuestion tw = new TransitSurveyQuestion();
            tw.Question = inputQuestion.Text;
            tw.Id = RequestId;
            tw.SurveyId = SurveyId;
            SystemService.AddSurveyQuestion(SessionManager.Ticket, tw);
            Redirect("SystemSurveyEdit.aspx?id=" + SurveyId.ToString());
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }

    }
}
