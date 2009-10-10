using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Reflection;
using System.Resources;
using System.IO;
using SnCore.Tools.Drawing;
using System.Drawing;
using Gif.Components;

namespace SnCore.Tools.Tests
{
    [TestFixture]
    public class ThumbnailBitmapTest
    {
        [Test]
        public void TestLoadAll()
        {
            string[] images = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            foreach (string image in images)
            {
                if (!image.StartsWith("SnCore.Tools.Tests.Images."))
                    continue;

                Console.WriteLine(image);

                Stream sm = Assembly.GetExecutingAssembly().GetManifestResourceStream(image);
                Assert.IsNotNull(sm);
                ThumbnailBitmap b = new ThumbnailBitmap(sm, new Size(1, 1), ThumbnailBitmap.s_FullSize, ThumbnailBitmap.s_ThumbnailSize);
                Console.WriteLine("Size: {0}x{1}", b.FullSize.Width, b.FullSize.Height);
                Assert.IsNotNull(b.Bitmap);
            }
        }

        [Test]
        public void TestLoadJPG()
        {
            Stream sm = Assembly.GetExecutingAssembly().GetManifestResourceStream("SnCore.Tools.Tests.Images.test.jpg");
            Assert.IsNotNull(sm);
            ThumbnailBitmap b = new ThumbnailBitmap(sm);
            Console.WriteLine("Size: {0}x{1}", b.FullSize.Width, b.FullSize.Height);
            Assert.AreEqual(ThumbnailBitmap.s_FullSize, b.FullSize);
            Assert.IsNotNull(b.Thumbnail);
            Assert.IsNotNull(b.Bitmap);
            ThumbnailBitmap th = new ThumbnailBitmap(b.Thumbnail);
            Console.WriteLine("Size: {0}x{1}", th.FullSize.Width, th.FullSize.Height);
            // horizontal bitmap, thumbnail size assumes vertical, thumbnail adjusted to horizontal
            Assert.AreEqual(new Size(ThumbnailBitmap.s_ThumbnailSize.Height, ThumbnailBitmap.s_ThumbnailSize.Width), th.FullSize);
        }

        [Test]
        public void TestLoadGIF89a()
        {
            GifDecoder a_decoder = new GifDecoder();
            Stream sm = Assembly.GetExecutingAssembly().GetManifestResourceStream("SnCore.Tools.Tests.Images.animated.gif");
            Console.WriteLine("Bytes: {0}", sm.Length);
            a_decoder.Read(sm);
            Console.WriteLine("Frames: {0}", a_decoder.GetFrameCount());

            sm.Seek(0, SeekOrigin.Begin);
            Assert.IsNotNull(sm);
            ThumbnailBitmap b = new ThumbnailBitmap(sm);
            Console.WriteLine("Size: {0}x{1}", b.FullSize.Width, b.FullSize.Height);
            Assert.AreEqual(new Size(320, 240), b.FullSize);
            Assert.IsNotNull(b.Thumbnail);
            Assert.IsNotNull(b.Bitmap);
            ThumbnailBitmap th = new ThumbnailBitmap(b.Thumbnail);
            Console.WriteLine("Size: {0}x{1}", th.FullSize.Width, th.FullSize.Height);
            Assert.AreEqual(new Size(150, 100), th.FullSize);
            
            // make sure that the bitmap is still animated            
            Console.WriteLine("Bytes: {0}", b.Bitmap.Length);
            GifDecoder decoder = new GifDecoder();
            decoder.Read(new MemoryStream(b.Bitmap));
            Console.WriteLine("Frames: {0}", decoder.GetFrameCount());

            GifDecoder th_decoder = new GifDecoder();
            Console.WriteLine("Bytes: {0}", b.Thumbnail.Length);
            th_decoder.Read(new MemoryStream(b.Thumbnail));
            Console.WriteLine("Frames: {0}", th_decoder.GetFrameCount());
            Assert.AreEqual(th_decoder.GetFrameCount(), decoder.GetFrameCount());
        }

