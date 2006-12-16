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
using System.Drawing;
using SnCore.Tools.Drawing;
using System.IO;
using SnCore.SiteMap;

public partial class SystemBookmarkEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode("Bookmarks", Request, "SystemBookmarksManage.aspx"));

                if (RequestId > 0)
                {
                    TransitBookmark t = SessionManager.SystemService.GetBookmarkById(RequestId);
                    inputName.Text = t.Name;
                    inputDescription.Text = t.Description;
                    inputUrl.Text = t.Url;
                    imageFullBitmap.ImageUrl = string.Format("SystemBookmark.aspx?id={0}&CacheDuration=0", t.Id);
                    imageFullBitmap.Visible = t.HasFullBitmap;
                    imageLinkBitmap.ImageUrl = string.Format("SystemBookmark.aspx?id={0}&CacheDuration=0&ShowThumbnail=true", t.Id);
                    imageLinkBitmap.Visible = t.HasLinkBitmap;
                    sitemapdata.Add(new SiteMapDataAttributeNode(t.Name, Request.Url));
                }
                else
                {
                    imageFullBitmap.Visible = false;
                    imageLinkBitmap.Visible = false;
                    sitemapdata.Add(new SiteMapDataAttributeNode("New Bookmark", Request.Url));
                }
                StackSiteMap(sitemapdata);
            }

            SetDefaultButton(manageAdd);
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
            if (inputFullBitmap.HasFile) t.FullBitmap = new ThumbnailBitmap(inputFullBitmap.FileContent, new Size(16, 16)).Bitmap;
            if (inputLinkBitmap.HasFile) t.LinkBitmap = new ThumbnailBitmap(inputLinkBitmap.FileContent, new Size(16, 16)).Bitmap;
            SessionManager.SystemService.CreateOrUpdateBookmark(SessionManager.Ticket, t);
            Redirect("SystemBookmarksManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
