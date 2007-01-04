using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedFeatureTest : ManagedCRUDTest<Feature, TransitFeature, ManagedFeature>
    {
        public ManagedFeatureTest()
        {

        }

        public override TransitFeature GetTransitInstance()
        {
            TransitFeature t_instance = new TransitFeature();
            return t_instance;
        }
    }
}
