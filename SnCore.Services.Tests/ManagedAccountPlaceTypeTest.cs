using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountPlaceTypeTest : ManagedCRUDTest<AccountPlaceType, TransitAccountPlaceType, ManagedAccountPlaceType>
    {
        public ManagedAccountPlaceTypeTest()
        {

        }

        public override TransitAccountPlaceType GetTransitInstance()
        {
            TransitAccountPlaceType t_instance = new TransitAccountPlaceType();
            t_instance.Name = Guid.NewGuid().ToString().Substring(0, 23);
            t_instance.Description = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}
