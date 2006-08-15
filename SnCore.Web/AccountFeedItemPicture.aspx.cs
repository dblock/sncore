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
using SnCore.Services;
using System.Net;

public partial class AccountFeedItemPicture : PicturePage
{
    public override PicturePage.PicturePageType PageType
    {
        get
        {
            return PicturePageType.Bitmap;
        }
    }

    public AccountFeedItemPicture()
    {

    }

    public override TransitPicture GetPictureWithThumbnail(int id, string ticket, DateTime ifModifiedSince)
    {
        throw new NotImplementedException();
    }

    public override TransitPicture GetPictureWithBitmap(int id, string ticket, DateTime ifModifiedSince)
    {
        // these pictures are never modified (external source)
        return null;
    }

    public override TransitPicture GetPictureWithBitmap(int id, string ticket)
    {
        string url = Request["Src"];

        if (string.IsNullOrEmpty(url))
        {
            throw new Exception("Invalid url.");
        }

        // fetch the image to get its size
        WebClient client = new WebClient();
        
        byte[] data = client.DownloadData(url);
        if (data == null)
        {
            throw new Exception("Missing file data.");
        }

        TransitPicture result = new TransitPicture();
        result.Bitmap = data;
        result.Name = url;
        object created = client.ResponseHeaders["Created"];
        object modified = client.ResponseHeaders["Modified"];
        result.Created = (created != null) ? DateTime.Parse(created.ToString()) : DateTime.Now;
        result.Modified = (modified != null) ? DateTime.Parse(modified.ToString()) : DateTime.Now;
        return result;
    }

    public override TransitPicture GetPictureWithThumbnail(int id, string ticket)
    {
        throw new NotImplementedException();
    }

    public override TransitPicture GetRandomPictureWithThumbnail()
    {
        throw new NotImplementedException();
    }
}
