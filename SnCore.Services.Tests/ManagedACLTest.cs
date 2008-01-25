using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SnCore.Data.Tests;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedACLTest : NHibernateTest
    {
        [Test, ExpectedException(typeof(ManagedAccount.AccessDeniedException))]
        public void TestBlankACLNoAccount()
        {
            ACL acl = new ACL();
            acl.Check(new ManagedSecurityContext((Account) null), DataOperation.Create);
        }

        [Test, ExpectedException(typeof(ManagedAccount.AccessDeniedException))]
        public void TestBlankACLNonAdminAccount()
        {
            ACL acl = new ACL();
            acl.Check(new ManagedSecurityContext(new Account()), DataOperation.Create);
        }

        [Test, ExpectedException(typeof(ManagedAccount.AccessDeniedException))]
        public void TestBlankACLAdminAccount()
        {
            ACL acl = new ACL();
            acl.Check(new ManagedSecurityContext(ManagedAccount.GetAdminAccount(Session)), DataOperation.Create);
        }

        [Test, ExpectedException(typeof(ManagedAccount.AccessDeniedException))]
        public void TestBlankACLInvalidAccount()
        {
            ACL acl = new ACL();
            acl.Add(new ACLAccount(new Account(), DataOperation.All));
            acl.Check(new ManagedSecurityContext((Account) null), DataOperation.Create);
        }

        [Test]
        public void TestBlankACLValidAccount()
        {
            ACL acl = new ACL();
            Account account = new Account();
            acl.Add(new ACLAccount(account, DataOperation.All));
            acl.Check(new ManagedSecurityContext(account), DataOperation.Create);
        }

        [Test, ExpectedException(typeof(ManagedAccount.AccessDeniedException))]
        public void TestACLEveryoneRetreiveOnCreate()
        {
            ACL acl = new ACL();
            acl.Add(new ACLEveryoneAllowRetrieve());
            acl.Check(new ManagedSecurityContext(new Account()), DataOperation.Create);
        }

        [Test]
        public void TestACLEveryoneRetreive()
        {
            ACL acl = new ACL();
            acl.Add(new ACLEveryoneAllowRetrieve());
            acl.Check(new ManagedSecurityContext(new Account()), DataOperation.Retreive);
        }

        [Test]
        public void TestAdminACL()
        {
            // make sure that the administrative ACL is a cloned fresh copy 
            // (so that more ACLs can be added)
            ACL acl = ACL.GetAdministrativeACL(Session);
            int count1 = acl.Count;
            acl.Add(new ACLAuthenticatedAllowCreate());
            ACL acl2 = ACL.GetAdministrativeACL(Session);
            Assert.AreNotEqual(acl, acl2);
            Assert.AreEqual(count1, acl2.Count);
        }
    }
}
