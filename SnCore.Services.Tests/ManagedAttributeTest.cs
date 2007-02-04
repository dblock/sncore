using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAttributeTest : ManagedCRUDTest<Attribute, TransitAttribute, ManagedAttribute>
    {
        public ManagedAttributeTest()
        {

        }

        public override TransitAttribute GetTransitInstance()
        {
            TransitAttribute t_instance = new TransitAttribute();
            t_instance.DefaultUrl = GetNewUri();
            t_instance.DefaultValue = GetNewString();
            t_instance.Description = GetNewString();
            t_instance.Name = GetNewString();
            return t_instance;
        }
    }
}
