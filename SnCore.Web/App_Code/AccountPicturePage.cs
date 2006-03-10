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
        TransitAccountPictureWithBitmap p =
            AccountService.GetAccountPictureWithBitmapByIdIfModifiedSince(
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
        TransitAccountPictureWithThumbnail p =
            AccountService.GetAccountPictureWithThumbnailByIdIfModifiedSince(
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
        TransitAccountPictureWithBitmap p =
            AccountService.GetAccountPictureWithBitmapById(
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
        TransitAccountPictureWithThumbnail p =
            AccountService.GetAccountPictureWithThumbnailById(
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
        TransitPictureWithThumbnail tp = SystemService.GetRandomPictureWithThumbnailByType("Account");

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
