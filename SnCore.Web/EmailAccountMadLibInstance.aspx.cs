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
using SnCore.Tools.Web;

public partial class EmailAccountMadLibInstance : AuthenticatedPage
{
    private TransitMadLibInstance mMadLibInstance;

    public TransitMadLibInstance MadLibInstance
    {
        get
        {
            if (mMadLibInstance == null)
            {
                mMadLibInstance = SessionManager.MadLibService.GetMadLibInstanceById(
                    SessionManager.Ticket, RequestId);
            }
            return mMadLibInstance;
        }
    }

    private TransitAccount mRecepient = null;

    public TransitAccount Recepient
    {
        get
        {
            if (mRecepient == null)
            {
                mRecepient = SessionManager.AccountService.GetAccountById(
                    SessionManager.Ticket, GetId("aid"));
            }

            return mRecepient;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {

    }

    public static string RenderMadLib(string value)
    {
        return value.Replace("[", "<em>").Replace("]", "</em>");
    }

    public string ReturnUrl
    {
        get
        {
            string result = Request.Params["ReturnUrl"];
            if (string.IsNullOrEmpty(result)) result = "Default.aspx";
            return result;
        }
    }
}

