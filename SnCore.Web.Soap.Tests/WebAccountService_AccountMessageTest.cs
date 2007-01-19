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

        [SetUp]
        public override void SetUp()
        {
            _folder.SetUp();
            _folder_id = _folder.Create(GetAdminTicket());
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
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
            t_instance.Subject = Guid.NewGuid().ToString();
            t_instance.Body = Guid.NewGuid().ToString();
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
            int folder1_id = _folder.Create(GetAdminTicket());
            int folder1_count0 = EndPoint.GetAccountMessagesCount(GetAdminTicket(), folder1_id);
            int folder2_id = _folder.Create(GetAdminTicket());
            Console.WriteLine("Folder: {0} [{1}]", folder1_id, folder1_count0);
            Console.WriteLine("Folder: {0}", folder2_id);
            WebAccountService.TransitAccountMessage t_message = GetTransitInstance();
            t_message.AccountMessageFolderId = folder1_id;
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
            _folder.Delete(GetAdminTicket(), folder1_id);
            _folder.Delete(GetAdminTicket(), folder2_id);
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
    }
}
