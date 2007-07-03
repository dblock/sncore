using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SnCore.Data.Tests;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedQuotaTest : NHibernateTest
    {
        [Test]
        public void TestNoQuota()
        {
            List<Guid> lotsofitems = new List<Guid>();
            for (int i = 0; i < ManagedQuota.DefaultQuotaUpperLimit + 1; i++)
                lotsofitems.Add(Guid.NewGuid());
            ManagedQuota.GetDefaultNoQuota().Check<Guid, ManagedAccount.QuotaExceededException>(lotsofitems);
        }

        [Test, ExpectedException(typeof(ManagedAccount.QuotaExceededException))]
        public void TestHasQuota()
        {
            List<Guid> lotsofitems = new List<Guid>();
            for (int i = 0; i < ManagedQuota.DefaultQuotaUpperLimit + 1; i++)
                lotsofitems.Add(Guid.NewGuid());
            ManagedQuota.GetDefaultEnabledQuota().Check<Guid, ManagedAccount.QuotaExceededException>(lotsofitems);
        }
    }
}
