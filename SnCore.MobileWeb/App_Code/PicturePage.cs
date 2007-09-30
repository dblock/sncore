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
using System.Drawing;

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

    public abstract TransitPicture GetPictureWithBitmap(int id, DateTime ifModifiedSince);
    public abstract TransitPicture GetPictureWithThumbnail(int id, DateTime ifModifiedSince);
    public abstract TransitPicture GetPictureWithBitmap(int id);
    public abstract TransitPicture GetPictureWithThumbnail(int id);
    public abstract TransitPicture GetRandomPictureWithThumbnail();

    public abstract PicturePageType PageType { get; }

    public Nullable<DateTime> IfModifiedSince
    {
        get
        {
            Nullable<DateTime> result = new Nullable<DateTime>();
            object o = Request.Headers["If-Modified-Since"];
            if (o == null) return result;
            string s = o.ToString().Split(';')[0];
            DateTime dt;
            if (DateTime.TryParse(s, out dt)) result = dt;
            return result;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Nullable<Size> resize = Size;
            Nullable<DateTime> ims = IfModifiedSince;

            if (ims.HasValue)
            {
                if (ims.Value.ToUniversalTime().AddSeconds(CacheDuration) > DateTime.UtcNow)
                {
                    Response.StatusCode = 304;
                    return;
                }
            }

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
                            if (!resize.HasValue) resize = new Size(100, 150);
                            p.Bitmap = ThumbnailBitmap.GetBitmapDataFromText("?", 72, 
                                resize.Value.Width, resize.Value.Height);
                        }
                    }
                    else
                    {
                        p = ims.HasValue ?
                                GetPictureWithThumbnail(RequestId, ims.Value) :
                                GetPictureWithThumbnail(RequestId);
                    }

                    break;
                case PicturePageType.Bitmap:
                    
                    p = ims.HasValue ?
                            GetPictureWithBitmap(RequestId, ims.Value) :
                            GetPictureWithBitmap(RequestId);

                    break;
            }

            if (p == null)
            {
                if (ims.HasValue)
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

            if (resize.HasValue)
            {
                ThumbnailBitmap resized = new ThumbnailBitmap(new MemoryStream(p.Bitmap), new Size(0, 0),
                    resize.Value, resize.Value);

                switch (PageType)
                {
                    case PicturePageType.Bitmap:
                        p.Bitmap = resized.Bitmap;
                        break;
                    case PicturePageType.Thumbnail:
                        p.Bitmap = resized.Thumbnail;
                        break;
                }
            }

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

    public Nullable<Size> Size
    {
        get
        {
            object w = Request["Width"];
            object h = Request["Height"];
            if (w == null || h == null) return new Nullable<Size>();
            return new Nullable<Size>(new Size(int.Parse(w.ToString()), int.Parse(h.ToString())));
        }
    }

    public override SessionManager SessionManager
    {
        get
        {
            if (mSessionManager == null)
            {
                mSessionManager = new SessionManager(this);
            }
            return mSessionManager;
        }
    }
}
