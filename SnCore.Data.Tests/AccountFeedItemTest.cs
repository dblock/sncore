using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Expression;
using System.Collections.Generic;
using SnCore.Data.Hibernate;
using NHibernate.Transform;

namespace SnCore.Data.Tests
{
    [TestFixture]
    public class AccountFeedItemTest : NHibernateTest
    {
        public AccountFeedItemTest()
        {

        }

        [Test]
        public void TestGetAccountFeedItemFeaturesByAccountFeedId()
        {
            IList<AccountFeed> feeds = Session.CreateCriteria(typeof(AccountFeed))
                .SetMaxResults(5)
                .List<AccountFeed>();

            foreach(AccountFeed feed in feeds)
            {
                Console.WriteLine("Feed: {0}", feed.Name);

                IQuery query = Session.GetNamedQuery("GetAccountFeedItemFeaturesByAccountFeedId")
                    .SetInt32("AccountFeed_Id", feed.Id);

                IList<Feature> features = query.List<Feature>();
                Console.WriteLine("Features: {0}", features.Count);

                foreach (Feature feature in features)
                {
                    Console.WriteLine(string.Format("Feature {0}: {1}", feature.Id, feature.DataRowId));
                }
            }
        }
    }
}
