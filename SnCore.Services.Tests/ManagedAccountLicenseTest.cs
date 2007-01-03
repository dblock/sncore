using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountLicenseTest : ManagedAccountCRUDTest<AccountLicense, TransitAccountLicense, ManagedAccountLicense>
    {
        public ManagedAccountLicenseTest()
        {

        }

        public override TransitAccountLicense GetTransitInstance()
        {
            TransitAccountLicense t_instance = new TransitAccountLicense();
            t_instance.ImageUrl = Guid.NewGuid().ToString();
            t_instance.LicenseUrl = Guid.NewGuid().ToString(); 
            t_instance.Name = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}
