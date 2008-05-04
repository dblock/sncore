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
using SnCore.Data.Hibernate;
using SnCore.WebControls;

public partial class AccountFeedEdit : AuthenticatedPage
{
    public string ReturnUrl
    {
        get
        {
            string returnurl = Request.QueryString["ReturnUrl"];
            if (string.IsNullOrEmpty(returnurl)) returnurl = "AccountFeedsManage.aspx";
            return returnurl;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Syndication", Request, "AccountFeedsManage.aspx"));

            DomainClass cs = SessionManager.GetDomainClass("AccountFeed");
            inputName.MaxLength = cs["Name"].MaxLengthInChars;
            inputFeedUrl.MaxLength = cs["FeedUrl"].MaxLengthInChars;
            inputUsername.MaxLength = cs["Username"].MaxLengthInChars;
            inputPassword.MaxLength = cs["Password"].MaxLengthInChars;
            inputLinkUrl.MaxLength = cs["LinkUrl"].MaxLengthInChars;

            GetFeedTypes(sender, e);

            string feedtype = Request.Params["type"];
            ListItemManager.TrySelect(selectType, feedtype);

            if (RequestId > 0)
            {
                TransitAccountFeed tf = SessionManager.SyndicationService.GetAccountFeedById(
                    SessionManager.Ticket, RequestId);

                inputName.Text = tf.Name;
                inputDescription.Text = tf.Description;
                inputLinkUrl.Text = tf.LinkUrl;
                inputFeedUrl.Text = tf.FeedUrl;
                inputUsername.Text = tf.Username;
                inputShown.Checked = ! tf.Hidden;
                inputPublish.Checked = tf.Publish;
                inputPublishImgs.Checked = tf.PublishImgs;
                inputPublishMedia.Checked = tf.PublishMedia;
                inputPassword.Attributes["value"] = tf.Password;

                if (tf.UpdateFrequency > 0)
                {
                    ListItemManager.SelectAdd(
                        inputUpdateFrequency, 
                        string.Format("Every {0} Hours", tf.UpdateFrequency), 
                        tf.UpdateFrequency);
                }

                ListItemManager.TrySelect(selectType, tf.FeedType);
                sitemapdata.Add(new SiteMapDataAttributeNode(tf.Name, Request.Url));
                feedredirect.TargetUri = string.Format("AccountFeedView.aspx?id={0}", tf.Id);
            }
            else
            {
                if (Request.Params["name"] != null)
                {
                    inputName.Text = Request.Params["name"];
                    inputUsername.Enabled = false;
                    inputPassword.Enabled = false;
                    inputUpdateFrequency.Enabled = false;
                }

                if (Request.Params["feed"] != null)
                {
                    inputFeedUrl.Text = Request.Params["feed"];
                    inputFeedUrl.Enabled = false;
                }

                if (Request.Params["link"] != null)
                {
                    inputLinkUrl.Text = Request.Params["link"];
                    inputLinkUrl.Enabled = false;
                }

                if (Request.Params["description"] != null)
                {
                    inputDescription.Text = Request.Params["description"];
                }

                string type = Request.Params["type"];
                ListItemManager.TrySelect(selectType, type);
                sitemapdata.Add(new SiteMapDataAttributeNode("New Feed", Request.Url));
                feedredirect.Visible = false;
            }

            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(linkSave);
    }

    public void save(object sender, EventArgs e)
    {
        TransitAccountFeed s = new TransitAccountFeed();
        s.Id = RequestId;
        s.Name = inputName.Text;
        s.Description = inputDescription.Text;
        s.AccountId = SessionManager.Account.Id;
        s.FeedType = selectType.SelectedValue;
        s.UpdateFrequency = int.Parse(inputUpdateFrequency.SelectedValue);

        if (!string.IsNullOrEmpty(inputFeedUrl.Text) && !Uri.IsWellFormedUriString(inputFeedUrl.Text, UriKind.Absolute))
            inputFeedUrl.Text = "http://" + inputFeedUrl.Text;

        if (!string.IsNullOrEmpty(inputLinkUrl.Text) && !Uri.IsWellFormedUriString(inputLinkUrl.Text, UriKind.Absolute))
            inputLinkUrl.Text = "http://" + inputLinkUrl.Text;

        s.FeedUrl = inputFeedUrl.Text;
        s.LinkUrl = inputLinkUrl.Text;
        s.Publish = inputPublish.Checked;
        s.Hidden = ! inputShown.Checked;
        s.PublishImgs = inputPublishImgs.Checked;
        s.PublishMedia = inputPublishMedia.Checked;

        if (s.Id == 0)
        {
            TransitAccountFeedQueryOptions qopt = new TransitAccountFeedQueryOptions();
            qopt.AccountId = SessionManager.AccountId;
            qopt.PublishedOnly = false;
            qopt.PicturesOnly = false;
            qopt.WithFeedItemsOnly = false;
            List<TransitAccountFeed> feeds = SessionManager.SyndicationService.GetAccountFeeds(
                SessionManager.Ticket, qopt, null);

            foreach (TransitAccountFeed feed in feeds)
            {
                if (feed.Name.Trim().ToLower() == s.Name.Trim().ToLower())
                {
                    throw new Exception(string.Format("A syndicated feed with the same name '{0}' already exists. " +
                        "Click <a href='AccountFeedEdit.aspx?id={1}'>here</a> to modify it.", 
                        Renderer.Render(feed.Name), feed.Id));
                }

                if (feed.FeedUrl.Trim().ToLower() == s.FeedUrl.Trim().ToLower())
                {
                    throw new Exception(string.Format("A syndicated feed with the same feed address already exists. " +
                        "The feed name is '{0}' and the address is '{1}'. " +
                        "Click <a href='AccountFeedEdit.aspx?id={2}'>here</a> to modify it.", 
                        Renderer.Render(feed.Name), Renderer.Render(feed.FeedUrl), feed.Id));
                }
            }
        }

        SessionManager.CreateOrUpdate<TransitAccountFeed>(
            s, SessionManager.SyndicationService.CreateOrUpdateAccountFeed);
        Redirect("AccountFeedsManage.aspx");
    }

    public void linkBack_Click(object sender, EventArgs e)
    {
        Redirect(ReturnUrl);
    }

    private void GetFeedTypes(object sender, EventArgs e)
    {
        List<TransitFeedType> types = SessionManager.SyndicationService.GetFeedTypes(SessionManager.Ticket, null);

        TransitFeedType selected = null;
        foreach (TransitFeedType Feedtype in types)
        {
            if (Feedtype.DefaultType)
            {
                selected = Feedtype;
                break;
            }
        }

        if (selected == null)
        {
            types.Insert(0, new TransitFeedType());
        }

        selectType.DataSource = types;
        selectType.DataBind();

        if (selected != null)
        {
            ListItemManager.TrySelect(selectType, selected.Name);
        }
    }
}
