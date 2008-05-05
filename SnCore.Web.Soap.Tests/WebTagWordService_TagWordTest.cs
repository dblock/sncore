using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebTagWordServiceTests
{
    [TestFixture]
    public class TagWordTest : WebServiceTest<WebTagWordService.TransitTagWord, WebTagWordServiceNoCache>
    {
        private UserInfo _user = null;

        public override void SetUp()
        {
            base.SetUp();
            _user = CreateUserWithVerifiedEmailAddress();
        }

        public override void TearDown()
        {
            DeleteUser(_user.id);
            base.TearDown();
        }

        public TagWordTest()
            : base("TagWord")
        {

        }

        public override WebTagWordService.TransitTagWord GetTransitInstance()
        {
            WebTagWordService.TransitTagWord t_instance = new WebTagWordService.TransitTagWord();
            t_instance.Excluded = false;
            t_instance.Frequency = 1;
            t_instance.Promoted = false;
            t_instance.Word = GetNewString();
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
            WebTagWordService.TransitAccount[] accounts = EndPoint.GetTagWordAccountsById(_user.ticket, id, null);
            Assert.IsNotNull(accounts);
            Console.WriteLine("Found {0} accounts.", accounts.Length);
            Delete(GetAdminTicket(), id);
        }

        [Test]
        public void SearchTagWordAccountsTest()
        {
            WebTagWordService.TransitTagWord t_instance = GetTransitInstance();
            int id = Create(GetAdminTicket(), t_instance);
            WebTagWordService.TransitAccount[] accounts = EndPoint.SearchTagWordAccounts(_user.ticket, "khtzw" /*t_instance.Word*/, null);
            Console.WriteLine("Found {0} accounts.", accounts.Length);
            Assert.IsNotNull(accounts);
            Delete(GetAdminTicket(), id);
        }
    }
}
