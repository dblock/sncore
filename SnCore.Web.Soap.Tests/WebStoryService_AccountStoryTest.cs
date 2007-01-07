using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using System.Threading;

namespace SnCore.Web.Soap.Tests.WebStoryServiceTests
{
    [TestFixture]
    public class AccountStoryTest : WebServiceTest<WebStoryService.TransitAccountStory>
    {
        public AccountStoryTest()
            : base("AccountStory", "AccountStories", new WebStoryServiceNoCache())
        {

        }

        public override WebStoryService.TransitAccountStory GetTransitInstance()
        {
            WebStoryService.TransitAccountStory t_instance = new WebStoryService.TransitAccountStory();
            t_instance.AccountId = GetUserAccount().Id;
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.Publish = true;
            t_instance.Summary = Guid.NewGuid().ToString();
            return t_instance;
        }

        public override object[] GetCountArgs(string ticket)
        {
            WebStoryService.AccountStoryQueryOptions query_options = new WebStoryService.AccountStoryQueryOptions();
            query_options.PublishedOnly = false;
            object[] args = { ticket, GetUserAccount().Id, query_options };
            return args;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            WebStoryService.AccountStoryQueryOptions query_options = new WebStoryService.AccountStoryQueryOptions();
            query_options.PublishedOnly = false;
            object[] args = { ticket, GetUserAccount().Id, query_options, options };
            return args;
        }

        [Test]
        public void TestGetAllStories()
        {
            WebStoryServiceNoCache endpoint = (WebStoryServiceNoCache)EndPoint;
            int count1 = endpoint.GetAllAccountStoriesCount(GetAdminTicket());
            int id = Create(GetAdminTicket());
            int count2 = endpoint.GetAllAccountStoriesCount(GetAdminTicket());
            Assert.AreEqual(count1 + 1, count2);
            WebStoryService.TransitAccountStory[] stories = endpoint.GetAllAccountStories(GetAdminTicket(), null);
            Assert.AreEqual(stories.Length, count2);
            Delete(GetAdminTicket(), id);
        }

        [Test]
        public void TestSearchStories()
        {
            WebStoryServiceNoCache endpoint = (WebStoryServiceNoCache)EndPoint;
            WebStoryService.TransitAccountStory t_instance = GetTransitInstance();
            int id = Create(GetAdminTicket(), t_instance);
            Thread.Sleep(3 * 1000); // wait for the search index to flush
            int count = endpoint.SearchAccountStoriesCount(GetAdminTicket(), t_instance.Name);
            Console.WriteLine("Search {0}: {1}", t_instance.Name, count);
            Assert.IsTrue(count > 0);
            WebStoryService.TransitAccountStory[] searchresults = endpoint.SearchAccountStories(GetAdminTicket(), t_instance.Name, null);
            bool bFound = false;
            foreach (WebStoryService.TransitAccountStory story in searchresults)
            {
                if (story.Id == id)
                {
                    bFound = true;
                    break;
                }
            }
            Assert.IsTrue(bFound);
            Delete(GetAdminTicket(), id);
        }
    }
}
