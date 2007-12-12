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
using SnCore.Data.Hibernate;
using SnCore.WebControls;

public partial class AccountFlagEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Flags", Request, "AccountFlagsManage.aspx"));

            inputType.DataSource = SessionManager.AccountService.GetAccountFlagTypes(
                SessionManager.Ticket, null);
            inputType.DataBind();

            string type = Request["type"];
            ListItemManager.TrySelect(inputType, type);
 
            TransitAccount account = SessionManager.AccountService.GetAccountById(
                SessionManager.Ticket, GetId("aid"));

            if (account == null)
            {
                ReportWarning("Account has already been deleted.");
                panelMessage.Visible = false;
                return;
            }

            linkKeen.NavigateUrl = string.Format("AccountView.aspx?id={0}", account.Id.ToString());
            linkKeen.Text = Renderer.Render(account.Name);
            imageKeen.ImageUrl = string.Format("AccountPictureThumbnail.aspx?id={0}&width=75&height=75", account.PictureId);

            inputDescription.Text = string.Format("Dear Administrator,\n\nI would like to report {0} from {1}. Please take a look at {2}.\n\nThx\n{3}",
                Renderer.Render(Request["type"]), Renderer.Render(account.Name), Renderer.Render(Request["url"]), Renderer.Render(SessionManager.Account.Name));

            sitemapdata.Add(new SiteMapDataAttributeNode(string.Format("Report {0}", 
                Renderer.Render(Request["type"])), Request.Url));

            linkBack.NavigateUrl = ReturnUrl;

            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(manageAdd);
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitAccountFlag tw = new TransitAccountFlag();
        tw.Description = inputDescription.Text;
        tw.Url = Request["url"];
        tw.FlaggedAccountId = GetId("aid");
        tw.AccountFlagType = inputType.SelectedValue;
        tw.Id = RequestId;
        SessionManager.CreateOrUpdate<TransitAccountFlag>(
            tw, SessionManager.AccountService.CreateOrUpdateAccountFlag);
        Redirect(ReturnUrl);
    }

    public string ReturnUrl
    {
        get
        {
            string result = Request.QueryString["ReturnUrl"];
            if (string.IsNullOrEmpty(result)) result = "AccountFlagsManage.aspx";
            return result;
        }
    }
}
