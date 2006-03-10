using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using SnCore.Data.Tests;
using System.Collections;
using SnCore.Services;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using NHibernate.Expression;

namespace SnCore.Stress.Tests
{
    [TestFixture]
    public class CreateAccountsStressTest : NHibernateTest
    {
        public CreateAccountsStressTest()
        {

        }

        [Test]
        public void Create100Accounts()
        {
            CreateManyAccounts(100);
        }

        [Test]
        public void Create1000Accounts()
        {
            CreateManyAccounts(1000);
        }

        [Test]
        public void Create10000Accounts()
        {
            CreateManyAccounts(10000);
        }

        [Test]
        public void Create3StoriesPerAccount()
        {
            CreateManyStoriesPerAccount(3, 10);
        }

        public void CreateManyAccounts(int count)
        {
            TransitCountry tc = new TransitCountry();
            tc.Name = Guid.NewGuid().ToString();
            TransitState ts = new TransitState();
            ts.Name = Guid.NewGuid().ToString();
            ts.Country = tc.Name;

            ManagedCountry c = new ManagedCountry(Session);
            c.Create(tc);

            ManagedState s = new ManagedState(Session);
            s.Create(ts);

            TransitAccountAddress ta = new TransitAccountAddress();
            ta.Apt = "123";
            ta.City = "New York";
            ta.Country = tc.Name;
            ta.Name = "My Address";
            ta.State = ts.Name;
            ta.Street = "Houston St.";
            ta.Zip = "10001";

            for (int i = 0; i < count; i++)
            {

                ManagedAccount a = new ManagedAccount(Session);
                string name = Guid.NewGuid().ToString();
                a.Create(
                    name,
                    "password",
                    string.Format("{0}@localhost.com", name),
                    DateTime.UtcNow);

                a.CreateOrUpdate(ta);
            }
        }

        public void CreateManyStoriesPerAccount(int count, int wordsperstory)
        {
            Random r = new Random();
            IList accounts = Session.CreateCriteria(typeof(Account)).List();

            foreach (Account a in accounts)
            {
                for (int i = 0; i < count; i++)
                {
                    TransitAccountStory s = new TransitAccountStory();
                    s.Name = Guid.NewGuid().ToString();

                    List<string> words = new List<string>(wordsperstory);

                    for (int w = 0; w < wordsperstory; w++)
                    {
                        StringBuilder word = new StringBuilder();
                        int len = r.Next() % 10 + 1;
                        for (int l = 0; l < len; l++)
                        {
                            word.Append((char)(r.Next() % 26 + 'a'));
                        }

                        s.Summary = s.Summary + word.ToString() + (w == 0 ? "" : " ");
                    }

                    ManagedAccount ma = new ManagedAccount(Session, a);
                    ma.CreateOrUpdate(s);
                }
            }
        }

    }
}
