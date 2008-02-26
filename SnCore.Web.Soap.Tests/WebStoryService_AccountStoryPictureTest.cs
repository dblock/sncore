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
    }
}
