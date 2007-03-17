using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using SnCore.Web.Soap.Tests.WebAccountServiceTests;

namespace SnCore.Web.Soap.Tests.WebGroupServiceTests
{
    [TestFixture]
    public class AccountGroupPictureTest : WebServiceTest<WebGroupService.TransitAccountGroupPicture, WebGroupServiceNoCache>
    {
        private AccountGroupTest _group = new AccountGroupTest();
        private int _group_id = 0;
        private AccountTest _account = new AccountTest();
        private int _account_id = 0;

        [SetUp]
        public override void SetUp()
        {
            _group.SetUp();
            _group_id = _group.Create(GetAdminTicket());
            _account.SetUp();
            _account_id = _account.Create(GetAdminTicket());
        }

        [TearDown]
        public override void TearDown()
        {
            _account.Delete(GetAdminTicket(), _account_id);
            _account.TearDown();
            _group.Delete(GetAdminTicket(), _group_id);
            _group.TearDown();
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, _group_id };
            return args;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, _group_id, options };
            return args;
        }

        public AccountGroupPictureTest()
            : base("AccountGroupPicture")
        {

        }

        public override WebGroupService.TransitAccountGroupPicture GetTransitInstance()
        {
            WebGroupService.TransitAccountGroupPicture t_instance = new WebGroupService.TransitAccountGroupPicture();
            t_instance.AccountGroupId = _group_id;
            t_instance.AccountId = _account_id;
            t_instance.Bitmap = GetNewBitmap();
            t_instance.Description = GetNewString();
            t_instance.Name = GetNewString();
            return t_instance;
        }

        [Test]
        public void CreateOrUpdateAccountGroupPictureTest()
        {
            // make sure only members can add a picture to a group
            WebGroupService.TransitAccountGroupPicture t_instance = GetTransitInstance();
            t_instance.AccountId = GetUserAccount().Id;
            try
            {
                // make sure the user is not a member of the group
                Assert.IsNull(EndPoint.GetAccountGroupAccountByAccountGroupId(
                    GetAdminTicket(), GetUserAccount().Id, _group_id));
                // create the account group picture
                EndPoint.CreateOrUpdateAccountGroupPicture(GetUserTicket(), t_instance);
                Assert.IsTrue(false, "Expected Access Denied exception.");
            }
            catch (SoapException ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
                Assert.AreEqual("SnCore.Services.ManagedAccount+AccessDeniedException: Access denied",
                    ex.Message.Split("\n".ToCharArray(), 2)[0],
                    string.Format("Unexpected exception: {0}", ex.Message));
            }
            // the user joins the group
            WebGroupService.TransitAccountGroupAccount t_accountinstance = new WebGroupService.TransitAccountGroupAccount();
            t_accountinstance.AccountGroupId = _group_id;
            t_accountinstance.AccountId = GetUserAccount().Id;
            t_accountinstance.IsAdministrator = false;
            t_accountinstance.Id = EndPoint.CreateOrUpdateAccountGroupAccount(GetAdminTicket(), t_accountinstance);
            Assert.AreNotEqual(0, t_accountinstance.Id);
            // check that the user now can add a picture
            t_instance.Id = EndPoint.CreateOrUpdateAccountGroupPicture(GetUserTicket(), t_instance);
            Assert.AreNotEqual(0, t_instance.Id);
        }

        [Test]
        public void GetPrivateAccountGroupPicturesTest()
        {
            WebGroupService.TransitAccountGroup t_group = new WebGroupService.TransitAccountGroup();
            t_group.Name = GetNewString();
            t_group.IsPrivate = true;
            t_group.Description = GetNewString();
            t_group.Id = EndPoint.CreateOrUpdateAccountGroup(GetAdminTicket(), t_group);
            WebGroupService.TransitAccountGroupPicture t_instance = new WebGroupService.TransitAccountGroupPicture();
            t_instance.AccountGroupId = t_group.Id;
            t_instance.AccountId = GetAdminAccount().Id;
            t_instance.Bitmap = GetNewBitmap();
            t_instance.Description = GetNewString();
            t_instance.Name = GetNewString();
            t_instance.Id = Create(GetAdminTicket(), t_instance);
            try
            {
                WebGroupService.TransitAccountGroupPicture[] pictures = EndPoint.GetAccountGroupPictures(
                    GetUserTicket(), t_group.Id, null);
                Assert.IsTrue(false, "Expected Access Denied exception.");
            }
            catch (SoapException ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
                Assert.AreEqual("SnCore.Services.ManagedAccount+AccessDeniedException: Access denied",
                    ex.Message.Split("\n".ToCharArray(), 2)[0],
                    string.Format("Unexpected exception: {0}", ex.Message));
            }
            Delete(GetAdminTicket(), t_instance.Id);
            EndPoint.DeleteAccountGroup(GetAdminTicket(), t_group.Id);
        }
    }
}
