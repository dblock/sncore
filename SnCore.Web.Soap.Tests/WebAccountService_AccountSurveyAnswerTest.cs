using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountSurveyAnswerTest : WebServiceTest<WebAccountService.TransitAccountSurveyAnswer, WebAccountServiceNoCache>
    {
        public AccountSurveyAnswerTest()
            : base("AccountSurveyAnswer")
        {
        }


        public override WebAccountService.TransitAccountSurveyAnswer GetTransitInstance()
        {
            WebAccountService.TransitAccountSurveyAnswer t_instance = new WebAccountService.TransitAccountSurveyAnswer();
            return t_instance;
        }
    }
}
