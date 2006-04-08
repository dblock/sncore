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
    public void Page_Load()
    {
        try
        {
            if (!IsPostBack)
            {
                TransitAccountEventPicture p = EventService.GetAccountEventPictureById(SessionManager.Ticket, RequestId);
                inputPicture.Src = string.Format("AccountEventPicture.aspx?id={0}", RequestId);
                inputName.Text = Renderer.Render(p.Name);
                inputDescription.Text = Renderer.Render(p.Description);
                inputCreated.Text = Adjust(p.Created).ToString();

                TransitAccountEvent l = EventService.GetAccountEventById(SessionManager.Ticket, p.AccountEventId);
                this.Title = string.Format("{0} {1}", Renderer.Render(l.Name), Renderer.Render(p.Name));

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

                linkComments.Visible = p.CommentCount > 0;
                linkComments.Text = string.Format("{0} comment{1}",
                    (p.CommentCount > 0) ? p.CommentCount.ToString() : "no",
                    (p.CommentCount == 1) ? "" : "s");

                discussionComments.DiscussionId = DiscussionService.GetAccountEventPictureDiscussionId(RequestId);
                discussionComments.DataBind();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }

    }
}
