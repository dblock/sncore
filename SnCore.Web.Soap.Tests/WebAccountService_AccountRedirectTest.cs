using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountRedirectTest : AccountBaseTest<WebAccountService.TransitAccountRedirect>
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

        public AccountRedirectTest()
            : base("AccountRedirect")
        {
        }

        public override WebAccountService.TransitAccountRedirect GetTransitInstance()
        {
            WebAccountService.TransitAccountRedirect t_instance = new WebAccountService.TransitAccountRedirect();
            t_instance.AccountId = _account_id;
            t_instance.SourceUri = GetNewString().Replace('-', 'x');
            t_instance.TargetUri = GetNewString().Replace('-', 'x');
            return t_instance;
        }

        [Test]
        public void GetAccountRedirectByTargetUriTest()
        {
            WebAccountService.TransitAccountRedirect t_instance = GetTransitInstance();
            t_instance.AccountId = GetAdminAccount().Id;
            t_instance.Id = Create(GetAdminTicket(), t_instance);
            WebAccountService.TransitAccountRedirect t_redirect = EndPoint.GetAccountRedirectByTargetUri(
                GetAdminTicket(), GetAdminAccount().Id, t_instance.TargetUri);
            Assert.AreEqual(t_redirect.Id, t_redirect.Id);
            Delete(GetAdminTicket(), t_instance.Id);
        }

        [Test]
        public void GetAccountRedirectBySourceUriTest()
        {
            WebAccountService.TransitAccountRedirect t_instance = GetTransitInstance();
            t_instance.AccountId = GetAdminAccount().Id;
            t_instance.Id = Create(GetAdminTicket(), t_instance);
            WebAccountService.TransitAccountRedirect t_redirect = EndPoint.GetAccountRedirectBySourceUri(
                GetAdminTicket(), GetAdminAccount().Id, t_instance.SourceUri);
            Assert.AreEqual(t_redirect.Id, t_redirect.Id);
            Delete(GetAdminTicket(), t_instance.Id);
        }
    }
}
