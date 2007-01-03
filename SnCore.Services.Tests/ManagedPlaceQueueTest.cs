using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using SnCore.Data.Tests;
using System.Collections;
using NHibernate.Expression;
using System.Diagnostics;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedPlaceQueueTest : ManagedServiceTest
    {
        public ManagedPlaceQueueTest()
        {

        }

        [Test]
        public void CreatePlaceQueue()
        {
            ManagedAccount a = new ManagedAccount(Session);
            PlaceQueue q = null;

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);
                TransitPlaceQueue t_q = new TransitPlaceQueue();

                t_q.AccountId = a.Id;
                t_q.Name = Guid.NewGuid().ToString();
                t_q.PublishAll = false;
                t_q.PublishFriends = true;

                q = t_q.GetInstance(Session, a.GetSecurityContext());
                q.Created = q.Modified = DateTime.UtcNow;
                Session.Save(q);
                Session.Flush();

                Assert.IsTrue(q.Id > 0, "Place queue not commited");
            }
            finally
            {
                if (q != null) Session.Delete(q);
                a.Delete(AdminSecurityContext);
            }
        }
    }
}
