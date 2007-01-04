using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedTagWordAccountTest : ManagedCRUDTest<TagWordAccount, TransitTagWordAccount, ManagedTagWordAccount>
    {
        private ManagedTagWordTest _tagword = new ManagedTagWordTest();
        private ManagedAccountTest _account = new ManagedAccountTest();

        public ManagedTagWordAccountTest()
        {

        }

        protected override ManagedSecurityContext GetSecurityContext()
        {
            return AdminSecurityContext;
        }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _account.SetUp();
            _tagword.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _tagword.TearDown();
            _tagword.TearDown();
            base.TearDown();
        }

        public override TransitTagWordAccount GetTransitInstance()
        {
            TransitTagWordAccount t_instance = new TransitTagWordAccount();
            t_instance.AccountId = _account.Instance.Id;
            t_instance.Word = _tagword.Instance.Instance.Word;
            return t_instance;
        }
    }
}
