using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using SnCore.Data.Tests;
using System.Collections;
using NHibernate.Expression;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountStoryTest : ManagedCRUDTest<AccountStory, TransitAccountStory, ManagedAccountStory>
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

        public override TransitAccountStory GetTransitInstance()
        {
            TransitAccountStory t_instance = new TransitAccountStory();
            t_instance.AccountId = _account.Instance.Id;
            t_instance.Name = GetNewString();
            t_instance.Summary = GetNewString();
            return t_instance;
        }

        public ManagedAccountStoryTest()
        {

        }

        [Test]
        public void CreateAccountStory()
        {
            ManagedAccount a = new ManagedAccount(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);
                a.VerifyAllEmails();
                a.AddDefaultPicture();

                TransitAccountStory s = new TransitAccountStory();
                s.Name = GetNewString();
                s.Summary = GetNewString();
                ManagedAccountStory m_s = new ManagedAccountStory(Session);
                m_s.CreateOrUpdate(s, a.GetSecurityContext());
            }
            finally
            {
                a.Delete(AdminSecurityContext);
                Session.Flush();
            }
        }

        [Test]
        public void CreateAccountStoryPicture()
        {
            ManagedAccount a = new ManagedAccount(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);
                a.VerifyAllEmails();
                a.AddDefaultPicture();

                TransitAccountStory s = new TransitAccountStory();
                s.Name = GetNewString();
                s.Summary = GetNewString();
                ManagedAccountStory ms = new ManagedAccountStory(Session);
                int story_id = ms.CreateOrUpdate(s, a.GetSecurityContext());
                TransitAccountStoryPicture p = new TransitAccountStoryPicture();
                p.Name = GetNewString();
                p.AccountStoryId = story_id;
                ManagedAccountStoryPicture mp = new ManagedAccountStoryPicture(Session);
                mp.CreateOrUpdate(p, a.GetSecurityContext());
            }
            finally
            {
                a.Delete(AdminSecurityContext);
                Session.Flush();
            }
        }
    }
}
