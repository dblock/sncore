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
            acl.Check(new ManagedSecurityContext(null), DataOperation.Create);
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
            acl.Check(new ManagedSecurityContext(null), DataOperation.Create);
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
    }
}
