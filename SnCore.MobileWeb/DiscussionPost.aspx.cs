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
using SnCore.WebServices;
using SnCore.SiteMap;
using System.Collections.Generic;
using System.Text;

public partial class DiscussionPostNew : AuthenticatedPage
{
    public int PostId
    {
        get
        {
            return RequestId;
        }
    }

    public int DiscussionId
    {
        get
        {
            return GetId("did");
        }
    }

    public int ParentId
    {
        get
        {
            return GetId("pid");
        }
    }

    public bool Quote
    {
        get
        {
            object result = Request["Quote"];
            if (result != null) return bool.Parse(result.ToString());
            return false;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();

            if (DiscussionId > 0)
            {
                DiscussionService.TransitDiscussion td = SessionManager.DiscussionService.GetDiscussionById(
                    SessionManager.Ticket, DiscussionId);

                inputSticky.Enabled = td.CanUpdate;

                if (!string.IsNullOrEmpty(td.ParentObjectName))
                {
                    sitemapdata.Add(new SiteMapDataAttributeNode(td.ParentObjectName, Request, td.ParentObjectUri));
                    discussionLabelLink.Text = Renderer.Render(td.ParentObjectName);
                    discussionLabelLink.NavigateUrl = td.ParentObjectUri;
                }
                else
                {
                    discussionLabelLink.Text = Renderer.Render(td.Name);
                    discussionLabelLink.NavigateUrl = string.Format("DiscussionView.aspx?id={0}", td.Id);
                    sitemapdata.Add(new SiteMapDataAttributeNode("Discussions", Request, "DiscussionsView.aspx"));
                    sitemapdata.Add(new SiteMapDataAttributeNode(td.Name, Request, string.Format("DiscussionView.aspx?id={0}", td.Id)));
                }
            }

            StringBuilder body = new StringBuilder();

            if (PostId > 0)
            {
                DiscussionService.TransitDiscussionPost tw = SessionManager.DiscussionService.GetDiscussionPostById(
                    SessionManager.Ticket, PostId);
                inputSubject.Text = tw.Subject;
                inputSticky.Checked = tw.Sticky;
                body.Append(tw.Body);
                sitemapdata.Add(new SiteMapDataAttributeNode(tw.Subject, Request.Url));
            }
            else
            {
                sitemapdata.Add(new SiteMapDataAttributeNode("New Post", Request.Url));
            }

            if (ParentId > 0)
            {
                DiscussionService.TransitDiscussionPost rp = SessionManager.DiscussionService.GetDiscussionPostById(
                    SessionManager.Ticket, ParentId);
                panelReplyTo.Visible = true;
                replytoSenderName.NavigateUrl = string.Format("AccountView.aspx?id={0}", rp.AccountId);
                replytoSenderName.Text = Renderer.Render(rp.AccountName);
                replyToBody.Text = Renderer.RenderEx(rp.Body);
                replytoCreated.Text = SessionManager.ToAdjustedString(rp.Created);
                replytoSubject.Text = Renderer.Render(rp.Subject);
                inputSubject.Text = rp.Subject.StartsWith("Re:") ? rp.Subject : "Re: " + rp.Subject;
                rowsubject.Attributes["style"] = "display: none;";
                labelPostingReplying.Text = "replying";

                if (Quote)
                {
                    body.AppendFormat("<P>[quote]<DIV>on {0} {1} wrote:</DIV><DIV>{2}</DIV>[/quote]</P>",
                        rp.Created.ToString("d"), rp.AccountName, rp.Body);
                }
            }

            if ((PostId == 0) && !string.IsNullOrEmpty(SessionManager.Account.Signature))
            {
                body.Append("<BR /><BR />");
                body.Append("<P>");
                body.Append(Renderer.RenderEx(SessionManager.Account.Signature));
                body.Append("</P>");
            }

            inputBody.Text = body.ToString();

            StackSiteMap(sitemapdata);
        }

        if (!SessionManager.HasVerifiedEmailAddress())
        {
            ReportWarning("You don't have any verified e-mail addresses.\n" +
                "You must add/confirm a valid e-mail address before posting messages.");

            panelPost.Visible = false;
            post.Enabled = false;
        }

        SetDefaultButton(post);
    }

    public void post_Click(object sender, EventArgs e)
    {
        DiscussionService.TransitDiscussionPost tw = new DiscussionService.TransitDiscussionPost();
        tw.Subject = inputSubject.Text;
        if (string.IsNullOrEmpty(tw.Subject)) tw.Subject = "Untitled";
        tw.Body = inputBody.Text;
        tw.Id = PostId;
        tw.DiscussionPostParentId = ParentId;
        tw.DiscussionId = DiscussionId;
        if (inputSticky.Enabled) tw.Sticky = inputSticky.Checked;
        SessionManager.CreateOrUpdate<DiscussionService.TransitDiscussionPost, DiscussionService.ServiceQueryOptions>(
            tw, SessionManager.DiscussionService.CreateOrUpdateDiscussionPost);
        SessionManager.InvalidateCache<DiscussionService.TransitDiscussion, DiscussionService.ServiceQueryOptions>();
        SessionManager.InvalidateCache<DiscussionService.TransitDiscussionThread, DiscussionService.ServiceQueryOptions>();
        Redirect(ReturnUrl);
    }

    public string ReturnUrl
    {
        get
        {
            object o = Request.QueryString["ReturnUrl"];
            return (o == null ? string.Format("DiscussionView.aspx?id={0}", DiscussionId) : o.ToString());
        }
    }
}
