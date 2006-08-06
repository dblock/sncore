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

public abstract class AccountPicturePage : PicturePage
{
    public AccountPicturePage()
    {

    }

    public override TransitPicture GetPictureWithBitmap(int id, string ticket, DateTime ifModifiedSince)
    {
        object[] args = { ticket, id, ifModifiedSince };
        TransitAccountPictureWithBitmap p = SessionManager.GetCachedItem<TransitAccountPictureWithBitmap>(
            AccountService, "GetAccountPictureWithBitmapByIdIfModifiedSince", args);

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
        TransitAccountPictureWithThumbnail p = SessionManager.GetCachedItem<TransitAccountPictureWithThumbnail>(
            AccountService, "GetAccountPictureWithThumbnailByIdIfModifiedSince", args);

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
        object[] args = { ticket, id };
        TransitAccountPictureWithBitmap p = SessionManager.GetCachedItem<TransitAccountPictureWithBitmap>(
            AccountService, "GetAccountPictureWithBitmapById", args);

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
        TransitAccountPictureWithThumbnail p = SessionManager.GetCachedItem<TransitAccountPictureWithThumbnail>(
            AccountService, "GetAccountPictureWithThumbnailById", args);

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
        object[] args = { "Account" };
        TransitPictureWithThumbnail tp = SessionManager.GetCachedItem < TransitPictureWithThumbnail>(
            SystemService, "GetRandomPictureWithThumbnailByType", args);

        if (tp == null)
            return null;

        TransitPicture result = new TransitPicture();
        result.Id = tp.Id;
        result.Bitmap = tp.Thumbnail;
        result.Created = tp.Created;
        result.Modified = tp.Modified;
        result.Name = tp.Name;
        return result;
    }
}
