using System;
using System.Collections.Generic;
using System.Text;
using SnCore.Services;
using NHibernate;
using NHibernate.Expression;

namespace SnCore.WebServices
{
    public class WebServiceImpl<TransitType, ManagedType, DataType>
        where TransitType : ITransitService, new()
        where ManagedType : IManagedService, new()
        where DataType : IDbObject
    {
        public static int CreateOrUpdate(string ticket, TransitType t_instance)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(WebService.GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedType m_instance = new ManagedType();
                m_instance.Session = session;
                m_instance.CreateOrUpdateDbObject(t_instance, sec);
                SnCore.Data.Hibernate.Session.Flush();
                return m_instance.Id;
            }
        }

        public static void Delete(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(WebService.GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedType m_instance = new ManagedType();
                m_instance.LoadInstance(session, id);
                m_instance.Delete(sec);
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        public static TransitType GetById(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(WebService.GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedType m_type = new ManagedType();
                m_type.LoadInstance(session, id);
                return (TransitType)m_type.GetTransitServiceInstance(sec);
            }
        }

        public static List<TransitType> GetList(string ticket, ServiceQueryOptions options)
        {
            return GetList(ticket, options, (ICriterion[]) null, (Order[]) null);
        }

        private static List<TransitType> GetTransformedList(ISession session, ManagedSecurityContext sec, IList<DataType> list)
        {
            List<TransitType> result = new List<TransitType>(list.Count);
            foreach (DataType instance in list)
            {
                ManagedType m_instance = new ManagedType();
                m_instance.SetDbObjectInstance(session, instance);
                result.Add((TransitType)m_instance.GetTransitServiceInstance(sec));
            }
            return result;
        }

        public static List<TransitType> GetList(string ticket, ServiceQueryOptions options, string sqlquery, string returnalias)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(WebService.GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                IQuery query = session.CreateSQLQuery(sqlquery, returnalias, typeof(DataType));
                if (options != null && options.PageSize > 0) query.SetMaxResults(options.PageSize);
                if (options != null && options.FirstResult > 0) query.SetFirstResult(options.FirstResult);
                return GetTransformedList(session, sec, query.List<DataType>());
            }
        }

        public static List<TransitType> GetList(string ticket, ServiceQueryOptions options, ICriterion[] expressions, Order[] orders)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(WebService.GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ICriteria criteria = session.CreateCriteria(typeof(DataType));
                // optional criterion expressions
                if (expressions != null)
                    foreach (ICriterion criterion in expressions)
                        criteria.Add(criterion);
                // options orders
                if (orders != null)
                    foreach (Order order in orders)
                        criteria.AddOrder(order);
                // query options
                if (options != null && options.PageSize > 0) criteria.SetMaxResults(options.PageSize);
                if (options != null && options.FirstResult > 0) criteria.SetFirstResult(options.FirstResult);
                return GetTransformedList(session, sec, criteria.List<DataType>());
            }
        }

        public static int GetCount(string ticket)
        {
            return GetCount(ticket, string.Empty);
        }

        public static int GetCount(string ticket, string expression)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(WebService.GetNewConnection()))
            {
                // TODO: check permissions
                ISession session = SnCore.Data.Hibernate.Session.Current;
                string query = string.Format("SELECT COUNT(*) FROM {0} {0} {1}",
                    typeof(DataType).Name, expression);
                return (int)session.CreateQuery(query).UniqueResult();
            }
        }

    }
}
