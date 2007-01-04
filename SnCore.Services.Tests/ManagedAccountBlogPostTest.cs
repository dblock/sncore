using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountBlogPostTest : ManagedCRUDTest<AccountBlogPost, TransitAccountBlogPost, ManagedAccountBlogPost>
    {
        private ManagedAccountBlogTest _blog = new ManagedAccountBlogTest();
        private ManagedAccountTest _account = new ManagedAccountTest();

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _account.SetUp();
            _blog.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _blog.TearDown();
            _account.TearDown();
            base.TearDown();
        }

        public ManagedAccountBlogPostTest()
        {

        }

        public override TransitAccountBlogPost GetTransitInstance()
        {
            TransitAccountBlogPost t_instance = new TransitAccountBlogPost();
            t_instance.AccountBlogId = _blog.Instance.Id;
            t_instance.AccountId = _account.Instance.Id;
            t_instance.Body = Guid.NewGuid().ToString();
            t_instance.Title = Guid.NewGuid().ToString();
            t_instance.AccountName = _account.Instance.Name;
            return t_instance;
        }
    }
}
