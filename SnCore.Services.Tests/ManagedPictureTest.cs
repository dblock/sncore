using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedPictureTest : ManagedCRUDTest<Picture, TransitPicture, ManagedPicture>
    {
        public ManagedPictureTest()
        {

        }

        public override TransitPicture GetTransitInstance()
        {
            TransitPicture t_instance = new TransitPicture();
            t_instance.Name = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}
