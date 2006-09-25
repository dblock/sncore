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
    public class ManagedAccountEmailMessageTest : NHibernateTest
    {
        public ManagedAccountEmailMessageTest()
        {

        }

        [Test]
        public void CreateAccountEmailMessage()
        {
            ManagedAccount a = new ManagedAccount(Session);
            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow);
                a.SendAccountEmailMessage("foo@localhost.com", "bar@localhost.com", "subject", "body", true);
            }
            finally
            {
                a.Delete();
            }
        }
    }
}
