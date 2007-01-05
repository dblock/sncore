using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedMadLibTest : ManagedCRUDTest<MadLib, TransitMadLib, ManagedMadLib>
    {
        private ManagedAccountTest _account = new ManagedAccountTest();

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _account.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _account.TearDown();
            base.TearDown();
        }

        public ManagedMadLibTest()
        {

        }

        public override TransitMadLib GetTransitInstance()
        {
            TransitMadLib t_instance = new TransitMadLib();
            t_instance.AccountId = _account.Instance.Id;
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.Template = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}
