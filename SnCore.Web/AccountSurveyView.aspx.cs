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

public partial class AccountSurveyView : Page
{
    private TransitAccount mAccount = null;
    private TransitSurvey mSurvey = null;

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            accountSurveyAnswers.OnGetDataSource += new EventHandler(accountSurveyAnswers_OnGetDataSource);
            if (!IsPostBack)
            {
                
                linkAccount.Text = accountName.Text = Renderer.Render(Account.Name);
                accountImage.Src = string.Format("AccountPictureThumbnail.aspx?id={0}", Account.PictureId);
                linkAccount.NavigateUrl = accountLink.HRef = string.Format("AccountView.aspx?id={0}", Account.Id);

                surveyName.Text = Renderer.Render(Survey.Name);
                linkAccountSurvey.Text = Renderer.Render(Survey.Name);

                this.Title = string.Format("{0}'s {1}", Renderer.Render(Account.Name), Renderer.Render(Survey.Name));

                accountSurveyAnswers_OnGetDataSource(sender, e);
                accountSurveyAnswers.DataBind();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public TransitAccount Account
    {
        get
        {
            if (mAccount == null)
            {
                mAccount = (SessionManager.Account != null && AccountId == SessionManager.Account.Id)
                    ? SessionManager.Account
                    : AccountService.GetAccountById(AccountId);
            }
            return mAccount;
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
        accountSurveyAnswers.DataSource = AccountService.GetAccountSurveyAnswersById(
            AccountId, SurveyId);
    }

    public int AccountId
    {
        get
        {
            try
            {
                int result = GetId("aid");
                if (result > 0) return result;
                if (SessionManager.Account == null) return 0;
                return SessionManager.Account.Id;
            }
            catch (Exception ex)
            {
                ReportException(ex);
                return 0;
            }
        }
    }

    public int SurveyId
    {
        get
        {
            return RequestId;
        }
    }
}
