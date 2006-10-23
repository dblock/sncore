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
using System.Text;
using Wilco.Web.UI.WebControls;
using SnCore.Tools.Drawing;
using System.IO;
using System.Drawing;
using AjaxControlToolkit;

public partial class AccountMadLibInstanceEdit : AuthenticatedPage
{
    public int MadLibInstanceId
    {
        get
        {
            return RequestId;
        }
    }

    public int MadLibId
    {
        get
        {
            return GetId("mid");
        }
    }

    public int ObjectId
    {
        get
        {
            return GetId("oid");
        }
    }

    public int ObjectAccountId
    {
        get
        {
            return GetId("aid");
        }
    }

    public string ObjectName
    {
        get
        {
            string result = Request.Params["ObjectName"];
            
            if (string.IsNullOrEmpty(result))
            {
                throw new Exception("Missing Object Name");
            }

            return result;
        }
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

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(post);
            if (!IsPostBack)
            {
                linkCancel.NavigateUrl = ReturnUrl;

                if (!AccountService.HasVerifiedEmail(SessionManager.Ticket))
                {
                    ReportWarning("You don't have any verified e-mail addresses.\n" +
                        "You must add/confirm a valid e-mail address before posting mad libs.");

                    panelPost.Visible = false;
                    post.Enabled = false;
                }

                madLibInstance.MadLibId = MadLibId;
                madLibInstance.DataBind();

                if (MadLibInstanceId > 0)
                {
                    TransitMadLibInstance tmi = MadLibService.GetMadLibInstanceById(MadLibInstanceId);
                    madLibInstance.TextBind(tmi.Text);
                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void post_Click(object sender, EventArgs e)
    {
        try
        {
            string text = string.Empty;
            if (madLibInstance.TryGetText(ref text))
            {
                TransitMadLibInstance madlib = new TransitMadLibInstance();
                madlib.Id = RequestId;
                madlib.AccountId = SessionManager.Account.Id;
                madlib.MadLibId = MadLibId;
                madlib.ObjectId = ObjectId;
                madlib.ObjectName = ObjectName;
                madlib.Text = text;
                madlib.ObjectUri = ReturnUrl;
                madlib.ObjectAccountId = ObjectAccountId;
                MadLibService.CreateOrUpdateMadLibInstance(SessionManager.Ticket, madlib);
                Redirect(linkCancel.NavigateUrl);
            }            
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
