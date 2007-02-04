using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebStatsServiceTests
{
    [TestFixture]
    public class RefererAccountTest : WebServiceTest<WebStatsService.TransitRefererAccount, WebStatsServiceNoCache>
    {
        RefererHostTest _host = new RefererHostTest();
        private int _host_id = 0;

        public RefererAccountTest()
            : base("RefererAccount")
        {

        }

        [SetUp]
        public override void SetUp()
        {
            _host_id = _host.Create(GetAdminTicket());
        }

        [TearDown]
        public override void TearDown()
        {
            _host.Delete(GetAdminTicket(), _host_id);
        }

        public override WebStatsService.TransitRefererAccount GetTransitInstance()
        {
            WebStatsService.TransitRefererAccount t_instance = new WebStatsService.TransitRefererAccount();
            t_instance.AccountId = base.GetUserAccount().Id;
            t_instance.RefererHostLastRefererUri = GetNewUri();
            t_instance.RefererHostName = (string)_host.GetInstancePropertyById(GetAdminTicket(), _host_id, "Host");
            t_instance.RefererHostTotal = 1;
            return t_instance;
        }
    }
}
