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

public abstract class SystemAttributePage : PicturePage
{
    public SystemAttributePage()
    {

    }

    public override TransitPicture GetPictureWithBitmap(int id, string ticket, DateTime ifModifiedSince)
    {
        object[] args = { ticket, id, ifModifiedSince };
        TransitAttribute p = SessionManager.GetCachedItem<TransitAttribute>(
            SessionManager.ObjectService, "GetAttributeIfModifiedSinceById", args);

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
        object[] args = { ticket, id, ifModifiedSince };
        TransitAttribute p = SessionManager.GetCachedItem<TransitAttribute>(
            SessionManager.SystemService, "GetAttributeIfModifiedSinceById", args);

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

    public override TransitPicture GetPictureWithBitmap(int id, string ticket)
    {
        object[] args = { ticket, id };
        TransitAttribute p = SessionManager.GetCachedItem<TransitAttribute>(
            SessionManager.ObjectService, "GetAttributeById", args);

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
        object[] args = { ticket, id };
        TransitAttribute p = SessionManager.GetCachedItem<TransitAttribute>(
            SessionManager.ObjectService, "GetAttributeById", args);

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

    public override TransitPicture GetRandomPictureWithThumbnail()
    {
        return null;
    }
}
