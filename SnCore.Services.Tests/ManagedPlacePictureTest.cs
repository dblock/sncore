using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedPlacePictureTest : ManagedCRUDTest<PlacePicture, TransitPlacePicture, ManagedPlacePicture>
    {
        public ManagedPlacePictureTest()
        {

        }

        public override TransitPlacePicture GetTransitInstance()
        {
            TransitPlacePicture t_instance = new TransitPlacePicture();
            t_instance.Name = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}
