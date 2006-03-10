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

public partial class PlacePicturesManage : AuthenticatedPage
{
    public void Page_Load()
    {
        try
        {
            this.addFile.Attributes["onclick"] = this.files.GetAddFileScriptReference() + "return false;";
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

            if (!IsPostBack)
            {
                TransitPlace place = PlaceService.GetPlaceById(RequestId);
                linkPlaceName.Text = Renderer.Render(place.Name);
                linkPlaceName.NavigateUrl = string.Format("PlaceView.aspx?id={0}", place.Id);

                gridManage_OnGetDataSource(this, null);
                gridManage.DataBind();
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
            gridManage.DataSource = PlaceService.GetPlacePictures(RequestId);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    private enum Cells
    {
        id = 0
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
                            PlaceService.DeletePlacePicture(SessionManager.Ticket, id);
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
                TransitPlacePictureWithBitmap p = new TransitPlacePictureWithBitmap();
                ThumbnailBitmap t = new ThumbnailBitmap(file.InputStream);
                p.Bitmap = t.Bitmap;
                p.Name = Path.GetFileName(file.FileName);
                p.Description = string.Empty;
                p.PlaceId = RequestId;
                PlaceService.CreateOrUpdatePlacePicture(SessionManager.Ticket, p);
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
