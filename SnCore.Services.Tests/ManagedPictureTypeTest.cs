using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedPictureTypeTest : ManagedCRUDTest<PictureType, TransitPictureType, ManagedPictureType>
    {
        public ManagedPictureTypeTest()
        {

        }

        public override TransitPictureType GetTransitInstance()
        {
            TransitPictureType t_instance = new TransitPictureType();
            return t_instance;
        }
    }
}
