using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using SnCore.Web.Soap.Tests.WebPlaceServiceTests;

namespace SnCore.Web.Soap.Tests.WebGroupServiceTests
{
    [TestFixture]
    public class AccountGroupPlaceTest : WebServiceTest<WebGroupService.TransitAccountGroupPlace, WebGroupServiceNoCache>
    {
        private AccountGroupTest _group = new AccountGroupTest();
        private int _group_id = 0;
        private PlaceTest _place = new PlaceTest();
        private int _place_id = 0;
        private UserInfo _user = null;

        [SetUp]
        public override void SetUp()
        {
            _place.SetUp();
            _place_id = _place.Create(GetAdminTicket());
            _group.SetUp();
            _group_id = _group.Create(GetAdminTicket());
            _user = CreateUserWithVerifiedEmailAddress();
        }

        [TearDown]
        public override void TearDown()
        {
            DeleteUser(_user.id);
            _group.Delete(GetAdminTicket(), _group_id);
            _group.TearDown();
            _place.Delete(GetAdminTicket(), _place_id);
            _place.TearDown();
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

        public AccountGroupPlaceTest()
            : base("AccountGroupPlace")
        {

        }

        public override WebGroupService.TransitAccountGroupPlace GetTransitInstance()
        {
            WebGroupService.TransitAccountGroupPlace t_instance = new WebGroupService.TransitAccountGroupPlace();
            t_instance.AccountGroupId = _group_id;
            t_instance.PlaceId = _place_id;
            return t_instance;
        }

        [Test]
        public void CreateOrUpdateAccountGroupPlaceTest()
        {
            // make sure only members can add a place to a group
            WebGroupService.TransitAccountGroupPlace t_instance = GetTransitInstance();
            try
            {
                // make sure the user is not a member of the group
                Assert.IsNull(EndPoint.GetAccountGroupAccountByAccountGroupId(
                    GetAdminTicket(), _user.id, _group_id));
                // create the account group place
                EndPoint.CreateOrUpdateAccountGroupPlace(_user.ticket, t_instance);
                Assert.IsTrue(false, "Expected Access Denied exception.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
                Assert.AreEqual("System.Web.Services.Protocols.SoapException: Server was unable to process request. ---> SnCore.Services.ManagedAccount+AccessDeniedException: Access denied", 
                    ex.Message.Split("\n".ToCharArray(), 2)[0],
                    string.Format("Unexpected exception: {0}", ex.Message));
            }
            // the user joins the group
            WebGroupService.TransitAccountGroupAccount t_accountinstance = new WebGroupService.TransitAccountGroupAccount();
            t_accountinstance.AccountGroupId = _group_id;
            t_accountinstance.AccountId = _user.id;
            t_accountinstance.IsAdministrator = false;
            t_accountinstance.Id = EndPoint.CreateOrUpdateAccountGroupAccount(GetAdminTicket(), t_accountinstance);
            Assert.AreNotEqual(0, t_accountinstance.Id);
            // check that the user now can add a place
            t_instance.Id = EndPoint.CreateOrUpdateAccountGroupPlace(_user.ticket, t_instance);
            Assert.AreNotEqual(0, t_instance.Id);
        }

        [Test]
        public void GetAccountGroupPlacesTest()
        {
            WebGroupService.TransitAccountGroupPlace t_instance = GetTransitInstance();
            t_instance.Id = Create(GetAdminTicket(), t_instance);
            try
            {
                WebGroupService.TransitAccountGroupPlace[] places = EndPoint.GetAccountGroupPlaces(
                    _user.ticket, _group_id, null);
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
