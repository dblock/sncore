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
            _account.SetUp();
            _bug.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _bug.TearDown();
            _account.TearDown();
        }

        public ManagedBugNoteTest()
        {

        }

        public override TransitBugNote GetTransitInstance()
        {
            TransitBugNote t_instance = new TransitBugNote();
            t_instance.BugId = _bug.Instance.Id;
            t_instance.AccountId = _account.Instance.Id;
            t_instance.Details = GetNewString();
            return t_instance;
        }
    }
}
