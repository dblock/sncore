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

public partial class AccountSurveyQuestionView : Page
{
    private TransitSurvey mSurvey = null;
    private TransitSurveyQuestion mSurveyQuestion = null;

    private void GetData()
    {
        accountSurveyAnswers.CurrentPageIndex = 0;
        accountSurveyAnswers.VirtualItemCount = AccountService.GetAccountSurveyAnswersCountByQuestionId(SurveyQuestionId);
        accountSurveyAnswers_OnGetDataSource(this, null);
        accountSurveyAnswers.DataBind();
    }

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            accountSurveyAnswers.OnGetDataSource += new EventHandler(accountSurveyAnswers_OnGetDataSource);
            if (!IsPostBack)
            {
                surveyName.Text = this.Title = linkSurvey.Text = Renderer.Render(Survey.Name);
                linkSurvey.NavigateUrl = ReturnUrl;
                linkSurveyQuestion.Text = Renderer.Render(SurveyQuestion.Question);
                GetData();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public TransitSurveyQuestion SurveyQuestion
    {
        get
        {
            if (mSurveyQuestion == null)
            {
                mSurveyQuestion = SystemService.GetSurveyQuestionById(SurveyQuestionId);
            }
            return mSurveyQuestion;
        }
    }

    public TransitSurvey Survey
    {
        get
        {
            if (mSurvey == null)
            {
                mSurvey = SystemService.GetSurveyById(SurveyId);
            }

            return mSurvey;
        }
    }

    void accountSurveyAnswers_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions serviceoptions = new ServiceQueryOptions();
        serviceoptions.PageSize = accountSurveyAnswers.PageSize;
        serviceoptions.PageNumber = accountSurveyAnswers.CurrentPageIndex;

        accountSurveyAnswers.DataSource = AccountService.GetAccountSurveyAnswersByQuestionId(
            SurveyQuestionId, serviceoptions);            
    }

    public int SurveyQuestionId
    {
        get
        {
            return RequestId;
        }
    }

    public int SurveyId
    {
        get
        {
            return SurveyQuestion.SurveyId;
        }
    }

    public string ReturnUrl
    {
        get
        {
            string result = Request.QueryString["ReturnUrl"];
            if (string.IsNullOrEmpty(result) && SessionManager.IsLoggedIn) result = string.Format("AccountSurveyView.aspx?aid={0}&id={1}", SessionManager.Account.Id, SurveyId);
            if (string.IsNullOrEmpty(result)) return string.Format("AccountSurveyView.aspx?id={0}", SurveyId);
            return result;
        }
    }
}
