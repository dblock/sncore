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
using System.Drawing.Imaging;

namespace SnCore.Tools.Tests
{
    [TestFixture]
    public class WebsiteBitmapTest
    {
        [Test]
        public void TestLoadLocalHost()
        {
            WebsiteBitmap t = new WebsiteBitmap();
            Bitmap bmp = t.GetBitmapFromWeb("http://localhost/alert.htm");
            Assert.IsNotNull(bmp);
            Console.WriteLine("{0}:{1}", bmp.Width, bmp.Height);
        }

        [Test]
        public void TestLoadLocalHostSnCoreWeb()
        {
            WebsiteBitmap t = new WebsiteBitmap();
            Bitmap bmp = t.GetBitmapFromWeb("http://localhost/SnCoreWeb/");
            Assert.IsNotNull(bmp);
            Console.WriteLine("{0}:{1}", bmp.Width, bmp.Height);
            bmp.Save("c:\\temp\\test.jpg", ImageFormat.Jpeg);
        }

        [Test]
        public void TestBogusUri()
        {
            WebsiteBitmap t = new WebsiteBitmap();
            Bitmap bmp = t.GetBitmapFromWeb(Guid.NewGuid().ToString());
        }
    }
}
