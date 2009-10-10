using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.IO;
using SnCore.Tools.Drawing;
using Gif.Components;

namespace SnCore.Tools.Tests
{
    [TestFixture]
    public class NGifTest
    {
        [Test]
        public void CreateAnimatedGIFTest()
        {
            MemoryStream ms = new MemoryStream();
            AnimatedGifEncoder encoder = new AnimatedGifEncoder();
            // encoder.SetDelay(200);
            encoder.SetFrameRate(5);
            encoder.Start(ms);
            for (char i = 'a'; i <= 'z'; i++)
            {
                Console.Write(i.ToString());
                encoder.AddFrame(ThumbnailBitmap.GetBitmapFromText(i.ToString(), 48, 100, 200));
            }
            encoder.Finish();
            Console.WriteLine();
        }

        [Test]
        public void ConsumeAnimatedGIFTest()
        {
            MemoryStream ms = new MemoryStream();
            AnimatedGifEncoder encoder = new AnimatedGifEncoder();
            // encoder.SetDelay(200);
            encoder.SetFrameRate(5);
            encoder.Start(ms);
            for (char i = 'a'; i <= 'z'; i++)
            {
                Console.Write(i.ToString());
                encoder.AddFrame(ThumbnailBitmap.GetBitmapFromText(i.ToString(), 48, 100, 200));
            }
            Console.WriteLine();
            encoder.Finish();
            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);

            GifDecoder decoder = new GifDecoder();
            decoder.Read(ms);
            Console.WriteLine("Frames: {0}", decoder.GetFrameCount());
            Assert.AreEqual(26, decoder.GetFrameCount());
        }

        [Test]
        public void ResizeAnimatedGIFTest()
        {
            string filename = Path.GetTempFileName();
            Console.WriteLine("Creating: {0}", filename);
            string resizedfilename = Path.GetTempFileName();
            Console.WriteLine("Resizing: {0}", resizedfilename);
            try
            {
                using (FileStream ms = File.OpenWrite(filename))
                {
                    AnimatedGifEncoder encoder = new AnimatedGifEncoder();
                    encoder.SetFrameRate(5);
                    encoder.Start(ms);
                    for (char i = 'a'; i <= 'e'; i++)
                    {
                        Console.Write(i.ToString());
                        encoder.AddFrame(ThumbnailBitmap.GetBitmapFromText(
                            i.ToString(), 48, 300, 200));
                    }
                    Console.WriteLine();
                    encoder.Finish();
                    ms.Flush();
                }

                using (FileStream ms = File.OpenRead(filename))
                {
                    using (FileStream resizedms = File.OpenWrite(resizedfilename))
                    {
                        AnimatedGifEncoder.Resize(ms, resizedms, 200, 150, 100);
                        resizedms.Flush();
                    }
                }
            }
            finally
            {
                if (File.Exists(filename))
                    File.Delete(filename);
                if (File.Exists(resizedfilename))
                    File.Delete(resizedfilename);
            }
        }

        [Test]
        public void ResizeToThumbnailTest()
        {
            string filename = Path.GetTempFileName();
            Console.WriteLine("Creating: {0}", filename);
            try
            {
                MemoryStream ms = new MemoryStream();
                AnimatedGifEncoder encoder = new AnimatedGifEncoder();
                encoder.SetFrameRate(5);
                encoder.Start(ms);
                for (char i = 'a'; i <= 'e'; i++)
                {
                    Console.Write(i.ToString());
                    encoder.AddFrame(ThumbnailBitmap.GetBitmapFromText(
                        i.ToString(), 48, 300, 200));
                }
                Console.WriteLine();
                encoder.Finish();
                ms.Flush();
                ms.Seek(0, SeekOrigin.Begin);

                ThumbnailBitmap bitmap = new ThumbnailBitmap(ms);
                FileStream fs = File.Create(filename);
                byte[] th = bitmap.Thumbnail;
                fs.Write(th, 0, th.Length);
                fs.Close();
            }
            finally
            {
                if (File.Exists(filename))
                    File.Delete(filename);
            }
        }
    }
}
