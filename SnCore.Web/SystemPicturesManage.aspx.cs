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
using System.IO;
using SnCore.Services;
using SnCore.WebServices;
using SnCore.Tools;
using SnCore.Tools.Web;
using SnCore.SiteMap;

public partial class SystemPicturesManage : AuthenticatedPage
{
    public void Page_Load()
    {
        this.addFile.Attributes["onclick"] = this.files.GetAddFileScriptReference() + "return false;";
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {

            gridManage_OnGetDataSource(this, null);
            gridManage.DataBind();

            selectPictureType.DataSource = SessionManager.SystemService.GetPictureTypes();
            selectPictureType.DataBind();

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Pictures", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        gridManage.DataSource = SessionManager.SystemService.GetPictures();
    }

    private enum Cells
    {
        id = 0,
        image,
        name,
        edit,
        delete
    };

    public void gridManage_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case ListItemType.AlternatingItem:
            case ListItemType.Item:
            case ListItemType.SelectedItem:
            case ListItemType.EditItem:
                int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                switch (e.CommandName)
                {
                    case "Delete":
                        SessionManager.SystemService.DeletePicture(SessionManager.Ticket, id);
                        ReportInfo("Picture deleted.");
                        gridManage.CurrentPageIndex = 0;
                        gridManage_OnGetDataSource(source, e);
                        gridManage.DataBind();
                        break;
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
                    TransitPictureWithBitmap p = new TransitPictureWithBitmap();
                    ThumbnailBitmap t = new ThumbnailBitmap(file.InputStream);
                    p.Bitmap = t.Bitmap;
                    p.Name = Path.GetFileName(file.FileName);
                    p.Type = selectPictureType.SelectedValue;
                    p.Description = string.Empty;

                    SessionManager.SystemService.CreateOrUpdatePicture(SessionManager.Ticket, p);
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
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

}
