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
using System.Drawing;
using SnCore.Tools.Drawing;
using Wilco.Web.UI.WebControls;
using System.IO;
using SnCore.Services;
using SnCore.WebServices;
using System.Collections.Generic;
using SnCore.Tools;
using SnCore.Tools.Web;
using SnCore.SiteMap;

public partial class AccountPicturesManage : AuthenticatedPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.addFile.Attributes["onclick"] = this.files.GetAddFileScriptReference() + "return false;";

        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            GetData(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Pictures", Request.Url));
            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(save);
    }

    public void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.AccountService.GetAccountPicturesCount(
            SessionManager.Ticket, SessionManager.AccountId, null);
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageSize = gridManage.PageSize;
        options.PageNumber = gridManage.CurrentPageIndex;
        gridManage.DataSource = SessionManager.AccountService.GetAccountPictures(
            SessionManager.Ticket, SessionManager.AccountId, null, options);
    }

    public string GetShowHideButtonText(bool hidden)
    {
        return hidden ? "&#187; Show on Profile" : "&#187; Hide from Profile";
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
                    TransitAccountPicture p = new TransitAccountPicture();

                    ThumbnailBitmap t = new ThumbnailBitmap(file.InputStream);
                    p.Bitmap = t.Bitmap;
                    p.Name = Path.GetFileName(file.FileName);
                    p.Description = string.Empty;
                    p.Hidden = false;

                    SessionManager.CreateOrUpdate<TransitAccountPicture>(
                        p, SessionManager.AccountService.CreateOrUpdateAccountPicture);
                }
                catch (Exception ex)
                {
                    exceptions.Add(new Exception(string.Format("Error processing {0}: {1}",
                        Renderer.Render(file.FileName), ex.Message), ex));
                }
            }

            GetData(sender, e);
            exceptions.Throw();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void gridManage_ItemCommand(object sender, DataListCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Delete":
                {
                    int id = int.Parse(e.CommandArgument.ToString());
                    SessionManager.Delete<TransitAccountPicture>(id, SessionManager.AccountService.DeleteAccountPicture);
                    ReportInfo("Picture deleted.");
                    GetData(sender, e);
                }
                break;
            case "ShowHide":
                {
                    int id = int.Parse(e.CommandArgument.ToString());
                    TransitAccountPicture p = SessionManager.AccountService.GetAccountPictureById(
                        SessionManager.Ticket, id);
                    p.Hidden = !p.Hidden;
                    SessionManager.CreateOrUpdate<TransitAccountPicture>(
                        p, SessionManager.AccountService.CreateOrUpdateAccountPicture);
                    gridManage_OnGetDataSource(sender, e);
                    gridManage.DataBind();
                }
                break;
        }
    }
}
