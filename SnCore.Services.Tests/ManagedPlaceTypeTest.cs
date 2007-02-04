using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedPlaceTypeTest : ManagedCRUDTest<PlaceType, TransitPlaceType, ManagedPlaceType>
    {
        public ManagedPlaceTypeTest()
        {

        }

        public override TransitPlaceType GetTransitInstance()
        {
            TransitPlaceType t_instance = new TransitPlaceType();
            t_instance.Name = GetNewString();
            return t_instance;
        }
    }
}
