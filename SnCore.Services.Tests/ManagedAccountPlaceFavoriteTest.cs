using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using SnCore.Data.Tests;
using System.Collections;
using NHibernate.Expression;
using SnCore.Tools;
using System.IO;
using System.Text;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountPlaceFavoriteTest : ManagedServiceTest
    {
        [Test]
        public void TestAccountPlaceFavoritesCount()
        {
            IQuery q = Session.CreateQuery(
                "SELECT COUNT(DISTINCT apf.Place) FROM AccountPlaceFavorite apf");
            Console.WriteLine("{0}", q.UniqueResult());
        }
    }
}
