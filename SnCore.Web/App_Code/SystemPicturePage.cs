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

public abstract class SystemPicturePage : PicturePage
{
    public SystemPicturePage()
    {

    }

    public override TransitPicture GetPictureWithBitmap(int id, DateTime ifModifiedSince)
    {
        SnCore.Services.TransitPicture p = SessionManager.GetInstance<SnCore.Services.TransitPicture, int, DateTime>(
            id, ifModifiedSince, SessionManager.ObjectService.GetPictureIfModifiedSinceById);

        if (p == null)
            return null;

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
        SnCore.Services.TransitPicture p = SessionManager.GetInstance<SnCore.Services.TransitPicture, int, DateTime>(
            id, ifModifiedSince, SessionManager.ObjectService.GetPictureIfModifiedSinceById);

        if (p == null)
            return null;

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
        SnCore.Services.TransitPicture p = SessionManager.GetInstance<SnCore.Services.TransitPicture, int>(
            id, SessionManager.ObjectService.GetPictureById);

        if (p == null)
            return null;

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
        SnCore.Services.TransitPicture p = SessionManager.GetInstance<SnCore.Services.TransitPicture, int>(
            id, SessionManager.ObjectService.GetPictureById);

        if (p == null)
            return null;

        TransitPicture result = new TransitPicture();
        result.Id = p.Id;
        result.Bitmap = p.Thumbnail;
        result.Created = p.Created;
        result.Modified = p.Modified;
        result.Name = p.Name;
        return result;
    }

    public override TransitPicture GetRandomPictureWithThumbnail()
    {
        SnCore.Services.TransitPicture p = SessionManager.GetInstance<SnCore.Services.TransitPicture, string>(
            "System", SessionManager.ObjectService.GetRandomPictureByType);

        if (p == null)
            return null;

        TransitPicture result = new TransitPicture();
        result.Id = p.Id;
        result.Bitmap = p.Thumbnail;
        result.Created = p.Created;
        result.Modified = p.Modified;
        result.Name = p.Name;
        return result;
    }
}
