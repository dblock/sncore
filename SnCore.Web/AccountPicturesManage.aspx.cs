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
                GetData(sender, e);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = AccountService.GetAccountPicturesCount(SessionManager.Ticket, null);
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageSize = gridManage.PageSize;
        options.PageNumber = gridManage.CurrentPageIndex;
        gridManage.DataSource = AccountService.GetAccountPictures(SessionManager.Ticket, null, options);
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

            foreach (HttpPostedFile file in e.PostedFiles)
            {
                TransitAccountPictureWithBitmap p = new TransitAccountPictureWithBitmap();

                ThumbnailBitmap t = new ThumbnailBitmap(file.InputStream);
                p.Bitmap = t.Bitmap;
                p.Name = Path.GetFileName(file.FileName);
                p.Description = string.Empty;
                p.Hidden = false;

                AccountService.AddAccountPicture(SessionManager.Ticket, p);
            }

            GetData(sender, e);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void gridManage_ItemCommand(object sender, DataListCommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "Delete":
                    {
                        int id = int.Parse(e.CommandArgument.ToString());
                        AccountService.DeleteAccountPicture(SessionManager.Ticket, id);
                        ReportInfo("Picture deleted.");
                        GetData(sender, e);
                    }
                    break;
                case "ShowHide":
                    {
                        int id = int.Parse(e.CommandArgument.ToString());
                        TransitAccountPictureWithBitmap p = AccountService.GetAccountPictureWithBitmapById(
                            SessionManager.Ticket, id);
                        p.Hidden = !p.Hidden;
                        AccountService.AddAccountPicture(SessionManager.Ticket, p);
                        gridManage_OnGetDataSource(sender, e);
                        gridManage.DataBind();
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
