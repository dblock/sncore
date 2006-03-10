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

public partial class SystemPicturesManage : AuthenticatedPage
{
    public void Page_Load()
    {
        try
        {
            this.addFile.Attributes["onclick"] = this.files.GetAddFileScriptReference() + "return false;";
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

            if (!IsPostBack)
            {

                gridManage_OnGetDataSource(this, null);
                gridManage.DataBind();

                selectPictureType.DataSource = SystemService.GetPictureTypes();
                selectPictureType.DataBind();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            gridManage.DataSource = SystemService.GetPictures();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
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
        try
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
                            SystemService.DeletePicture(SessionManager.Ticket, id);
                            ReportInfo("Picture deleted.");
                            gridManage.CurrentPageIndex = 0;
                            gridManage_OnGetDataSource(source, e);
                            gridManage.DataBind();
                            break;
                    }
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

            foreach (HttpPostedFile file in e.PostedFiles)
            {
                TransitPictureWithBitmap p = new TransitPictureWithBitmap();
                ThumbnailBitmap t = new ThumbnailBitmap(file.InputStream);
                p.Bitmap = t.Bitmap;
                p.Name = Path.GetFileName(file.FileName);
                p.Type = selectPictureType.SelectedValue;
                p.Description = string.Empty;

                SystemService.CreateOrUpdatePicture(SessionManager.Ticket, p);
            }

            gridManage.CurrentPageIndex = 0;
            gridManage_OnGetDataSource(sender, e);
            gridManage.DataBind();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

}
