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
using Wilco.Web.UI.WebControls;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using SnCore.Tools.Drawing;
using SnCore.Tools.Web;
using SnCore.WebServices;
using SnCore.Services;
using SnCore.SiteMap;

public partial class AccountRssWatchEdit : AuthenticatedPage
{
    public string ReturnUrl
    {
        get
        {
            string returnurl = Request.QueryString["ReturnUrl"];
            if (string.IsNullOrEmpty(returnurl)) returnurl = "AccountRssWatchsManage.aspx";
            return returnurl;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Subscriptions", Request, "AccountRssWatchsManage.aspx"));

            if (RequestId > 0)
            {
                TransitAccountRssWatch tf = SessionManager.SyndicationService.GetAccountRssWatchById(
                    SessionManager.Ticket, RequestId);

                inputName.Text = tf.Name;
                inputRssWatchUrl.Text = tf.Url;
                inputEnabled.Checked = tf.Enabled;

                if (tf.UpdateFrequency > 0)
                {
                    ListItem item = inputUpdateFrequency.Items.FindByValue(
                        tf.UpdateFrequency.ToString());

                    if (item == null)
                    {
                        item = new ListItem(string.Format(
                            "Every {0} Hours", tf.UpdateFrequency),
                            tf.UpdateFrequency.ToString());
                        inputUpdateFrequency.Items.Add(item);
                    }

                    item.Selected = true;
                }

                sitemapdata.Add(new SiteMapDataAttributeNode(tf.Name, Request.Url));
            }

            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(linkSave);
    }

    public void save(object sender, EventArgs e)
    {
        TransitAccountRssWatch s = new TransitAccountRssWatch();
        s.Id = RequestId;
        s.Name = inputName.Text;
        s.AccountId = SessionManager.Account.Id;
        s.Url = inputRssWatchUrl.Text;
        s.Enabled = inputEnabled.Checked;
        s.UpdateFrequency = int.Parse(inputUpdateFrequency.SelectedValue);

        if (s.Id == 0)
        {
            List<TransitAccountRssWatch> rsswatchs = SessionManager.SyndicationService.GetAccountRssWatchs(
                SessionManager.Ticket, SessionManager.AccountId, null);

            foreach (TransitAccountRssWatch rsswatch in rsswatchs)
            {
                if (rsswatch.Name.ToLower() == s.Name.ToLower())
                {
                    throw new Exception(string.Format("A subscription with the same name '{0}' already exists. " +
                        "Click <a href='AccountRssWatchEdit.aspx?id={1}'>here</a> to modify it.", 
                        Renderer.Render(rsswatch.Name), rsswatch.Id));
                }

                if (rsswatch.Url.ToLower() == s.Url.ToLower())
                {
                    throw new Exception(string.Format("A subscription with the same address already exists. " +
                        "The subscription name is '{0}' and the address is '{1}'. " +
                        "Click <a href='AccountRssWatchEdit.aspx?id={2}'>here</a> to modify it.", 
                        Renderer.Render(rsswatch.Name), Renderer.Render(rsswatch.Url), rsswatch.Id));
                }
            }
        }

        SessionManager.CreateOrUpdate<TransitAccountRssWatch>(
            s, SessionManager.SyndicationService.CreateOrUpdateAccountRssWatch);

        Redirect("AccountRssWatchsManage.aspx");
    }

    public void linkBack_Click(object sender, EventArgs e)
    {
        Redirect(ReturnUrl);
    }
}
