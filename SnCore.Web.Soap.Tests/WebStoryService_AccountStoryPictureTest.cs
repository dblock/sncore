using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using System.Threading;
using SnCore.Tools.Drawing;

namespace SnCore.Web.Soap.Tests.WebStoryServiceTests
{
    [TestFixture]
    public class AccountStoryPictureTest : WebServiceTest<WebStoryService.TransitAccountStoryPicture, WebStoryServiceNoCache>
    {
        private AccountStoryTest _story = new AccountStoryTest();
        int _story_id = 0;

        [SetUp]
        public override void SetUp()
        {
            _story_id = _story.Create(GetAdminTicket());
        }

        [TearDown]
        public override void TearDown()
        {
            _story.Delete(GetAdminTicket(), _story_id);
        }

        public AccountStoryPictureTest()
            : base("AccountStoryPicture")
        {

        }

        public override WebStoryService.TransitAccountStoryPicture GetTransitInstance()
        {
            WebStoryService.TransitAccountStoryPicture t_instance = new WebStoryService.TransitAccountStoryPicture();
            t_instance.AccountStoryId = _story_id;
            t_instance.Name = GetNewString();
            t_instance.Picture = GetNewBitmap();
            return t_instance;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, _story_id };
            return args;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, _story_id, options };
            return args;
        }

        [Test]
        public void GetAccountStoryPictureIfModifiedSinceTest()
        {
            WebStoryService.TransitAccountStoryPicture t_instance = GetTransitInstance();
            t_instance.Id = Create(GetAdminTicket(), t_instance);
            WebStoryService.TransitAccountStoryPicture t_instance2 = EndPoint.GetAccountStoryPictureById(GetUserTicket(), t_instance.Id);
            Assert.AreEqual(t_instance.Id, t_instance2.Id);
            WebStoryService.TransitAccountStoryPicture t_instance3 = EndPoint.GetAccountStoryPictureIfModifiedSinceById(GetUserTicket(), t_instance.Id, t_instance2.Modified);
            Assert.IsNull(t_instance3, "If-Modified-Since at last modified date returned a non-null value.");
            WebStoryService.TransitAccountStoryPicture t_instance4 = EndPoint.GetAccountStoryPictureIfModifiedSinceById(GetUserTicket(), t_instance.Id, t_instance2.Modified.AddHours(-1));
            Assert.IsNotNull(t_instance4, "If-Modified-Since at before last modified date returned a null value.");
            WebStoryService.TransitAccountStoryPicture t_instance5 = EndPoint.GetAccountStoryPictureIfModifiedSinceById(GetUserTicket(), t_instance.Id, t_instance2.Modified.AddHours(1));
            Assert.IsNull(t_instance3, "If-Modified-Since after last modified date returned a non-null value.");
            Delete(GetAdminTicket(), t_instance.Id);
        }

        [Test]
        public void MoveAccountStoryPictureTest()
        {
            WebStoryService.TransitAccountStoryPicture t_instance1 = GetTransitInstance();
            t_instance1.Id = Create(GetAdminTicket(), t_instance1);
            WebStoryService.TransitAccountStoryPicture t_instance2 = GetTransitInstance();
            t_instance2.Id = Create(GetAdminTicket(), t_instance2);

            {
                WebStoryService.TransitAccountStoryPicture t_instance1_copy = EndPoint.GetAccountStoryPictureById(GetUserTicket(), t_instance1.Id);
                WebStoryService.TransitAccountStoryPicture t_instance2_copy = EndPoint.GetAccountStoryPictureById(GetUserTicket(), t_instance2.Id);
                Assert.AreEqual(1, t_instance1_copy.Position, "First instance location is not one.");
                Assert.AreEqual(2, t_instance2_copy.Position, "Second instnace location is not two.");
            }

            EndPoint.MoveAccountStoryPicture(GetAdminTicket(), t_instance2.Id, -1);

            {
                WebStoryService.TransitAccountStoryPicture t_instance1_copy = EndPoint.GetAccountStoryPictureById(GetUserTicket(), t_instance1.Id);
                WebStoryService.TransitAccountStoryPicture t_instance2_copy = EndPoint.GetAccountStoryPictureById(GetUserTicket(), t_instance2.Id);
                Assert.AreEqual(2, t_instance1_copy.Position, "First instance location is not two after move.");
                Assert.AreEqual(1, t_instance2_copy.Position, "Second instnace location is not one after move.");
            }

            EndPoint.MoveAccountStoryPicture(GetAdminTicket(), t_instance2.Id, 1);

            {
                WebStoryService.TransitAccountStoryPicture t_instance1_copy = EndPoint.GetAccountStoryPictureById(GetUserTicket(), t_instance1.Id);
                WebStoryService.TransitAccountStoryPicture t_instance2_copy = EndPoint.GetAccountStoryPictureById(GetUserTicket(), t_instance2.Id);
                Assert.AreEqual(1, t_instance1_copy.Position, "First instance location is not one after move.");
                Assert.AreEqual(2, t_instance2_copy.Position, "Second instnace location is not two after move.");
            }

            EndPoint.MoveAccountStoryPicture(GetAdminTicket(), t_instance2.Id, 100);

            {
                WebStoryService.TransitAccountStoryPicture t_instance1_copy = EndPoint.GetAccountStoryPictureById(GetUserTicket(), t_instance1.Id);
                WebStoryService.TransitAccountStoryPicture t_instance2_copy = EndPoint.GetAccountStoryPictureById(GetUserTicket(), t_instance2.Id);
                Assert.AreEqual(1, t_instance1_copy.Position, "First instance location is not one after move.");
                Assert.AreEqual(2, t_instance2_copy.Position, "Second instnace location is not two after move.");
            }

            Delete(GetAdminTicket(), t_instance1.Id);
            Delete(GetAdminTicket(), t_instance2.Id);
        }

        class MoveAction
        {
            public int _index;
            public int _disp;
            public int[] _result;
            public WebStoryService.TransitAccountStoryPicture[] _pictures;

            public MoveAction(WebStoryService.TransitAccountStoryPicture[] pictures, int index, int disp, int[] result)
            {
                _index = index;
                _disp = disp;
                _result = result;
                _pictures = pictures;
            }

            public bool Compare(WebStoryService.TransitAccountStoryPicture[] pictures)
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
        public void MoveAccountStoryPictureTest2()
        {
            // create pictures
            const int count = 7;
            for (int i = 0; i < count; i++)
            {
                WebStoryService.TransitAccountStoryPicture t_instance = new WebStoryService.TransitAccountStoryPicture();
                t_instance.Picture = GetNewBitmap();
                t_instance.Name = GetNewString();
                t_instance.AccountStoryId = _story_id;
                t_instance.Id = EndPoint.CreateOrUpdateAccountStoryPicture(GetUserTicket(), t_instance);
                Console.WriteLine("Picture: {0}", t_instance.Id);
            }

            // check that the pictures are numbered 1 through count
            WebStoryService.TransitAccountStoryPicture[] t_instances = EndPoint.GetAccountStoryPictures(
                    GetUserTicket(), _story_id, null);
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
                EndPoint.MoveAccountStoryPicture(GetAdminTicket(), t_instances[action._index - 1].Id, action._disp);
                Assert.IsTrue(action.Compare(EndPoint.GetAccountStoryPictures(GetUserTicket(), _story_id, null)));
            }
        }
    }
}
