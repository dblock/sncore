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

        public override void EndToEndTest()
        {
            string ticket = GetTestTicket();
            int count1 = GetCount(ticket);
            int id1 = Create(ticket);
            int count2 = GetCount(ticket);
            Assert.IsTrue(count2 >= 0 && count1 + 1 == count2);
            int id2 = (int)GetInstancePropertyById(ticket, id1, "Id");
            Assert.AreEqual(id1, id2);
            int count3 = GetMany(ticket, null);
            Assert.IsTrue(count2 == count3);
            int count4 = GetMany(ticket, GetServiceQueryOptions(0, 0));
            Assert.IsTrue(count2 == count4);
            const int page_size = 10;
            int count5 = GetMany(ticket, GetServiceQueryOptions(0, page_size));
            Assert.IsTrue(count5 >= 1 && count5 <= page_size);
            Delete(ticket, id1);
            int count6 = GetCount(ticket);
            // deleted configurations are added as unset, count will be the same after delete
            Assert.IsTrue(count1 == count3);
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
