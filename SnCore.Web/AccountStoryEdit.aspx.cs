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
using SnCore.Services;
using SnCore.WebServices;

public partial class AccountStoryEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        SetDefaultButton(linkSave);

        try
        {
            if (!IsPostBack)
            {
                if (RequestId > 0)
                {
                    TransitAccountStory ts = StoryService.GetAccountStoryById(
                        SessionManager.Ticket, RequestId);

                    inputName.Text = ts.Name;
                    inputSummary.Text = ts.Summary;
                    inputPublish.Checked = ts.Publish;

                    labelName.Text = Render(ts.Name);
                    linkAccount.Text = Render(ts.AccountName);
                    linkAccount.NavigateUrl = string.Format("AccountView.aspx?id={0}", ts.AccountId);

                    linkAddPictures.NavigateUrl = string.Format("AccountStoryPicturesManage.aspx?id={0}", ts.Id);

                    labelTitle.Text = "Edit Story";
                }
                else
                {
                    linkAccount.Text = Render(SessionManager.Account.Name);
                    linkAccount.NavigateUrl = string.Format("AccountView.aspx?id={0}", SessionManager.Account.Id);
                    linkAddPictures.Visible = false;
                }

                GetImagesData(sender, e);
            }

            if (!AccountService.HasVerifiedEmail(SessionManager.Ticket))
            {
                ReportWarning("You don't have any verified e-mail addresses.\n" +
                    "You must add/confirm a valid e-mail address before posting stories.");

                linkSave.Enabled = false;                
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void GetImagesData(object sender, EventArgs e)
    {
        gridManage.DataSource = StoryService.GetAccountStoryPictures(SessionManager.Ticket, RequestId);
        gridManage.DataBind();
    }

    public void gridManage_ItemCommand(object source, DataListCommandEventArgs e)
    {
        try
        {
            int id = int.Parse(e.CommandArgument.ToString());

            switch (e.CommandName)
            {
                case "Delete":
                    StoryService.DeleteAccountStoryPicture(SessionManager.Ticket, id);
                    ReportInfo("Image deleted.");
                    GetImagesData(source, e);
                    break;
                case "Up":
                    StoryService.MoveAccountStoryPicture(SessionManager.Ticket, id, -1);
                    GetImagesData(source, e);
                    break;
                case "Down":
                    StoryService.MoveAccountStoryPicture(SessionManager.Ticket, id, 1);
                    GetImagesData(source, e);
                    break;
                case "Insert":
                    inputSummary.Text = inputSummary.Text +
                            string.Format("<P><IMG SRC=\"AccountStoryPicture.aspx?id={0}\" WIDTH=\"250\" /></P>", id);
                    break;
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
            TransitAccountStory s = new TransitAccountStory();
            s.Name = inputName.Text;
            s.Summary = inputSummary.Text;
            s.Publish = inputPublish.Checked;
            s.Id = RequestId;
            s.Id = StoryService.AddAccountStory(SessionManager.Ticket, s);
            Redirect(string.Format("AccountStoryView.aspx?id={0}", s.Id));
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
