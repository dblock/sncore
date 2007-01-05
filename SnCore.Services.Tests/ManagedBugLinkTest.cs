using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedBugLinkTest : ManagedCRUDTest<BugLink, TransitBugLink, ManagedBugLink>
    {
        private ManagedBugTest _bug1 = new ManagedBugTest();
        private ManagedBugTest _bug2 = new ManagedBugTest();

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _bug1.SetUp();
            _bug2.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _bug2.TearDown();
            _bug2.TearDown();
            base.TearDown();
        }

        public ManagedBugLinkTest()
        {

        }

        public override TransitBugLink GetTransitInstance()
        {
            TransitBugLink t_instance = new TransitBugLink();
            t_instance.BugId = _bug1.Instance.Id;
            t_instance.RelatedBugId = _bug2.Instance.Id;
            return t_instance;
        }
    }
}
