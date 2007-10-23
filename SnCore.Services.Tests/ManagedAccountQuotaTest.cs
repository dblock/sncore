using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountQuotaTest : ManagedCRUDTest<AccountQuota, TransitAccountQuota, ManagedAccountQuota>
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        public ManagedAccountQuotaTest()
        {

        }

        public override TransitAccountQuota GetTransitInstance()
        {
            TransitAccountQuota t_instance = new TransitAccountQuota();
            t_instance.DataObjectName = typeof(Place).Name;
            t_instance.Limit = 123; 
            return t_instance;
        }
    }
}
