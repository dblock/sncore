using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using NHibernate.Cfg;

namespace SnCore.Data.Tests
{
    public abstract class NHibernateCrudTest<ObjectType> : NHibernateTest
        where ObjectType : IDbObject
    {
        public abstract ObjectType GetNewObject();

        public NHibernateCrudTest()
        {

        }

        [Test]
        public void CreateAndDelete()
        {
            ObjectType obj = GetNewObject();
            Console.WriteLine("Creating " + obj.ToString());
            Session.Save(obj);
            Session.Flush();
            Session.Delete(obj);
            Session.Flush();
        }

        [Test]
        public void RetrieveAndUpdate()
        {
            ObjectType obj = GetNewObject();
            Session.Save(obj);
            Session.Flush();
            Console.WriteLine("Retrieving " + obj.ToString());
            ObjectType objcopy = Session.Get<ObjectType>(obj.Id);
            Console.WriteLine("Retrieved " + objcopy.ToString());
            Session.Update(objcopy, obj.Id);
            Session.Flush();
            Session.Delete(obj);
            Session.Flush();
        }

        [Test]
        public void CountMany()
        {
            ObjectType obj = GetNewObject();
            Session.Save(obj);
            Session.Flush();
            Console.WriteLine("Counting " + obj.ToString());
            int count = Session.CreateQuery(string.Format(
                "SELECT COUNT(*) FROM {0}", obj.GetType().Name)).UniqueResult<int>();
            Console.WriteLine("Count: " + count);
            Session.Delete(obj);
            Session.Flush();
        }

        [Test]
        public void GetMaxId()
        {
            ObjectType obj = GetNewObject();
            Session.Save(obj);
            Session.Flush();
            Console.WriteLine("Getting max Id of " + obj.ToString());
            int max = Session.CreateQuery(string.Format(
                "SELECT MAX({0}.Id) FROM {0} {0}", obj.GetType().Name)).UniqueResult<int>();
            Console.WriteLine("Max: " + max);
            Session.Delete(obj);
            Session.Flush();
        }

        [Test]
        public void GetSumId()
        {
            ObjectType obj = GetNewObject();
            Session.Save(obj);
            Session.Flush();
            Console.WriteLine("Getting sum Id of " + obj.ToString());
            int sum = Session.CreateQuery(string.Format(
                "SELECT SUM({0}.Id) FROM {0} {0}", obj.GetType().Name)).UniqueResult<int>();
            Console.WriteLine("Sum: " + sum);
            Session.Delete(obj);
            Session.Flush();
        }

        [Test]
        public void GetAvgId()
        {
            ObjectType obj = GetNewObject();
            Session.Save(obj);
            Session.Flush();
            Console.WriteLine("Getting avg Id of " + obj.ToString());
            float avg = Session.CreateQuery(string.Format(
                "SELECT AVG({0}.Id) FROM {0} {0}", obj.GetType().Name)).UniqueResult<float>();
            Console.WriteLine("Avg: " + avg);
            Session.Delete(obj);
            Session.Flush();
        }

    }
}
