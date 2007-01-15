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
        protected void MoveAccountMessageToFolderTest()
        {

        }

        [Test]
        protected void MoveAccountMessageToFolderByIdTest()
        {

        }

        [Test]
        protected void DeleteAccountMessagesByFolderTest()
        {

        }
    }
}
