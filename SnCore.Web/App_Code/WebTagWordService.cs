using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using SnCore.Services;
using NHibernate;
using NHibernate.Expression;
using System.Data.SqlClient;
using System.Web.Security;
using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Design;
using System.Text.RegularExpressions;
using System.Text;

namespace SnCore.WebServices
{
    /// <summary>
    /// Managed web tag word services.
    /// </summary>
    [WebService(Namespace = "http://www.vestris.com/sncore/ns/", Name = "WebTagWordService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WebTagWordService : WebService
    {
        public WebTagWordService()
        {

        }

        /// <summary>
        /// Create or update a tag word.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="tagword">transit tag word</param>
        [WebMethod(Description = "Create or update a tag word.")]
        public int CreateOrUpdateTagWord(string ticket, TransitTagWord tagword)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedTagWord m_tagword = new ManagedTagWord(session);
                m_tagword.CreateOrUpdate(tagword);
                SnCore.Data.Hibernate.Session.Flush();
                return m_tagword.Id;
            }
        }

        /// <summary>
        /// Get a tag word.
        /// </summary>
        /// <returns>transit tag word</returns>
        [WebMethod(Description = "Get a tag word.")]
        public TransitTagWord GetTagWordById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitTagWord result = new ManagedTagWord(session, id).TransitTagWord;
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get all tag words.
        /// </summary>
        /// <param name="max">maximum number of tag words</param>
        /// <param name="options">options</param>
        /// <returns>list of transit tag words</returns>
        [WebMethod(Description = "Get all tag words.")]
        public List<TransitTagWord> GetTagWords(TransitTagWordQueryOptions options, int max)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                // IList tagwords = session.CreateCriteria(typeof(TagWord)).List();

                string where = string.Empty;
                switch (options)
                {
                    case TransitTagWordQueryOptions.Excluded:
                        where = "WHERE Excluded = 1";
                        break;
                    case TransitTagWordQueryOptions.New:
                        where = "WHERE Excluded = 0 and Promoted = 0";
                        break;
                    case TransitTagWordQueryOptions.Promoted:
                        where = "WHERE Promoted = 1";
                        break;
                }

                IQuery query = session.CreateQuery(string.Format(
                    "SELECT word FROM TagWord word LEFT JOIN word.TagWordAccounts acct " + 
                    "GROUP BY word.Id, word.Promoted, word.Excluded, word.Word {0} ORDER BY COUNT(acct) DESC", where));

                if (max > 0) query.SetMaxResults(max);

                IList tagwords = query.List();

                List<TransitTagWord> result = new List<TransitTagWord>(tagwords.Count);
                foreach (TagWord tagword in tagwords)
                {
                    result.Add(new ManagedTagWord(session, tagword).TransitTagWord);
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get all promoted tag words (cached).
        /// </summary>
        /// <param name="max">max number of tag words</param>
        /// <returns>list of transit tag words</returns>
        [WebMethod(Description = "Get all promoted tag words.", CacheDuration = 60)]
        public List<TransitTagWord> GetPromotedTagWords(int max)
        {
            return GetTagWords(TransitTagWordQueryOptions.Promoted, max);
        }

        /// <summary>
        /// Delete a tag word.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a tag word.")]
        public void DeleteTagWord(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedTagWord m_tagword = new ManagedTagWord(session, id);
                m_tagword.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Get tag word accounts.
        /// </summary>
        /// <param name="id">tag word id</param>
        /// <returns>list of transit accounts</returns>
        [WebMethod(Description = "Get tag word accounts.", CacheDuration = 60)]
        public List<TransitAccount> GetTagWordAccountsById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList tagwordaccounts = session.CreateQuery(
                    string.Format(
                        "SELECT acct FROM Account acct, TagWordAccount twa " +
                        "WHERE acct.Id = twa.AccountId " + 
                        "AND twa.TagWord.Id = {0} " +
                        "ORDER BY acct.LastLogin DESC",
                    id)).List();

                List<TransitAccount> result = new List<TransitAccount>(tagwordaccounts.Count);
                foreach (Account acct in tagwordaccounts)
                {
                    result.Add(new ManagedAccount(session, acct).TransitAccount);
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Search tag word accounts.
        /// </summary>
        /// <param name="search">search string</param>
        /// <returns>list of transit accounts</returns>
        [WebMethod(Description = "Search tag word accounts.", CacheDuration = 60)]
        public List<TransitAccount> SearchTagWordAccounts(string search)
        {
            MatchCollection mc = Regex.Matches(search, @"\w+");

            if (mc.Count == 0)
            {
                return null;
            }

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                StringBuilder query = new StringBuilder(
                    "SELECT DISTINCT acct FROM Account acct, TagWordAccount twa " +
                    "WHERE acct.Id = twa.AccountId AND (");

                StringBuilder subquery = new StringBuilder();
                foreach (Match m in mc)
                {
                    if (subquery.Length > 0)
                    {
                        subquery.Append(" OR ");
                    }
                    subquery.Append(string.Format("twa.TagWord.Word LIKE '%{0}%'", m.Value.ToLower()));
                }

                query.Append(subquery);        
                query.Append(") ORDER BY acct.LastLogin DESC");

                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList tagwordaccounts = session.CreateQuery(query.ToString()).List();
                List<TransitAccount> result = new List<TransitAccount>(tagwordaccounts.Count);
                foreach (Account acct in tagwordaccounts)
                {
                    result.Add(new ManagedAccount(session, acct).TransitAccount);
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }
    }
}