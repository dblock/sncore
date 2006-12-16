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
using SnCore.Services;
using SnCore.SiteMap;

public partial class AccountEventPictureView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        picturesView.OnGetDataSource += new EventHandler(picturesView_OnGetDataSource);
        if (!IsPostBack)
        {
            mPictureId = RequestId;
            GetPictureData(sender, e);
            GetPicturesData(sender, e);

            TransitAccountEvent t = AccountEvent;
            TransitAccountEventPicture p = AccountEventPicture;

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Events", Request, "AccountEventsView.aspx"));
            sitemapdata.AddRange(SiteMapDataAttribute.GetLocationAttributeNodes(Request, "AccountEventsToday.aspx", t.PlaceCountry, t.PlaceState, t.PlaceCity, t.PlaceNeighborhood, t.AccountEventType));
            sitemapdata.Add(new SiteMapDataAttributeNode(t.Name, Request, string.Format("AccountEventView.aspx?id={0}", t.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Pictures", Request.Url));
            StackSiteMap(sitemapdata);
        }

    }

    private int mPictureId = 0;

    public int PictureId
    {
        get
        {
            if (mPictureId == 0)
            {
                mPictureId = RequestId;
            }
            return mPictureId;
        }
    }

    private TransitAccountEventPicture mAccountEventPicture = null;

    TransitAccountEventPicture AccountEventPicture
    {
        get
        {
            if (mAccountEventPicture == null)
            {
                object[] sp_args = { SessionManager.Ticket, PictureId };
                mAccountEventPicture = SessionManager.GetCachedItem<TransitAccountEventPicture>(
                    SessionManager.EventService, "GetAccountEventPictureById", sp_args);
            }
            return mAccountEventPicture;
        }
    }

    private TransitAccountEvent mAccountEvent = null;

    public TransitAccountEvent AccountEvent
    {
        get
        {
            if (mAccountEvent == null)
            {
                object[] as_args = { SessionManager.Ticket, AccountEventPicture.AccountEventId, SessionManager.UtcOffset };
                mAccountEvent = SessionManager.GetCachedItem<TransitAccountEvent>(
                    SessionManager.EventService, "GetAccountEventById", as_args);
            }
            return mAccountEvent;
        }
    }

    void GetPicturesData(object sender, EventArgs e)
    {
        object[] p_args = { AccountEvent.Id };
        picturesView.CurrentPageIndex = 0;
        picturesView.VirtualItemCount = SessionManager.GetCachedCollectionCount(
            SessionManager.EventService, "GetAccountEventPicturesCountById", p_args);
        picturesView_OnGetDataSource(sender, e);
        picturesView.DataBind();
    }

    void GetPictureData(object sender, EventArgs e)
    {
        TransitAccountEventPicture p = AccountEventPicture;

        inputPicture.Src = string.Format("AccountEventPicture.aspx?id={0}", p.Id);
        inputName.Text = Renderer.Render(p.Name);
        inputDescription.Text = Renderer.Render(p.Description);
        inputCreated.Text = Adjust(p.Created).ToString("d");
        inputCounter.Text = p.Counter.Total.ToString();

        TransitAccountEvent t = AccountEvent;

        labelAccountEventName.Text = this.Title = string.Format("{0}: {1}",
            Renderer.Render(t.Name), string.IsNullOrEmpty(p.Name) ? "Untitled" : Renderer.Render(p.Name));

        linkBack.NavigateUrl = string.Format("AccountEventView.aspx?id={0}", t.Id);
        linkBack.Text = string.Format("&#187; Back to {0}", Renderer.Render(t.Name));
        linkComments.Visible = p.CommentCount > 0;
        linkComments.Text = string.Format("&#187; {0} comment{1}",
            (p.CommentCount > 0) ? p.CommentCount.ToString() : "no",
            (p.CommentCount == 1) ? "" : "s");

        linkPrev.Enabled = p.PrevId > 0;
        linkPrev.CommandArgument = p.PrevId.ToString();
        linkNext.Enabled = p.NextId > 0;
        linkNext.CommandArgument = p.NextId.ToString();
        labelIndex.Text = string.Format("{0} / {1}", p.Index + 1, p.Count);

        discussionComments.ReturnUrl = string.Format("AccountEventPictureView.aspx?id={0}", PictureId);
        discussionComments.DiscussionId = SessionManager.DiscussionService.GetAccountEventPictureDiscussionId(PictureId);
        discussionComments.DataBind();
    }

    public void picturesView_ItemCommand(object source, CommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Picture":
                mPictureId = int.Parse(e.CommandArgument.ToString());
                GetPictureData(source, e);
                break;
        }
    }

    void picturesView_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions(picturesView.PageSize, picturesView.CurrentPageIndex);
        object[] args = { AccountEvent.Id, options };
        picturesView.DataSource = SessionManager.GetCachedCollection<TransitAccountEventPicture>(
            SessionManager.EventService, "GetAccountEventPicturesById", args);
    }

}
