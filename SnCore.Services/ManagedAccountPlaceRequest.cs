using System;
using NHibernate;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Expression;
using System.Web.Services.Protocols;
using System.Xml;
using System.Resources;
using System.Net.Mail;
using System.IO;
using SnCore.Tools.Web;
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public class TransitAccountPlaceRequest : TransitService<AccountPlaceRequest>
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

        public TransitAccountPlaceRequest(AccountPlaceRequest value)
            : base(value)
        {

        }

        public override void SetInstance(AccountPlaceRequest value)
        {
            Submitted = value.Submitted;
            AccountId = value.Account.Id;
            PlaceId = value.Place.Id;
            Type = value.Type.Name;
            Message = value.Message;
            PlaceName = value.Place.Name;
            AccountName = value.Account.Name;
            base.SetInstance(value);
        }

        public override AccountPlaceRequest GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountPlaceRequest instance = base.GetInstance(session, sec);
            instance.Message = this.Message;
            instance.Submitted = this.Submitted;
            if (Id == 0)
            {
                instance.Type = ManagedAccountPlaceType.Find(session, this.Type);
                instance.Place = (Place)session.Load(typeof(Place), PlaceId);
                instance.Account = GetOwner(session, AccountId, sec);
            }
            return instance;
        }
    }

    public class ManagedAccountPlaceRequest : ManagedService<AccountPlaceRequest, TransitAccountPlaceRequest>
    {
        public ManagedAccountPlaceRequest()
        {

        }

        public ManagedAccountPlaceRequest(ISession session)
            : base(session)
        {

        }

        public ManagedAccountPlaceRequest(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountPlaceRequest(ISession session, AccountPlaceRequest value)
            : base(session, value)
        {

        }

        public Account Account
        {
            get
            {
                return mInstance.Account;
            }
        }

        public void Reject(ManagedSecurityContext sec, string message)
        {
            GetACL().Check(sec, DataOperation.Delete);

            ManagedAccount recepient = new ManagedAccount(Session, mInstance.Account);
            string sentto = recepient.GetActiveEmailAddress();
            if (sentto != null)
            {
                ManagedSiteConnector.SendAccountEmailMessageUriAsAdmin(
                    Session,
                    new MailAddress(sentto, recepient.Name).ToString(),
                    string.Format("EmailAccountPlaceRequestReject.aspx?id={0}&message={1}", this.Id, Renderer.UrlEncode(message)));
            }

            // delete the request when user notified
            Collection<AccountPlaceRequest>.GetSafeCollection(mInstance.Account.AccountPlaceRequests).Remove(mInstance);
            Session.Delete(mInstance);
        }

        // TODO: introduce security context
        public void Accept(ManagedSecurityContext sec, string message)
        {
            GetACL().Check(sec, DataOperation.Delete);

            AccountPlace place = new AccountPlace();
            place.Account = mInstance.Account;
            place.Place = mInstance.Place;
            place.Created = place.Modified = DateTime.UtcNow;
            place.Type = mInstance.Type;
            place.Description = string.Format("System-approved on {0}.", DateTime.UtcNow.ToString());
            Session.Save(place);

            if (mInstance.Account.AccountPlaces == null) mInstance.Account.AccountPlaces = new List<AccountPlace>();
            mInstance.Account.AccountPlaces.Add(place);

            if (mInstance.Place.AccountPlaces == null) mInstance.Place.AccountPlaces = new List<AccountPlace>();
            mInstance.Place.AccountPlaces.Add(place);

            ManagedAccount recepient = new ManagedAccount(Session, mInstance.Account);
            string sentto = recepient.GetActiveEmailAddress();
            if (sentto != null)
            {
                ManagedSiteConnector.SendAccountEmailMessageUriAsAdmin(
                    Session,
                    new MailAddress(sentto, recepient.Name).ToString(),
                    string.Format("EmailAccountPlaceRequestAccept.aspx?id={0}&message={1}", this.Id, Renderer.UrlEncode(message)));
            }

            // delete the request when user notified
            Collection<AccountPlaceRequest>.GetSafeCollection(mInstance.Account.AccountPlaceRequests).Remove(mInstance);
            Session.Delete(mInstance);
        }

        public Place Place
        {
            get
            {
                return mInstance.Place;
            }
        }

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            acl.Add(new ACLAuthenticatedAllowCreateAndDelete());
            acl.Add(new ACLAccount(mInstance.Account, DataOperation.Delete | DataOperation.Retreive));
            return acl;
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            if (Id == 0) mInstance.Submitted = DateTime.UtcNow;
            base.Save(sec);
        }

        public override int CreateOrUpdate(TransitAccountPlaceRequest t_instance, ManagedSecurityContext sec)
        {
            if (t_instance.Id == 0)
            {
                AccountPlace e_place = (AccountPlace)Session.CreateCriteria(typeof(AccountPlace))
                    .Add(Expression.Eq("Account.Id", t_instance.AccountId))
                    .Add(Expression.Eq("Place.Id", t_instance.PlaceId))
                    .UniqueResult();

                if (e_place != null)
                {
                    throw new Exception("You already have a relationship with this place.");
                }
            }

            int id = base.CreateOrUpdate(t_instance, sec);
            Session.Flush();

            ManagedSiteConnector.SendAccountEmailMessageUriAsAdmin(
                Session,
                new MailAddress(ManagedConfiguration.GetValue(Session, "SnCore.Admin.EmailAddress", "admin@localhost.com"),
                    ManagedConfiguration.GetValue(Session, "SnCore.Admin.Name", "Admin")).ToString(),
                string.Format("EmailAccountPlaceRequest.aspx?id={0}", id));

            foreach (AccountPlace place in Collection<AccountPlace>.GetSafeCollection(mInstance.Place.AccountPlaces))
            {
                if (place.Type.CanWrite)
                {
                    ManagedAccount acct = new ManagedAccount(Session, place.Account);

                    if (!acct.HasVerifiedEmail(sec))
                        continue;

                    ManagedSiteConnector.SendAccountEmailMessageUriAsAdmin(
                        Session,
                        new MailAddress(acct.GetActiveEmailAddress(), acct.Name).ToString(),
                        string.Format("EmailAccountPlaceRequest.aspx?id={0}", id));
                }
            }

            return id;
        }

        protected override void Check(TransitAccountPlaceRequest t_instance, ManagedSecurityContext sec)
        {
            base.Check(t_instance, sec);
            if (t_instance.Id == 0)
            {
                sec.CheckVerifiedEmail();
                GetQuota().Check(mInstance.Account.AccountPlaceRequests);
            }
        }
    }
}
