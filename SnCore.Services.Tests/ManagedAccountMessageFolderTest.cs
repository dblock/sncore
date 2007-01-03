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
    public class ManagedAccountMessageFolderTest : ManagedServiceTest
    {
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
