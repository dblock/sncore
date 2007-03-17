using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedNeighborhoodTest : ManagedCRUDTest<Neighborhood, TransitNeighborhood, ManagedNeighborhood>
    {
        private ManagedCityTest _city = new ManagedCityTest();

        [SetUp]
        public override void SetUp()
        {
            _city.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _city.TearDown();
        }

        public ManagedNeighborhoodTest()
        {

        }

        public override TransitNeighborhood GetTransitInstance()
        {
            TransitNeighborhood t_instance = new TransitNeighborhood();
            t_instance.Name = GetNewString();
            t_instance.City = _city.Instance.Name;
            t_instance.Country = _city.Instance.Instance.Country.Name;
            t_instance.State = _city.Instance.Instance.State.Name;
            return t_instance;
        }
    }
}
