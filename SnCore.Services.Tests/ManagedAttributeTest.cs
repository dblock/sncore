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
            t_instance.DefaultUrl = string.Format("http://uri/{0}", Guid.NewGuid());
            t_instance.DefaultValue = Guid.NewGuid().ToString();
            t_instance.Description = Guid.NewGuid().ToString();
            t_instance.Name = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}
