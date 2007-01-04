using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedPlaceNameTest : ManagedCRUDTest<PlaceName, TransitPlaceName, ManagedPlaceName>
    {
        public ManagedPlaceNameTest()
        {

        }

        public override TransitPlaceName GetTransitInstance()
        {
            TransitPlaceName t_instance = new TransitPlaceName();
            t_instance.Name = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}
