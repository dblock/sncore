using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedPlacePropertyGroupTest : ManagedCRUDTest<PlacePropertyGroup, TransitPlacePropertyGroup, ManagedPlacePropertyGroup>
    {
        public ManagedPlacePropertyGroupTest()
        {

        }

        public override TransitPlacePropertyGroup GetTransitInstance()
        {
            TransitPlacePropertyGroup t_instance = new TransitPlacePropertyGroup();
            t_instance.Description = Guid.NewGuid().ToString();
            t_instance.Name = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}
