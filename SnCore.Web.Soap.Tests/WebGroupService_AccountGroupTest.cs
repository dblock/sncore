using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebGroupServiceTests
{
    [TestFixture]
    public class AccountGroupTest : WebServiceTest<WebGroupService.TransitAccountGroup, WebGroupServiceNoCache>
    {
        public AccountGroupTest()
            : base("AccountGroup")
        {

        }

        public override WebGroupService.TransitAccountGroup GetTransitInstance()
        {
            WebGroupService.TransitAccountGroup t_instance = new WebGroupService.TransitAccountGroup();
            t_instance.Name = GetNewString();
            t_instance.IsPrivate = false;
            t_instance.Description = GetNewString();
            return t_instance;
        }

        [Test]
        public void CreateAccountGroupCreatesAccountGroupAccountTest()
        {
            // test that creating an account group automatically creates at least one admin account
            WebGroupService.TransitAccountGroup t_instance = GetTransitInstance();
            int id = Create(GetAdminTicket(), t_instance);
            WebGroupService.TransitAccountGroupAccount[] groupaccounts = EndPoint.GetAccountGroupAccounts(
                GetAdminTicket(), id, null);
            Assert.AreEqual(1, groupaccounts.Length);
            Assert.AreEqual(groupaccounts[0].AccountId, GetAdminAccount().Id);
            Assert.IsTrue(groupaccounts[0].IsAdministrator);
            Delete(GetAdminTicket(), id);
        }

        [Test]
        public void DeleteAccountGroupAccountTest()
        {
            // test that leaving a group with a single owner orphans it correctly to the admin
            WebGroupService.TransitAccountGroup t_instance = GetTransitInstance();
            int id = Create(GetUserTicket(), t_instance);
            WebGroupService.TransitAccountGroupAccount[] groupaccounts = EndPoint.GetAccountGroupAccounts(
                GetUserTicket(), id, null);
            Assert.AreEqual(1, groupaccounts.Length, "Group accounts size isn't one.");
            Console.WriteLine("Owner: {0}", groupaccounts[0].AccountName);
            Assert.AreEqual(groupaccounts[0].AccountId, GetUserAccount().Id);
            Assert.IsTrue(groupaccounts[0].IsAdministrator);
            // delete the user group account
            EndPoint.DeleteAccountGroupAccount(GetUserTicket(), groupaccounts[0].Id);
            // a system admin must now be the owner of the group
            WebGroupService.TransitAccountGroupAccount[] groupaccounts2 = EndPoint.GetAccountGroupAccounts(
                GetAdminTicket(), id, null);
            Assert.AreEqual(1, groupaccounts2.Length, "Group accounts size isn't one ater deleting last admin.");
            Assert.AreNotEqual(groupaccounts2[0].AccountId, GetUserAccount().Id);
            Assert.IsTrue(groupaccounts2[0].IsAdministrator);
            Console.WriteLine("New owner: {0}", groupaccounts2[0].AccountName);
            Delete(GetAdminTicket(), id);
        }

        [Test]
        protected void DeleteAccountGroupLastAdminAccountTest()
        {
            // test that leaving a group that the last system admin owns throws
        }

    }
}
