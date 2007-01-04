using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedRefererAccountTest : ManagedCRUDTest<RefererAccount, TransitRefererAccount, ManagedRefererAccount>
    {
        public ManagedRefererAccountTest()
        {

        }

        public override TransitRefererAccount GetTransitInstance()
        {
            TransitRefererAccount t_instance = new TransitRefererAccount();
            return t_instance;
        }
    }
}
