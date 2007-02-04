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
    public class ManagedAccountAddressTest : ManagedServiceTest
    {
        public ManagedAccountAddressTest()
        {

        }

        [Test]
        public void CreateAccountAddress()
        {
            ManagedAccount a = new ManagedAccount(Session);
            ManagedCountry c = new ManagedCountry(Session);
            ManagedState s = new ManagedState(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);

                TransitCountry tc = new TransitCountry();
                tc.Name = GetNewString();
                TransitState ts = new TransitState();
                ts.Name = GetNewString();
                ts.Country = tc.Name;

                c.CreateOrUpdate(tc, AdminSecurityContext);
                s.CreateOrUpdate(ts, AdminSecurityContext);
                
                TransitAccountAddress ta = new TransitAccountAddress();
                ta.Apt = "123";
                ta.City = "New York";
                ta.Country = tc.Name;
                ta.Name = "My Address";
                ta.State = ts.Name;
                ta.Street = "Houston St.";
                ta.Zip = "10001";

                ManagedAccountAddress m_a = new ManagedAccountAddress(Session);
                m_a.CreateOrUpdate(ta, new ManagedSecurityContext(a.Instance));
            }
            finally
            {
                try
                {
                    a.Delete(AdminSecurityContext);
                    s.Delete(AdminSecurityContext);
                    c.Delete(AdminSecurityContext);
                }
                catch
                {
                }
            }
        }
    }
}
