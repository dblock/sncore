using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using SnCore.Tools.Drawing;

namespace SnCore.Web.Soap.Tests.WebPlaceServiceTests
{
    [TestFixture]
    public class PlacePictureTest : WebServiceTest<WebPlaceService.TransitPlacePicture, WebPlaceServiceNoCache>
    {
        private PlaceTest _place = new PlaceTest();
        private int _place_id = 0;
        private UserInfo _user = null;

        [SetUp]
        public override void SetUp()
        {
            _place.SetUp();
            _place_id = _place.Create(GetAdminTicket());
            _user = CreateUserWithVerifiedEmailAddress();
        }

        [TearDown]
        public override void TearDown()
        {
            _place.Delete(GetAdminTicket(), _place_id);
            _place.TearDown();
            DeleteUser(_user.id);
        }

        public PlacePictureTest()
            : base("PlacePicture")
        {
        }

        public override WebPlaceService.TransitPlacePicture GetTransitInstance()
        {
            WebPlaceService.TransitPlacePicture t_instance = new WebPlaceService.TransitPlacePicture();
            t_instance.Name = GetNewString();
            t_instance.Description = GetNewString();
            t_instance.PlaceId = _place_id;
            t_instance.Bitmap = GetNewBitmap();
            return t_instance;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, _place_id, options };
            return args;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, _place_id };
            return args;
        }

        class MoveAction
        {
            public int _index;
            public int _disp;
            public int[] _result;
            public WebPlaceService.TransitPlacePicture[] _pictures;

            public MoveAction(WebPlaceService.TransitPlacePicture[] pictures, int index, int disp, int[] result)
            {
                _index = index;
                _disp = disp;
                _result = result;
                _pictures = pictures;
            }

            public bool Compare(WebPlaceService.TransitPlacePicture[] pictures)
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
        public void MovePlacePictureTest()
        {
            // create pictures
            const int count = 7;
            for (int i = 0; i < count; i++)
            {
                WebPlaceService.TransitPlacePicture t_instance = new WebPlaceService.TransitPlacePicture();
                t_instance.Bitmap = GetNewBitmap();
                t_instance.Description = GetNewString();
                t_instance.Name = GetNewString();
                t_instance.PlaceId = _place_id;
                t_instance.Id = EndPoint.CreateOrUpdatePlacePicture(_user.ticket, t_instance);
                Console.WriteLine("Picture: {0}", t_instance.Id);
            }

            // check that the pictures are numbered 1 through count
            WebPlaceService.TransitPlacePicture[] t_instances = EndPoint.GetPlacePictures(
                    _user.ticket, _place_id, null);
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
                EndPoint.MovePlacePicture(GetAdminTicket(), t_instances[action._index - 1].Id, action._disp);
                Assert.IsTrue(action.Compare(EndPoint.GetPlacePictures(_user.ticket, _place_id, null)));
            }
        }
    }
}
