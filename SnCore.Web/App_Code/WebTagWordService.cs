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

        #region Tag Words

        /// <summary>
        /// Create or update a tag word.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="tagword">transit tag word</param>
        [WebMethod(Description = "Create or update a tag word.")]
        public int CreateOrUpdateTagWord(string ticket, TransitTagWord tagword)
        {
            return WebServiceImpl<TransitTagWord, ManagedTagWord, TagWord>.CreateOrUpdate(
                ticket, tagword);
        }

        /// <summary>
        /// Get a tag word.
        /// </summary>
        /// <returns>transit tag word</returns>
        [WebMethod(Description = "Get a tag word.")]
        public TransitTagWord GetTagWordById(string ticket, int id)
        {
            return WebServiceImpl<TransitTagWord, ManagedTagWord, TagWord>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all tag words.
        /// </summary>
        /// <param name="max">maximum number of tag words</param>
        /// <param name="options">options</param>
        /// <returns>list of transit tag words</returns>
        [WebMethod(Description = "Get all tag words.")]
        public List<TransitTagWord> GetTagWords(string ticket, TransitTagWordQueryOptions queryoptions, ServiceQueryOptions options)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT word FROM TagWord word LEFT JOIN word.TagWordAccounts acct");
            query.Append(" GROUP BY word.Id, word.Promoted, word.Excluded, word.Word");
            switch(queryoptions)
            {
                case TransitTagWordQueryOptions.Excluded:
                    query.Append(" WHERE Excluded = 1");
                    break;
                case TransitTagWordQueryOptions.New:
                    query.Append(" WHERE Excluded = 0 and Promoted = 0");
                    break;
                case TransitTagWordQueryOptions.Promoted:
                    query.Append(" WHERE Promoted = 1");
                    break;
            }
            query.Append(" ORDER BY COUNT(acct) DESC");

            return WebServiceImpl<TransitTagWord, ManagedTagWord, TagWord>.GetList(
                ticket, options, query.ToString());
        }

        /// <summary>
        /// Get all tag words count.
        /// </summary>
        /// <param name="max">maximum number of tag words</param>
        /// <param name="options">options</param>
        [WebMethod(Description = "Get all tag words count.")]
        public int GetTagWordsCount(string ticket, TransitTagWordQueryOptions queryoptions)
        {
            string query = null;
            switch (queryoptions)
            {
                case TransitTagWordQueryOptions.Excluded:
                    query = "WHERE TagWord.Excluded = 1";
                    break;
                case TransitTagWordQueryOptions.New:
                    query = "WHERE TagWord.Excluded = 0 and TagWord.Promoted = 0";
                    break;
                case TransitTagWordQueryOptions.Promoted:
                    query = "WHERE TagWord.Promoted = 1";
                    break;
            }

            return WebServiceImpl<TransitTagWord, ManagedTagWord, TagWord>.GetCount(
                ticket, query.ToString());
        }

        /// <summary>
        /// Delete a tag word.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        [WebMethod(Description = "Delete a tag word.")]
        public void DeleteTagWord(string ticket, int id)
        {
            WebServiceImpl<TransitTagWord, ManagedTagWord, TagWord>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Get tag word accounts.
        /// </summary>
        /// <param name="id">tag word id</param>
        /// <returns>list of transit accounts</returns>
        [WebMethod(Description = "Get tag word accounts.", CacheDuration = 60)]
        public List<TransitAccount> GetTagWordAccountsById(string ticket, int id, ServiceQueryOptions options)
        {
            StringBuilder query = new StringBuilder();
            query.AppendFormat(
                "SELECT acct FROM Account acct, TagWordAccount twa " +
                "WHERE acct.Id = twa.AccountId " +
                "AND twa.TagWord.Id = {0} " +
                "ORDER BY acct.LastLogin DESC",
                id);

            return WebServiceImpl<TransitAccount, ManagedAccount, Account>.GetList(
                ticket, options, query.ToString());
        }

        /// <summary>
        /// Search tag word accounts.
        /// </summary>
        /// <param name="search">search string</param>
        /// <returns>list of transit accounts</returns>
        [WebMethod(Description = "Search tag word accounts.", CacheDuration = 60)]
        public List<TransitAccount> SearchTagWordAccounts(string ticket, string search, ServiceQueryOptions options)
        {
            MatchCollection mc = Regex.Matches(search, @"\w+");

            if (mc.Count == 0)
                return null;

            StringBuilder query = new StringBuilder();
            query.Append("SELECT DISTINCT acct FROM Account acct, TagWordAccount twa");
            query.Append(" WHERE acct.Id = twa.AccountId AND (");
            StringBuilder subquery = new StringBuilder();
            foreach (Match m in mc)
            {
                if (subquery.Length > 0)
                    subquery.Append(" OR ");

                subquery.Append(string.Format("twa.TagWord.Word LIKE '%{0}%'", m.Value.ToLower()));
            }
            query.Append(subquery);
            query.Append(") ORDER BY acct.LastLogin DESC");

            return WebServiceImpl<TransitAccount, ManagedAccount, Account>.GetList(
                ticket, options, query.ToString());
        }

        #endregion
    }
}