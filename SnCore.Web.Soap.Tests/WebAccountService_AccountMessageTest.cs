using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountMessageTest : WebServiceTest<WebAccountService.TransitAccountMessage, WebAccountServiceNoCache>
    {
        public AccountMessageFolderTest _folder = new AccountMessageFolderTest();
        public int _folder_id = 0;
        private AccountTest _account = new AccountTest();
        private int _account_id = 0;
        private AccountTest _account2 = new AccountTest();
        private int _account2_id = 0;

        [SetUp]
        public override void SetUp()
        {
            _folder.SetUp();
            _folder_id = _folder.Create(_folder.GetTestTicket());
            _account.SetUp();
            _account_id = _account.Create(GetAdminTicket());
            _account2.SetUp();
            _account2_id = _account.Create(GetAdminTicket());
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _account.Delete(GetAdminTicket(), _account_id);
            _account.TearDown();
            _folder.Delete(GetAdminTicket(), _folder_id);
            _folder.TearDown();
        }

        public AccountMessageTest()
            : base("AccountMessage")
        {

        }

        public override WebAccountService.TransitAccountMessage GetTransitInstance()
        {
            WebAccountService.TransitAccountMessage t_instance = new WebAccountService.TransitAccountMessage();
            t_instance.AccountId = _folder._account_id;
            t_instance.RecepientAccountId = GetAdminAccount().Id;
            t_instance.SenderAccountId = _folder._account_id;
            t_instance.Subject = GetNewString();
            t_instance.Body = GetNewString();
            t_instance.AccountMessageFolderId = _folder_id;
            return t_instance;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, _folder_id, options };
            return args;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, _folder_id };
            return args;
        }

        [Test]
        public void MoveAccountMessageToFolderByIdTest()
        {
            WebAccountService.TransitAccountMessageFolder t_instance1 = _folder.GetTransitInstance();
            t_instance1.AccountId = GetAdminAccount().Id;
            WebAccountService.TransitAccountMessageFolder t_instance2 = _folder.GetTransitInstance();
            t_instance2.AccountId = GetAdminAccount().Id;
            t_instance1.Id = _folder.Create(GetAdminTicket(), t_instance1);
            int folder1_count0 = EndPoint.GetAccountMessagesCount(GetAdminTicket(), t_instance1.Id);
            t_instance2.Id = _folder.Create(GetAdminTicket(), t_instance2);
            Console.WriteLine("Folder: {0} [{1}]", t_instance1.Id, folder1_count0);
            Console.WriteLine("Folder: {0}", t_instance2.Id);
            WebAccountService.TransitAccountMessage t_message = GetTransitInstance();
            t_message.AccountMessageFolderId = t_instance1.Id;
            t_message.Id = EndPoint.CreateOrUpdateAccountMessage(GetAdminTicket(), t_message);
            Console.WriteLine("Message: {0}", t_message.Id);
            // get number of messages in folder1 and folder2
            int folder1_count1 = EndPoint.GetAccountMessagesCount(GetAdminTicket(), t_instance1.Id);
            Console.WriteLine("Folder count: {0}", folder1_count1);
            Assert.IsTrue(folder1_count1 > 0);
            Assert.AreEqual(folder1_count1, folder1_count0 + 1);
            int folder2_count1 = EndPoint.GetAccountMessagesCount(GetAdminTicket(), t_instance2.Id);
            Console.WriteLine("Folder count: {0}", folder2_count1);
            // move message
            EndPoint.MoveAccountMessageToFolderById(GetAdminTicket(), t_message.Id, t_instance2.Id);
            int folder1_count2 = EndPoint.GetAccountMessagesCount(GetAdminTicket(), t_instance1.Id);
            Console.WriteLine("Folder count: {0}", folder1_count2);
            Assert.AreEqual(folder1_count0, folder1_count2);
            int folder2_count2 = EndPoint.GetAccountMessagesCount(GetAdminTicket(), t_instance2.Id);
            Console.WriteLine("Folder count: {0}", folder2_count2);
            Assert.AreEqual(folder2_count1 + 1, folder2_count2);
            _folder.Delete(GetAdminTicket(), t_instance1.Id);
            _folder.Delete(GetAdminTicket(), t_instance2.Id);
        }

        [Test]
        public void MoveAccountMessageToFolderTest()
        {
            int inbox_id = EndPoint.GetAccountMessageSystemFolder(GetAdminTicket(), GetAdminAccount().Id, "inbox").Id;
            int trash_id = EndPoint.GetAccountMessageSystemFolder(GetAdminTicket(), GetAdminAccount().Id, "trash").Id;
            int folder1_id = inbox_id;
            int folder1_count0 = EndPoint.GetAccountMessagesCount(GetAdminTicket(), folder1_id);
            int folder2_id = trash_id;
            Console.WriteLine("Folder: {0} [{1}]", folder1_id, folder1_count0);
            Console.WriteLine("Folder: {0}", folder2_id);
            WebAccountService.TransitAccountMessage t_message = GetTransitInstance();
            t_message.AccountMessageFolderId = folder1_id;
            t_message.AccountId = GetAdminAccount().Id;
            t_message.Id = EndPoint.CreateOrUpdateAccountMessage(GetAdminTicket(), t_message);
            Console.WriteLine("Message: {0}", t_message.Id);
            // get number of messages in folder1 and folder2
            int folder1_count1 = EndPoint.GetAccountMessagesCount(GetAdminTicket(), folder1_id);
            Console.WriteLine("Folder count: {0}", folder1_count1);
            Assert.IsTrue(folder1_count1 > 0);
            Assert.AreEqual(folder1_count1, folder1_count0 + 1);
            int folder2_count1 = EndPoint.GetAccountMessagesCount(GetAdminTicket(), folder2_id);
            Console.WriteLine("Folder count: {0}", folder2_count1);
            // move message
            EndPoint.MoveAccountMessageToFolderById(GetAdminTicket(), t_message.Id, folder2_id);
            int folder1_count2 = EndPoint.GetAccountMessagesCount(GetAdminTicket(), folder1_id);
            Console.WriteLine("Folder count: {0}", folder1_count2);
            Assert.AreEqual(folder1_count0, folder1_count2);
            int folder2_count2 = EndPoint.GetAccountMessagesCount(GetAdminTicket(), folder2_id);
            Console.WriteLine("Folder count: {0}", folder2_count2);
            Assert.AreEqual(folder2_count1 + 1, folder2_count2);
        }

        [Test]
        public void DeleteAccountMessagesByFolderTest()
        {
            int folder_id = _folder.Create(GetAdminTicket());
            int folder_count0 = EndPoint.GetAccountMessagesCount(GetAdminTicket(), folder_id);
            Console.WriteLine("Folder: {0} [{1}]", folder_id, folder_count0);
            WebAccountService.TransitAccountMessage t_message = GetTransitInstance();
            t_message.AccountMessageFolderId = folder_id;
            t_message.Id = EndPoint.CreateOrUpdateAccountMessage(GetAdminTicket(), t_message);
            Console.WriteLine("Message: {0}", t_message.Id);
            // get number of messages in folder
            int folder_count1 = EndPoint.GetAccountMessagesCount(GetAdminTicket(), folder_id);
            Console.WriteLine("Folder count: {0}", folder_count1);
            Assert.IsTrue(folder_count1 > 0);
            Assert.AreEqual(folder_count1, folder_count0 + 1);
            // delete messages
            EndPoint.DeleteAccountMessagesByFolder(GetAdminTicket(), folder_id);
            int folder_count2 = EndPoint.GetAccountMessagesCount(GetAdminTicket(), folder_id);
            Console.WriteLine("Folder count: {0}", folder_count2);
            Assert.AreEqual(0, folder_count2);
            _folder.Delete(GetAdminTicket(), folder_id);

        }

        [Test]
        public void AccountMessageQuotaTest()
        {
            List<int> ids = new List<int>();
            int limit = 10; // ManagedAccountMessage.DefaultHourlyLimit

            for (int i = 0; i < limit; i++)
            {
                WebAccountService.TransitAccountMessage t_message = GetTransitInstance();
                t_message.RecepientAccountId = _account2_id;
                t_message.Id = EndPoint.CreateOrUpdateAccountMessage(_folder.GetTestTicket(), t_message);
                Console.WriteLine("{0}: Message: {1}", i, t_message.Id);
                ids.Add(t_message.Id);
            }

            try
            {
                WebAccountService.TransitAccountMessage t_message = GetTransitInstance();
                t_message.RecepientAccountId = _account_id;
                t_message.Id = EndPoint.CreateOrUpdateAccountMessage(_folder.GetTestTicket(), t_message);
                Console.WriteLine("Message: {0}", t_message.Id);
                Assert.IsTrue(false, "Expected a quota exceeded exception.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Expected exception: {0}", ex.Message);
                Assert.IsTrue(ex.Message.StartsWith("System.Web.Services.Protocols.SoapException: Server was unable to process request. ---> SnCore.Services.ManagedAccount+QuotaExceededException: Quota exceeded"));
            }

            // make these two friends
            WebSocialService.WebSocialService socialendpoint = new WebSocialService.WebSocialService();
            int friend_request_id = socialendpoint.CreateOrUpdateAccountFriendRequest(_folder.GetTestTicket(), _account_id, GetNewString());
            Console.WriteLine("Created friend request: {0}", friend_request_id);
            socialendpoint.AcceptAccountFriendRequest(GetAdminTicket(), friend_request_id, GetNewString());

            // now the quota is lifted 

            {
                WebAccountService.TransitAccountMessage t_message = GetTransitInstance();
                t_message.RecepientAccountId = _account_id;
                t_message.Id = EndPoint.CreateOrUpdateAccountMessage(_folder.GetTestTicket(), t_message);
                Console.WriteLine("Over quota friend message: {0}", t_message.Id);
                ids.Add(t_message.Id);
            }

            // delete one message
            EndPoint.DeleteAccountMessage(GetAdminTicket(), ids[0]);
            ids.RemoveAt(0);

            // send to yet another user, quota still enforced, but the message sent to a friend doesn't count
            WebAccountService.TransitAccountMessage t_message2 = GetTransitInstance();
            t_message2.RecepientAccountId = _account2_id;
            t_message2.Id = EndPoint.CreateOrUpdateAccountMessage(_folder.GetTestTicket(), t_message2);
            Console.WriteLine("Message: {0}", t_message2.Id);

            try
            {
                WebAccountService.TransitAccountMessage t_message = GetTransitInstance();
                t_message.RecepientAccountId = _account2_id;
                t_message.Id = EndPoint.CreateOrUpdateAccountMessage(_folder.GetTestTicket(), t_message);
                Console.WriteLine("Message: {0}", t_message.Id);
                Assert.IsTrue(false, "Expected a quota exceeded exception.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Expected exception: {0}", ex.Message);
                Assert.IsTrue(ex.Message.StartsWith("System.Web.Services.Protocols.SoapException: Server was unable to process request. ---> SnCore.Services.ManagedAccount+QuotaExceededException: Quota exceeded"));
            }

            // delete all these

            foreach (int id in ids)
            {
                EndPoint.DeleteAccountMessage(GetAdminTicket(), id);
            }
        }
    }
}
