using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedPlacePropertyTest : ManagedCRUDTest<PlaceProperty, TransitPlaceProperty, ManagedPlaceProperty>
    {
        private ManagedPlacePropertyGroupTest _group = new ManagedPlacePropertyGroupTest();

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _group.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _group.TearDown();
            base.TearDown();
        }


        public ManagedPlacePropertyTest()
        {

        }

        public override TransitPlaceProperty GetTransitInstance()
        {
            TransitPlaceProperty t_instance = new TransitPlaceProperty();
            t_instance.DefaultValue = Guid.NewGuid().ToString();
            t_instance.Description = Guid.NewGuid().ToString();
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.PlacePropertyGroupId = _group.Instance.Id;
            t_instance.TypeName = "System.String";
            t_instance.Publish = true;
            return t_instance;
        }
    }
}
