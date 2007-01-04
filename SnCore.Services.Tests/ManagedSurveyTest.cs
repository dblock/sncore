using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedSurveyTest : ManagedCRUDTest<Survey, TransitSurvey, ManagedSurvey>
    {
        public ManagedSurveyTest()
        {

        }

        public override TransitSurvey GetTransitInstance()
        {
            TransitSurvey t_instance = new TransitSurvey();
            t_instance.Name = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}
