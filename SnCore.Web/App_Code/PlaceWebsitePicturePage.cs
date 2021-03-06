using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SnCore.WebServices;
using SnCore.BackEndServices;
using SnCore.Services;
using SnCore.Tools.Drawing;

public abstract class PlaceWebsitePicturePage : PicturePage
{
    public PlaceWebsitePicturePage()
    {

    }

    public override TransitPicture GetPictureWithBitmap(int id, DateTime ifModifiedSince)
    {
        TransitPlaceWebsite p = SessionManager.GetInstance<TransitPlaceWebsite, int, DateTime>(
            id, ifModifiedSince, SessionManager.PlaceService.GetPlaceWebsiteIfModifiedSinceById);

        if (p == null || p.Bitmap == null)
        {
            return GetRandomPictureWithThumbnail();
        }

        TransitPicture result = new TransitPicture();
        result.Id = p.Id;
        result.Bitmap = p.Bitmap;
        result.Created = p.Created;
        result.Modified = p.Modified;
        result.Name = p.Name;
        return result;
    }

    public override TransitPicture GetPictureWithThumbnail(int id, DateTime ifModifiedSince)
    {
        TransitPlaceWebsite p = SessionManager.GetInstance<TransitPlaceWebsite, int, DateTime>(
            id, ifModifiedSince, SessionManager.PlaceService.GetPlaceWebsiteIfModifiedSinceById);

        if (p == null || p.Bitmap == null)
        {
            return GetRandomPictureWithThumbnail();
        }

        TransitPicture result = new TransitPicture();
        result.Id = p.Id;
        result.Bitmap = p.Thumbnail;
        result.Created = p.Created;
        result.Modified = p.Modified;
        result.Name = p.Name;
        return result;
    }

    public override TransitPicture GetPictureWithBitmap(int id)
    {
        TransitPlaceWebsite p = SessionManager.GetInstance<TransitPlaceWebsite, int>(
            id, SessionManager.PlaceService.GetPlaceWebsiteById);

        if (p == null || p.Bitmap == null)
        {
            return GetRandomPictureWithThumbnail();
        }

        TransitPicture result = new TransitPicture();
        result.Id = p.Id;
        result.Bitmap = p.Bitmap;
        result.Created = p.Created;
        result.Modified = p.Modified;
        result.Name = p.Name;
        return result;
    }

    public override TransitPicture GetPictureWithThumbnail(int id)
    {
        TransitPlaceWebsite p = SessionManager.GetInstance<TransitPlaceWebsite, int>(
            id, SessionManager.PlaceService.GetPlaceWebsiteById);

        if (p == null || p.Bitmap == null)
        {
            return GetRandomPictureWithThumbnail();
        }

        TransitPicture result = new TransitPicture();
        result.Id = p.Id;
        result.Bitmap = p.Thumbnail;
        result.Created = p.Created;
        result.Modified = p.Modified;
        result.Name = p.Name;
        return result;
    }

    public override PicturePage.TransitPicture GetRandomPictureWithThumbnail()
    {
        SnCore.Services.TransitPicture p = SessionManager.GetInstance<SnCore.Services.TransitPicture, string>(
            "PlaceWebsite", SessionManager.ObjectService.GetRandomPictureByType);

        if (p == null || p.Bitmap == null)
        {
            p = new SnCore.Services.TransitPicture();
            const string s_Unavailable = "Thumbnail\nQueued";
            // flip the bitmaps horizontally
            p.Bitmap = ThumbnailBitmap.GetBitmapDataFromText(s_Unavailable, 10, ThumbnailBitmap.s_FullSize.Height, ThumbnailBitmap.s_FullSize.Width);
            p.Thumbnail = ThumbnailBitmap.GetBitmapDataFromText(s_Unavailable, 10, ThumbnailBitmap.s_ThumbnailSize.Height, ThumbnailBitmap.s_ThumbnailSize.Width);
            p.Created = p.Modified = DateTime.UtcNow;
        }

        TransitPicture result = new TransitPicture();
        result.Id = p.Id;
        result.Bitmap = p.Thumbnail;
        result.Created = p.Created;
        result.Modified = p.Modified;
        result.Name = p.Name;
        return result;
    }
}
