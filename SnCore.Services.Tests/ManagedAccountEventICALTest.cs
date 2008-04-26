using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Semaview.Shared.ICalParser;
using System.Web;
using System.Net;
using System.IO;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountEventICALTest
    {
        [Test]
        public void TestEventful()
        {
            // string url = "http://eventful.com/ical/events/E0-001-003054648-3/E0-001-003054648-3.ics";
            string url = "http://eventful.com/ical/events/E0-001-002992261-4/E0-001-002992261-4.ics";
            Console.WriteLine(SnCore.Tools.Web.Renderer.UrlEncode(url));
            TransitAccountEventICALEmitter emitter = new TransitAccountEventICALEmitter();
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            WebResponse response = request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream());
            Parser parser = new Parser(sr, emitter);
            parser.Parse();
            Console.WriteLine(emitter.AccountEvent.Name);
            Console.WriteLine(emitter.AccountEvent.Description);
            Console.WriteLine(emitter.AccountEvent.StartDateTime);
        }

        [Test]
        public void TestMeetup()
        {
            string url = "http://dessert.meetup.com/3/calendar/7801237/ical/Cupcake+Social+2.0/";
            Console.WriteLine(SnCore.Tools.Web.Renderer.UrlEncode(url));
            TransitAccountEventICALEmitter emitter = new TransitAccountEventICALEmitter();
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            WebResponse response = request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream());
            Parser parser = new Parser(sr, emitter);
            parser.Parse();
            Console.WriteLine(emitter.AccountEvent.Name);
            Console.WriteLine(emitter.AccountEvent.Description);
            Console.WriteLine(emitter.AccountEvent.StartDateTime);
        }
    }
}
