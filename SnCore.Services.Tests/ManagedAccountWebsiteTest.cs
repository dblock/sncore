using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using SnCore.Data.Tests;
using System.Collections;
using NHibernate.Expression;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountWebsiteTest : ManagedServiceTest
    {
        public ManagedAccountWebsiteTest()
        {

        }

        [Test]
        public void CreateAccountWebsite()
        {
            ManagedAccount a = new ManagedAccount(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow);

                TransitAccountWebsite ta = new TransitAccountWebsite();
                ta.Name = "My Website";
                ta.Description = "Lots of details.";
                ta.Url = "http://www.dblock.org";

                a.CreateOrUpdate(ta);
            }
            finally
            {
                a.Delete();
            }
        }

        [Test]
        public void CreateAccountWebsiteAccrossAccounts()
        {
            ManagedAccount a = new ManagedAccount(Session);
            ManagedAccount b = new ManagedAccount(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow);
                b.Create("Test User 2", "testpassword", "foo2@localhost.com", DateTime.UtcNow);

                TransitAccountWebsite ta = new TransitAccountWebsite();
                ta.Name = "My Website";
                ta.Description = "Lots of details.";
                ta.Url = "http://www.dblock.org";

                a.CreateOrUpdate(ta);
                b.CreateOrUpdate(ta);
            }
            finally
            {
                a.Delete();
                b.Delete();
            }
        }


        [Test]
        [ExpectedException(typeof(ManagedAccountWebsite.InvalidUriException))]
        public void CreateAccountWebsiteInvalid()
        {
            ManagedAccount a = new ManagedAccount(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow);

                TransitAccountWebsite ta = new TransitAccountWebsite();
                ta.Name = "My Website";
                ta.Description = "Lots of details.";
                ta.Url = "<script>attack!</script>";

                a.CreateOrUpdate(ta);
            }
            finally
            {
                a.Delete();
            }
        }

    }
}
