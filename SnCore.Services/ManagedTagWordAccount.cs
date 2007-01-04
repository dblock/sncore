using System;
using NHibernate;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using NHibernate.Expression;
using System.Web.Services.Protocols;
using System.Xml;
using System.Resources;
using System.Net.Mail;
using System.IO;

namespace SnCore.Services
{
    public class TransitTagWordAccount : TransitService<TagWordAccount>
    {
        private DateTime mCreated;

        public DateTime Created
        {
            get
            {
                return mCreated;
            }
            set
            {
                mCreated = value;
            }
        }

        private DateTime mModified;

        public DateTime Modified
        {
            get
            {
                return mModified;
            }
            set
            {
                mModified = value;
            }
        }

        private string mWord;

        public string Word
        {
            get
            {
                return mWord;
            }
            set
            {
                mWord = value;
            }
        }

        private int mAccountId;

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

        public TransitTagWordAccount()
        {

        }

        public TransitTagWordAccount(TagWordAccount o)
            : base(o)
        {

        }

        public override void SetInstance(TagWordAccount value)
        {
            Word = value.TagWord.Word;
            AccountId = value.AccountId;
            Created = value.Created;
            Modified = value.Modified;
            base.SetInstance(value);
        }

        public override TagWordAccount GetInstance(ISession session, ManagedSecurityContext sec)
        {
            TagWordAccount instance = base.GetInstance(session, sec);
            instance.TagWord = ManagedTagWord.Find(session, this.Word);

            if (Id == 0)
            {
                instance.AccountId = this.AccountId;
            }

            return instance;
        }
    }

    public class ManagedTagWordAccount : ManagedService<TagWordAccount, TransitTagWordAccount>
    {
        public ManagedTagWordAccount()
        {

        }

        public ManagedTagWordAccount(ISession session)
            : base(session)
        {

        }

        public ManagedTagWordAccount(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedTagWordAccount(ISession session, TagWordAccount value)
            : base(session, value)
        {

        }

        public static TagWordAccount Find(ISession session, string word)
        {
            return (TagWordAccount)session.CreateCriteria(typeof(TagWordAccount))
                .Add(Expression.Eq("Word", word))
                .UniqueResult();
        }

        public static int FindId(ISession session, string name)
        {
            return Find(session, name).Id;
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Modified = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Created = mInstance.Modified;
            base.Save(sec);
        }

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            acl.Add(new ACLEveryoneAllowRetrieve());
            return acl;
        }
    }
}
