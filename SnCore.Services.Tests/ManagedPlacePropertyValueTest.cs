using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedPlacePropertyValueTest : ManagedCRUDTest<PlacePropertyValue, TransitPlacePropertyValue, ManagedPlacePropertyValue>
    {
        public ManagedPlacePropertyValueTest()
        {

        }

        public override TransitPlacePropertyValue GetTransitInstance()
        {
            TransitPlacePropertyValue t_instance = new TransitPlacePropertyValue();
            return t_instance;
        }
    }
}
