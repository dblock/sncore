using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedNeighborhoodTest : ManagedCRUDTest<Neighborhood, TransitNeighborhood, ManagedNeighborhood>
    {
        public ManagedNeighborhoodTest()
        {

        }

        public override TransitNeighborhood GetTransitInstance()
        {
            TransitNeighborhood t_instance = new TransitNeighborhood();
            t_instance.Name = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}
