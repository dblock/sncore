using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;

public partial class SystemBookmark : SystemBookmarkPage
{
    public override PicturePage.PicturePageType PageType
    {
        get
        {
            object o_thumbnail = Request["ShowThumbnail"];  
            if (o_thumbnail == null) return PicturePageType.Bitmap;
            return bool.Parse(o_thumbnail.ToString()) ? PicturePageType.Thumbnail : PicturePageType.Bitmap;
        }
    }
}
