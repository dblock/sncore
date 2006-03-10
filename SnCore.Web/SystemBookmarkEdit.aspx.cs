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
using SnCore.Tools.Web;
using SnCore.Services;
using SnCore.WebServices;
using SnCore.Tools.Drawing;
using System.IO;

public partial class SystemBookmarkEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(manageAdd);
            if (!IsPostBack)
            {
                if (RequestId > 0)
                {
                    TransitBookmark t = SystemService.GetBookmarkById(RequestId);
                    inputName.Text = t.Name;
                    inputDescription.Text = t.Description;
                    inputUrl.Text = t.Url;
                    imageFullBitmap.ImageUrl = string.Format("SystemBookmark.aspx?id={0}&CacheDuration=0", t.Id);
                    imageLinkBitmap.ImageUrl = string.Format("SystemBookmark.aspx?id={0}&CacheDuration=0&ShowThumbnail=true", t.Id);
                }
                else
                {
                    imageFullBitmap.Visible = false;
                    imageLinkBitmap.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void save_Click(object sender, EventArgs e)
    {
        try
        {
            TransitBookmarkWithBitmaps t = new TransitBookmarkWithBitmaps();
            t.Name = inputName.Text;
            t.Description = inputDescription.Text;
            t.Url = inputUrl.Text;
            t.Id = RequestId;
            if (inputFullBitmap.HasFile) t.FullBitmap = new ThumbnailBitmap(inputFullBitmap.FileContent).Bitmap;
            if (inputLinkBitmap.HasFile) t.LinkBitmap = new ThumbnailBitmap(inputLinkBitmap.FileContent).Bitmap;
            SystemService.CreateOrUpdateBookmark(SessionManager.Ticket, t);
            Redirect("SystemBookmarksManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
