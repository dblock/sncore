using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using SnCore.Tools.Drawing;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountPictureTest : AccountBaseTest<WebAccountService.TransitAccountPicture>
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

        public AccountPictureTest()
            : base("AccountPicture")
        {

        }

        public override WebAccountService.TransitAccountPicture GetTransitInstance()
        {
            WebAccountService.TransitAccountPicture t_instance = new WebAccountService.TransitAccountPicture();
            t_instance.AccountId = _account_id;
            t_instance.Description = GetNewString();
            t_instance.Name = GetNewString();
            t_instance.Bitmap = GetNewBitmap();
            return t_instance;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            WebAccountService.AccountPicturesQueryOptions qopt = new WebAccountService.AccountPicturesQueryOptions();
            qopt.Hidden = true;
            object[] args = { ticket, _account_id, qopt, options };
            return args;
        }

        public override object[] GetCountArgs(string ticket)
        {
            WebAccountService.AccountPicturesQueryOptions qopt = new WebAccountService.AccountPicturesQueryOptions();
            qopt.Hidden = true;
            object[] args = { ticket, _account_id, qopt };
            return args;
        }
    }
}
