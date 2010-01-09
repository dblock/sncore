using System;
using System.Collections.Generic;
using System.Text;
using Semaview.Shared.ICalParser;
using System.Net;
using System.IO;
using NHibernate;

namespace SnCore.Services
{
    /// <summary>
    /// This emmits an AccountEvent 
    /// </summary>
    public class TransitAccountEventICALEmitter : IEmitter
    {
        private TransitAccountEvent mAccountEvent;
        private TransitSchedule mSchedule;
        private TransitPlace mPlace;
        private TimeSpan mUtcOffset = TimeSpan.Zero;

        public TimeSpan UtcOffset
        {
            get
            {
                return mUtcOffset;
            }
            set
            {
                mUtcOffset = value;
            }
        }

        public TransitAccountEvent AccountEvent
        {
            get
            {
                return mAccountEvent;
            }
        }

        public TransitSchedule Schedule
        {
            get
            {
                return mSchedule;
            }
        }

        public TransitPlace Place
        {
            get
            {
                return mPlace;
            }
        }

        private Parser mVParser;

        public Parser VParser
        {
            get
            {
                return mVParser;
            }
            set
            {
                mVParser = value;
            }
        }

        private Token mComponentToken;
        private Token mIdToken;

        public TransitAccountEventICALEmitter()
        {

        }

        public void doIntro()
        {
            mAccountEvent = new TransitAccountEvent();
            mAccountEvent.Publish = true;

            mPlace = new TransitPlace();
            mSchedule = new TransitSchedule();
        }

        public void doOutro() { }
        public void doEnd(Token t) { }
        public void doResourceBegin(Token t) { }
        public void doBegin(Token t) { }
        
        public void doComponentBegin(Token t)
        {
            mComponentToken = t;
            // Console.WriteLine(t.TokenText);
        }

        public void doComponent() { }

        public void doEndComponent() 
        {
            // Console.WriteLine(mComponentToken.TokenText);
            mComponentToken = null;
        }

        public void doID(Token t)
        {
            mIdToken = t;
        }

        public void doSymbolic(Token t) { }
        public void doResource(Token t) { } 
        public void doURIResource(Token t) { }
        public void doMailto(Token t) { }

        public void doValueProperty(Token t, Token iprop) 
        {
            if (mComponentToken == null)
                return;

            switch (mComponentToken.TokenText)
            {
                case "Vevent":

                    if (mAccountEvent == null || mIdToken == null || mSchedule == null)
                        throw new Exception(string.Format(
                            "Unexpected {0}", t.TokenText));

                    switch (mIdToken.TokenText)
                    {
                        case "dtstart":
                            mSchedule.StartDateTime = mAccountEvent.StartDateTime = DateTime.Parse(
                                Token.ParseDateTime(t.TokenText)).Add(-UtcOffset);
                            mSchedule.EndDateTime = mAccountEvent.EndDateTime = mAccountEvent.StartDateTime.AddHours(1);
                            break;
                        case "dtend":
                            mSchedule.EndDateTime = mAccountEvent.EndDateTime = DateTime.Parse(
                                Token.ParseDateTime(t.TokenText)).Add(-UtcOffset);
                            break;
                    }

                    break;            
            }
        
            // Console.WriteLine(mIdToken.TokenText  + ": "  + t.TokenText);
        }

        public void doIprop(Token t, Token iprop) { }

        public void doRest(Token t, Token id) 
        {
            if (mComponentToken == null)
                return;

            switch (mComponentToken.TokenText)
            {
                case "Vevent":
                    
                    if (mAccountEvent == null)
                        throw new Exception(string.Format(
                            "Unexpected {0}", id.TokenText));

                    switch (id.TokenText)
                    {
                        case "summary":
                            mAccountEvent.Name = t.TokenText;
                            break;
                        case "description":
                            mAccountEvent.Description = t.TokenText;
                            break;
                        case "url":
                            mAccountEvent.Website = t.TokenText;
                            break;
                    }

                    break;

                case "Vvenue":

                    if (mAccountEvent == null || mPlace == null)
                        throw new Exception(string.Format(
                            "Unexpected {0}", id.TokenText));

                    switch (id.TokenText)
                    {
                        case "name":
                            mPlace.Name = mAccountEvent.PlaceName = t.TokenText;
                            break;
                        case "address":
                            mPlace.Street = t.TokenText;
                            break;
                        case "city":
                            mPlace.City = mAccountEvent.PlaceCity = t.TokenText;
                            break;
                        case "region":
                            mPlace.State = mAccountEvent.PlaceState = t.TokenText;
                            break;
                        case "country":
                            mPlace.Country = mAccountEvent.PlaceCountry = t.TokenText;
                            break;
                        case "postalcode":
                            mPlace.Zip = t.TokenText;
                            break;
                    }

                    break;
            }

            // Console.WriteLine("\t" + id.TokenText + "=>" + t.TokenText);
        } 

        public void doAttribute(Token key, Token val) { }
        public void emit(string val) { }

        public static TransitAccountEventICALEmitter Parse(string url, string useragent)
        {
            return Parse(url, TimeSpan.Zero, useragent);
        }

        public static TransitAccountEventICALEmitter Parse(string url, TimeSpan utcoffset, string useragent)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url.Replace("webcal:", "http:"));
            request.UserAgent = useragent;
            WebResponse response = request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream());
            TransitAccountEventICALEmitter emitter = new TransitAccountEventICALEmitter();
            emitter.UtcOffset = utcoffset;
            Semaview.Shared.ICalParser.Parser parser = new Semaview.Shared.ICalParser.Parser(sr, emitter);
            parser.Parse();
            return emitter;
        }
    }
}
