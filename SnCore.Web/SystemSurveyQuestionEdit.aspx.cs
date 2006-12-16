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
using SnCore.SiteMap;

public partial class SystemSurveyQuestionEdit : AuthenticatedPage
{
    private TransitSurvey mSurvey = null;

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode("Surveys", Request, "SystemSurveysManage.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode(Survey.Name, Request, string.Format("SystemSurveyEdit.aspx?id={0}", SurveyId)));

                linkBack.NavigateUrl = "SystemSurveyEdit.aspx?id=" + SurveyId.ToString();
                if (RequestId > 0)
                {
                    TransitSurveyQuestion tw = SessionManager.SystemService.GetSurveyQuestionById(RequestId);
                    inputQuestion.Text = Renderer.Render(tw.Question);
                    sitemapdata.Add(new SiteMapDataAttributeNode(tw.Question, Request.Url));
                }
                else
                {
                    sitemapdata.Add(new SiteMapDataAttributeNode("New Question", Request.Url));
                }

                StackSiteMap(sitemapdata);
            }

            SetDefaultButton(manageAdd);
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
            SessionManager.SystemService.AddSurveyQuestion(SessionManager.Ticket, tw);
            Redirect("SystemSurveyEdit.aspx?id=" + SurveyId.ToString());
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }

    }

    public TransitSurvey Survey
    {
        get
        {
            if (mSurvey == null)
            {
                mSurvey = SessionManager.SystemService.GetSurveyById(SurveyId);
            }
            return mSurvey;
        }
    }
}
