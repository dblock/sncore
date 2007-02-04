using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedDataObjectTest : ManagedCRUDTest<DataObject, TransitDataObject, ManagedDataObject>
    {
        public ManagedDataObjectTest()
        {

        }

        public override TransitDataObject GetTransitInstance()
        {
            TransitDataObject t_instance = new TransitDataObject();
            t_instance.Name = GetNewString();
            return t_instance;
        }
    }
}
