using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedMadLibInstanceTest : ManagedCRUDTest<MadLibInstance, TransitMadLibInstance, ManagedMadLibInstance>
    {
        private ManagedAccountTest _account = new ManagedAccountTest();
        private ManagedMadLibTest _madlib = new ManagedMadLibTest();
        private ManagedDataObjectTest _object = new ManagedDataObjectTest();

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _account.SetUp();
            _madlib.SetUp();
            _object.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _object.TearDown();
            _madlib.TearDown();
            _account.TearDown();
            base.TearDown();
        }



        public ManagedMadLibInstanceTest()
        {

        }

        public override TransitMadLibInstance GetTransitInstance()
        {
            TransitMadLibInstance t_instance = new TransitMadLibInstance();
            t_instance.AccountId = _account.Instance.Id;
            t_instance.MadLibId = _madlib.Instance.Id;
            t_instance.ObjectId = _object.Instance.Id;
            t_instance.ObjectName = _object.Instance.Name;
            t_instance.ObjectUri = GetNewUri();
            t_instance.Text = GetNewString();
            return t_instance;
        }
    }
}
