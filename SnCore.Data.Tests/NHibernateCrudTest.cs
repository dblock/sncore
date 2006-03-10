using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using NHibernate.Cfg;

namespace SnCore.Data.Tests
{
    public abstract class NHibernateCrudTest : NHibernateTest
    {
        public abstract object Object { get; }
        public abstract int ObjectId { get; }

        public NHibernateCrudTest()
        {

        }

        [Test]
        public void CreateAndDelete()
        {
            Console.WriteLine("Creating " + Object.ToString());
            Session.SaveOrUpdate(Object);
            Session.Flush();
            Session.Delete(Object);
            Session.Flush();
        }

        [Test]
        public void RetrieveAndUpdate()
        {
            Session.Save(Object);
            Session.Flush();
            Console.WriteLine("Retrieving " + Object.ToString());
            object o = Session.Get(Object.GetType(), ObjectId);
            Console.WriteLine("Retrieved " + o.ToString());
            Session.Update(o, ObjectId);
            Session.Flush();
            Session.Delete(Object);
            Session.Flush();
        }
    }
}
