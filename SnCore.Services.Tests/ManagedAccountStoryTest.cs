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
    public class ManagedAccountStoryTest : ManagedServiceTest
    {
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

                TransitAccountStory s = new TransitAccountStory();
                s.Name = Guid.NewGuid().ToString();
                s.Summary = Guid.NewGuid().ToString();
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

                TransitAccountStory s = new TransitAccountStory();
                s.Name = Guid.NewGuid().ToString();
                s.Summary = Guid.NewGuid().ToString();
                ManagedAccountStory ms = new ManagedAccountStory(Session);
                int story_id = ms.CreateOrUpdate(s, a.GetSecurityContext());
                TransitAccountStoryPicture p = new TransitAccountStoryPicture();
                p.Name = Guid.NewGuid().ToString();
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
