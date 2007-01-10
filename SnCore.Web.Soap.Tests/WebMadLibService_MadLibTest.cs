using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebMadLibServiceTests
{
    [TestFixture]
    public class MadLibTest : WebServiceTest<WebMadLibService.TransitMadLib, WebMadLibServiceNoCache>
    {
        public MadLibTest()
            : base("MadLib")
        {

        }

        public override WebMadLibService.TransitMadLib GetTransitInstance()
        {
            WebMadLibService.TransitMadLib t_instance = new WebMadLibService.TransitMadLib();
            t_instance.AccountId = GetAdminAccount().Id;
            t_instance.Template = Guid.NewGuid().ToString();
            t_instance.Name = Guid.NewGuid().ToString();
            return t_instance;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, GetAdminAccount().Id };
            return args;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, GetAdminAccount().Id, options };
            return args;
        }
    }
}
