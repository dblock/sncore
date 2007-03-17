using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountBlogAuthorTest : ManagedCRUDTest<AccountBlogAuthor, TransitAccountBlogAuthor, ManagedAccountBlogAuthor>
    {
        private ManagedAccountTest _account = new ManagedAccountTest();
        private ManagedAccountBlogTest _blog = new ManagedAccountBlogTest();

        [SetUp]
        public override void SetUp()
        {
            _account.SetUp();
            _blog.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _blog.TearDown();
            _account.TearDown();
        }

        public ManagedAccountBlogAuthorTest()
        {

        }

        public override TransitAccountBlogAuthor GetTransitInstance()
        {
            TransitAccountBlogAuthor t_instance = new TransitAccountBlogAuthor();
            t_instance.AccountBlogId = _blog.Instance.Id;
            t_instance.AccountId = _account.Instance.Id;
            t_instance.AllowDelete = t_instance.AllowEdit = t_instance.AllowPost = true;
            return t_instance;
        }
    }
}
