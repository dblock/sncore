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

public partial class AccountEventPictureView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            picturesView.OnGetDataSource += new EventHandler(picturesView_OnGetDataSource);
            if (!IsPostBack)
            {
                mPictureId = RequestId;
                GetPictureData(sender, e);
                GetPicturesData(sender, e);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
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
                    EventService, "GetAccountEventPictureById", sp_args);
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
                    EventService, "GetAccountEventById", as_args);
            }
            return mAccountEvent;
        }
    }

    void GetPicturesData(object sender, EventArgs e)
    {
        object[] p_args = { AccountEvent.Id };
        picturesView.CurrentPageIndex = 0;
        picturesView.VirtualItemCount = SessionManager.GetCachedCollectionCount(
            EventService, "GetAccountEventPicturesCountById", p_args);
        picturesView_OnGetDataSource(sender, e);
        picturesView.DataBind();
    }

    void GetPictureData(object sender, EventArgs e)
    {
        TransitAccountEventPicture p = AccountEventPicture;

        inputPicture.Src = string.Format("AccountEventPicture.aspx?id={0}", p.Id);
        inputName.Text = Renderer.Render(p.Name);
        inputDescription.Text = Renderer.Render(p.Description);
        inputCreated.Text = Adjust(p.Created).ToString();

        TransitAccountEvent l = AccountEvent;

        labelAccountEventName.Text = this.Title = string.Format("{0}: {1}",
            Renderer.Render(l.Name), string.IsNullOrEmpty(p.Name) ? "Untitled" : Renderer.Render(p.Name));

        labelPicture.Text = Renderer.Render(p.Name);

        linkAccountEvent.Text = Renderer.Render(l.Name);
        linkCity.Text = Renderer.Render(l.PlaceCity);
        linkState.Text = Renderer.Render(l.PlaceState);
        linkCountry.Text = Renderer.Render(l.PlaceCountry);
        linkType.Text = l.AccountEventType + "s";

        linkType.NavigateUrl = string.Format("AccountEventsView.aspx?city={0}&state={1}&country={2}&type={3}",
            Renderer.UrlEncode(l.PlaceCity),
            Renderer.UrlEncode(l.PlaceState),
            Renderer.UrlEncode(l.PlaceCountry),
            Renderer.UrlEncode(l.AccountEventType));

        linkCity.NavigateUrl = string.Format("AccountEventsView.aspx?city={0}&state={1}&country={2}",
            Renderer.UrlEncode(l.PlaceCity),
            Renderer.UrlEncode(l.PlaceState),
            Renderer.UrlEncode(l.PlaceCountry));

        linkState.NavigateUrl = string.Format("AccountEventsView.aspx?state={0}&country={1}",
            Renderer.UrlEncode(l.PlaceState),
            Renderer.UrlEncode(l.PlaceCountry));

        linkCountry.NavigateUrl = string.Format("AccountEventsView.aspx?country={0}",
            Renderer.UrlEncode(l.PlaceCountry));

        linkAccountEvent.NavigateUrl = string.Format("AccountEventView.aspx?id={0}", l.Id);

        linkBack.NavigateUrl = linkAccountEvent.NavigateUrl = string.Format("AccountEventView.aspx?id={0}", l.Id);
        linkBack.Text = string.Format("&#187; Back to {0}", Renderer.Render(l.Name));
        linkComments.Visible = p.CommentCount > 0;
        linkComments.Text = string.Format("&#187; {0} comment{1}",
            (p.CommentCount > 0) ? p.CommentCount.ToString() : "no",
            (p.CommentCount == 1) ? "" : "s");

        linkPrev.Enabled = p.PrevId > 0;
        linkPrev.CommandArgument = p.PrevId.ToString();
        linkNext.Enabled = p.NextId > 0;
        linkNext.CommandArgument = p.NextId.ToString();
        labelIndex.Text = string.Format("{0} / {1}", p.Index + 1, p.Count); 

        discussionComments.DiscussionId = DiscussionService.GetAccountEventPictureDiscussionId(PictureId);
        discussionComments.DataBind();
    }

    public void picturesView_ItemCommand(object source, CommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "Picture":
                    mPictureId = int.Parse(e.CommandArgument.ToString());
                    GetPictureData(source, e);
                    break;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void picturesView_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions options = new ServiceQueryOptions(picturesView.PageSize, picturesView.CurrentPageIndex);
            object[] args = { AccountEvent.Id, options };
            picturesView.DataSource = SessionManager.GetCachedCollection<TransitAccountEventPicture>(
                EventService, "GetAccountEventPicturesById", args);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

}
