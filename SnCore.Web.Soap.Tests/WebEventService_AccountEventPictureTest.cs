using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using SnCore.Tools.Drawing;

namespace SnCore.Web.Soap.Tests.WebEventServiceTests
{
    [TestFixture]
    public class AccountEventPictureTest : WebServiceTest<WebEventService.TransitAccountEventPicture, WebEventServiceNoCache>
    {
        private AccountEventTest _event = new AccountEventTest();
        private int _event_id = 0;
        private UserInfo _user = null;

        [SetUp]
        public override void SetUp()
        {
            _event.SetUp();
            _event_id = _event.Create(GetAdminTicket());
            _user = CreateUserWithVerifiedEmailAddress();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _event.Delete(GetAdminTicket(), _event_id);
            _event.TearDown();
            DeleteUser(_user.id);
        }

        public AccountEventPictureTest()
            : base("AccountEventPicture")
        {
        }

        public override WebEventService.TransitAccountEventPicture GetTransitInstance()
        {
            WebEventService.TransitAccountEventPicture t_instance = new WebEventService.TransitAccountEventPicture();
            t_instance.AccountEventId = _event_id;
            t_instance.Description = GetNewString();
            t_instance.Name = GetNewString();
            t_instance.Picture = GetNewBitmap();
            return t_instance;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, _event_id, options };
            return args;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, _event_id };
            return args;
        }

        class MoveAction
        {
            public int _index;
            public int _disp;
            public int[] _result;
            public WebEventService.TransitAccountEventPicture[] _pictures;

            public MoveAction(WebEventService.TransitAccountEventPicture[] pictures, int index, int disp, int[] result)
            {
                _index = index;
                _disp = disp;
                _result = result;
                _pictures = pictures;
            }

            public bool Compare(WebEventService.TransitAccountEventPicture[] pictures)
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
        public void MoveAccountEventPictureTest()
        {
            // create pictures
            const int count = 7;
            for (int i = 0; i < count; i++)
            {
                WebEventService.TransitAccountEventPicture t_instance = new WebEventService.TransitAccountEventPicture();
                t_instance.Picture = GetNewBitmap();
                t_instance.Description = GetNewString();
                t_instance.Name = GetNewString();
                t_instance.AccountEventId = _event_id;
                t_instance.Id = EndPoint.CreateOrUpdateAccountEventPicture(GetAdminTicket(), t_instance);
                Console.WriteLine("Picture: {0}", t_instance.Id);
            }

            // check that the pictures are numbered 1 through count
            WebEventService.TransitAccountEventPicture[] t_instances = EndPoint.GetAccountEventPictures(
                    _user.ticket, _event_id, null);
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
                EndPoint.MoveAccountEventPicture(GetAdminTicket(), t_instances[action._index - 1].Id, action._disp);
                Assert.IsTrue(action.Compare(EndPoint.GetAccountEventPictures(_user.ticket, _event_id, null)));
            }
        }
    }
}
