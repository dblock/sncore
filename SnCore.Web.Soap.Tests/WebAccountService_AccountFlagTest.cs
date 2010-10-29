using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using SnCore.Web.Soap.Tests.WebObjectServiceTests;
using SnCore.Web.Soap.Tests.WebAccountServiceTests;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountFlagTest : WebServiceTest<WebAccountService.TransitAccountFlag, WebAccountServiceNoCache>
    {
        private AccountFlagTypeTest _type = new AccountFlagTypeTest();
        private int _type_id = 0;
        private UserInfo _bad_user = null;
        private UserInfo _good_user = null;
        
        [SetUp]
        public override void SetUp()
        {
            _type_id = _type.Create(GetAdminTicket());
            _bad_user = CreateUserWithVerifiedEmailAddress();
            _good_user = CreateUserWithVerifiedEmailAddress();
        }

        [TearDown]
        public override void TearDown()
        {
            DeleteUser(_good_user.id);
            DeleteUser(_bad_user.id);
            _type.Delete(GetAdminTicket(), _type_id);
        }

        public AccountFlagTest()
            : base("AccountFlag")
        {
        }

        public override WebAccountService.TransitAccountFlag GetTransitInstance()
        {
            WebAccountService.TransitAccountFlag t_instance = new WebAccountService.TransitAccountFlag();
            t_instance.Description = GetNewString();
            t_instance.AccountFlagType = (string) _type.GetInstancePropertyById(GetAdminTicket(), _type_id, "Name");
            t_instance.AccountId = _good_user.id;
            t_instance.FlaggedAccountId = _bad_user.id;
            return t_instance;
        }

        [Test]
        public void GetAccountFlagsByAccountIdTest()
        {
            WebAccountService.TransitAccountFlag t_instance = GetTransitInstance();
            t_instance.Id = Create(_good_user.ticket, t_instance);
            int count = EndPoint.GetAccountFlagsByAccountIdCount(
                _good_user.ticket, _good_user.id);
            Console.WriteLine("Count: {0}", count);
            Assert.IsTrue(count > 0);
            WebAccountService.TransitAccountFlag[] flags = EndPoint.GetAccountFlagsByAccountId(
                _good_user.ticket, _good_user.id, null);
            Console.WriteLine("Flags: {0}", flags.Length);
            Assert.AreEqual(count, flags.Length);
            Delete(GetAdminTicket(), t_instance.Id);
        }

        [Test]
        public void GetAccountFlagsByFlaggedAccountIdTest()
        {
            WebAccountService.TransitAccountFlag t_instance = GetTransitInstance();
            t_instance.Id = Create(_good_user.ticket, t_instance);
            int count = EndPoint.GetAccountFlagsByFlaggedAccountIdCount(
                GetAdminTicket(), _bad_user.id);
            Console.WriteLine("Count: {0}", count);
            Assert.IsTrue(count > 0);
            WebAccountService.TransitAccountFlag[] flags = EndPoint.GetAccountFlagsByFlaggedAccountId(
                GetAdminTicket(), _bad_user.id, null);
            Console.WriteLine("Flags: {0}", flags.Length);
            Assert.AreEqual(count, flags.Length);
            Delete(GetAdminTicket(), t_instance.Id);
        }

        [Test]
        public void DefaultAccountFlagThresholdTest()
        {
            UserInfo user = CreateUserWithVerifiedEmailAddress();

            WebAccountService.TransitAccountMessage t_message = new WebAccountService.TransitAccountMessage();
            t_message.SenderAccountId = user.id;
            t_message.RecepientAccountId = t_message.AccountId = GetAdminAccount().Id;
            t_message.Subject = GetNewString();
            t_message.Body = GetNewString();

            int message_id = EndPoint.CreateOrUpdateAccountMessage(
                user.ticket, t_message);

            Assert.IsTrue(message_id != 0);

            // flag the account
            int threshold = 0;
            List<int> accounts = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                string temp_email = GetNewEmailAddress();
                string temp_password = GetNewString();
                int temp_user_id = CreateUser(temp_email, temp_password);
                accounts.Add(temp_user_id);
                Console.WriteLine("Temp user: {0}", temp_user_id);
                string temp_ticket = Login(temp_email, temp_password);

                // flag the user
                WebAccountService.TransitAccountFlag t_flag = new WebAccountService.TransitAccountFlag();
                t_flag.AccountFlagType = (string)_type.GetInstancePropertyById(GetAdminTicket(), _type_id, "Name");
                t_flag.AccountId = temp_user_id;
                t_flag.Description = GetNewString();
                t_flag.FlaggedAccountId = user.id;
                t_flag.Id = EndPoint.CreateOrUpdateAccountFlag(temp_ticket, t_flag);
                Console.WriteLine("Flag: {0}", t_flag.Id);

                try
                {
                    int temp_message_id = EndPoint.CreateOrUpdateAccountMessage(
                        user.ticket, t_message);
                    Console.WriteLine("Message: {0}", temp_message_id);
                    threshold++;
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Expected exception: {0}", ex.Message);
                    Assert.IsTrue(ex.Message.StartsWith("System.Web.Services.Protocols.SoapException: Server was unable to process request. ---> SnCore.Services.ManagedAccountFlag+AccountFlaggedException: "));
                }
            }

            Console.WriteLine("Threshold: {0}", threshold);
            Assert.IsTrue(threshold > 0);
            DeleteUser(user.id);

            for (int i = 0; i < accounts.Count; i++)
            {
                Console.WriteLine("Deleting temp user: {0}", accounts[i]);
                DeleteUser(accounts[i]);
            }
        }
    }
}
