using System;
using System.Collections.Generic;
using System.Text;
using SnCore.Data.Hibernate;
using NUnit.Framework;
using SnCore.Data.Tests;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountAuditEntryCollectionTest : NHibernateTest
    {
        [Test]
        public void TestEmptyCollection()
        {
            ManagedAccountAuditEntryCollection coll = new ManagedAccountAuditEntryCollection();
            IList<string> strings = coll.GetAccountAuditStrings();
            Assert.IsNotNull(strings);
            Assert.AreEqual(0, strings.Count);
        }

        [Test]
        public void TestOneItemCollection()
        {
            ManagedAccountAuditEntryCollection coll = new ManagedAccountAuditEntryCollection();
            coll.MessageFormat = "[user:xyz] posted {0} in [feed:zyx]";
            coll.Add(Guid.NewGuid().ToString());
            IList<string> strings = coll.GetAccountAuditStrings();
            Assert.IsNotNull(strings);
            Console.WriteLine(strings[0]);
            Assert.AreEqual(1, strings.Count);
        }

        [Test]
        public void TestSeveralItemCollection()
        {
            ManagedAccountAuditEntryCollection coll = new ManagedAccountAuditEntryCollection();
            coll.MessageFormat = "[user:xyz] posted {0} in [feed:zyx]";
            const int max = 5;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < max; i++)
            {
                string guid = Guid.NewGuid().ToString();
                if ((i != 0) && (i + 1 < max)) sb.Append(coll.Delimiter);
                else if (i != 0) sb.Append(coll.LastDelimiter);
                sb.Append(guid);
                coll.Add(guid);
            }
            string expected = string.Format(coll.MessageFormat, sb);
            IList<string> strings = coll.GetAccountAuditStrings();
            Assert.IsNotNull(strings);
            Assert.AreEqual(1, strings.Count);
            Console.WriteLine(strings[0]);
            Assert.AreEqual(expected, strings[0]);
        }

        [Test]
        public void TestMultipleItemCollection()
        {
            ManagedAccountAuditEntryCollection coll = new ManagedAccountAuditEntryCollection();
            coll.MessageFormat = "[user:xyz] posted {0} in [feed:zyx]";
            const int max = 23;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < max; i++)
            {
                string guid = Guid.NewGuid().ToString();
                coll.Add(guid);
            }
            IList<string> strings = coll.GetAccountAuditStrings();
            Assert.IsNotNull(strings);
            foreach (string s in strings)
            {
                Console.WriteLine(s);
                Assert.IsTrue(s.Length < ManagedAccountAuditEntryCollection.MaxMessageLength);
            }
        }

        [Test]
        public void TestSetMaxMessageLength()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ManagedAccountAuditEntryCollection.SetMaxMessageLength();
                Console.WriteLine("Max length: {0}", ManagedAccountAuditEntryCollection.MaxMessageLength);
                DomainClass dc = SnCore.Data.Hibernate.Session.Model["AccountAuditEntry"];
                Assert.AreEqual(dc["Description"].MaxLengthInChars, ManagedAccountAuditEntryCollection.MaxMessageLength);
            }
        }
    }
}
