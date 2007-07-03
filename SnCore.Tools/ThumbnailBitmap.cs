using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using Gif.Components;

namespace SnCore.Tools.Drawing
{
    public class InvalidImageSizeException : Exception
    {
        public InvalidImageSizeException(string filename, int size, int maxbytes)
            : base(string.Format("Image {0} is {1} Kb., which is too big.\nIt needs to be less than {2} Kb.",
                filename, (int)(size / 1024), (int)(maxbytes / 1024)))
        {

        }

        public InvalidImageSizeException(string filename, Size cur, Size min)
            : base(string.Format("Image {0} is {1}x{2}, which is too small.\nIt needs to be at least {3}x{4}.",
                filename, cur.Width, cur.Height, min.Width, min.Height))
        {

        }

    }

    public class ThumbnailBitmap
    {
        public static Size ResizeSize = new Size(640, 480);
        public static Size ThumbnailSize = new Size(100, 150);
        public static int ImageQuality = 80;

        private Size mSize = ThumbnailSize;
        byte[] mThumbnail = null;
        byte[] mBitmap = null;

        public byte[] Thumbnail
        {
            get
            {
                return mThumbnail;
            }
        }

        public byte[] Bitmap
        {
            get
            {
                return mBitmap;
            }
        }

        public Size Size
        {
            get
            {
                return mSize;
            }
        }

        public ThumbnailBitmap(byte[] bitmap, Size min)
            : this(new MemoryStream(bitmap), min)
        {

        }

        public ThumbnailBitmap(byte[] bitmap)
            : this(bitmap, ThumbnailSize)
        {

        }

        public ThumbnailBitmap(Stream bitmap)
            : this(bitmap, ThumbnailSize)
        {

        }

        private bool TryLoadJPG(Stream bitmap, Size min)
        {
            long offset = bitmap.Position;

            try
            {
                Bitmap b = new Bitmap(bitmap);
                mSize = new Size(b.Width, b.Height);
                mBitmap = GetResizedImageBytesJPEG(bitmap, ResizeSize, ImageQuality);
                mThumbnail = GetThumbnailJPEG(bitmap, min);
                return true;
            }
            catch (ArgumentException)
            {

            }

            bitmap.Seek(offset, SeekOrigin.Begin);
            return false;
        }

        private bool TryLoadGIF89a(Stream bitmap, Size min)
        {
            long offset = bitmap.Position;

            try
            {
                mBitmap = GetResizedImageBytesGIF89a(bitmap, ResizeSize, ImageQuality, out mSize);
                bitmap.Seek(0, SeekOrigin.Begin);
                mThumbnail = GetThumbnailGIF89a(bitmap, min);
                return true;
            }
            catch
            {

            }

            bitmap.Seek(offset, SeekOrigin.Begin);
            return false;
        }

        public ThumbnailBitmap(Stream bitmap, Size min)
        {
            if (TryLoadGIF89a(bitmap, min))
                return;

            if (TryLoadJPG(bitmap, min))
                return;

            throw new FormatException("I don't understand this picture format.");
        }

        public ThumbnailBitmap(Bitmap bitmap)
            : this(bitmap, ThumbnailSize)
        {

        }

        public ThumbnailBitmap(Bitmap bitmap, Size min)
        {
            mSize = new Size(bitmap.Width, bitmap.Height);
            mBitmap = GetResizedImageBytesJPEG(bitmap, ResizeSize, ImageQuality);
            mThumbnail = GetThumbnailJPEG(bitmap, min);
        }

        public static byte[] GetBitmap(Bitmap originalimage)
        {
            MemoryStream ds = new MemoryStream();
            new Bitmap(originalimage).Save(ds, ImageFormat.Jpeg);
            byte[] result = new byte[ds.Length];
            MemoryStream memorystream = new MemoryStream(result);
            ds.WriteTo(memorystream);
            return result;
        }

        public static byte[] GetThumbnailJPEG(Bitmap originalimage, Size min)
        {
            if (min != null && originalimage.Width < min.Width && originalimage.Height < min.Height)
            {
                throw new InvalidImageSizeException(
                    string.Empty, originalimage.Size, min);
            }

            return GetResizedImageBytesJPEG(originalimage, ThumbnailSize, ImageQuality);
        }

        public static byte[] GetThumbnailGIF89a(Stream stream, Size min)
        {
            long offset = stream.Position;
            GifDecoder decoder = new GifDecoder();
            decoder.Read(stream);
            Size sz = decoder.GetFrameSize();
            stream.Seek(offset, SeekOrigin.Begin);

            if (min != null && sz.Width < min.Width && sz.Height < min.Height)
            {
                throw new InvalidImageSizeException(
                    string.Empty, sz, min);
            }

            return GetResizedImageBytesGIF89a(decoder, stream, ThumbnailSize, ImageQuality);
        }

        public Size GetNewSize(Size ts)
        {
            return GetNewSize(this.Size, ts);
        }

        public static Size GetNewSize(Bitmap originalimage, Size ts)
        {
            return GetNewSize(originalimage.Size, ts);
        }

        public static Size GetNewSize(Size from, Size to)
        {
            int fixedimagedimension =
                (from.Height > from.Width) ? to.Height : to.Width;

            return
                (from.Height > from.Width ?
                    new Size(fixedimagedimension, (fixedimagedimension * from.Height / from.Width)) :
                    new Size((fixedimagedimension * from.Width / from.Height), fixedimagedimension));
        }

