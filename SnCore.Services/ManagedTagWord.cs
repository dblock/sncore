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

    public class TransitTagWord : TransitService
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

        public TransitTagWord(TagWord o)
            : base(o.Id)
        {
            Word = o.Word;
            Promoted = o.Promoted;
            Excluded = o.Excluded;
        }

        public TagWord GetTagWord(ISession session)
        {
            TagWord p = (Id != 0) ? (TagWord)session.Load(typeof(TagWord), Id) : new TagWord();
            p.Word = this.Word;
            p.Promoted = this.Promoted;
            p.Excluded = this.Excluded;
            return p;
        }
    }

    /// <summary>
    /// Managed tag word.
    /// </summary>
    public class ManagedTagWord : ManagedService
    {
        private TagWord mTagWord = null;

        public ManagedTagWord(ISession session)
            : base(session)
        {

        }

        public ManagedTagWord(ISession session, int id)
            : base(session)
        {
            mTagWord = (TagWord)session.Load(typeof(TagWord), id);
        }

        public ManagedTagWord(ISession session, TagWord value)
            : base(session)
        {
            mTagWord = value;
        }

        public ManagedTagWord(ISession session, TransitTagWord value)
            : base(session)
        {
            mTagWord = value.GetTagWord(session);
        }

        public int Id
        {
            get
            {
                return mTagWord.Id;
            }
        }

        public TransitTagWord TransitTagWord
        {
            get
            {
                TransitTagWord word = new TransitTagWord(mTagWord);
                word.Frequency = (int)Session.CreateQuery(string.Format(
                    "SELECT COUNT(w) FROM TagWord w, TagWordAccount a WHERE w.Id = {0} AND a.TagWord.Id = w.Id",
                    word.Id)).UniqueResult();
                return word;
            }
        }

        public void CreateOrUpdate(TransitTagWord o)
        {
            mTagWord = o.GetTagWord(Session);
            Session.Save(mTagWord);
        }

        public void Delete()
        {
            Session.Delete(mTagWord);
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
