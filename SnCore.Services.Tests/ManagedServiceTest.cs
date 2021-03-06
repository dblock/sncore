using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using SnCore.Data.Tests;
using System.Collections;
using NHibernate.Expression;
using System.Diagnostics;
using SnCore.Tools.Web;

namespace SnCore.Services.Tests
{
    public class ManagedServiceTest : NHibernateTest
    {
        private ManagedSecurityContext mAdminSecurityContext;

        public override void SetUp()
        {
            base.SetUp();
            mAdminSecurityContext = ManagedAccount.GetAdminSecurityContext(Session);
        }

        public ManagedSecurityContext AdminSecurityContext
        {
            get
            {
                return mAdminSecurityContext;
            }
        }
    }
}
