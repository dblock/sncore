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
    public class TransitFeedType : TransitService<FeedType>
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
            : base(o)
        {

        }

        public override void SetInstance(FeedType value)
        {
            Name = value.Name;
            Xsl = value.Xsl;
            SpanRows = value.SpanRows;
            SpanColumns = value.SpanColumns;
            SpanRowsPreview = value.SpanRowsPreview;
            SpanColumnsPreview = value.SpanColumnsPreview;
            base.SetInstance(value);
        }

        public override FeedType GetInstance(ISession session, ManagedSecurityContext sec)
        {
            FeedType instance = base.GetInstance(session, sec);
            instance.Name = this.Name;
            instance.SpanColumns = this.SpanColumns;
            instance.SpanRows = this.SpanRows;
            instance.SpanColumnsPreview = this.SpanColumnsPreview;
            instance.SpanRowsPreview = this.SpanRowsPreview;
            instance.Xsl = this.Xsl;
            return instance;
        }
    }

    public class ManagedFeedType : ManagedService<FeedType, TransitFeedType>
    {
        public ManagedFeedType()
        {

        }

        public ManagedFeedType(ISession session)
            : base(session)
        {

        }

        public ManagedFeedType(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedFeedType(ISession session, FeedType value)
            : base(session, value)
        {

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

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLEveryoneAllowRetrieve());
            return acl;
        }
    }
}
