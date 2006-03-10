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
    public class TransitTagWordAccount : TransitService
    {
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
            : base(o.Id)
        {
            Word = o.TagWord.Word;
            AccountId = o.AccountId;
        }

        public TagWordAccount GetTagWordAccount(ISession session)
        {
            TagWordAccount p = (Id != 0) ? (TagWordAccount)session.Load(typeof(TagWordAccount), Id) : new TagWordAccount();
            p.TagWord = ManagedTagWord.Find(session, this.Word);

            if (Id == 0)
            {
                p.AccountId = this.AccountId;
            }

            return p;
        }
    }

    /// <summary>
    /// Managed tag word.
    /// </summary>
    public class ManagedTagWordAccount : ManagedService
    {
        private TagWordAccount mTagWordAccount = null;

        public ManagedTagWordAccount(ISession session)
            : base(session)
        {

        }

        public ManagedTagWordAccount(ISession session, int id)
            : base(session)
        {
            mTagWordAccount = (TagWordAccount)session.Load(typeof(TagWordAccount), id);
        }

        public ManagedTagWordAccount(ISession session, TagWordAccount value)
            : base(session)
        {
            mTagWordAccount = value;
        }

        public ManagedTagWordAccount(ISession session, TransitTagWordAccount value)
            : base(session)
        {
            mTagWordAccount = value.GetTagWordAccount(session);
        }

        public int Id
        {
            get
            {
                return mTagWordAccount.Id;
            }
        }

        public TransitTagWordAccount TransitTagWordAccount
        {
            get
            {
                return new TransitTagWordAccount(mTagWordAccount);
            }
        }

        public void CreateOrUpdate(TransitTagWordAccount o)
        {
            mTagWordAccount = o.GetTagWordAccount(Session);
            mTagWordAccount.Modified = DateTime.UtcNow;
            if (Id == 0) mTagWordAccount.Created = mTagWordAccount.Modified;
            Session.Save(mTagWordAccount);
        }

        public void Delete()
        {
            Session.Delete(mTagWordAccount);
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

    }
}
