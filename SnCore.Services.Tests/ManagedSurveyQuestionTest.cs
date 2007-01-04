using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedSurveyQuestionTest : ManagedCRUDTest<SurveyQuestion, TransitSurveyQuestion, ManagedSurveyQuestion>
    {
        public ManagedSurveyQuestionTest()
        {

        }

        public override TransitSurveyQuestion GetTransitInstance()
        {
            TransitSurveyQuestion t_instance = new TransitSurveyQuestion();
            return t_instance;
        }
    }
}
