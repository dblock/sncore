using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountGroupAccountTest : ManagedCRUDTest<AccountGroupAccount, TransitAccountGroupAccount, ManagedAccountGroupAccount>
    {
        private ManagedAccountTest _account = new ManagedAccountTest();
        private ManagedAccountGroupTest _group = new ManagedAccountGroupTest();

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _account.SetUp();
            _group.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _group.SetUp();
            _account.TearDown();
            base.TearDown();
        }

        public ManagedAccountGroupAccountTest()
        {

        }

        public override TransitAccountGroupAccount GetTransitInstance()
        {
            TransitAccountGroupAccount t_instance = new TransitAccountGroupAccount();
            t_instance.AccountGroupId = _group.Instance.Id;
            t_instance.AccountId = _account.Instance.Id;
            t_instance.IsAdministrator = true;
            return t_instance;
        }

        [Test]
        public void PromoteDemoteTest()
        {
            TransitAccountGroupAccount t_instance = GetTransitInstance();
            ManagedAccountGroupAccount m_account = new ManagedAccountGroupAccount(Session);
            t_instance.IsAdministrator = false                     ;
            t_instance.Id = m_account.CreateOrUpdate(t_instance, AdminSecurityContext);
            ManagedAccountGroupAccount m_account_copy1 = new ManagedAccountGroupAccount(Session, t_instance.Id);
            Assert.IsFalse(m_account_copy1.Instance.IsAdministrator);
            t_instance.IsAdministrator = true;
            m_account.CreateOrUpdate(t_instance, AdminSecurityContext);
            ManagedAccountGroupAccount m_account_copy2 = new ManagedAccountGroupAccount(Session, t_instance.Id);
            Assert.IsTrue(m_account_copy2.Instance.IsAdministrator);
            m_account.Delete(AdminSecurityContext);
        }

        [Test, ExpectedException(typeof(Exception))]
        public void DemoteLastGroupAdministratorTest()
        {
            ManagedAccountGroup m_group = new ManagedAccountGroup(Session, _group.Instance.Id);
            Assert.AreEqual(1, m_group.Instance.AccountGroupAccounts.Count);
            ManagedAccountGroupAccount m_account = new ManagedAccountGroupAccount(Session, m_group.Instance.AccountGroupAccounts[0]);
            m_account.Delete(AdminSecurityContext);
        }
    }
}
