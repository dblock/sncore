using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Expression;
using NHibernate;
using SnCore.Data.Tests;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedPlaceChangeRequestTest : ManagedCRUDTest<PlaceChangeRequest, TransitPlaceChangeRequest, ManagedPlaceChangeRequest>
    {
        private ManagedAccountTest _account = new ManagedAccountTest();
        private ManagedPlaceTest _place = new ManagedPlaceTest();
        private ManagedCityTest _city = new ManagedCityTest();
        private ManagedPlaceTypeTest _type = new ManagedPlaceTypeTest();

        [SetUp]
        public override void SetUp()
        {
            _place.SetUp();
            _account.SetUp();
            _city.SetUp();
            _type.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _place.TearDown();
            _account.TearDown();
            _city.TearDown();
            _type.TearDown();
        }

        public ManagedPlaceChangeRequestTest()
        {

        }

        public override TransitPlaceChangeRequest GetTransitInstance()
        {
            TransitPlaceChangeRequest t_instance = new TransitPlaceChangeRequest();
            t_instance.PlaceId = _place.Instance.Id;
            t_instance.AccountId = _account.Instance.Id;
            t_instance.City = _city.Instance.Name;
            t_instance.Country = _city.Instance.Instance.Country.Name;
            t_instance.Name = GetNewString();
            t_instance.State = _city.Instance.Instance.State.Name;
            t_instance.Type = _type.Instance.Instance.Name;
            t_instance.Website = GetNewUri();
            return t_instance;
        }
    }
}
