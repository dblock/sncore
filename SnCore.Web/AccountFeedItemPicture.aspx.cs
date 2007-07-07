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
using SnCore.Tools.Drawing;
using SnCore.Services;
using System.Net;
using System.Web.Caching;
using System.Drawing;

public partial class AccountFeedItemPicture : PicturePage
{
    public override PicturePage.PicturePageType PageType
    {
        get
        {
            return PicturePageType.Bitmap;
        }
    }

    public AccountFeedItemPicture()
    {

    }

    public override TransitPicture GetPictureWithThumbnail(int id, DateTime ifModifiedSince)
    {
        throw new NotImplementedException();
    }

    public override TransitPicture GetPictureWithBitmap(int id, DateTime ifModifiedSince)
    {
        // these pictures are never modified (external source)
        return null;
    }

    public override TransitPicture GetPictureWithBitmap(int id)
    {
        string url = Request["Src"];

        if (string.IsNullOrEmpty(url))
        {
            throw new Exception("Invalid url.");
        }

        string key = string.Format("AccountFeedItemPicture:{0}", url.GetHashCode());
        TransitPicture result = (TransitPicture) Cache[key];

        if (result != null)
            return result;

        // fetch the image to get its size
        WebClient client = new WebClient();
        result = new TransitPicture();
        result.Name = url;

        try
        {
            byte[] data = client.DownloadData(url);

            if (data == null)
            {
                throw new Exception("Missing file data.");
            }

            result.Bitmap = new ThumbnailBitmap(new MemoryStream(data), new Size(0, 0), 
                ThumbnailBitmap.s_FullSize, ThumbnailBitmap.s_ThumbnailSize).Bitmap;

            string created = (string)client.ResponseHeaders["Created"];
            if (string.IsNullOrEmpty(created)) created = (string)client.ResponseHeaders["Date"];
            if (string.IsNullOrEmpty(created) || !DateTime.TryParse(created, out result.Created))
                result.Created = DateTime.Now;

            string modified = (string)client.ResponseHeaders["Modified"];
            if (string.IsNullOrEmpty(modified) || !DateTime.TryParse(modified, out result.Modified))
                result.Modified = DateTime.Now;
        }
        catch(Exception ex)
        {
            string message = string.Format("This image cannot be displayed.\n{0}\n{1}",
                ex.Message, url);
            result.Bitmap = ThumbnailBitmap.GetBitmapDataFromText(message, 8, 0, 0);
            result.Modified = result.Created = DateTime.Now;
        }

        Cache.Insert(key, result, null, Cache.NoAbsoluteExpiration,
            SessionManager.DefaultCacheTimeSpan);

        return result;
    }

    public override TransitPicture GetPictureWithThumbnail(int id)
    {
        throw new NotImplementedException();
    }

    public override TransitPicture GetRandomPictureWithThumbnail()
    {
        throw new NotImplementedException();
    }
}
