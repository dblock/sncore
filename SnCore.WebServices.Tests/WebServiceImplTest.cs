using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SnCore.WebServices;
using SnCore.Services;
using SnCore.Data.Hibernate;
using SnCore.Data.Tests;
using System.Web.Security;

namespace SnCore.WebServices.Tests
{
    public abstract class WebServiceImplTest<TransitType, ManagedType, DataType> : NHibernateTest
        where TransitType : ITransitService, new()
        where ManagedType : IManagedService, new()
        where DataType : IDbObject
    {
        public virtual string GetTicket()
        {
            return ManagedAccount.GetAdminTicket(Session);
        }

        public abstract TransitType CreateTransitInstance();

        [Test]
        public void TestGetList()
        {
            List<TransitType> result = WebServiceImpl<TransitType, ManagedType, DataType>.GetList(
                GetTicket(), null);
            Console.WriteLine("{0}: retreived {1} items of type {2}.",
                typeof(DataType).Name, result.Count, typeof(TransitType).Name);
            Assert.IsTrue(result.Count >= 0);
        }

        [Test]
        public void TestGetListWithQueryOptions()
        {
            int pagesize = 10;
            ServiceQueryOptions options = new ServiceQueryOptions(10, 1);
            List<TransitType> result = WebServiceImpl<TransitType, ManagedType, DataType>.GetList(
                GetTicket(), options);
            Console.WriteLine("{0}: retreived {1} paged items of type {2}.",
                typeof(DataType).Name, result.Count, typeof(TransitType).Name);
            Assert.IsTrue(result.Count >= 0 && result.Count <= pagesize);
        }

        [Test]
        public void TestCRUD()
        {
            Console.WriteLine("{0}: testing CRUD.", typeof(DataType).Name);
            int id = WebServiceImpl<TransitType, ManagedType, DataType>.CreateOrUpdate(
                GetTicket(), CreateTransitInstance());
            Console.WriteLine("{0}: created object with id={1}.", typeof(DataType).Name, id);
            Assert.IsTrue(id > 0, string.Format("{0} object created doesn't have an Id.", typeof(DataType).Name));
            TransitType t_instance = WebServiceImpl<TransitType, ManagedType, DataType>.GetById(string.Empty, id);
            Assert.IsNotNull(t_instance, string.Format("{0} object created couldn't be fetched from store.", typeof(DataType).Name));
            WebServiceImpl<TransitType, ManagedType, DataType>.Delete(
                GetTicket(), id);

            TransitType deleted_instance = WebServiceImpl<TransitType, ManagedType, DataType>.GetById(
                GetTicket(), id);
            Assert.IsNull(deleted_instance, string.Format("{0} object created hasn't been deleted from store.", typeof(DataType).Name));
        }
    }
}
