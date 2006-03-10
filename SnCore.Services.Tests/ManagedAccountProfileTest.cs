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
    public class ManagedAccountProfileTest : NHibernateTest
    {
        public ManagedAccountProfileTest()
        {

        }

        [Test]
        public void CreateAccountProfile()
        {
            ManagedAccount a = new ManagedAccount(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.Now.ToUniversalTime());

                TransitAccountProfile ta = new TransitAccountProfile();
                ta.Name = "My Profile";
                ta.Details = "Lots of details.";

                a.AddAccountProfile(ta);
            }
            finally
            {
                a.Delete();
            }
        }
    }
}
