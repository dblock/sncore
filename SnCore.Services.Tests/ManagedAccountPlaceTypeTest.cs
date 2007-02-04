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
            t_instance.Name = GetNewString().Substring(0, 23);
            t_instance.Description = GetNewString();
            return t_instance;
        }
    }
}
