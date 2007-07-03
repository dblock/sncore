using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using SnCore.Data.Tests;
using System.Collections;
using NHibernate.Expression;
using Rss;
using Atom.Core;
using System.Net;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Sgml;
using System.Xml;
using System.Text;
using SnCore.Tools.Web;
using System.Reflection;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountFlagTest : ManagedCRUDTest<AccountFlag, TransitAccountFlag, ManagedAccountFlag>
    {
        private ManagedAccountTest _account = new ManagedAccountTest();
        private ManagedAccountFlagTypeTest _type = new ManagedAccountFlagTypeTest();

        [SetUp]
        public override void SetUp()
        {
            _account.SetUp();
            _type.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _type.TearDown();
            _account.TearDown();
        }

        public ManagedAccountFlagTest()
        {

        }

        public override TransitAccountFlag GetTransitInstance()
        {
            TransitAccountFlag t_instance = new TransitAccountFlag();
            t_instance.AccountId = AdminSecurityContext.Account.Id;
            t_instance.FlaggedAccountId = _account.Instance.Id;
            t_instance.Description = GetNewString();
            t_instance.AccountFlagType = _type.Instance.Object.Name;
            return t_instance;
        }
    }
}
