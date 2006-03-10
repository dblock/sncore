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
    public class ManagedAccountStoryTest : NHibernateTest
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
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow);

                TransitAccountStory s = new TransitAccountStory();
                s.Name = Guid.NewGuid().ToString();
                s.Summary = Guid.NewGuid().ToString();
                a.CreateOrUpdate(s);                
            }
            finally
            {
                a.Delete();
                Session.Flush();
            }
        }

        [Test]
        public void CreateAccountStoryPicture()
        {
            ManagedAccount a = new ManagedAccount(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow);

                TransitAccountStory s = new TransitAccountStory();
                s.Name = Guid.NewGuid().ToString();
                s.Summary = Guid.NewGuid().ToString();
                ManagedAccountStory ms = new ManagedAccountStory(Session, a.CreateOrUpdate(s));
                TransitAccountStoryPicture p = new TransitAccountStoryPicture();
                p.Name = Guid.NewGuid().ToString();

                ms.AddAccountStoryPicture(p);
            }
            finally
            {
                a.Delete();
                Session.Flush();
            }
        }

    }
}
