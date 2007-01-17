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

public abstract class SystemBookmarkPage : PicturePage
{
    public SystemBookmarkPage()
    {

    }

    public override TransitPicture GetPictureWithBitmap(int id, DateTime ifModifiedSince)
    {
        TransitBookmark p = SessionManager.GetInstance<TransitBookmark, int, DateTime>(
            id, ifModifiedSince, SessionManager.ObjectService.GetBookmarkIfModifiedSinceById);

        if (p == null)
            return null;

        TransitPicture result = new TransitPicture();
        result.Id = p.Id;
        result.Bitmap = p.FullBitmap;
        result.Created = p.Created;
        result.Modified = p.Modified;
        result.Name = p.Name;
        return result;
    }

    public override TransitPicture GetPictureWithThumbnail(int id, DateTime ifModifiedSince)
    {
        TransitBookmark p = SessionManager.GetInstance<TransitBookmark, int, DateTime>(
            id, ifModifiedSince, SessionManager.ObjectService.GetBookmarkIfModifiedSinceById);

        if (p == null)
            return null;

        TransitPicture result = new TransitPicture();
        result.Id = p.Id;
        result.Bitmap = p.LinkBitmap;
        result.Created = p.Created;
        result.Modified = p.Modified;
        result.Name = p.Name;
        return result;
    }

    public override TransitPicture GetPictureWithBitmap(int id)
    {
        TransitBookmark p = SessionManager.GetInstance<TransitBookmark, int>(
            id, SessionManager.ObjectService.GetBookmarkById);

        if (p == null)
            return null;

        TransitPicture result = new TransitPicture();
        result.Id = p.Id;
        result.Bitmap = p.FullBitmap;
        result.Created = p.Created;
        result.Modified = p.Modified;
        result.Name = p.Name;
        return result;
    }

    public override TransitPicture GetPictureWithThumbnail(int id)
    {
        TransitBookmark p = SessionManager.GetInstance<TransitBookmark, int>(
            id, SessionManager.ObjectService.GetBookmarkById);

        if (p == null)
            return null;

        TransitPicture result = new TransitPicture();
        result.Id = p.Id;
        result.Bitmap = p.LinkBitmap;
        result.Created = p.Created;
        result.Modified = p.Modified;
        result.Name = p.Name;
        return result;
    }

    public override TransitPicture GetRandomPictureWithThumbnail()
    {
        return null;
    }
}
