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
    public class ManagedPlaceQueueTest : ManagedCRUDTest<PlaceQueue, TransitPlaceQueue, ManagedPlaceQueue>
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

        public ManagedPlaceQueueTest()
        {

        }

        public override TransitPlaceQueue GetTransitInstance()
        {
            TransitPlaceQueue t_instance = new TransitPlaceQueue();
            t_instance.AccountId = _account.Instance.Id;
            t_instance.Description = GetNewString();
            t_instance.Name = GetNewString();
            return t_instance;
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
                t_q.Name = GetNewString();
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
