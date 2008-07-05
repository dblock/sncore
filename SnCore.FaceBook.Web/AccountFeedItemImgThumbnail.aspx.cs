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

public partial class AccountFeedItemImgThumbnail : PicturePage
{
    public override PicturePage.PicturePageType PageType
    {
        get
        {
            return PicturePageType.Thumbnail;
        }
    }

    public AccountFeedItemImgThumbnail()
    {

    }

    public override TransitPicture GetPictureWithThumbnail(int id, DateTime ifModifiedSince)
    {
        // these pictures are never modified and ids never reused
        return null;
    }

    public override TransitPicture GetPictureWithBitmap(int id, DateTime ifModifiedSince)
    {
        throw new NotImplementedException();
    }

    public override TransitPicture GetPictureWithBitmap(int id)
    {
        throw new NotImplementedException();
    }

    public override TransitPicture GetPictureWithThumbnail(int id)
    {
        SyndicationService.TransitAccountFeedItemImg p = SessionManager.GetInstance<
            SyndicationService.TransitAccountFeedItemImg, SyndicationService.ServiceQueryOptions, int>(
            id, SessionManager.SyndicationService.GetAccountFeedItemImgById);

        if (p == null)
            return null;

        TransitPicture result = new TransitPicture();
        result.Id = p.Id;
        result.Bitmap = p.Thumbnail;
        result.Created = p.Created;
        result.Modified = p.Modified;
        result.Name = p.Url;
        return result;
    }

    public override TransitPicture GetRandomPictureWithThumbnail()
    {
        throw new NotImplementedException();
    }
}
