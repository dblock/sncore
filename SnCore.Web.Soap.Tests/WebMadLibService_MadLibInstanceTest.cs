using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebMadLibServiceTests
{
    [TestFixture]
    public class MadLibInstanceTest : WebServiceTest<WebMadLibService.TransitMadLibInstance>
    {
        private MadLibTest _madlib = new MadLibTest();
        private int _madlib_id = 0;

        [SetUp]
        public void SetUp()
        {
            _madlib_id = _madlib.Create(GetAdminTicket());
        }

        [TearDown]
        public void TearDown()
        {
            _madlib.Delete(GetAdminTicket(), _madlib_id);
        }

        public MadLibInstanceTest()
            : base("MadLibInstance", new WebMadLibServiceNoCache())
        {

        }

        public override WebMadLibService.TransitMadLibInstance GetTransitInstance()
        {
            WebMadLibService.TransitMadLibInstance t_instance = new WebMadLibService.TransitMadLibInstance();
            t_instance.AccountId = GetUserAccount().Id;
            t_instance.ObjectName = "Place";
            t_instance.ObjectId = 123;
            t_instance.Text = Guid.NewGuid().ToString();
            t_instance.MadLibId = _madlib_id;
            return t_instance;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, "Place", 123 };
            return args;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, "Place", 123, options };
            return args;
        }
    }
}
