using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SnCore.BackEndServices;
using SnCore.Data.Tests;
using SnCore.Services;

namespace SnCore.BackEnd.Tests
{
    [TestFixture]
    public class SystemsTest : NHibernateTest
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

        [Test]
        public void CleanupTestGeneratedContent()
        {
            ManagedSecurityContext sec = ManagedAccount.GetAdminSecurityContext(Session);
            CleanupTestGenerated<ManagedAccountEvent, AccountEvent>(sec);
            CleanupTestGenerated<ManagedAccountGroup, AccountGroup>(sec);
            CleanupTestGenerated<ManagedPlace, Place>(sec);
            CleanupTestGenerated<ManagedAccountMessageFolder, AccountMessageFolder>(sec);
            CleanupTestGenerated<ManagedAccount, Account>(sec);
            CleanupTestGenerated<ManagedBug, Bug>(sec, "Subject");
            CleanupTestGenerated<ManagedCampaign, Campaign>(sec);
            CleanupTestGenerated<ManagedRefererHost, RefererHost>(sec, "Host");
            CleanupTestGenerated<ManagedNeighborhood, Neighborhood>(sec);
            CleanupTestGenerated<ManagedCity, City>(sec);
            CleanupTestGenerated<ManagedState, State>(sec);
            CleanupTestGenerated<ManagedCountry, Country>(sec);
            CleanupTestGenerated<ManagedReminder, Reminder>(sec, "Url");
            CleanupTestGenerated<ManagedSurvey, Survey>(sec);
            CleanupTestGenerated<ManagedTagWord, TagWord>(sec, "Word");
            CleanupTestGenerated<ManagedDiscussion, Discussion>(sec);
            CleanupTestGenerated<ManagedCounter, Counter>(sec, "Uri");
        }

        public void CleanupTestGenerated<ManagedType, DatabaseType>(ManagedSecurityContext sec)
            where DatabaseType : IDbObject
            where ManagedType : IManagedService, new()
        {
            CleanupTestGenerated<ManagedType, DatabaseType>(sec, "Name");
        }

        public void CleanupTestGenerated<ManagedType, DatabaseType>(ManagedSecurityContext sec, string col)
            where DatabaseType: IDbObject
            where ManagedType : IManagedService, new()
        {
            // fetch all places with names like GUID
            IEnumerable<DatabaseType> instances = Session.CreateQuery(string.Format(
                "FROM {0} instance WHERE {1} LIKE '%-%-%'", typeof(DatabaseType).Name, col))
                .Enumerable<DatabaseType>();
            IEnumerator<DatabaseType> enumerator = instances.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Console.WriteLine("{0}: {1}", typeof(DatabaseType).Name, enumerator.Current.Id);
                ManagedType m_instance = new ManagedType();
                m_instance.LoadInstance(Session, enumerator.Current.Id);
                m_instance.Delete(sec);
            }
            Session.Flush();
        }
    }
}
