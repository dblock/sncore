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
            t_instance.Description = GetNewString();
            t_instance.Name = GetNewString();
            return t_instance;
        }
    }
}
