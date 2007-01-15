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

    public override TransitPicture GetPictureWithBitmap(int id, string ticket, DateTime ifModifiedSince)
    {
        SnCore.Services.TransitPicture p =
            SessionManager.ObjectService.GetPictureIfModifiedSinceById(
                ticket,
                id,
                ifModifiedSince);

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

    public override TransitPicture GetPictureWithThumbnail(int id, string ticket, DateTime ifModifiedSince)
    {
        SnCore.Services.TransitPicture p =
            SessionManager.ObjectService.GetPictureIfModifiedSinceById(
                ticket,
                id,
                ifModifiedSince);

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

    public override TransitPicture GetPictureWithBitmap(int id, string ticket)
    {
        SnCore.Services.TransitPicture p =
            SessionManager.ObjectService.GetPictureById(
                ticket,
                id);

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

    public override TransitPicture GetPictureWithThumbnail(int id, string ticket)
    {
        SnCore.Services.TransitPicture p =
            SessionManager.ObjectService.GetPictureById(
                ticket,
                id);

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
        return null;
    }
}
