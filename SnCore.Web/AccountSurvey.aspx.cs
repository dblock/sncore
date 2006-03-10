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
using SnCore.Services;
using System.Collections.Generic;

public class AccountSurveyEntry
{
    public TransitAccountSurveyAnswer SurveyAnswer;
    public TextBox AnswerTextBox;

    public AccountSurveyEntry(TransitAccountSurveyAnswer p, TextBox t)
    {
        SurveyAnswer = p;
        AnswerTextBox = t;
    }
}

public partial class AccountSurvey : AuthenticatedPage
{
    private ArrayList mSurveyEntries = new ArrayList();

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected override void OnInit(EventArgs e)
    {
        try
        {
            TransitSurvey ts = SystemService.GetSurveyById(RequestId);
            this.Title = surveyName.Text = Render(ts.Name);

            List<TransitAccountSurveyAnswer> answers = AccountService.GetAccountSurveyAnswers(
                SessionManager.Ticket, RequestId);

            int index = 1;
            foreach (TransitAccountSurveyAnswer answer in answers)
            {
                WizardStep step = new WizardStep();
                step.Title = answer.SurveyQuestion;

                HyperLink q = new HyperLink();
                q.ID = "question_" + index.ToString();
                q.Text = answer.SurveyQuestion;
                q.NavigateUrl = string.Format("AccountSurveyQuestionView.aspx?id={0}", answer.SurveyQuestionId);
                q.Font.Bold = true;
                step.Controls.Add(q);

                step.Controls.Add(new HtmlGenericControl("br"));
                step.Controls.Add(new HtmlGenericControl("br"));

                TextBox t = new TextBox();
                t.ID = "answer_" + index.ToString();
                t.Text = answer.Answer;
                t.TextMode = TextBoxMode.MultiLine;
                t.Rows = 7;
                t.CssClass = "sncore_form_textbox";
                step.Controls.Add(t);

                surveyWizard.WizardSteps.Insert(surveyWizard.WizardSteps.Count - 1, step);

                mSurveyEntries.Add(new AccountSurveyEntry(answer, t));
                index++;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }

        base.OnInit(e);
    }

    public void surveyWizard_ActiveStepChanged(object sender, EventArgs e)
    {
        try
        {
            foreach (AccountSurveyEntry entry in mSurveyEntries)
            {
                if (string.IsNullOrEmpty(entry.AnswerTextBox.Text) && entry.SurveyAnswer.Id == 0)
                    continue;

                TransitAccountSurveyAnswer p = new TransitAccountSurveyAnswer();
                p.SurveyQuestion = entry.SurveyAnswer.SurveyQuestion;
                p.SurveyQuestionId = entry.SurveyAnswer.SurveyQuestionId;
                p.Id = entry.SurveyAnswer.Id;
                p.Answer = entry.AnswerTextBox.Text;
                AccountService.AddAccountSurveyAnswer(SessionManager.Ticket, p);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
