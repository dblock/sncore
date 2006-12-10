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
using SnCore.Tools;

public partial class AccountStoryPicturesManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        this.addFile.Attributes["onclick"] = this.files.GetAddFileScriptReference() + "return false;";
        SetDefaultButton(picturesAdd);
        try
        {
            if (!IsPostBack)
            {
                TransitAccountStory ts = StoryService.GetAccountStoryById(
                    SessionManager.Ticket, RequestId);

                linkAccountStory.Text = Render(ts.Name);
                linkBack.NavigateUrl = linkAccountStory.NavigateUrl = string.Format("AccountStoryEdit.aspx?id={0}", ts.Id);
                linkAccount.Text = Render(ts.AccountName);
                linkAccount.NavigateUrl = string.Format("AccountView.aspx?id={0}", ts.AccountId);

                GetImagesData(sender, e);
            }

            if (!AccountService.HasVerifiedEmail(SessionManager.Ticket))
            {
                ReportWarning("You don't have any verified e-mail addresses.\n" +
                    "You must add/confirm a valid e-mail address before posting stories.");

                picturesAdd.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void GetImagesData(object sender, EventArgs e)
    {
        gridManage.DataSource = StoryService.GetAccountStoryPictures(SessionManager.Ticket, RequestId, null);
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
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    protected void files_FilesPosted(object sender, FilesPostedEventArgs e)
    {
        try
        {
            if (e.PostedFiles.Count == 0)
                return;

            TransitAccountStory s = StoryService.GetAccountStoryById(SessionManager.Ticket, RequestId);

            List<TransitAccountStoryPictureWithPicture> ps =
                new List<TransitAccountStoryPictureWithPicture>(e.PostedFiles.Count);

            ExceptionCollection exceptions = new ExceptionCollection();
            foreach (HttpPostedFile file in e.PostedFiles)
            {
                try
                {
                    TransitAccountStoryPictureWithPicture p =
                        new TransitAccountStoryPictureWithPicture();

                    ThumbnailBitmap t = new ThumbnailBitmap(file.InputStream);
                    p.Picture = t.Bitmap;
                    p.Name = Path.GetFileName(file.FileName);
                    ps.Add(p);
                }
                catch (Exception ex)
                {
                    exceptions.Add(new Exception(string.Format("Error processing {0}: {1}",
                        Renderer.Render(file.FileName), ex.Message), ex));
                }
            }

            StoryService.AddAccountStoryWithPictures(SessionManager.Ticket, s, ps.ToArray());
            GetImagesData(sender, e);
            exceptions.Throw();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
