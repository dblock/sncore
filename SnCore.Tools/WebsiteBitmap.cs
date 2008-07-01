using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Threading;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Net;

namespace SnCore.Tools.Drawing
{
    public class WebsiteBitmap
    {
        const int s_RequestTimeout = 60000;

        Bitmap bmp = null;
        ManualResetEvent mre;

        public Bitmap GetBitmapFromWeb(string url)
        {
            mre = new ManualResetEvent(false);

            Thread t = new Thread(new ParameterizedThreadStart(BitmapThreadWorker));
            t.SetApartmentState(ApartmentState.STA);
            t.Start(url);

            if (!mre.WaitOne(s_RequestTimeout / 2, false))
                t.Abort();

            if (!t.Join(s_RequestTimeout / 2))
                t.Abort();

            return bmp;
        }

        public void BitmapThreadWorker(object url)
        {
            try
            {
                DateTime started = DateTime.Now;
                WebBrowser browser = new WebBrowser();
                browser.ScrollBarsEnabled = false;
                browser.ClientSize = new Size(800, 600);
                browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(browser_DocumentCompleted);
                browser.Navigate((string)url);

                while (bmp == null)
                {
                    Thread.Sleep(1000);
                    Application.DoEvents();
                    TimeSpan elapsed = DateTime.Now.Subtract(started);
                    if (elapsed.TotalMilliseconds > s_RequestTimeout / 2)
                    {
                        browser.Dispose();
                        mre.Set();
                        break;
                    }
                }
            }
            catch
            {
                mre.Set();
            }
        }

        private void browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                WebBrowser browser = (WebBrowser)sender;
                browser.Document.Window.Error += new HtmlElementErrorEventHandler(Window_Error);

                bmp = new Bitmap(browser.Width, browser.Height);
                browser.DrawToBitmap(bmp, browser.Bounds);
                browser.Dispose();
            }
            finally
            {
                mre.Set();
            }
        }

        void Window_Error(object sender, HtmlElementErrorEventArgs e)
        {
            e.Handled = true;
        }
    }
}
