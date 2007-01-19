using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountEventInstanceTest : ManagedServiceTest
    {
        public ManagedAccountEventTest _event = new ManagedAccountEventTest();

        [SetUp]
        public override void SetUp()
        {
            _event.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _event.TearDown();
        }

        public ManagedAccountEventInstanceTest()
        {

        }

        [Test]
        public void TestAccountEventInstances()
        {
            TransitAccountEvent t_instance = _event.GetTransitInstance();
            ManagedAccountEvent m_instance = new ManagedAccountEvent(Session);
            m_instance.CreateOrUpdate(t_instance, AdminSecurityContext);

            Nullable<DateTime> previous_startdatetime = null;
            Assert.IsTrue(m_instance.Instance.Schedule.ScheduleInstances.Count > 0);
            foreach (ScheduleInstance schedule in m_instance.Instance.Schedule.ScheduleInstances)
            {
                if (previous_startdatetime.HasValue) Assert.IsTrue(previous_startdatetime < schedule.StartDateTime);
                Console.WriteLine(string.Format("{0}: {1}", schedule.Id, schedule.StartDateTime));
                previous_startdatetime = schedule.StartDateTime;
            }

            m_instance.Delete(AdminSecurityContext);
        }
    }
}
