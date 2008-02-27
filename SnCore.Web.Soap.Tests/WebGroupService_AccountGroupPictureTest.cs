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

        [SetUp]
        public override void SetUp()
        {
            _group.SetUp();
            _group_id = _group.Create(GetAdminTicket());
        }

        [TearDown]
        public override void TearDown()
        {
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
            t_instance.AccountId = GetUserAccount().Id;
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
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
                Assert.AreEqual("System.Web.Services.Protocols.SoapException: Server was unable to process request. ---> SnCore.Services.ManagedAccount+AccessDeniedException: Access denied",
                    ex.Message.Split("\n".ToCharArray(), 2)[0],
                    string.Format("Unexpected exception: {0}", ex.Message));
            }
            Delete(GetAdminTicket(), t_instance.Id);
            EndPoint.DeleteAccountGroup(GetAdminTicket(), t_group.Id);
        }

        class MoveAction
        {
            public int _index;
            public int _disp;
            public int[] _result;
            public WebGroupService.TransitAccountGroupPicture[] _pictures;

            public MoveAction(WebGroupService.TransitAccountGroupPicture[] pictures, int index, int disp, int[] result)
            {
                _index = index;
                _disp = disp;
                _result = result;
                _pictures = pictures;
            }

            public bool Compare(WebGroupService.TransitAccountGroupPicture[] pictures)
            {
                if (pictures.Length != _result.Length)
                    throw new Exception("Invalid picture count.");

                for (int i = 0; i < pictures.Length; i++)
                {
                    if (pictures[i].Position != i + 1)
                        throw new Exception(string.Format("Expected position {0} at {1}.", pictures[i].Position, i + 1));

                    int expected_position = _result[i]; // i should find expected_position item in this position
                    int expected_index = _pictures[expected_position - 1].Id; // i should find expected index in this position
                    if (pictures[i].Id != expected_index)
                        return false;
                }

                return true;
            }
        }

        [Test]
        public void MoveAccountGroupPictureTest()
        {
            // create pictures
            const int count = 7;
            for (int i = 0; i < count; i++)
            {
                WebGroupService.TransitAccountGroupPicture t_instance = new WebGroupService.TransitAccountGroupPicture();
                t_instance.Bitmap = GetNewBitmap();
                t_instance.Description = GetNewString();
                t_instance.Name = GetNewString();
                t_instance.AccountGroupId = _group_id;
                t_instance.Id = EndPoint.CreateOrUpdateAccountGroupPicture(GetAdminTicket(), t_instance);
                Console.WriteLine("Picture: {0}", t_instance.Id);
            }

            // check that the pictures are numbered 1 through count
            WebGroupService.TransitAccountGroupPicture[] t_instances = EndPoint.GetAccountGroupPictures(
                    GetUserTicket(), _group_id, null);
            Assert.AreEqual(count, t_instances.Length);
            for (int i = 0; i < count; i++)
            {
                Assert.AreEqual(i + 1, t_instances[i].Position);
            }

            int[] seq_nomove = { 1, 2, 3, 4, 5, 6, 7 };

            MoveAction[] actions = {
                new MoveAction(t_instances, 1, 0, seq_nomove),
                new MoveAction(t_instances, 5, 0, seq_nomove),
                new MoveAction(t_instances, 7, 0, seq_nomove),
                new MoveAction(t_instances, 1, -1, seq_nomove),
                new MoveAction(t_instances, 1, -100, seq_nomove),
                new MoveAction(t_instances, 7, 1, seq_nomove),
                new MoveAction(t_instances, 7, 100, seq_nomove),
                new MoveAction(t_instances, 1, 1, new int[]{ 2, 1, 3, 4, 5, 6, 7 }),
                new MoveAction(t_instances, 1, 1, new int[]{ 2, 3, 1, 4, 5, 6, 7 }),
                new MoveAction(t_instances, 1, 2, new int[]{ 2, 3, 4, 5, 1, 6, 7 }),
                new MoveAction(t_instances, 2, 1, new int[]{ 3, 2, 4, 5, 1, 6, 7 }),
                new MoveAction(t_instances, 2, -1, new int[]{ 2, 3, 4, 5, 1, 6, 7 }),
                new MoveAction(t_instances, 3, -2, new int[]{ 3, 2, 4, 5, 1, 6, 7 }),
                new MoveAction(t_instances, 3, 6, new int[]{ 2, 4, 5, 1, 6, 7, 3 }),
            };

            foreach (MoveAction action in actions)
            {
                Console.WriteLine("Moving {0} by {1}", t_instances[action._index - 1].Id, action._disp);
                EndPoint.MoveAccountGroupPicture(GetAdminTicket(), t_instances[action._index - 1].Id, action._disp);
                Assert.IsTrue(action.Compare(EndPoint.GetAccountGroupPictures(GetUserTicket(), _group_id, null)));
            }
        }
    }
}
