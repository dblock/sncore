using System;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using SnCore.Services;
using NHibernate;
using NHibernate.Expression;
using System.Data.SqlClient;
using System.Web.Security;
using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Design;
using System.Text;
using SnCore.Tools.Web;

namespace SnCore.WebServices
{
    /// <summary>
    /// Managed web mad libs services.
    /// </summary>
    [WebService(Namespace = "http://www.vestris.com/sncore/ns/", Name = "WebMadLibService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WebMadLibService : WebService
    {
        public WebMadLibService()
        {

        }

        #region Mad Libs

        /// <summary>
        /// Gets mad libs count.
        /// </summary>
        [WebMethod(Description = "Get mad libs count.")]
        public int GetMadLibsCount(string ticket)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            return GetMadLibsCountById(userid);
        }

        /// <summary>
        /// Gets mad libs count.
        /// </summary>
        [WebMethod(Description = "Get mad libs count.", CacheDuration = 60)]
        public int GetMadLibsCountById(int accountid)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                
                return (int) session.CreateQuery(string.Format("SELECT COUNT(m) FROM MadLib m WHERE m.Account.Id = {0}",
                    accountid)).UniqueResult();
            }
        }

        /// <summary>
        /// Get all mad libs.
        /// </summary>
        [WebMethod(Description = "Get all mad libs.", CacheDuration = 60)]
        public List<TransitMadLib> GetMadLibs(string ticket, ServiceQueryOptions options)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            return GetMadLibsById(userid, options);
        }

        /// <summary>
        /// Get all mad libs.
        /// </summary>
        [WebMethod(Description = "Get all mad libs.", CacheDuration = 60)]
        public List<TransitMadLib> GetMadLibsById(int accountid, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ICriteria c = session.CreateCriteria(typeof(MadLib))
                    .Add(Expression.Eq("Account.Id", accountid))
                    .AddOrder(Order.Desc("Created"));

                if (options != null)
                {
                    c.SetFirstResult(options.FirstResult);
                    c.SetMaxResults(options.PageSize);
                }

                IList list = c.List();

                List<TransitMadLib> result = new List<TransitMadLib>(list.Count);
                foreach (MadLib madlib in list)
                {
                    result.Add(new TransitMadLib(madlib));
                }

                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Create or update a mad lib.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Create or update a mad lib.")]
        public int CreateOrUpdateMadLib(string ticket, TransitMadLib madlib)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                if (madlib.AccountId == 0) madlib.AccountId = userid;

                // for now simple users have no use of madlibs and cannot create them
                ManagedAccount user = new ManagedAccount(session, userid);
                if (!user.IsAdministrator() /*&& (madlib.AccountId != userid) */)
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedMadLib m_madlib = new ManagedMadLib(session);
                m_madlib.CreateOrUpdate(madlib);

                SnCore.Data.Hibernate.Session.Flush();
                return m_madlib.Id;
            }
        }

        /// <summary>
        /// Delete a mad lib.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Delete a mad lib.")]
        public void DeleteMadLib(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedMadLib madlib = new ManagedMadLib(session, id);
                ManagedAccount user = new ManagedAccount(session, userid);
                if (madlib.AccountId != userid && ! user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }
 
                madlib.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Get a mad lib by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Get a mad lib by id.")]
        public TransitMadLib GetMadLibById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return new ManagedMadLib(session, id).TransitMadLib;
            }
        }

        #endregion

        #region Mad Lib Instances

        /// <summary>
        /// Gets mad lib instances count.
        /// </summary>
        [WebMethod(Description = "Get mad lib instances count.", CacheDuration = 60)]
        public int GetMadLibInstancesCount(string table, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery(string.Format("SELECT COUNT(m) FROM MadLibInstance m WHERE m.DataObject.Id = {0} AND m.ObjectId = {1}",
                    ManagedDataObject.Find(session, table), id)).UniqueResult();
            }
        }

        /// <summary>
        /// Get all mad lib instances.
        /// </summary>
        [WebMethod(Description = "Get all mad lib instances.", CacheDuration = 60)]
        public List<TransitMadLibInstance> GetMadLibInstances(string table, int id, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ICriteria c = session.CreateCriteria(typeof(MadLibInstance))
                    .Add(Expression.Eq("DataObject.Id", ManagedDataObject.Find(session, table)))
                    .Add(Expression.Eq("ObjectId", id))
                    .AddOrder(Order.Desc("Created"));

                if (options != null)
                {
                    c.SetFirstResult(options.FirstResult);
                    c.SetMaxResults(options.PageSize);
                }

                IList list = c.List();

                List<TransitMadLibInstance> result = new List<TransitMadLibInstance>(list.Count);
                foreach (MadLibInstance madlibinstance in list)
                {
                    result.Add(new ManagedMadLibInstance(session, madlibinstance).TransitMadLibInstance);
                }

                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Create or update a mad lib.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Create or update a mad lib.")]
        public int CreateOrUpdateMadLibInstance(string ticket, TransitMadLibInstance madlibinstance)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                if (madlibinstance.AccountId == 0) madlibinstance.AccountId = userid;

                ManagedMadLibInstance m_madlibinstance = new ManagedMadLibInstance(session);
                m_madlibinstance.CreateOrUpdate(madlibinstance);

                SnCore.Data.Hibernate.Session.Flush();
                return m_madlibinstance.Id;
            }
        }

        /// <summary>
        /// Delete a mad lib.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Delete a mad lib.")]
        public void DeleteMadLibInstance(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedMadLibInstance madlibinstance = new ManagedMadLibInstance(session, id);
                ManagedAccount user = new ManagedAccount(session, userid);
                if (madlibinstance.AccountId != userid && !user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                madlibinstance.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Get a mad lib instance by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Get a mad lib instance by id.")]
        public TransitMadLibInstance GetMadLibInstanceById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return new ManagedMadLibInstance(session, id).TransitMadLibInstance;
            }
        }

        #endregion
    }
}