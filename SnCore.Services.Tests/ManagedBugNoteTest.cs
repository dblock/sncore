using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedBugNoteTest : ManagedCRUDTest<BugNote, TransitBugNote, ManagedBugNote>
    {
        public ManagedBugTest _bug = new ManagedBugTest();
        public ManagedAccountTest _account = new ManagedAccountTest();

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _account.SetUp();
            _bug.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _bug.TearDown();
            _account.TearDown();
            base.TearDown();
        }

        public ManagedBugNoteTest()
        {

        }

        public override TransitBugNote GetTransitInstance()
        {
            TransitBugNote t_instance = new TransitBugNote();
            t_instance.BugId = _bug.Instance.Id;
            t_instance.AccountId = _account.Instance.Id;
            t_instance.Details = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}
