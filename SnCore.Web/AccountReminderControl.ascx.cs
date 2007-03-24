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
using SnCore.WebServices;

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

            int invitationscount = SessionManager.GetCount<TransitAccountInvitation, int>(
                SessionManager.AccountId, SessionManager.AccountService.GetAccountInvitationsCountByAccountId);

            if (invitationscount == 0)
            {
                noticeReminder.HtmlEncode = false;
                noticeReminder.Info = string.Format(
                    "Invite your friends to {0}! " +
                    "<a href='AccountInvitationsManage.aspx'>Click here!</a>",
                    Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore")));
                return;
            }

            IList<TransitSurvey> surveys = SessionManager.GetCollection<TransitSurvey>(
                (ServiceQueryOptions) null, SessionManager.ObjectService.GetSurveys);

            if (surveys != null)
            {
                foreach (TransitSurvey survey in surveys)
                {
                    int answers = SessionManager.GetCount<TransitAccountSurveyAnswer, int, int>(
                        SessionManager.AccountId, survey.Id, SessionManager.AccountService.GetAccountSurveyAnswersCount);

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
        }
    }
}
