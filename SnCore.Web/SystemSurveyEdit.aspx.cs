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
using SnCore.SiteMap;

public partial class SystemSurveyEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode("Surveys", Request, "SystemSurveysManage.aspx"));

                if (RequestId > 0)
                {
                    TransitSurvey tw = SessionManager.SystemService.GetSurveyById(RequestId);
                    inputName.Text = Renderer.Render(tw.Name);
                    sitemapdata.Add(new SiteMapDataAttributeNode(tw.Name, Request.Url));
                }
                else
                {
                    sitemapdata.Add(new SiteMapDataAttributeNode("New Survey", Request.Url));
                }

                StackSiteMap(sitemapdata);
            }
        
            SetDefaultButton(manageAdd);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }


        if (RequestId > 0)
        {
            try
            {
                linkNewQuestion.NavigateUrl = "SystemSurveyQuestionEdit.aspx?sid=" + RequestId.ToString();
                gridManage.DataSource = SessionManager.SystemService.GetSurveyQuestions(RequestId);
                gridManage.DataBind();
            }
            catch (Exception ex)
            {
                ReportException(ex);
            }
        }
        else
        {
            panelQuestions.Visible = false;
        }
    }

    public void save_Click(object sender, EventArgs e)
    {
        try
        {
            TransitSurvey tw = new TransitSurvey();
            tw.Name = inputName.Text;
            tw.Id = RequestId;
            SessionManager.SystemService.AddSurvey(SessionManager.Ticket, tw);
            Redirect("SystemSurveysManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    private enum Cells
    {
        id = 0,
        image,
        full,
        edit,
        delete
    };

    public void gridManage_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        try
        {
            int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
            switch (e.CommandName)
            {
                case "Edit":
                    Redirect(string.Format("SystemSurveyQuestionEdit.aspx?sid={0}&id={1}", RequestId, id));
                    break;
                case "Delete":
                    SessionManager.SystemService.DeleteSurveyQuestion(SessionManager.Ticket, id);
                    ReportInfo("Survey question deleted.");
                    gridManage.CurrentPageIndex = 0;
                    gridManage.DataSource = SessionManager.SystemService.GetSurveyQuestions(RequestId);
                    gridManage.DataBind();
                    break;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
