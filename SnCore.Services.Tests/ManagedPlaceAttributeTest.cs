using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedPlaceAttributeTest : ManagedCRUDTest<PlaceAttribute, TransitPlaceAttribute, ManagedPlaceAttribute>
    {
        public ManagedPlaceAttributeTest()
        {

        }

        public override TransitPlaceAttribute GetTransitInstance()
        {
            TransitPlaceAttribute t_instance = new TransitPlaceAttribute();
            return t_instance;
        }
    }
}
