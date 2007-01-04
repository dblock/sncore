using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountPlaceTest : ManagedCRUDTest<AccountPlace, TransitAccountPlace, ManagedAccountPlace>
    {
        public ManagedAccountPlaceTest()
        {

        }

        public override TransitAccountPlace GetTransitInstance()
        {
            TransitAccountPlace t_instance = new TransitAccountPlace();
            return t_instance;
        }
    }
}
