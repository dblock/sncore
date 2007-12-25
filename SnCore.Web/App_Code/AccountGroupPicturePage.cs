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

public abstract class AccountGroupPicturePage : PicturePage
{
    public AccountGroupPicturePage()
    {

    }

    public override TransitPicture GetPictureWithBitmap(int id, DateTime ifModifiedSince)
    {
        TransitAccountGroupPicture p = SessionManager.GroupService.GetAccountGroupPictureIfModifiedSinceById(
            SessionManager.Ticket, id, ifModifiedSince);

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
        TransitAccountGroupPicture p = SessionManager.GroupService.GetAccountGroupPictureIfModifiedSinceById(
            SessionManager.Ticket, id, ifModifiedSince);

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
        TransitAccountGroupPicture p = SessionManager.GroupService.GetAccountGroupPictureById(
            SessionManager.Ticket, id);

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
        TransitAccountGroupPicture p = SessionManager.GetInstance<TransitAccountGroupPicture, int>(
            id, SessionManager.GroupService.GetAccountGroupPictureById);

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
            "AccountGroup", SessionManager.ObjectService.GetRandomPictureByType);

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
