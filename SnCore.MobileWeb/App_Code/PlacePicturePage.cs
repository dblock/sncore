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
using PlaceService;

public abstract class PlacePicturePage : PicturePage
{
    public PlacePicturePage()
    {

    }

    public override TransitPicture GetPictureWithBitmap(int id, DateTime ifModifiedSince)
    {
        TransitPlacePicture p = SessionManager.GetInstance<TransitPlacePicture, PlaceService.ServiceQueryOptions, int, DateTime>(
            id, ifModifiedSince, SessionManager.PlaceService.GetPlacePictureIfModifiedSinceById);

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
        TransitPlacePicture p = SessionManager.GetInstance<TransitPlacePicture, PlaceService.ServiceQueryOptions, int, DateTime>(
            id, ifModifiedSince, SessionManager.PlaceService.GetPlacePictureIfModifiedSinceById);

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
        TransitPlacePicture p = SessionManager.GetInstance<TransitPlacePicture, PlaceService.ServiceQueryOptions, int>(
            id, SessionManager.PlaceService.GetPlacePictureById);

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
        TransitPlacePicture p = SessionManager.GetInstance<TransitPlacePicture, PlaceService.ServiceQueryOptions, int>(
            id, SessionManager.PlaceService.GetPlacePictureById);

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

    public override PicturePage.TransitPicture GetRandomPictureWithThumbnail()
    {
        ObjectService.TransitPicture p = SessionManager.GetInstance<ObjectService.TransitPicture, ObjectService.ServiceQueryOptions, string>(
            "Place", SessionManager.ObjectService.GetRandomPictureByType);

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
