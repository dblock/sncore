using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using NHibernate.Cfg;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Persister.Entity;
using SnCore.Data.Hibernate;
using NHibernate.Transform;

namespace SnCore.Data.Tests
{
    [TestFixture]
    public class DomainModelTest : NHibernateTest
    {
        [Test]
        public void TestDumpSchema()
        {
            DomainModel model = new DomainModel(Session);
            foreach (DomainClass domainclass in model.Classes)
            {
                Console.WriteLine("class: {0}", domainclass.Name);
                foreach (sp_column column in domainclass.Columns)
                {
                    Console.WriteLine(" # {0} ({1}/{2}) - len: {3}", 
                        column.COLUMN_NAME, column.TYPE_NAME, column.SQL_DATA_TYPE, column.LENGTH);
                }
            }
        }
    }
}
