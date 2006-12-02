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
    public class TransitFeedType : TransitService
    {
        private string mName;

        public string Name
        {
            get
            {

                return mName;
            }
            set
            {
                mName = value;
            }
        }

        private string mXsl;

        public string Xsl
        {
            get
            {

                return mXsl;
            }
            set
            {
                mXsl = value;
            }
        }

        private int mSpanRows;

        public int SpanRows
        {
            get
            {

                return mSpanRows;
            }
            set
            {
                mSpanRows = value;
            }
        }

        private int mSpanColumns;

        public int SpanColumns
        {
            get
            {

                return mSpanColumns;
            }
            set
            {
                mSpanColumns = value;
            }
        }

        private int mSpanRowsPreview;

        public int SpanRowsPreview
        {
            get
            {

                return mSpanRowsPreview;
            }
            set
            {
                mSpanRowsPreview = value;
            }
        }

        private int mSpanColumnsPreview;

        public int SpanColumnsPreview
        {
            get
            {

                return mSpanColumnsPreview;
            }
            set
            {
                mSpanColumnsPreview = value;
            }
        }

        public TransitFeedType()
        {

        }

        public TransitFeedType(FeedType o)
            : base(o.Id)
        {
            Name = o.Name;
            Xsl = o.Xsl;
            SpanRows = o.SpanRows;
            SpanColumns = o.SpanColumns;
            SpanRowsPreview = o.SpanRowsPreview;
            SpanColumnsPreview = o.SpanColumnsPreview;
        }

        public FeedType GetFeedType(ISession session)
        {
            FeedType p = (Id != 0) ? (FeedType)session.Load(typeof(FeedType), Id) : new FeedType();
            p.Name = this.Name;
            p.SpanColumns = this.SpanColumns;
            p.SpanRows = this.SpanRows;
            p.SpanColumnsPreview = this.SpanColumnsPreview;
            p.SpanRowsPreview = this.SpanRowsPreview;
            if (!string.IsNullOrEmpty(this.Xsl)) p.Xsl = this.Xsl;
            return p;
        }
    }

    public class ManagedFeedType : ManagedService
    {
        private FeedType mFeedType = null;

        public ManagedFeedType(ISession session)
            : base(session)
        {

        }

        public ManagedFeedType(ISession session, int id)
            : base(session)
        {
            mFeedType = (FeedType)session.Load(typeof(FeedType), id);
        }

        public ManagedFeedType(ISession session, FeedType value)
            : base(session)
        {
            mFeedType = value;
        }

        public ManagedFeedType(ISession session, TransitFeedType value)
            : base(session)
        {
            mFeedType = value.GetFeedType(session);
        }

        public int Id
        {
            get
            {
                return mFeedType.Id;
            }
        }

        public TransitFeedType TransitFeedType
        {
            get
            {
                return new TransitFeedType(mFeedType);
            }
        }

        public void CreateOrUpdate(TransitFeedType o)
        {
            mFeedType = o.GetFeedType(Session);
            Session.Save(mFeedType);
        }

        public void Delete()
        {
            Session.Delete(mFeedType);
        }

        public static FeedType Find(ISession session, string name)
        {
            return (FeedType)session.CreateCriteria(typeof(FeedType))
                .Add(Expression.Eq("Name", name))
                .UniqueResult();
        }

        public static int FindId(ISession session, string name)
        {
            return Find(session, name).Id;
        }

        public static FeedType GetDefaultFeedType(ISession session)
        {
            FeedType result = null;
            IList feedtypes = session.CreateCriteria(typeof(FeedType)).List();
            foreach(FeedType feedtype in feedtypes)
            {
                if (result == null || feedtype.Name.Contains("RSS"))
                    result = feedtype;
            }
            return result;
        }
    }
}
