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
            t_instance.Name = GetNewString();
            t_instance.Value = GetNewString();
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
            string value = EndPoint.GetConfigurationByNameValueWithDefault(GetAdminTicket(), t_instance.Name, GetNewString());
            Assert.AreEqual(t_instance.Value, value);
            string defaultvalue = GetNewString();
            string value2 = EndPoint.GetConfigurationByNameValueWithDefault(GetAdminTicket(), GetNewString(), defaultvalue);
            Assert.AreNotEqual(t_instance.Value, value2);
            Assert.AreEqual(value2, defaultvalue);
            base.Delete(GetAdminTicket(), id);
        }

        [Test]
        public void GetConfigurationByNameWithDefaultTest()
        {
            WebSystemService.TransitConfiguration t_instance = GetTransitInstance();
            int id = base.Create(GetAdminTicket(), t_instance);
            WebSystemService.TransitConfiguration t_instance_2 = EndPoint.GetConfigurationByNameWithDefault(GetAdminTicket(), t_instance.Name, GetNewString());
            Assert.AreEqual(t_instance.Value, t_instance_2.Value);
            string defaultvalue = GetNewString();
            WebSystemService.TransitConfiguration t_instance_3 = EndPoint.GetConfigurationByNameWithDefault(GetAdminTicket(), GetNewString(), defaultvalue);
            Assert.AreNotEqual(t_instance.Value, t_instance_3.Value);
            Assert.AreEqual(t_instance_3.Value, defaultvalue);
            base.Delete(GetAdminTicket(), id);
        }
    }
}
