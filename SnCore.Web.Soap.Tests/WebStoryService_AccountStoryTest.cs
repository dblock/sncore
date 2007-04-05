using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using System.Threading;

namespace SnCore.Web.Soap.Tests.WebStoryServiceTests
{
    [TestFixture]
    public class AccountStoryTest : WebServiceTest<WebStoryService.TransitAccountStory, WebStoryServiceNoCache>
    {
        public AccountStoryTest()
            : base("AccountStory", "AccountStories")
        {

        }

        public override WebStoryService.TransitAccountStory GetTransitInstance()
        {
            WebStoryService.TransitAccountStory t_instance = new WebStoryService.TransitAccountStory();
            t_instance.AccountId = GetAdminAccount().Id;
            t_instance.Name = GetNewString();
            t_instance.Publish = true;
            t_instance.Summary = GetNewString();
            return t_instance;
        }

        public override object[] GetCountArgs(string ticket)
        {
            WebStoryService.AccountStoryQueryOptions query_options = new WebStoryService.AccountStoryQueryOptions();
            query_options.PublishedOnly = false;
            object[] args = { GetAdminTicket(), GetAdminAccount().Id, query_options };
            return args;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            WebStoryService.AccountStoryQueryOptions query_options = new WebStoryService.AccountStoryQueryOptions();
            query_options.PublishedOnly = false;
            object[] args = { GetAdminTicket(), GetAdminAccount().Id, query_options, options };
            return args;
        }

        [Test]
        public void GetAllStoriesTest()
        {
            int count1 = EndPoint.GetAllAccountStoriesCount(GetAdminTicket());
            int id = Create(GetAdminTicket());
            int count2 = EndPoint.GetAllAccountStoriesCount(GetAdminTicket());
            Assert.AreEqual(count1 + 1, count2);
            WebStoryService.TransitAccountStory[] stories = EndPoint.GetAllAccountStories(GetAdminTicket(), null);
            Assert.AreEqual(stories.Length, count2);
            Delete(GetAdminTicket(), id);
        }

        [Test]
        public void SearchStoriesTest()
        {
            WebStoryService.TransitAccountStory t_instance = GetTransitInstance();
            int id = Create(GetAdminTicket(), t_instance);
            Thread.Sleep(3 * 1000); // wait for the search index to flush
            int count = EndPoint.SearchAccountStoriesCount(GetAdminTicket(), t_instance.Name);
            Console.WriteLine("Search {0}: {1}", t_instance.Name, count);
            Assert.IsTrue(count > 0);
            WebStoryService.TransitAccountStory[] searchresults = EndPoint.SearchAccountStories(GetAdminTicket(), t_instance.Name, null);
            bool bFound = new TransitServiceCollection<WebStoryService.TransitAccountStory>(searchresults).ContainsId(id);
            Assert.IsTrue(bFound);
            Delete(GetAdminTicket(), id);
        }
    }
}
