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
    public class ManagedAccountMessageFolderTest : ManagedCRUDTest<AccountMessageFolder, TransitAccountMessageFolder, ManagedAccountMessageFolder>
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

        public override TransitAccountMessageFolder GetTransitInstance()
        {
            TransitAccountMessageFolder t_instance = new TransitAccountMessageFolder();
            t_instance.AccountId = _account.Instance.Id;
            t_instance.Name = Guid.NewGuid().ToString(); ;
            t_instance.System = false;
            return t_instance;
        }

        public ManagedAccountMessageFolderTest()
        {

        }

        [Test]
        public void CreateAccountMessageFolder()
        {
            ManagedAccount a = new ManagedAccount(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);

                TransitAccountMessageFolder tf = new TransitAccountMessageFolder();
                tf.Name = Guid.NewGuid().ToString();
                tf.System = false;

                ManagedAccountMessageFolder m_f = new ManagedAccountMessageFolder(Session);
                m_f.CreateOrUpdate(tf, AdminSecurityContext);
            }
            finally
            {
                a.Delete(AdminSecurityContext);
            }
        }
    }
}
