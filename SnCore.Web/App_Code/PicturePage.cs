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

public abstract class PicturePage : Page
{
    public enum PicturePageType
    {
        Thumbnail,
        Bitmap
    };

    public class TransitPicture
    {
        public int Id;
        public DateTime Created;
        public DateTime Modified;
        public string Name;
        public Byte[] Bitmap;

        public TransitPicture()
        {

        }
    }

    public abstract TransitPicture GetPictureWithBitmap(int id, string ticket, DateTime ifModifiedSince);
    public abstract TransitPicture GetPictureWithThumbnail(int id, string ticket, DateTime ifModifiedSince);
    public abstract TransitPicture GetPictureWithBitmap(int id, string ticket);
    public abstract TransitPicture GetPictureWithThumbnail(int id, string ticket);
    public abstract TransitPicture GetRandomPictureWithThumbnail();

    public abstract PicturePageType PageType { get; }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HttpCookie ticketCookie = Request.Cookies["SnCoreAuth"];
            object ifmodifiedsince = Request.Headers["If-Modified-Since"];

            if (ifmodifiedsince != null && CacheDuration > 0)
            {
                DateTime ifmodifiedsincedt;
                if (DateTime.TryParse(ifmodifiedsince.ToString(), out ifmodifiedsincedt))
                {
                    if (ifmodifiedsincedt.ToUniversalTime().AddSeconds(CacheDuration) > DateTime.UtcNow)
                    {
                        Response.StatusCode = 304;
                        return;
                    }
                }
            }

            string ticket = (ticketCookie != null) ? ticketCookie.Value : null;

            TransitPicture p = null;

            switch (PageType)
            {
                case PicturePageType.Thumbnail:
                    
                    if (RequestId == 0)
                    {
                        p = GetRandomPictureWithThumbnail();

                        if (p == null)
                        {
                            p = new TransitPicture();
                            p.Id = 0;
                            p.Created = p.Modified = DateTime.Now;
                            p.Name = Guid.NewGuid().ToString();
                            p.Bitmap = ThumbnailBitmap.GetBitmapDataFromText("?", 72, 100, 150);
                        }
                    }
                    else
                    {
                        p = (ifmodifiedsince != null) ?
                            GetPictureWithThumbnail(RequestId, ticket, DateTime.Parse(ifmodifiedsince.ToString())) :
                            GetPictureWithThumbnail(RequestId, ticket);
                    }

                    break;
                case PicturePageType.Bitmap:
                    
                    p = (ifmodifiedsince != null) ?
                        GetPictureWithBitmap(RequestId, ticket, DateTime.Parse(ifmodifiedsince.ToString())) :
                        GetPictureWithBitmap(RequestId, ticket);

                    break;
            }

            if (p == null)
            {
                if (ifmodifiedsince != null)
                {
                    Response.StatusCode = 304;
                    return;
                }
                else
                {
                    throw new Exception("Invalid picture id.");
                }
            }

            Response.Cache.SetLastModified(p.Modified.ToLocalTime());
            Response.Cache.SetCacheability(HttpCacheability.Private);

            p.Name = (string.IsNullOrEmpty(p.Name)) ? p.Id.ToString() + ".jpg" : p.Id.ToString() + "-" + p.Name;

            switch (PageType)
            {
                case PicturePageType.Thumbnail:
                    p.Name.Insert(0, "thumbnail-");
                    break;
            }

            Response.ContentType = "image/" + Path.GetExtension(p.Name).TrimStart(".".ToCharArray());
            Response.AddHeader("Content-disposition", "attachment; filename=" + p.Name);
            Response.AddHeader("Created", p.Created.ToString("r"));
            Response.AddHeader("Modified", p.Modified.ToString("r"));
            Response.BinaryWrite(p.Bitmap);
            Response.End();
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    public int CacheDuration
    {
        get
        {
            object duration = Request["CacheDuration"];
            if (duration == null) return 60;
            return int.Parse(duration.ToString());
        }
    }
}
