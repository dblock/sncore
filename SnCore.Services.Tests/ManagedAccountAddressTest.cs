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
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow);

                TransitCountry tc = new TransitCountry();
                tc.Name = Guid.NewGuid().ToString();
                TransitState ts = new TransitState();
                ts.Name = Guid.NewGuid().ToString();
                ts.Country = tc.Name;

                c.Create(tc);
                s.Create(ts);
                
                TransitAccountAddress ta = new TransitAccountAddress();
                ta.Apt = "123";
                ta.City = "New York";
                ta.Country = tc.Name;
                ta.Name = "My Address";
                ta.State = ts.Name;
                ta.Street = "Houston St.";
                ta.Zip = "10001";

                a.CreateOrUpdate(ta);
            }
            finally
            {
                try
                {
                    a.Delete();
                    s.Delete();
                    c.Delete();
                }
                catch
                {
                }
            }
        }
    }
}
