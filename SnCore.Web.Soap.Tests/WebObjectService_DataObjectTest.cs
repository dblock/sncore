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
        protected void GetDataObjectFieldsByIdTest()
        {

        }

        [Test]
        protected void DeleteAllFeaturesTest()
        {

        }
    }
}
