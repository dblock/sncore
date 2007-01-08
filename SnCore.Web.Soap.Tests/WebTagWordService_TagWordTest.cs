using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebTagWordServiceTests
{
    [TestFixture]
    public class TagWordTest : WebServiceTest<WebTagWordService.TransitTagWord>
    {
        public TagWordTest()
            : base("TagWord", new WebTagWordServiceNoCache())
        {

        }

        public override WebTagWordService.TransitTagWord GetTransitInstance()
        {
            WebTagWordService.TransitTagWord t_instance = new WebTagWordService.TransitTagWord();
            t_instance.Excluded = false;
            t_instance.Frequency = 1;
            t_instance.Promoted = false;
            t_instance.Word = Guid.NewGuid().ToString();
            return t_instance;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, WebTagWordService.TransitTagWordQueryOptions.New };
            return args;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, WebTagWordService.TransitTagWordQueryOptions.New, options };
            return args;
        }

        [Test]
        public void GetTagWordAccountsByIdTest()
        {
            int id = Create(GetAdminTicket());
            WebTagWordServiceNoCache endpoint = (WebTagWordServiceNoCache) EndPoint;
            WebTagWordService.TransitAccount[] accounts = endpoint.GetTagWordAccountsById(GetUserTicket(), id, null);
            Assert.IsNotNull(accounts);
            Console.WriteLine("Found {0} accounts.", accounts.Length);
            Delete(GetAdminTicket(), id);
        }

        [Test]
        public void SearchTagWordAccountsTest()
        {
            WebTagWordService.TransitTagWord t_instance = GetTransitInstance();
            int id = Create(GetAdminTicket(), t_instance);
            WebTagWordServiceNoCache endpoint = (WebTagWordServiceNoCache)EndPoint;
            WebTagWordService.TransitAccount[] accounts = endpoint.SearchTagWordAccounts(GetUserTicket(), "khtzw" /*t_instance.Word*/, null);
            Console.WriteLine("Found {0} accounts.", accounts.Length);
            Assert.IsNotNull(accounts);
            Delete(GetAdminTicket(), id);
        }
    }
}
