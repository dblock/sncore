using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.Script.Services;
using SnCore.Tools.Web;
using SnCore.Data.Hibernate;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Expression;
using SnCore.Services;

namespace SnCore.WebServices
{
    /// <summary>
    /// AJAX Completion Script Services
    /// </summary>
    [WebService(Namespace = "http://www.vestris.com/sncore/ns/", Name = "ScriptServices")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class ScriptServices : System.Web.Services.WebService
    {
        public ScriptServices()
        {

        }

        #region ASP.NET AJAX Completion


        [WebMethod(Description = "Get cities completion list.")]
        [ScriptMethod]
        public string[] GetCitiesCompletionList(string prefixText, int count, string contextKey)
        {
            // TODO: manufacture a guest ticket and run this through WebServiceImpl

            // country;state
            string[] context_parts = contextKey.Split(";,|".ToCharArray(), 2);
            if (context_parts.Length != 2)
                return null;

            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                int country_id = 0;
                if (!string.IsNullOrEmpty(context_parts[0]))
                {
                    if (!ManagedCountry.TryGetCountryId(session, context_parts[0], out country_id))
                        return null;
                }

                int state_id = 0;
                if (! string.IsNullOrEmpty(context_parts[1]))
                {
                    if (!ManagedState.TryGetStateId(session, context_parts[1], context_parts[0], out state_id))
                        return null;
                }

                ICriteria ic = session.CreateCriteria(typeof(City))
                    .Add(Expression.Like("Name", string.Format("{0}%", Renderer.SqlEncode(prefixText))));

                if (country_id != 0) ic.Add(Expression.Eq("Country.Id", country_id));
                if (state_id != 0) ic.Add(Expression.Eq("State.Id", state_id));

                IList<City> nhs = ic.AddOrder(Order.Asc("Name"))
                    .SetMaxResults(count)
                    .List<City>();

                List<string> result = new List<string>(nhs.Count);
                foreach (City nh in nhs)
                {
                    result.Add(nh.Name);
                }

                return result.ToArray();
            }
        }

        [WebMethod(Description = "Get neighborhoods completion list.")]
        [ScriptMethod]
        public string[] GetNeighborhoodsCompletionList(string prefixText, int count, string contextKey)
        {
            // TODO: manufacture a guest ticket and run this through WebServiceImpl

            // country;state;city
            string[] context_parts = contextKey.Split(";,|".ToCharArray(), 3);
            if (context_parts.Length != 3)
                return null;

            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                int city_id = 0;
                if (!ManagedCity.TryGetCityId(session, context_parts[2], context_parts[1], context_parts[0], out city_id))
                    return null;

                IList<Neighborhood> nhs = session.CreateCriteria(typeof(Neighborhood))
                    .Add(Expression.Like("Name", string.Format("{0}%", Renderer.SqlEncode(prefixText))))
                    .Add(Expression.Eq("City.Id", city_id))
                    .AddOrder(Order.Asc("Name"))
                    .SetMaxResults(count)
                    .List<Neighborhood>();

                List<string> result = new List<string>(nhs.Count);
                foreach (Neighborhood nh in nhs)
                {
                    result.Add(nh.Name);
                }

                return result.ToArray();
            }
        }

        #endregion
    }
}