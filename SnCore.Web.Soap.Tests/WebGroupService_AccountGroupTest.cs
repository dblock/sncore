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

        [Test]
        public void GetAccountGroupDiscussionTest()
        {
            // make sure that anyone can access a discussion of a public group
            // create a public group
            WebGroupService.TransitAccountGroup t_group = GetTransitInstance();
            t_group.IsPrivate = false;
            t_group.Id = Create(GetAdminTicket(), t_group);
            // fetch the discussion
            WebDiscussionService.WebDiscussionService discussionendpoint = new WebDiscussionService.WebDiscussionService();
            int discussion_id = discussionendpoint.GetOrCreateDiscussionId(GetUserTicket(), "AccountGroup", t_group.Id);
            Console.WriteLine("Discussion: {0}", discussion_id);
            Assert.AreNotEqual(0, discussion_id);
            // make sure a regular user can't post a thread (must be member of a group)
            try
            {
                WebDiscussionService.TransitDiscussionPost t_post1 = new WebDiscussionService.TransitDiscussionPost();
                t_post1.AccountId = GetUserAccount().Id;
                t_post1.DiscussionId = discussion_id;
                t_post1.DiscussionThreadId = 0;
                t_post1.Subject = GetNewString();
                t_post1.Body = GetNewString();
                t_post1.Id = discussionendpoint.CreateOrUpdateDiscussionPost(GetUserTicket(), t_post1);
                Console.WriteLine("Post: {0}", t_post1.Id);
                Assert.IsFalse(true, "Expected an Access Denied exception.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
                Assert.AreEqual("System.Web.Services.Protocols.SoapException: Server was unable to process request. ---> SnCore.Services.ManagedAccount+AccessDeniedException: Access denied",
                    ex.Message.Split("\n".ToCharArray(), 2)[0],
                    string.Format("Unexpected exception: {0}", ex.Message));
            }
            // post a new thread (as admin)
            WebDiscussionService.TransitDiscussionPost t_post2 = new WebDiscussionService.TransitDiscussionPost();
            t_post2.AccountId = GetAdminAccount().Id;
            t_post2.DiscussionId = discussion_id;
            t_post2.DiscussionThreadId = 0;
            t_post2.Subject = GetNewString();
            t_post2.Body = GetNewString();
            t_post2.Id = discussionendpoint.CreateOrUpdateDiscussionPost(GetAdminTicket(), t_post2);
            Console.WriteLine("Post: {0}", t_post2.Id);
            // make sure regular users can't read posts from a public discussion
            try
            {
                WebDiscussionService.TransitDiscussionPost t_post3 = discussionendpoint.GetDiscussionPostById(
                    GetUserTicket(), t_post2.Id);
                Console.WriteLine("Post: {0}", t_post3.Id);
                Assert.IsFalse(true, "Expected an Access Denied exception.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
                Assert.AreEqual("System.Web.Services.Protocols.SoapException: Server was unable to process request. ---> SnCore.Services.ManagedAccount+AccessDeniedException: Access denied",
                    ex.Message.Split("\n".ToCharArray(), 2)[0],
                    string.Format("Unexpected exception: {0}", ex.Message));
            }
            // join the user to the group
            WebGroupService.TransitAccountGroupAccount t_account = new WebGroupService.TransitAccountGroupAccount();
            t_account.AccountGroupId = t_group.Id;
            t_account.AccountId = GetUserAccount().Id;
            t_account.IsAdministrator = false;
            t_account.Id = EndPoint.CreateOrUpdateAccountGroupAccount(GetAdminTicket(), t_account);
            Console.WriteLine("Joined: {0}", t_account.Id);
            Assert.AreNotEqual(0, t_account.Id);
            // the user now can retrieve the post
            WebDiscussionService.TransitDiscussionPost t_post4 = discussionendpoint.GetDiscussionPostById(
                GetUserTicket(), t_post2.Id);
            Console.WriteLine("Post: {0}", t_post4.Id);
            // done
            Delete(GetAdminTicket(), t_group.Id);
        }

        [Test]
        public void UserCannotElevateGroupPrivilegeTest()
        {
            // test that leaving a group with a single owner orphans it correctly to the admin
            WebGroupService.TransitAccountGroup t_instance = GetTransitInstance();
            t_instance.Id = Create(GetAdminTicket(), t_instance);
            // join a user to a group
            WebGroupService.TransitAccountGroupAccount t_account = new WebGroupService.TransitAccountGroupAccount();
            t_account.AccountGroupId = t_instance.Id;
            t_account.AccountId = GetUserAccount().Id;
            t_account.IsAdministrator = false;
            t_account.Id = EndPoint.CreateOrUpdateAccountGroupAccount(GetAdminTicket(), t_account);
            Console.WriteLine("Joined: {0}", t_account.Id);
            // attempt to elevate
            try
            {
                t_account.IsAdministrator = true;
                EndPoint.CreateOrUpdateAccountGroupAccount(GetUserTicket(), t_account);
                Assert.IsTrue(false, "Expected Access Denied exception.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
                Assert.AreEqual("System.Web.Services.Protocols.SoapException: Server was unable to process request. ---> SnCore.Services.ManagedAccount+AccessDeniedException: Access denied",
                    ex.Message.Split("\n".ToCharArray(), 2)[0],
                    string.Format("Unexpected exception: {0}", ex.Message));
            }
            Delete(GetAdminTicket(), t_instance.Id);
        }

    }
}
