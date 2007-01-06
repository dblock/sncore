using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebSystemServiceTests
{
    [TestFixture]
    public class ConfigurationTest : WebServiceTest<WebSystemService.TransitConfiguration>
    {
        public ConfigurationTest()
            : base("Configuration", new WebSystemService.WebSystemService())
        {
        }


        public override WebSystemService.TransitConfiguration GetTransitInstance()
        {
            WebSystemService.TransitConfiguration t_instance = new WebSystemService.TransitConfiguration();
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.Value = Guid.NewGuid().ToString();
            t_instance.Password = false;
            return t_instance;
        }

        [Test]
        public void TestPasswordValue()
        {
            WebSystemService.TransitConfiguration t_instance = GetTransitInstance();
            t_instance.Password = true;
            int id = base.Create(GetAdminTicket(), t_instance);
            string value = (string) base.GetInstancePropertyById(GetAdminTicket(), id, "Value");
            Console.WriteLine("Value: {0}", value);
            Assert.IsEmpty(value);
            base.Delete(GetAdminTicket(), id);
        }
    }
}
