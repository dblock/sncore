using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebSystemServiceTests
{
    [TestFixture]
    public class ConfigurationTest : WebServiceTest<WebSystemService.TransitConfiguration, WebSystemServiceNoCache>
    {
        public ConfigurationTest()
            : base("Configuration")
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
        public void PasswordValueTest()
        {
            WebSystemService.TransitConfiguration t_instance = GetTransitInstance();
            t_instance.Password = true;
            int id = base.Create(GetAdminTicket(), t_instance);
            string value = (string) base.GetInstancePropertyById(GetAdminTicket(), id, "Value");
            Console.WriteLine("Value: {0}", value);
            Assert.IsEmpty(value);
            base.Delete(GetAdminTicket(), id);
        }

        [Test]
        public void GetConfigurationByNameTest()
        {
            WebSystemService.TransitConfiguration t_instance = GetTransitInstance();
            int id = base.Create(GetAdminTicket(), t_instance);
            WebSystemService.TransitConfiguration t_instance2 = EndPoint.GetConfigurationByName(GetAdminTicket(), t_instance.Name);
            Assert.AreEqual(id, t_instance2.Id);
            Assert.AreEqual(t_instance.Name, t_instance2.Name);
            Assert.AreEqual(t_instance.Value, t_instance2.Value);
            base.Delete(GetAdminTicket(), id);
        }

        [Test]
        public void GetConfigurationValueByNameTest()
        {
            WebSystemService.TransitConfiguration t_instance = GetTransitInstance();
            int id = base.Create(GetAdminTicket(), t_instance);
            string value = EndPoint.GetConfigurationValue(GetAdminTicket(), t_instance.Name);
            Assert.AreEqual(t_instance.Value, value);
            base.Delete(GetAdminTicket(), id);
        }

        [Test]
        public void GetConfigurationValueByNameWithDefaultTest()
        {
            WebSystemService.TransitConfiguration t_instance = GetTransitInstance();
            int id = base.Create(GetAdminTicket(), t_instance);
            string value = EndPoint.GetConfigurationValueWithDefault(GetAdminTicket(), t_instance.Name, Guid.NewGuid().ToString());
            Assert.AreEqual(t_instance.Value, value);
            string defaultvalue = Guid.NewGuid().ToString();
            string value2 = EndPoint.GetConfigurationValueWithDefault(GetAdminTicket(), Guid.NewGuid().ToString(), defaultvalue);
            Assert.AreNotEqual(t_instance.Value, value2);
            Assert.AreEqual(value2, defaultvalue);
            base.Delete(GetAdminTicket(), id);
        }

    }
}
