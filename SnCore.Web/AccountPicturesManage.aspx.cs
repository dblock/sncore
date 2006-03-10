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

public partial class AccountPicturesManage : AuthenticatedPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SetDefaultButton(save);
        this.addFile.Attributes["onclick"] = this.files.GetAddFileScriptReference() + "return false;";

        try
        {
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

            if (!IsPostBack)
            {
                gridManage_OnGetDataSource(sender, e);
                gridManage.DataBind();

                if (gridManage.Items.Count == 0)
                {
                    ReportInfo("Why not upload a picture?");
                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        gridManage.DataSource = AccountService.GetAccountPictures(SessionManager.Ticket);
    }

    protected void files_FilesPosted(object sender, FilesPostedEventArgs e)
    {
        try
        {
            if (e.PostedFiles.Count == 0)
                return;

            foreach (HttpPostedFile file in e.PostedFiles)
            {
                TransitAccountPictureWithBitmap p = new TransitAccountPictureWithBitmap();

                ThumbnailBitmap t = new ThumbnailBitmap(file.InputStream);
                p.Bitmap = t.Bitmap;
                p.Name = Path.GetFileName(file.FileName);
                p.Description = string.Empty;

                AccountService.AddAccountPicture(SessionManager.Ticket, p);
            }

            gridManage_OnGetDataSource(sender, e);
            gridManage.DataBind();
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

    public void gridManage_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        try
        {
            int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
            switch (e.CommandName)
            {
                case "Delete":
                    AccountService.DeleteAccountPicture(SessionManager.Ticket, id);
                    ReportInfo("Picture deleted.");
                    gridManage.CurrentPageIndex = 0;
                    gridManage_OnGetDataSource(sender, e);
                    gridManage.DataBind();
                    break;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
