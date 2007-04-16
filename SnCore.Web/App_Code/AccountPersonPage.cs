using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SnCore.Tools.Web;
using SnCore.Services;

public class AccountPersonPage : Page
{
    private TransitAccount mAccount = null;

    public AccountPersonPage()
    {

    }

    public string WebsiteUrl
    {
        get
        {
            return SessionManager.WebsiteUrl;
        }
    }

    public TransitAccount Account
    {
        get
        {
            if (mAccount == null)
            {
                mAccount = (RequestId != 0)
                    ? SessionManager.AccountService.GetAccountById(SessionManager.Ticket, RequestId) 
                    : SessionManager.Account;
            }

            return mAccount;
        }
    }

    public string Link
    {
        get
        {
            return new Uri(SessionManager.WebsiteUri, "Default.aspx").ToString();
        }
    }

    public string GetNewPictures(int count)
    {
        if (count == 0)
        {
            return string.Empty;
        }
        else if (count == 1)
        {
            return "1 new picture";
        }
        else
        {
            return string.Format("{0} new pictures", count);
        }
    }

    public string GetNewFriends(int count)
    {
        if (count == 0)
        {
            return string.Empty;
        }
        else if (count == 1)
        {
            return "1 new friend";
        }
        else
        {
            return string.Format("{0} new friends", count);
        }
    }

    public string GetNewSyndicatedContent(int count)
    {
        if (count == 0)
        {
            return string.Empty;
        }
        else if (count == 1)
        {
            return "1 new syndicated feed";
        }
        else
        {
            return string.Format("{0} new syndicated feeds", count);
        }
    }

    public string GetNewDiscussionPosts(int count)
    {
        if (count == 0)
        {
            return string.Empty;
        }
        else if (count == 1)
        {
            return "1 new discussion post";
        }
        else
        {
            return string.Format("{0} new discussion posts", count);
        }
    }

    public int GetAccountStoryId(TransitAccountStory s)
    {
        return (s == null) ? 0 : s.Id;
    }

    public string GetAccountStory(TransitAccountStory s)
    {
        if (s == null)
        {
            return string.Empty;
        }
        else
        {
            return "new story: " + Renderer.Render(s.Name);
        }
    }

    public int GetSurveyId(TransitSurvey s)
    {
        return (s == null) ? 0 : s.Id;
    }

    public string GetSurvey(TransitSurvey s)
    {
        if (s == null)
        {
            return string.Empty;
        }
        else
        {
            return "new survey: " + Renderer.Render(s.Name);
        }
    }

    public int AccountId
    {
        get
        {
            try
            {
                if (RequestId > 0) return RequestId;
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
}
