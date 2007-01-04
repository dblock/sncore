using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountSurveyAnswerTest : ManagedCRUDTest<AccountSurveyAnswer, TransitAccountSurveyAnswer, ManagedAccountSurveyAnswer>
    {
        public ManagedAccountSurveyAnswerTest()
        {

        }

        public override TransitAccountSurveyAnswer GetTransitInstance()
        {
            TransitAccountSurveyAnswer t_instance = new TransitAccountSurveyAnswer();
            return t_instance;
        }
    }
}
