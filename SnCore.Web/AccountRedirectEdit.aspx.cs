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

public partial class AccountRedirectEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Redirects", Request, "AccountRedirectsManage.aspx"));

            int id = RequestId;

            if (id > 0)
            {
                TransitAccountRedirect tr = SessionManager.AccountService.GetAccountRedirectById(SessionManager.Ticket, id);
                inputSourceUri.Text = tr.SourceUri;
                inputTargetUri.Text = tr.TargetUri;
                sitemapdata.Add(new SiteMapDataAttributeNode(tr.SourceUri, Request.Url));
            }
            else
            {
                sitemapdata.Add(new SiteMapDataAttributeNode("New Redirect", Request.Url));
            }

            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(manageAdd);
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitAccountRedirect tr = new TransitAccountRedirect();

        if (!Uri.IsWellFormedUriString(inputSourceUri.Text, UriKind.Relative))
            throw new Exception(string.Format("Invalid relative uri {0}", inputSourceUri.Text));

        if (!Uri.IsWellFormedUriString(inputTargetUri.Text, UriKind.Relative))
            throw new Exception(string.Format("Invalid relative uri {0}", inputTargetUri.Text));

        tr.SourceUri = inputSourceUri.Text;
        tr.TargetUri = inputTargetUri.Text;
        tr.Id = RequestId;
        SessionManager.CreateOrUpdate<TransitAccountRedirect>(
            tr, SessionManager.AccountService.CreateOrUpdateAccountRedirect);
        Redirect("AccountRedirectsManage.aspx");
    }
}
