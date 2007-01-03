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
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);

                TransitAccountWebsite ta = new TransitAccountWebsite();
                ta.Name = "My Website";
                ta.Description = "Lots of details.";
                ta.Url = "http://www.dblock.org";

                ManagedAccountWebsite m_a = new ManagedAccountWebsite(Session);
                m_a.CreateOrUpdate(ta, a.GetSecurityContext());
            }
            finally
            {
                a.Delete(a.GetSecurityContext());
            }
        }

        [Test]
        public void CreateAccountWebsiteAccrossAccounts()
        {
            ManagedAccount a = new ManagedAccount(Session);
            ManagedAccount b = new ManagedAccount(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);
                b.Create("Test User 2", "testpassword", "foo2@localhost.com", DateTime.UtcNow, AdminSecurityContext);

                TransitAccountWebsite ta = new TransitAccountWebsite();
                ta.Name = "My Website";
                ta.Description = "Lots of details.";
                ta.Url = "http://www.dblock.org";

                ManagedAccountWebsite m_a = new ManagedAccountWebsite(Session);
                m_a.CreateOrUpdate(ta, a.GetSecurityContext());

                ManagedAccountWebsite m_b = new ManagedAccountWebsite(Session);
                m_b.CreateOrUpdate(ta, b.GetSecurityContext());
            }
            finally
            {
                a.Delete(a.GetSecurityContext());
                b.Delete(b.GetSecurityContext());
            }
        }


        [Test]
        [ExpectedException(typeof(ManagedAccountWebsite.InvalidUriException))]
        public void CreateAccountWebsiteInvalid()
        {
            ManagedAccount a = new ManagedAccount(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);

                TransitAccountWebsite ta = new TransitAccountWebsite();
                ta.Name = "My Website";
                ta.Description = "Lots of details.";
                ta.Url = "<script>attack!</script>";

                ManagedAccountWebsite m_w = new ManagedAccountWebsite(Session);
                m_w.CreateOrUpdate(ta, a.GetSecurityContext());
            }
            finally
            {
                a.Delete(AdminSecurityContext);
            }
        }

    }
}