        private static Bitmap GetResizedImage(Bitmap imgPhoto, Size ts)
        {
            // http://www.codeproject.com/csharp/imageresize.asp

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            bool sourceVertical = sourceWidth < sourceHeight;
            bool targetVeritcal = ts.Width < ts.Height;

            if (sourceVertical != targetVeritcal)
            {
                int t = ts.Width;
                ts.Width = ts.Height;
                ts.Height = t;
            }

            nPercentW = ((float)ts.Width / (float)sourceWidth);
            nPercentH = ((float)ts.Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((ts.Width -
                              (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((ts.Height -
                              (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(ts.Width, ts.Height,
                              PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                             imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.White);
            grPhoto.InterpolationMode =
                    InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        public static byte[] GetJpegBits(Bitmap bitmap, int quality)
        {
            byte[] result = null;
            MemoryStream ds = null;
            MemoryStream ms = null;

            try
            {
                EncoderParameter ratio = new EncoderParameter(Encoder.Quality, quality);
                EncoderParameters codecParams = new EncoderParameters(1);
                codecParams.Param[0] = ratio;

                ImageCodecInfo codec = FindEncoder(ImageFormat.Jpeg);

                ds = new MemoryStream();
                bitmap.Save(ds, codec, codecParams);
                result = new byte[ds.Length];
                ms = new MemoryStream(result);
                ds.WriteTo(ms);
            }
            finally
            {
                if (ds != null)
                {
                    ds.Close();
                }
                if (ms != null)
                {
                    ms.Close();
                }
            }

            return result;
        }

        public static byte[] GetResizedImageBytesGIF89a(Stream stream, Size ts, int quality, out Size sz)
        {
            GifDecoder originalimage = new GifDecoder();
            originalimage.Read(stream);
            sz = originalimage.GetFrameSize();
            return GetResizedImageBytesGIF89a(originalimage, stream, ts, quality);
        }

        public static byte[] GetResizedImageBytesGIF89a(GifDecoder originalimage, Stream stream, Size ts, int quality)
        {
            Size sz = originalimage.GetFrameSize();
            if (sz.Height > ts.Height || sz.Width > ts.Width)
            {
                MemoryStream ms = new MemoryStream();
                AnimatedGifEncoder.Resize(originalimage, ms, ts.Width, ts.Height, quality);
                ms.Flush();
                byte[] result = new byte[ms.Length];
                MemoryStream resultstream = new MemoryStream(result);
                ms.WriteTo(resultstream);
                return result;
            }
            else
            {
                stream.Seek(0, SeekOrigin.Begin);
                BinaryReader sr = new BinaryReader(stream);
                return sr.ReadBytes((int)stream.Length);
            }
        }

        public static byte[] GetResizedImageBytesJPEG(Stream stream, Size ts, int quality)
        {
            Bitmap originalimage = new Bitmap(stream);
            return GetResizedImageBytesJPEG(originalimage, ts, quality);
        }

        public static byte[] GetResizedImageBytesJPEG(Bitmap originalimage, Size ts, int quality)
        {
            Bitmap resizedimage = null;

            if (originalimage.Size.Height > ts.Height || originalimage.Size.Width > ts.Width)
            {
                resizedimage = GetResizedImage(originalimage, ts);
            }
            else
            {
                resizedimage = originalimage;
            }

            return GetJpegBits(resizedimage, quality);
        }

        public static byte[] GetThumbnailJPEG(Stream stream, Size min)
        {
            if (stream.Length == 0)
            {
                return null;
            }
            else
            {
                return GetThumbnailJPEG(new Bitmap(stream), min);
            }
        }

        /// <summary>
        /// From http://www.dotnetjunkies.com/WebLog/debasish/archive/2005/11/23/133966.aspx
        /// </summary>
        /// <param name="fmt"></param>
        /// <returns></returns>
        public static ImageCodecInfo FindEncoder(ImageFormat fmt)
        {
            ImageCodecInfo[] codecinfo = ImageCodecInfo.GetImageEncoders();

            for (int i = 0; i < codecinfo.Length; i++)
            {
                ImageCodecInfo codec = codecinfo[i];
                if (codec.FormatID.Equals(fmt.Guid))
                {
                    return codec;
                }
            }

            throw new Exception(string.Format("No encoder found for {0}.", fmt));
        }

        public static Bitmap GetBitmapFromText(string message, int fontsize, int width, int height)
        {
            Bitmap result = new Bitmap(1, 1);

            Font hFont = new Font("Tahoma", fontsize,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point);

            Graphics hGraphics = Graphics.FromImage(result);

            int nWidth = (int)hGraphics.MeasureString(message, hFont).Width;
            int nHeight = (int)hGraphics.MeasureString(message, hFont).Height;

            if (width <= 0) width = nWidth;
            if (height <= 0) height = nHeight;

            result = new Bitmap(result, new Size(width, height));

            hGraphics = Graphics.FromImage(result);
            hGraphics.Clear(Color.FromArgb(0xEE, 0xEE, 0xEE));
            hGraphics.DrawString(message, hFont, new SolidBrush(Color.Black), (width - nWidth) / 2, (height - nHeight) / 2);
            hGraphics.Flush();

            return result;
        }

        public static byte[] GetBitmapDataFromText(string message, int fontsize, int width, int height)
        {
            return GetBitmap(GetBitmapFromText(message, fontsize, width, height));
        }
    }
}