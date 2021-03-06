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
using SnCore.Data.Hibernate;

public partial class SystemSurveyQuestionEdit : AuthenticatedPage
{
    private TransitSurvey mSurvey = null;

    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("System Preferences", Request, "SystemPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Surveys", Request, "SystemSurveysManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(Survey.Name, Request, string.Format("SystemSurveyEdit.aspx?id={0}", SurveyId)));

            DomainClass cs = SessionManager.GetDomainClass("SurveyQuestion");
            inputQuestion.MaxLength = cs["Question"].MaxLengthInChars;

            linkBack.NavigateUrl = "SystemSurveyEdit.aspx?id=" + SurveyId.ToString();
            if (RequestId > 0)
            {
                TransitSurveyQuestion tw = SessionManager.ObjectService.GetSurveyQuestionById(
                    SessionManager.Ticket, RequestId);
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

    public int SurveyId
    {
        get
        {
            return GetId("sid");
        }
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitSurveyQuestion tw = new TransitSurveyQuestion();
        tw.Question = inputQuestion.Text;
        tw.Id = RequestId;
        tw.SurveyId = SurveyId;
        SessionManager.CreateOrUpdate<TransitSurveyQuestion>(
            tw, SessionManager.ObjectService.CreateOrUpdateSurveyQuestion);
        Redirect("SystemSurveyEdit.aspx?id=" + SurveyId.ToString());
    }

    public TransitSurvey Survey
    {
        get
        {
            if (mSurvey == null)
            {
                mSurvey = SessionManager.ObjectService.GetSurveyById(
                    SessionManager.Ticket, SurveyId);
            }
            return mSurvey;
        }
    }
}
