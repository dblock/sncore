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
    public class AccountCityTest : NHibernateTest
    {
        public AccountCityTest()
        {

        }

        [Test]
        public void TestGetAccountCities()
        {
            IQuery query = Session.GetNamedQuery("GetAccountCities");

            IList<AccountCity> accountcities = query
                .SetResultTransformer(Transformers.AliasToBean(typeof(AccountCity)))
                .List<AccountCity>();

            Console.WriteLine("AccountCities: {0}", accountcities.Count);

            foreach (AccountCity city in accountcities)
            {
                Console.WriteLine(city.City);
            }
        }
    }
}
