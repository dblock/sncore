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
using System.Collections.Generic;
using SnCore.SiteMap;

public partial class AccountFlagView : AuthenticatedPage
{
    private TransitAccountFlag mFlag = null;

    public TransitAccountFlag Flag
    {
        get
        {
            if (mFlag == null)
            {
                mFlag = SessionManager.AccountService.GetAccountFlagById(
                    SessionManager.Ticket, RequestId);
            }
            return mFlag;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            linkCancel.NavigateUrl = ReturnUrl;

            TransitAccountFlag flag = Flag;

            flagAccountImageLink.HRef = linkAccount.NavigateUrl = string.Format("AccountView.aspx?id={0}", flag.AccountId);
            linkAccount.Text = Renderer.Render(flag.AccountName);
            flagAccountImage.ImageUrl = string.Format("AccountPictureThumbnail.aspx?id={0}&width=75&height=75", flag.AccountPictureId);
            linkFlaggedAccount.NavigateUrl = string.Format("AccountView.aspx?id={0}", flag.FlaggedAccountId);
            linkFlaggedAccount.Text = Renderer.Render(flag.FlaggedAccountName);
            flagCreated.Text = SessionManager.ToAdjustedString(flag.Created);
            flagDescription.Text = Renderer.Render(flag.Description);
            flagType.Text = Renderer.Render(flag.AccountFlagType);

            linkReply.NavigateUrl = string.Format("AccountMessageEdit.aspx?id={0}&ReturnUrl={1}",
                flag.AccountId, UrlEncode(Request.Url.PathAndQuery));

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Reported Abuse", Request, "AccountFlagsManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(flag.FlaggedAccountName, Request, string.Format("AccountView.aspx?id={0}", flag.FlaggedAccountId)));
            sitemapdata.Add(new SiteMapDataAttributeNode(flag.AccountFlagType, Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    public void linkDelete_Click(object sender, EventArgs e)
    {
        SessionManager.Delete<TransitAccountFlag>(RequestId, SessionManager.AccountService.DeleteAccountFlag);
        Redirect(ReturnUrl);
    }

    public string ReturnUrl
    {
        get
        {
            object o = Request.QueryString["ReturnUrl"];
            return (o == null ? "AccountFlagsManage.aspx" : o.ToString());
        }
    }
}
