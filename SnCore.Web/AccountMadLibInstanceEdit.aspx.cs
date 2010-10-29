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
using SnCore.SiteMap;

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

    public string InstanceName
    {
        get
        {
            return (string)Request[string.Format("{0}.Name", ObjectName)];
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
        if (!IsPostBack)
        {
            linkCancel.NavigateUrl = ReturnUrl;

            if (!SessionManager.HasVerified())
            {
                ReportWarning("You don't have any verified e-mail addresses and/or profile photos.\n" +
                    "You must add/confirm a valid e-mail address and upload a profile photo before posting mad libs.");

                panelPost.Visible = false;
                post.Enabled = false;
            }

            madLibInstance.MadLibId = MadLibId;
            madLibInstance.DataBind();

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode(ObjectName + "s", Request, string.Format("{0}View.aspx", ObjectName)));
            sitemapdata.Add(new SiteMapDataAttributeNode(InstanceName, Request, ReturnUrl));

            if (MadLibInstanceId > 0)
            {
                TransitMadLibInstance tmi = SessionManager.GetInstance<TransitMadLibInstance, int>(
                    MadLibInstanceId, SessionManager.MadLibService.GetMadLibInstanceById);
                madLibInstance.TextBind(tmi.Text);
            }

            sitemapdata.Add(new SiteMapDataAttributeNode("Mad Lib", Request.Url));
            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(post);
    }

    public void linkToggleBlind_Click(object sender, EventArgs e)
    {
        madLibInstance.IsBlind = !madLibInstance.IsBlind;
        madLibInstance.DataBind();
        linkToggleBlind.Text = (madLibInstance.IsBlind ? "&#187; Reveal" : "&#187; Conceal");
    }

    public void post_Click(object sender, EventArgs e)
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
            SessionManager.CreateOrUpdate<TransitMadLibInstance>(
                madlib, SessionManager.MadLibService.CreateOrUpdateMadLibInstance);
            Redirect(linkCancel.NavigateUrl);
        }
    }
}
