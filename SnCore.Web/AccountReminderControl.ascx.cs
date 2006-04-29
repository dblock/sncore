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
using System.Collections.Generic;

public partial class AccountReminder : Control
{
    public string Style
    {
        get
        {
            return noticeReminder.Style;
        }
        set
        {
            noticeReminder.Style = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (!SessionManager.IsLoggedIn)
                    return; 

                if (SessionManager.Account.PictureId == 0)
                {
                    noticeReminder.HtmlEncode = false;
                    noticeReminder.Info = "Don't be shy! Upload a picture. " + 
                        "<a href='AccountPicturesManage.aspx'>Click here!</a>";
                    return;
                }

                List<TransitSurvey> surveys = SystemService.GetSurveys();

                if (surveys != null)
                {
                    foreach (TransitSurvey survey in surveys)
                    {
                        int answers = AccountService.GetAccountSurveyAnswersCountById(
                            SessionManager.Account.Id, survey.Id);

                        if (answers == 0)
                        {
                            noticeReminder.HtmlEncode = false;
                            noticeReminder.Info = string.Format(
                                "Participate! Complete the {1} survey. " + 
                                "<a href='AccountSurvey.aspx?id={0}'>Click here!</a>",
                                survey.Id,
                                Renderer.Render(survey.Name));

                            return;
                        }
                    }
                }

                int storiescount = StoryService.GetAccountStoriesCountById(SessionManager.Account.Id);

                if (storiescount == 0)
                {
                    noticeReminder.HtmlEncode = false;
                    noticeReminder.Info = 
                        "Participate! Post a story with pictures. " +
                        "<a href='AccountStoryEdit.aspx'>Click here!</a>";
                    return;
                }

                int invitationscount = AccountService.GetAccountInvitationsCountById(SessionManager.Account.Id);

                if (invitationscount == 0)
                {
                    noticeReminder.HtmlEncode = false;
                    noticeReminder.Info =
                        "Invite your friends to participate! " +
                        "<a href='AccountInvitationsManage.aspx'>Click here!</a>";
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            noticeReminder.Exception = ex;
        }
    }
}
