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

public partial class AccountEventPictureEdit : AuthenticatedPage
{
    public int AccountEventId
    {
        get
        {
            return GetId("pid");
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(manageAdd);
            if (!IsPostBack)
            {
                TransitAccountEvent p = EventService.GetAccountEventById(SessionManager.Ticket, AccountEventId, SessionManager.UtcOffset);
                linkBack.NavigateUrl = linkSection.NavigateUrl = string.Format("AccountEventPicturesManage.aspx?id={0}", p.Id);
                linkAccountEvent.Text = Renderer.Render(p.Name);
                linkAccountEvent.NavigateUrl = string.Format("AccountEventView.aspx?id={0}", p.Id);

                if (RequestId > 0)
                {
                    TransitAccountEventPicture t = EventService.GetAccountEventPictureById(SessionManager.Ticket, RequestId);
                    linkItem.Text = Renderer.Render(t.Name);
                    inputName.Text = t.Name;
                    inputDescription.Text = t.Description;
                    imageFull.ImageUrl = string.Format("AccountEventPicture.aspx?id={0}&CacheDuration=0", t.Id);
                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void save_Click(object sender, EventArgs e)
    {
        try
        {
            TransitAccountEventPicture t = new TransitAccountEventPicture();
            t.Name = inputName.Text;
            t.Description = inputDescription.Text;
            t.AccountEventId = AccountEventId;
            t.Id = RequestId;
            EventService.CreateOrUpdateAccountEventPicture(SessionManager.Ticket, t);
            Redirect(string.Format("AccountEventPicturesManage.aspx?id={0}", AccountEventId));
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
