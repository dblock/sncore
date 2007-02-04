using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebDiscussionServiceTests
{
    [TestFixture]
    public class DiscussionTest : WebServiceTest<WebDiscussionService.TransitDiscussion, WebDiscussionServiceNoCache>
    {
        public DiscussionTest()
            : base("Discussion")
        {

        }

        public override WebDiscussionService.TransitDiscussion GetTransitInstance()
        {
            WebDiscussionService.TransitDiscussion t_instance = new WebDiscussionService.TransitDiscussion();
            t_instance.Name = GetNewString();
            t_instance.Personal = false;
            t_instance.Description = GetNewString();
            return t_instance;
        }

        [Test]
        public void GetAccountDiscussionsTest()
        {
            int count = EndPoint.GetAccountDiscussionsCount(GetAdminTicket(), GetAdminAccount().Id);
            Console.WriteLine("Discussions: {0}", count);
            WebDiscussionService.TransitDiscussion[] discussions = EndPoint.GetAccountDiscussions(GetAdminTicket(), GetAdminAccount().Id, null);
            Assert.AreEqual(count, discussions.Length);
        }

    }
}
