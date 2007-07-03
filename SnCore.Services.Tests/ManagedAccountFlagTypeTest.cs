using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountFlagTypeTest : ManagedCRUDTest<AccountFlagType, TransitAccountFlagType, ManagedAccountFlagType>
    {
        public ManagedAccountFlagTypeTest()
        {

        }

        public override TransitAccountFlagType GetTransitInstance()
        {
            TransitAccountFlagType t_instance = new TransitAccountFlagType();
            t_instance.Name = GetNewString();
            return t_instance;
        }
    }
}
