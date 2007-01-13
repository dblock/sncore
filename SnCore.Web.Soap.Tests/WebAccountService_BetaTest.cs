using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SnCore.Web.Soap.Tests.WebSystemServiceTests;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class BetaTest : WebServiceBaseTest<WebAccountServiceNoCache>
    {
        [Test]
        public void DeleteBetaPasswordTest()
        {
            try
            {
                WebSystemServiceNoCache system = new WebSystemServiceNoCache();
                WebSystemService.TransitConfiguration t_betapassword = system.GetConfigurationByName(GetAdminTicket(), "SnCore.Beta.Password");
                system.DeleteConfiguration(GetAdminTicket(), t_betapassword.Id);
            }
            catch (SoapException)
            {
            }

            Assert.IsFalse(EndPoint.IsBetaPasswordSet());
        }

        [Test]
        public void IsBetaPasswordSetAndVerifyTest()
        {
            DeleteBetaPasswordTest();
            Assert.IsFalse(EndPoint.IsBetaPasswordSet());

            WebSystemServiceNoCache system = new WebSystemServiceNoCache();
            WebSystemService.TransitConfiguration t_betapassword = new WebSystemService.TransitConfiguration();
            t_betapassword = new WebSystemService.TransitConfiguration();
            t_betapassword.Name = "SnCore.Beta.Password";
            t_betapassword.Value = Guid.NewGuid().ToString();
            t_betapassword.Password = true;
            t_betapassword.Id = system.CreateOrUpdateConfiguration(GetAdminTicket(), t_betapassword);
            Assert.IsTrue(EndPoint.IsBetaPasswordSet());

            EndPoint.VerifyBetaPassword(t_betapassword.Value);

            try
            {
                EndPoint.VerifyBetaPassword(Guid.NewGuid().ToString());
                Assert.IsTrue(false, "Beta password verified when invalid.");
            }
            catch (SoapException)
            {

            }

            system.DeleteConfiguration(GetAdminTicket(), t_betapassword.Id);
            Assert.IsFalse(EndPoint.IsBetaPasswordSet());
        }
    }
}
