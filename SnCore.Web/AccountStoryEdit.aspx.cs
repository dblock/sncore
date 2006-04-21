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
    private List<TransitAccountStoryPicture> mps;

    public void Page_Load(object sender, EventArgs e)
    {
        this.addFile.Attributes["onclick"] = this.files.GetAddFileScriptReference() + "return false;";
        SetDefaultButton(linkSave);
        try
        {
            if (!IsPostBack)
            {
                GetData();
            }

            if (!AccountService.HasVerifiedEmail(SessionManager.Ticket))
            {
                ReportWarning("You don't have any verified e-mail addresses.\n" +
                    "You must add/confirm a valid e-mail address before posting stories.");

                picturesAdd.Enabled = false;
                linkSave.Enabled = false;                
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void GetData()
    {
        if (RequestId > 0)
        {
            TransitAccountStory ts = StoryService.GetAccountStoryById(
                SessionManager.Ticket, RequestId);

            inputName.Text = ts.Name;
            inputSummary.Text = ts.Summary;

            linkPreview.NavigateUrl = string.Format("AccountStoryView.aspx?id={0}", RequestId);

            mps = StoryService.GetAccountStoryPictures(SessionManager.Ticket, RequestId);
            gridManage.DataSource = mps;
            gridManage.DataBind();
        }
        else
        {
            panelImages.Visible = false;
            linkPreview.Visible = false;
        }
    }

    private enum Cells
    {
        id = 0,
        image,
        text,
        up,
        down,
        delete
    };

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
                    GetData();
                    break;
                case "Up":
                    StoryService.MoveAccountStoryPicture(SessionManager.Ticket, id, -1);
                    GetData();
                    break;
                case "Down":
                    StoryService.MoveAccountStoryPicture(SessionManager.Ticket, id, 1);
                    GetData();
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
            s.Id = RequestId;

            int id = StoryService.AddAccountStory(SessionManager.Ticket, s);

            //List<TransitAccountStoryPicture> ps =
            //    new List<TransitAccountStoryPicture>(gridManage.Items.Count);

            //foreach (DataGridItem item in gridManage.Items)
            //{
            //    TransitAccountStoryPicture p = new TransitAccountStoryPicture();
            //    p.Id = int.Parse(item.Cells[(int)Cells.id].Text);
            //    p.Title = ((TextBox)item.Cells[(int)Cells.text].Controls[1]).Text;
            //    p.Text = ((TextBox)item.Cells[(int)Cells.text].Controls[3]).Text;
            //    ps.Add(p);
            //}

            //StoryService.AddAccountStoryWithPictures(SessionManager.Ticket, s, ps.ToArray());
            Redirect(RequestId > 0 ? "AccountStoriesManage.aspx" : string.Format("AccountStoryEdit.aspx?id={0}", id));
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

            foreach (HttpPostedFile file in e.PostedFiles)
            {
                TransitAccountStoryPictureWithPicture p =
                    new TransitAccountStoryPictureWithPicture();

                ThumbnailBitmap t = new ThumbnailBitmap(file.InputStream);
                p.Picture = t.Bitmap;
                p.Name = Path.GetFileName(file.FileName);
                ps.Add(p);
            }

            StoryService.AddAccountStoryWithPictures(SessionManager.Ticket, s, ps.ToArray());
            GetData();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
