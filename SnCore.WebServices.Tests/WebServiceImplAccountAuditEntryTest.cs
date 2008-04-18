using System;
using System.Collections.Generic;
using System.Text;
using SnCore.Data;
using SnCore.Services;
using NUnit.Framework;
using SnCore.Data.Hibernate;
using SnCore.Data.Tests;

namespace SnCore.WebServices.Tests
{
    [TestFixture]
    public class WebServiceImplAccountAuditEntryTest : NHibernateTest 
    {
        public WebServiceImplAccountAuditEntryTest()
        {

        }

        [Test]
        public void GetAccountFriendAuditEntriesCountTest()
        {
            string ticket = ManagedAccount.GetAdminTicket(Session);
            int id = ManagedAccount.GetAdminAccount(Session).Id;
            int count = WebServiceImpl<TransitAccountAuditEntry, ManagedAccountAuditEntry, AccountAuditEntry>.GetSQLCount(
                ticket, string.Format(
                    "INNER JOIN AccountFriend AccountFriend ON ( " +
                        "AccountAuditEntry.Account_Id = AccountFriend.Account_Id OR AccountAuditEntry.Account_Id = AccountFriend.Keen_Id " +
                        ") WHERE ( " +
                        " ( AccountFriend.Account_Id = {0} OR AccountFriend.Keen_Id = {0} )" +
                        " AND AccountAuditEntry.Account_Id <> {0}" +
                        ")", id));

            Console.WriteLine("Count: {0}", count);

            List<TransitAccountAuditEntry> rs = WebServiceImpl<TransitAccountAuditEntry, ManagedAccountAuditEntry, AccountAuditEntry>.GetList(
                ticket, null, "SELECT {AccountAuditEntry.*} FROM AccountAuditEntry {AccountAuditEntry} " + string.Format(
                    "INNER JOIN AccountFriend AccountFriend ON ( " +
                        "AccountAuditEntry.Account_Id = AccountFriend.Account_Id OR AccountAuditEntry.Account_Id = AccountFriend.Keen_Id " +
                        ") WHERE ( " +
                        " ( AccountFriend.Account_Id = {0} OR AccountFriend.Keen_Id = {0} )" +
                        " AND AccountAuditEntry.Account_Id <> {0}" +
                        ")", id), "AccountAuditEntry");

            Console.WriteLine("Result Set: {0}", rs.Count);
        }
    }
}
