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
using SnCore.SiteMap;

public partial class AccountStoryPicturesManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        this.addFile.Attributes["onclick"] = this.files.GetAddFileScriptReference() + "return false;";
        if (!IsPostBack)
        {
            TransitAccountStory ts = SessionManager.StoryService.GetAccountStoryById(
                SessionManager.Ticket, RequestId);

            linkBack.NavigateUrl = string.Format("AccountStoryEdit.aspx?id={0}", ts.Id);

            GetImagesData(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Stories", Request, "AccountStoriesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(ts.Name, Request, string.Format("AccountStoryEdit.aspx?id={0}", ts.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Pictures", Request.Url));
            StackSiteMap(sitemapdata);
        }

        if (!SessionManager.HasVerifiedEmailAddress())
        {
            ReportWarning("You don't have any verified e-mail addresses.\n" +
                "You must add/confirm a valid e-mail address before posting stories.");

            picturesAdd.Enabled = false;
        }

        SetDefaultButton(picturesAdd);
    }

    public void GetImagesData(object sender, EventArgs e)
    {
        gridManage.DataSource = SessionManager.StoryService.GetAccountStoryPictures(
            SessionManager.Ticket, RequestId, null);
        gridManage.DataBind();
    }

    public void gridManage_ItemCommand(object source, DataListCommandEventArgs e)
    {
        int id = int.Parse(e.CommandArgument.ToString());

        switch (e.CommandName)
        {
            case "Delete":
                SessionManager.Delete<TransitAccountStoryPicture>(id, SessionManager.StoryService.DeleteAccountStoryPicture);
                ReportInfo("Image deleted.");
                GetImagesData(source, e);
                break;
            case "Up":
                SessionManager.StoryService.MoveAccountStoryPicture(SessionManager.Ticket, id, -1);
                GetImagesData(source, e);
                break;
            case "Down":
                SessionManager.StoryService.MoveAccountStoryPicture(SessionManager.Ticket, id, 1);
                GetImagesData(source, e);
                break;
        }
    }

    protected void files_FilesPosted(object sender, FilesPostedEventArgs e)
    {
        try
        {
            if (e.PostedFiles.Count == 0)
                return;

            ExceptionCollection exceptions = new ExceptionCollection();
            foreach (HttpPostedFile file in e.PostedFiles)
            {
                try
                {
                    TransitAccountStoryPicture p = new TransitAccountStoryPicture();
                    ThumbnailBitmap t = new ThumbnailBitmap(file.InputStream);
                    p.Picture = t.Bitmap;
                    p.Name = Path.GetFileName(file.FileName);
                    p.AccountStoryId = RequestId;
                    SessionManager.CreateOrUpdate<TransitAccountStoryPicture>(
                        p, SessionManager.StoryService.CreateOrUpdateAccountStoryPicture);
                }
                catch (Exception ex)
                {
                    exceptions.Add(new Exception(string.Format("Error processing {0}: {1}",
                        Renderer.Render(file.FileName), ex.Message), ex));
                }
            }


            GetImagesData(sender, e);
            exceptions.Throw();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
