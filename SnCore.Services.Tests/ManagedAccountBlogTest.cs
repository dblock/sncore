using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountBlogTest : ManagedCRUDTest<AccountBlog, TransitAccountBlog, ManagedAccountBlog>
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

        public ManagedAccountBlogTest()
        {

        }

        public override TransitAccountBlog GetTransitInstance()
        {
            TransitAccountBlog t_instance = new TransitAccountBlog();
            t_instance.Name = GetNewString();
            t_instance.AccountId = _account.Instance.Id;
            t_instance.Description = GetNewString();
            return t_instance;
        }
    }
}
