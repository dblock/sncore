using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountPropertyGroupTest : ManagedCRUDTest<AccountPropertyGroup, TransitAccountPropertyGroup, ManagedAccountPropertyGroup>
    {
        public ManagedAccountPropertyGroupTest()
        {

        }

        public override TransitAccountPropertyGroup GetTransitInstance()
        {
            TransitAccountPropertyGroup t_instance = new TransitAccountPropertyGroup();
            t_instance.Name = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}
