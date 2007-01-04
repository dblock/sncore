using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedPlacePropertyTest : ManagedCRUDTest<PlaceProperty, TransitPlaceProperty, ManagedPlaceProperty>
    {
        public ManagedPlacePropertyTest()
        {

        }

        public override TransitPlaceProperty GetTransitInstance()
        {
            TransitPlaceProperty t_instance = new TransitPlaceProperty();
            return t_instance;
        }
    }
}
