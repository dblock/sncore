using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NHibernate.Expression;
using NHibernate;
using SnCore.Data.Tests;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountGroupTest : ManagedCRUDTest<AccountGroup, TransitAccountGroup, ManagedAccountGroup>
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        public override TransitAccountGroup GetTransitInstance()
        {
            TransitAccountGroup t_instance = new TransitAccountGroup();
            t_instance.Name = GetNewString();
            t_instance.Description = GetNewString();
            t_instance.IsPrivate = false;
            return t_instance;
        }

        public ManagedAccountGroupTest()
        {

        }
    }
}
