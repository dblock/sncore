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
        public void TestLoadJPG()
        {
            Stream sm = Assembly.GetExecutingAssembly().GetManifestResourceStream("SnCore.Tools.Tests.Images.test.jpg");
            Assert.IsNotNull(sm);
            ThumbnailBitmap b = new ThumbnailBitmap(sm);
            Console.WriteLine("Size: {0}x{1}", b.Size.Width, b.Size.Height);
            Assert.AreEqual(new Size(768, 576), b.Size);
            Assert.IsNotNull(b.Thumbnail);
            Assert.IsNotNull(b.Bitmap);
            ThumbnailBitmap th = new ThumbnailBitmap(b.Thumbnail);
            Console.WriteLine("Size: {0}x{1}", th.Size.Width, th.Size.Height);
            // horizontal bitmap, thumbnail size assumes vertical, thumbnail adjusted to horizontal
            Assert.AreEqual(new Size(ThumbnailBitmap.ThumbnailSize.Height, ThumbnailBitmap.ThumbnailSize.Width), th.Size);
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
            Console.WriteLine("Size: {0}x{1}", b.Size.Width, b.Size.Height);
            Assert.AreEqual(new Size(320, 240), b.Size);
            Assert.IsNotNull(b.Thumbnail);
            Assert.IsNotNull(b.Bitmap);
            ThumbnailBitmap th = new ThumbnailBitmap(b.Thumbnail);
            Console.WriteLine("Size: {0}x{1}", th.Size.Width, th.Size.Height);
            Assert.AreEqual(new Size(150, 100), th.Size);
            
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
            Console.WriteLine("Size: {0}x{1}", b.Size.Width, b.Size.Height);
            Assert.AreEqual(new Size(592, 350), b.Size);
            Assert.IsNotNull(b.Thumbnail);
            Assert.IsNotNull(b.Bitmap);
            ThumbnailBitmap th = new ThumbnailBitmap(b.Thumbnail);
            Console.WriteLine("Size: {0}x{1}", th.Size.Width, th.Size.Height);
            // horizontal bitmap, thumbnail size assumes vertical, thumbnail adjusted to horizontal
            Assert.AreEqual(new Size(150, 100), th.Size);
        }

        [Test, ExpectedException(typeof(FormatException))]
        public void TestLoadInvalid()
        {
            MemoryStream s = new MemoryStream();
            ThumbnailBitmap b = new ThumbnailBitmap(s);
            Console.WriteLine("Size: {0}x{1}", b.Size.Width, b.Size.Height);
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
            Console.WriteLine("Size: {0}x{1}", b.Size.Width, b.Size.Height);
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
            Console.WriteLine("Size: {0}x{1}", b.Size.Width, b.Size.Height);
            // Assert.AreEqual(b.Size, new Size(227, 227));
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
