using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountMessageTest : ManagedCRUDTest<AccountMessage, TransitAccountMessage, ManagedAccountMessage>
    {
        private ManagedAccountTest _account2 = new ManagedAccountTest();
        private ManagedAccountMessageFolderTest _folder = new ManagedAccountMessageFolderTest();

        [SetUp]
        public override void SetUp()
        {
            _folder.SetUp();
            _account2.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _account2.TearDown();
            _folder.TearDown();
        }

        public ManagedAccountMessageTest()
        {

        }

        public override TransitAccountMessage GetTransitInstance()
        {
            TransitAccountMessage t_instance = new TransitAccountMessage();
            t_instance.AccountId = _folder.Instance.Account.Id;
            t_instance.AccountMessageFolderId = _folder.Instance.Id;
            t_instance.Body = GetNewString();
            t_instance.RecepientAccountId = _account2.Instance.Id;
            t_instance.SenderAccountId = _folder.Instance.Account.Id;
            t_instance.Subject = GetNewString();
            t_instance.Sent = DateTime.UtcNow;
            return t_instance;
        }

        [Test]
        protected void ReadUnReadMessageTest()
        {

        }

        [Test]
        public void GetAcountMessagesTest()
        {
            IList<AccountMessage> messages = ManagedAccountMessage.GetAcountMessages(
                Session, _account2.Instance.Id, DateTime.UtcNow.AddDays(-1));
            Console.WriteLine("Messages: {0}", messages.Count);
        }
    }
}
