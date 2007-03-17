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

        public ManagedAccountBlogPostTest()
        {

        }

        public override TransitAccountBlogPost GetTransitInstance()
        {
            TransitAccountBlogPost t_instance = new TransitAccountBlogPost();
            t_instance.AccountBlogId = _blog.Instance.Id;
            t_instance.AccountId = _account.Instance.Id;
            t_instance.Body = GetNewString();
            t_instance.Title = GetNewString();
            t_instance.AccountName = _account.Instance.Name;
            return t_instance;
        }

        [Test] // foodcandy bug #424 : Blogs: posts missing poster's picture
        public void AccountBlogPostAccountIdAndNameTest()
        {
            TransitAccountBlogPost t_instance = GetTransitInstance();
            ManagedAccountBlogPost m_instance = new ManagedAccountBlogPost(Session);
            m_instance.CreateOrUpdate(t_instance, GetSecurityContext());
            Session.Flush();
            ManagedAccountBlogPost m_instance1 = new ManagedAccountBlogPost(Session, m_instance.Id);
            TransitAccountBlogPost t_instance1 = m_instance1.GetTransitInstance(GetSecurityContext());
            Assert.AreNotEqual(0, t_instance.AccountId);
            Assert.IsNotEmpty(t_instance.AccountName);
            Assert.IsNotEmpty(t_instance.AccountBlogName);
            m_instance.Delete(GetSecurityContext());
        }
    }
}