        [Test]
        public void TestLoadGIF89()
        {
            Stream sm = Assembly.GetExecutingAssembly().GetManifestResourceStream("SnCore.Tools.Tests.Images.test.gif");
            Assert.IsNotNull(sm);
            ThumbnailBitmap b = new ThumbnailBitmap(sm);
            Console.WriteLine("Size: {0}x{1}", b.FullSize.Width, b.FullSize.Height);
            Assert.AreEqual(new Size(592, 350), b.FullSize);
            Assert.IsNotNull(b.Thumbnail);
            Assert.IsNotNull(b.Bitmap);
            ThumbnailBitmap th = new ThumbnailBitmap(b.Thumbnail);
            Console.WriteLine("Size: {0}x{1}", th.FullSize.Width, th.FullSize.Height);
            // horizontal bitmap, thumbnail size assumes vertical, thumbnail adjusted to horizontal
            Assert.AreEqual(new Size(150, 100), th.FullSize);
        }

        [Test, ExpectedException(typeof(FormatException))]
        public void TestLoadInvalid()
        {
            MemoryStream s = new MemoryStream();
            ThumbnailBitmap b = new ThumbnailBitmap(s);
            Console.WriteLine("Size: {0}x{1}", b.FullSize.Width, b.FullSize.Height);
        }

        [Test, ExpectedException(typeof(InvalidImageSizeException))]
        public void TestTooSmallGIF89()
        {
            Stream sm = Assembly.GetExecutingAssembly().GetManifestResourceStream("SnCore.Tools.Tests.Images.small.gif");
            // load original image in decoder
            GifDecoder originaldecoder = new GifDecoder();
            originaldecoder.Read(sm);
            sm.Seek(0, SeekOrigin.Begin);
            GifDecoder.GifFrame frame = originaldecoder.GetFrame(0);
            Console.WriteLine("Transparency: {0} - {1}", frame.transparency, Color.FromArgb(frame.bgcolor));
            // load in ThumnailBitmap
            Assert.IsNotNull(sm);
            ThumbnailBitmap b = new ThumbnailBitmap(sm);
            Console.WriteLine("Size: {0}x{1}", b.FullSize.Width, b.FullSize.Height);
        }

        [Test]
        public void TestResizeTransparentGIF89()
        {
            Stream sm = Assembly.GetExecutingAssembly().GetManifestResourceStream("SnCore.Tools.Tests.Images.transparent.gif");
            // load original image in decoder
            GifDecoder originaldecoder = new GifDecoder();
            originaldecoder.Read(sm);
            sm.Seek(0, SeekOrigin.Begin);
            GifDecoder.GifFrame frame = originaldecoder.GetFrame(0);
            Console.WriteLine("Transparency: {0} - {1}", frame.transparency, Color.FromArgb(frame.bgcolor));
            // load in ThumnailBitmap
            Assert.IsNotNull(sm);
            ThumbnailBitmap b = new ThumbnailBitmap(sm);
            Console.WriteLine("Size: {0}x{1}", b.FullSize.Width, b.FullSize.Height);
            // Assert.AreEqual(b.FullSize, new Size(227, 227));
            Assert.IsNotNull(b.Thumbnail);
            Assert.IsNotNull(b.Bitmap);
            // reload in decoder, verify transparency
            GifDecoder decoder = new GifDecoder();
            decoder.Read(new MemoryStream(b.Bitmap));
            frame = decoder.GetFrame(0);
            Console.WriteLine("Transparency: {0} - {1}", frame.transparency, Color.FromArgb(frame.bgcolor));
            //FileStream f = File.Create("C:\\temp\\chefout.gif");
            //f.Write(b.Bitmap, 0, b.Bitmap.Length);
            //f.Close();
            //f = File.Create("C:\\temp\\chefthumbnail.gif");
            //f.Write(b.Thumbnail, 0, b.Thumbnail.Length);
            //f.Close();
        }
    }
}
