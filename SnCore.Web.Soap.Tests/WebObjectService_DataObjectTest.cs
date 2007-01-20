using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebObjectServiceTests
{
    [TestFixture]
    public class DataObjectTest : WebServiceTest<WebObjectService.TransitDataObject, WebObjectServiceNoCache>
    {
        public DataObjectTest()
            : base("DataObject")
        {
        }


        public override WebObjectService.TransitDataObject GetTransitInstance()
        {
            WebObjectService.TransitDataObject t_instance = new WebObjectService.TransitDataObject();
            t_instance.Name = Guid.NewGuid().ToString();
            return t_instance;
        }

        [Test]
        public void GetDataObjectFieldsByIdTest()
        {
            bool bFound = false;
            // find the Account data object
            WebObjectService.TransitDataObject[] objects = EndPoint.GetDataObjects(GetAdminTicket(), null);
            foreach(WebObjectService.TransitDataObject instance in objects)
            {
                if (instance.Name == "Account")
                {
                    bFound = true;
                    Console.WriteLine("Account Data Object: {0}", instance.Id);
                    string[] fields = EndPoint.GetDataObjectFieldsById(GetAdminTicket(), instance.Id);
                    Console.WriteLine("Fields: {0}", fields.Length);
                    Assert.IsTrue(fields.Length > 0);
                    foreach (string field in fields)
                    {
                        Console.WriteLine("Field: {0}", field);
                    }
                }
            }
            Assert.IsTrue(bFound);
        }
    }
}
