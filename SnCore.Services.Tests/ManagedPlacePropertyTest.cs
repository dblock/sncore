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
            _group.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _group.TearDown();
        }

        public ManagedPlacePropertyTest()
        {

        }

        public override TransitPlaceProperty GetTransitInstance()
        {
            TransitPlaceProperty t_instance = new TransitPlaceProperty();
            t_instance.DefaultValue = GetNewString();
            t_instance.Description = GetNewString();
            t_instance.Name = GetNewString();
            t_instance.PlacePropertyGroupId = _group.Instance.Id;
            t_instance.TypeName = "System.String";
            t_instance.Publish = true;
            return t_instance;
        }
    }
}
