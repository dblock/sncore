using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using SnCore.Tools.Drawing;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountPictureTest : AccountBaseTest<WebAccountService.TransitAccountPicture>
    {
        private UserInfo _user = null;

        [SetUp]
        public override void SetUp()
        {
            _user = CreateUserWithVerifiedEmailAddress();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            DeleteUser(_user.id);
            base.TearDown();
        }

        public AccountPictureTest()
            : base("AccountPicture")
        {

        }

        [Test]
        public void HasPictureTest()
        {
            Assert.IsTrue(EndPoint.HasPicture(_user.ticket, _user.id));
            WebAccountService.TransitAccountPicture[] pictures = EndPoint.GetAccountPictures(
                _user.ticket, _user.id, null, null);
            Assert.AreEqual(1, pictures.Length);
            EndPoint.DeleteAccountPicture(_user.ticket, pictures[0].Id);
            Assert.IsFalse(EndPoint.HasPicture(_user.ticket, _user.id));
        }

        public override WebAccountService.TransitAccountPicture GetTransitInstance()
        {
            WebAccountService.TransitAccountPicture t_instance = new WebAccountService.TransitAccountPicture();
            t_instance.AccountId = GetTestAccountId();
            t_instance.Description = GetNewString();
            t_instance.Name = GetNewString();
            t_instance.Bitmap = GetNewBitmap();
            return t_instance;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            WebAccountService.AccountPicturesQueryOptions qopt = new WebAccountService.AccountPicturesQueryOptions();
            qopt.Hidden = true;
            object[] args = { ticket, GetTestAccountId(), qopt, options };
            return args;
        }

        public override object[] GetCountArgs(string ticket)
        {
            WebAccountService.AccountPicturesQueryOptions qopt = new WebAccountService.AccountPicturesQueryOptions();
            qopt.Hidden = true;
            object[] args = { ticket, GetTestAccountId(), qopt };
            return args;
        }

        class MoveAction
        {
            public int _index;
            public int _disp;
            public int[] _result;
            public WebAccountService.TransitAccountPicture[] _pictures;

            public MoveAction(WebAccountService.TransitAccountPicture[] pictures, int index, int disp, int[] result)
            {
                _index = index;
                _disp = disp;
                _result = result;
                _pictures = pictures;
            }

            public bool Compare(WebAccountService.TransitAccountPicture[] pictures)
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
        public void MoveAccountPictureTest()
        {
            // create a new user
            UserInfo user = CreateUserWithVerifiedEmailAddress();
            // create pictures, there's a default one, so start at 1
            const int count = 7;
            for (int i = 1; i < count; i++)
            {
                WebAccountService.TransitAccountPicture t_instance = new WebAccountService.TransitAccountPicture();
                t_instance.Bitmap = GetNewBitmap();
                t_instance.Description = GetNewString();
                t_instance.Name = GetNewString();
                t_instance.Id = EndPoint.CreateOrUpdateAccountPicture(user.ticket, t_instance);
                Console.WriteLine("Picture: {0}", t_instance.Id);
            }

            // check that the pictures are numbered 1 through count
            WebAccountService.AccountPicturesQueryOptions qopt = new WebAccountService.AccountPicturesQueryOptions();
            qopt.Hidden = true;
            WebAccountService.TransitAccountPicture[] t_instances = EndPoint.GetAccountPictures(
                    user.ticket, user.id, qopt, null);
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
                EndPoint.MoveAccountPicture(user.ticket, t_instances[action._index - 1].Id, action._disp);
                Assert.IsTrue(action.Compare(EndPoint.GetAccountPictures(user.ticket, user.id, qopt, null)));
            }

            DeleteUser(user.id);
        }
    }
}
