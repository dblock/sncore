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
using System.Text.RegularExpressions;
using System.Collections.Specialized;

namespace SnCore.Services
{
    public enum TransitTagWordQueryOptions
    {
        Promoted,
        Excluded,
        New
    };

    public class TransitTagWord : TransitService<TagWord>
    {
        private int mFrequency = -1;

        public int Frequency
        {
            get
            {
                return mFrequency;
            }
            set
            {
                mFrequency = value;
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

        private bool mPromoted;

        public bool Promoted
        {
            get
            {
                return mPromoted;
            }
            set
            {
                mPromoted = value;
            }
        }

        private bool mExcluded;

        public bool Excluded
        {
            get
            {
                return mExcluded;
            }
            set
            {
                mExcluded = value;
            }
        }

        public TransitTagWord()
        {

        }

        public TransitTagWord(TagWord value)
            : base(value)
        {

        }

        public override void SetInstance(TagWord value)
        {
            base.SetInstance(value);
            Word = value.Word;
            Promoted = value.Promoted;
            Excluded = value.Excluded;
        }

        public override TagWord GetInstance(ISession session, ManagedSecurityContext sec)
        {
            TagWord instance = base.GetInstance(session, sec);
            instance.Word = this.Word;
            instance.Promoted = this.Promoted;
            instance.Excluded = this.Excluded;
            return instance;
        }
    }

    public class ManagedTagWord : ManagedService<TagWord, TransitTagWord>
    {
        public ManagedTagWord(ISession session)
            : base(session)
        {

        }

        public ManagedTagWord(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedTagWord(ISession session, TagWord instance)
            : base(session, instance)
        {

        }

        public override TransitTagWord GetTransitInstance(ManagedSecurityContext sec)
        {
            TransitTagWord t_instance = base.GetTransitInstance(sec);
            t_instance.Frequency = (int) Session.CreateQuery(string.Format(
                "SELECT COUNT(*) FROM TagWord w, TagWordAccount a WHERE w.Id = {0} AND a.TagWord.Id = w.Id",
                t_instance.Id)).UniqueResult();
            return t_instance;
        }

        public static TagWord Find(ISession session, string word)
        {
            return (TagWord)session.CreateCriteria(typeof(TagWord))
                .Add(Expression.Eq("Word", word))
                .UniqueResult();
        }

        public static int FindId(ISession session, string name)
        {
            return Find(session, name).Id;
        }

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            return acl;
        }
    }

    public class ManagedTagWordCollection : StringCollection
    {
        public ManagedTagWordCollection()
        {

        }

        public ManagedTagWordCollection(string rawdata)
        {
            AddData(rawdata);
        }

        public void AddData(string rawdata)
        {
            MatchCollection mc = Regex.Matches(rawdata, @"\w+");
            foreach (Match m in mc)
            {
                AddWord(m.Value);
            }
        }

        public bool AddWord(string word)
        {
            if (word.Length < 3)
                return false;

            word = word.ToLower();

            // todo: sorted unique list of strings
            if (base.Contains(word))
                return false;

            base.Add(word.ToLower());
            return true;
        }
    }
}
