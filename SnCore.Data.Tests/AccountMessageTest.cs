using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NUnit.Framework;

namespace SnCore.Data.Tests
{
    [TestFixture]
    public class AccountMessageTest : NHibernateTest
    {
        [Test]
        public void GetOldAccountMessagesTest()
        {
            IQuery q = Session.CreateQuery(string.Format(
                "SELECT message FROM AccountMessage AS message" +
                " INNER JOIN message.AccountMessageFolder as folder" +
                " WHERE folder.System = 1" +
                " AND folder.AccountMessageFolderParent IS NULL" +
                " AND (folder.Name = 'Trash' OR folder.Name = 'Sent')" +
                " AND (message.Sent < '{0}')", DateTime.UtcNow.AddDays(-14)));

            IList<AccountMessage> messages = q.List<AccountMessage>();
            Console.WriteLine("Messages: {0}", messages.Count);
        }
    }
}
