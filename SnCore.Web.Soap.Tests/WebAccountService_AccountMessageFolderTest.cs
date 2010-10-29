using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountMessageFolderTest : AccountBaseTest<WebAccountService.TransitAccountMessageFolder>
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        public AccountMessageFolderTest()
            : base("AccountMessageFolder")
        {

        }

        public override WebAccountService.TransitAccountMessageFolder GetTransitInstance()
        {
            WebAccountService.TransitAccountMessageFolder t_instance = new WebAccountService.TransitAccountMessageFolder();
            t_instance.AccountId = GetTestAccountId();
            t_instance.Name = GetNewString();
            t_instance.System = false;
            return t_instance;
        }

        [Test]
        public void GetAccountMessageSystemFolderTest()
        {
            WebAccountService.TransitAccountMessageFolder inbox = EndPoint.GetAccountMessageSystemFolder(GetAdminTicket(), GetAdminAccount().Id, "inbox");
            WebAccountService.TransitAccountMessageFolder sent = EndPoint.GetAccountMessageSystemFolder(GetAdminTicket(), GetAdminAccount().Id, "sent");
            WebAccountService.TransitAccountMessageFolder trash = EndPoint.GetAccountMessageSystemFolder(GetAdminTicket(), GetAdminAccount().Id, "trash");
            Assert.AreNotEqual(inbox.Id, 0);
            Assert.AreEqual(inbox.AccountMessageFolderParentId, 0);
            Assert.AreNotEqual(sent.Id, 0);
            Assert.AreEqual(sent.AccountMessageFolderParentId, 0);
            Assert.AreNotEqual(trash.Id, 0);
            Assert.AreEqual(trash.AccountMessageFolderParentId, 0);
        }

        [Test]
        public void CreateAccountSystemMessageFoldersTest()
        {
            // admin user's folders are already created
            WebAccountService.TransitAccountMessageFolder[] folders = EndPoint.GetAccountMessageFolders(
                GetAdminTicket(), GetAdminAccount().Id, null);
            Console.WriteLine("Folders: {0}", folders.Length);
            EndPoint.CreateAccountSystemMessageFolders(GetAdminTicket(), GetAdminAccount().Id);
            WebAccountService.TransitAccountMessageFolder[] folders2 = EndPoint.GetAccountMessageFolders(
                GetAdminTicket(), GetAdminAccount().Id, null);
            Assert.AreEqual(folders.Length, folders2.Length);
        }
    }
}
