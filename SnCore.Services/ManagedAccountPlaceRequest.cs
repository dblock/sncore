using System;
using NHibernate;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using NHibernate.Expression;
using System.Web.Services.Protocols;
using System.Xml;
using System.Resources;
using System.Net.Mail;
using System.IO;
using SnCore.Tools.Web;

namespace SnCore.Services
{
    public class TransitAccountPlaceRequest : TransitService
    {
        private DateTime mSubmitted;

        public DateTime Submitted
        {
            get
            {

                return mSubmitted;
            }
            set
            {
                mSubmitted = value;
            }
        }

        private int mAccountId = 0;

        public int AccountId
        {
            get
            {

                return mAccountId;
            }
            set
            {
                mAccountId = value;
            }
        }

        private string mAccountName;

        public string AccountName
        {
            get
            {

                return mAccountName;
            }
            set
            {
                mAccountName = value;
            }
        }

        private int mPlaceId = 0;

        public int PlaceId
        {
            get
            {

                return mPlaceId;
            }
            set
            {
                mPlaceId = value;
            }
        }

        private string mPlaceName;

        public string PlaceName
        {
            get
            {

                return mPlaceName;
            }
            set
            {
                mPlaceName = value;
            }
        }

        private string mType;

        public string Type
        {
            get
            {

                return mType;
            }
            set
            {
                mType = value;
            }
        }

        private string mMessage;

        public string Message
        {
            get
            {

                return mMessage;
            }
            set
            {
                mMessage = value;
            }
        }

        public TransitAccountPlaceRequest()
        {

        }

        public TransitAccountPlaceRequest(AccountPlaceRequest o)
            : base(o.Id)
        {
            Submitted = o.Submitted;
            AccountId = o.Account.Id;
            PlaceId = o.Place.Id;
            Type = o.Type.Name;
            Message = o.Message;
            PlaceName = o.Place.Name;
            AccountName = o.Account.Name;
        }

        public AccountPlaceRequest GetAccountPlaceRequest(ISession session)
        {
            AccountPlaceRequest p = (Id != 0) ? (AccountPlaceRequest)session.Load(typeof(AccountPlaceRequest), Id) : new AccountPlaceRequest();
            p.Message = this.Message;
            p.Submitted = this.Submitted;
            if (Id == 0)
            {
                if (!string.IsNullOrEmpty(this.Type)) p.Type = ManagedAccountPlaceType.Find(session, this.Type);
                if (PlaceId > 0) p.Place = (Place)session.Load(typeof(Place), PlaceId);
                if (AccountId > 0) p.Account = (Account)session.Load(typeof(Account), AccountId);
            }
            return p;
        }
    }

    public class ManagedAccountPlaceRequest : ManagedService<AccountPlaceRequest>
    {
        private AccountPlaceRequest mAccountPlaceRequest = null;

        public ManagedAccountPlaceRequest(ISession session)
            : base(session)
        {

        }

        public ManagedAccountPlaceRequest(ISession session, int id)
            : base(session)
        {
            mAccountPlaceRequest = (AccountPlaceRequest)session.Load(typeof(AccountPlaceRequest), id);
        }

        public ManagedAccountPlaceRequest(ISession session, AccountPlaceRequest value)
            : base(session)
        {
            mAccountPlaceRequest = value;
        }

        public ManagedAccountPlaceRequest(ISession session, TransitAccountPlaceRequest value)
            : base(session)
        {
            mAccountPlaceRequest = value.GetAccountPlaceRequest(session);
        }

        public int Id
        {
            get
            {
                return mAccountPlaceRequest.Id;
            }
        }

        public TransitAccountPlaceRequest TransitAccountPlaceRequest
        {
            get
            {
                return new TransitAccountPlaceRequest(mAccountPlaceRequest);
            }
        }

        public Account Account
        {
            get
            {
                return mAccountPlaceRequest.Account;
            }
        }

        public void Delete()
        {
            Session.Delete(mAccountPlaceRequest);
        }

        public void Reject(string message)
        {
            ManagedAccount recepient = new ManagedAccount(Session, mAccountPlaceRequest.Account);
            string sentto = recepient.ActiveEmailAddress;
            if (sentto != null)
            {
                ManagedSiteConnector.SendAccountEmailMessageUriAsAdmin(
                    Session,
                    new MailAddress(sentto, recepient.Name).ToString(),
                    string.Format("EmailAccountPlaceRequestReject.aspx?id={0}&message={1}", this.Id, Renderer.UrlEncode(message)));
            }

            // delete the request when user notified
            mAccountPlaceRequest.Account.AccountPlaceRequests.Remove(mAccountPlaceRequest);
            Session.Delete(mAccountPlaceRequest);
        }

        public void Accept(string message)
        {
            AccountPlace place = new AccountPlace();
            place.Account = mAccountPlaceRequest.Account;
            place.Place = mAccountPlaceRequest.Place;
            place.Created = place.Modified = DateTime.UtcNow;
            place.Type = mAccountPlaceRequest.Type;
            place.Description = string.Format("System-approved on {0}.", DateTime.UtcNow.ToString());
            Session.Save(place);

            if (mAccountPlaceRequest.Account.AccountPlaces == null) mAccountPlaceRequest.Account.AccountPlaces = new List<AccountPlace>();
            mAccountPlaceRequest.Account.AccountPlaces.Add(place);

            if (mAccountPlaceRequest.Place.AccountPlaces == null) mAccountPlaceRequest.Place.AccountPlaces = new List<AccountPlace>();
            mAccountPlaceRequest.Place.AccountPlaces.Add(place);

            ManagedAccount recepient = new ManagedAccount(Session, mAccountPlaceRequest.Account);
            string sentto = recepient.ActiveEmailAddress;
            if (sentto != null)
            {
                ManagedSiteConnector.SendAccountEmailMessageUriAsAdmin(
                    Session,
                    new MailAddress(sentto, recepient.Name).ToString(),
                    string.Format("EmailAccountPlaceRequestAccept.aspx?id={0}&message={1}", this.Id, Renderer.UrlEncode(message)));
            }

            // delete the request when user notified
            mAccountPlaceRequest.Account.AccountPlaceRequests.Remove(mAccountPlaceRequest);
            Session.Delete(mAccountPlaceRequest);
        }

        public Place Place
        {
            get
            {
                return mAccountPlaceRequest.Place;
            }
        }
    }
}
