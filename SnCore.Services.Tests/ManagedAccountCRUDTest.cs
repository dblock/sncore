using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    public abstract class ManagedAccountCRUDTest<DatabaseType, TransitType, ManagedType> :
        ManagedCRUDTest<DatabaseType, TransitType, ManagedType>
        where ManagedType : IManagedService, new()
        where TransitType : ITransitService, new()
        where DatabaseType : IDbObject, new()
    {
        protected ManagedAccount _account = null;

        public ManagedAccountCRUDTest()
        {

        }

        protected override ManagedSecurityContext GetSecurityContext()
        {
            return _account.GetSecurityContext();
        }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            _account = new ManagedAccount(Session);
            _account.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);
        }

        [TearDown]
        public override void TearDown()
        {
            _account.Delete(AdminSecurityContext);
            _account = null;

            base.TearDown();
        }
    }
}
