using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedReminderTest : ManagedCRUDTest<Reminder, TransitReminder, ManagedReminder>
    {
        private ManagedDataObjectTest _dataobject = new ManagedDataObjectTest();

        [SetUp]
        public override void SetUp()
        {
            _dataobject.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _dataobject.TearDown();
        }

        public ManagedReminderTest()
        {

        }

        public override TransitReminder GetTransitInstance()
        {
            TransitReminder t_instance = new TransitReminder();
            t_instance.DataObject_Id = _dataobject.Instance.Id;
            t_instance.DataObjectField = GetNewString();
            t_instance.Url = GetNewUri();
            t_instance.LastRun = DateTime.UtcNow;
            return t_instance;
        }
    }
}
