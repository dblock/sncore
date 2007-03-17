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
            _account.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _account.TearDown();
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
