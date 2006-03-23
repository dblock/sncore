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

public partial class AccountFeedEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(linkSave);
            if (!IsPostBack)
            {
                ArrayList types = new ArrayList();
                types.Add(new TransitFeedType());
                types.AddRange(SyndicationService.GetFeedTypes());
                inputFeedType.DataSource = types;
                inputFeedType.DataBind();

                string feedtype = Request.Params["type"];
                if (!string.IsNullOrEmpty(feedtype))
                {
                    ListItem item = inputFeedType.Items.FindByValue(feedtype);
                    if (item != null) item.Selected = true;
                }

                if (RequestId > 0)
                {
                    TransitAccountFeed tf = SyndicationService.GetAccountFeedById(
                        SessionManager.Ticket, RequestId);

                    inputName.Text = tf.Name;
                    inputDescription.Text = tf.Description;
                    inputLinkUrl.Text = tf.LinkUrl;
                    inputFeedUrl.Text = tf.FeedUrl;
                    inputUsername.Text = tf.Username;
                    inputPublish.Checked = tf.Publish;
                    inputPassword.Attributes["value"] = tf.Password;

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

                    if (!string.IsNullOrEmpty(tf.FeedType))
                    {
                        inputFeedType.Items.FindByValue(tf.FeedType).Selected = true;
                    }
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

                    if (Request.Params["type"] != null)
                    {
                        ListItem item = inputFeedType.Items.FindByValue(Request.Params["type"]);
                        if (item != null)
                        {
                            item.Selected = true;
                            inputFeedType.Enabled = false;
                        }
                    }

                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void save(object sender, EventArgs e)
    {
        try
        {
            TransitAccountFeed s = new TransitAccountFeed();
            s.Id = RequestId;
            s.Name = inputName.Text;
            s.Description = inputDescription.Text;
            s.AccountId = SessionManager.Account.Id;
            s.FeedType = inputFeedType.SelectedValue;

            if (! string.IsNullOrEmpty(inputFeedUrl.Text) && !Uri.IsWellFormedUriString(inputFeedUrl.Text, UriKind.Absolute))
                inputFeedUrl.Text = "http://" + inputFeedUrl.Text;

            if (!string.IsNullOrEmpty(inputLinkUrl.Text) && !Uri.IsWellFormedUriString(inputLinkUrl.Text, UriKind.Absolute))
                inputLinkUrl.Text = "http://" + inputLinkUrl.Text;

            s.FeedUrl = inputFeedUrl.Text;
            s.LinkUrl = inputLinkUrl.Text;
            s.Publish = inputPublish.Checked;
            SyndicationService.CreateOrUpdateAccountFeed(SessionManager.Ticket, s);
            Redirect("AccountFeedsManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
