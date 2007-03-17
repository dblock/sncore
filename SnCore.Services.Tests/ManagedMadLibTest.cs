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
            _account.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _account.TearDown();
        }

        public ManagedMadLibTest()
        {

        }

        public override TransitMadLib GetTransitInstance()
        {
            TransitMadLib t_instance = new TransitMadLib();
            t_instance.AccountId = _account.Instance.Id;
            t_instance.Name = GetNewString();
            t_instance.Template = GetNewString();
            return t_instance;
        }
    }
}
