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
using System.Drawing;
using SnCore.Tools.Drawing;
using SnCore.Tools.Web;
using System.IO;
using SnCore.Services;
using SnCore.WebServices;
using SnCore.Tools;
using SnCore.SiteMap;

public partial class AccountEventPicturesManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        this.addFile.Attributes["onclick"] = this.files.GetAddFileScriptReference() + "return false;";
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            TransitAccountEvent p = SessionManager.EventService.GetAccountEventById(
                SessionManager.Ticket, RequestId, SessionManager.UtcOffset.Ticks);

            GetData(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Events", Request, "AccountEventsManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(p.Name, Request, string.Format("AccountEventView.aspx?id={0}", p.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Pictures", Request.Url));
            StackSiteMap(sitemapdata);
        }

        if (!SessionManager.HasVerified())
        {
            ReportWarning("You don't have any verified e-mail addresses and/or profile photos.\n" +
                "You must add/confirm a valid e-mail address and upload a profile photo before uploading event pictures.");

            addFile.Enabled = false;
        }
    }

    public void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.EventService.GetAccountEventPicturesCount(
            SessionManager.Ticket, RequestId);
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageSize = gridManage.PageSize;
        options.PageNumber = gridManage.CurrentPageIndex;
        gridManage.DataSource = SessionManager.EventService.GetAccountEventPictures(
            SessionManager.Ticket, RequestId, options);
    }

    private enum Cells
    {
        id = 0
    };

    public void gridManage_ItemCommand(object sender, DataListCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Delete":
                {
                    int id = int.Parse(e.CommandArgument.ToString());
                    SessionManager.Delete<TransitAccountEventPicture>(id, SessionManager.EventService.DeleteAccountEventPicture);
                    ReportInfo("Picture deleted.");
                    GetData(sender, e);
                }
                break;
            case "Right":
                {
                    int id = int.Parse(e.CommandArgument.ToString());
                    SessionManager.EventService.MoveAccountEventPicture(SessionManager.Ticket, id, 1);
                    if (e.Item.ItemIndex + 1 == gridManage.Items.Count && gridManage.CurrentPageIndex + 1 < gridManage.PagedDataSource.PageCount)
                        gridManage.CurrentPageIndex++;
                    SessionManager.InvalidateCache<TransitAccountEventPicture>();
                    gridManage_OnGetDataSource(sender, e);
                    gridManage.DataBind();
                }
                break;
            case "Left":
                {
                    int id = int.Parse(e.CommandArgument.ToString());
                    SessionManager.EventService.MoveAccountEventPicture(SessionManager.Ticket, id, -1);
                    if (e.Item.ItemIndex == 0 && gridManage.CurrentPageIndex > 0) gridManage.CurrentPageIndex--;
                    SessionManager.InvalidateCache<TransitAccountEventPicture>();
                    gridManage_OnGetDataSource(sender, e);
                    gridManage.DataBind();
                }
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
                    TransitAccountEventPicture p = new TransitAccountEventPicture();
                    ThumbnailBitmap t = new ThumbnailBitmap(file.InputStream);
                    p.Picture = t.Bitmap;
                    p.Name = Path.GetFileName(file.FileName);
                    p.Description = string.Empty;
                    p.AccountEventId = RequestId;
                    SessionManager.CreateOrUpdate<TransitAccountEventPicture>(
                        p, SessionManager.EventService.CreateOrUpdateAccountEventPicture);
                }
                catch (Exception ex)
                {
                    exceptions.Add(new Exception(string.Format("Error processing {0}: {1}",
                        Renderer.Render(file.FileName), ex.Message), ex));
                }
            }

            gridManage.CurrentPageIndex = 0;
            gridManage_OnGetDataSource(sender, e);
            gridManage.DataBind();
            exceptions.Throw();

            Redirect(string.Format("AccountEventView.aspx?id={0}", RequestId));
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

}
