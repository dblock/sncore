using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using SnCore.Services;
using System.Diagnostics;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Expression;
using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Design;

namespace SnCore.WebServices
{

    /// <summary>
    /// The WebLocationService provides location information.
    /// </summary>
    [WebService(Namespace = "http://www.vestris.com/sncore/ns/", Name = "WebLocationService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WebLocationService : WebService
    {

        public WebLocationService()
        {

        }

        #region Country

        /// <summary>
        /// Get countries count
        /// </summary>
        /// <returns>number of countries</returns>
        [WebMethod(Description = "Get countries count.")]
        public int GetCountriesCount()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery("SELECT COUNT(c) FROM Country c").UniqueResult();
            }
        }

        /// <summary>
        /// Get all countries.
        /// </summary>
        /// <returns>list of countries</returns>
        [WebMethod(Description = "Get all countries.", CacheDuration = 60)]
        public List<TransitCountry> GetCountries(ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ICriteria c = session.CreateCriteria(typeof(Country));
                if (options != null)
                {
                    c.SetFirstResult(options.FirstResult);
                    c.SetMaxResults(options.PageSize);
                }
                IList countries = c.List();
                List<TransitCountry> result = new List<TransitCountry>(countries.Count);
                foreach (Country country in countries)
                {
                    if (options == null && country.Name == "United States")
                    {
                        result.Insert(0, new ManagedCountry(session, country).TransitCountry);
                        result.Insert(1, new TransitCountry());
                    }

                    result.Add(new ManagedCountry(session, country).TransitCountry);
                }
                return result;
            }
        }

        /// <summary>
        /// Add a country.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="c">transit country information</param>
        /// <returns>new country id</returns>
        /// </summary>
        [WebMethod(Description = "Add a country.")]
        public int AddCountry(string ticket, TransitCountry c)
        {
            int userid = ManagedAccount.GetAccountId(ticket);

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                // check permissions: userid must have admin rights to the Accounts table
                ManagedAccount user = new ManagedAccount(session, userid);
                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedCountry m = new ManagedCountry(session);
                m.Create(c);
                session.Flush();
                return m.Id;
            }
        }

        /// <summary>
        /// Delete a country.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">country id</param>
        /// </summary>
        [WebMethod(Description = "Delete a country.")]
        public void DeleteCountry(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                // check permissions: userid must have admin rights to the Accounts table
                ManagedAccount user = new ManagedAccount(session, userid);
                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedCountry m = new ManagedCountry(session, id);
                m.Delete();
                session.Flush();
            }
        }

        /// <summary>
        /// Get country by id.
        /// </summary>
        /// <param name="id">country id</param>
        /// <returns></returns>
        [WebMethod(Description = "Get country by id.")]
        public TransitCountry GetCountryById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return new ManagedCountry(session, id).TransitCountry;
            }
        }

        #endregion

        #region State

        /// <summary>
        /// Get states count
        /// </summary>
        /// <returns>number of states</returns>
        [WebMethod(Description = "Get states count.")]
        public int GetStatesCount()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery("SELECT COUNT(s) FROM State s").UniqueResult();
            }
        }

        /// <summary>
        /// Get all states.
        /// </summary>
        /// <returns>list of states</returns>
        [WebMethod(Description = "Get all states.", CacheDuration = 60)]
        public List<TransitState> GetStates(ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ICriteria c = session.CreateCriteria(typeof(State));
                if (options != null)
                {
                    c.SetFirstResult(options.FirstResult);
                    c.SetMaxResults(options.PageSize);
                }
                IList states = c.List();
                List<TransitState> result = new List<TransitState>(states.Count);
                foreach (State state in states)
                {
                    result.Add(new ManagedState(session, state).TransitState);
                }
                return result;
            }
        }

        /// <summary>
        /// Get states within a country.
        /// </summary>
        /// <param name="country">country name</param>
        /// <returns>list of states</returns>
        [WebMethod(Description = "Get states within a country.", CacheDuration = 60)]
        public List<TransitState> GetStatesByCountry(string country)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                int country_id = string.IsNullOrEmpty(country) ? 0 : ManagedCountry.GetCountryId(session, country);

                IList states = session.CreateCriteria(typeof(State))
                    .Add(Expression.Eq("Country.Id", country_id))
                    .List();

                List<TransitState> result = new List<TransitState>(states.Count);
                foreach (State c in states)
                {
                    result.Add(new ManagedState(session, c).TransitState);
                }
                return result;
            }
        }

        /// <summary>
        /// Add a state.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="c">transit state information</param>
        /// <returns>new state id</returns>
        /// </summary>
        [WebMethod(Description = "Add a state.")]
        public int AddState(string ticket, TransitState c)
        {
            int userid = ManagedAccount.GetAccountId(ticket);

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                // check permissions: userid must have admin rights to the Accounts table
                ManagedAccount user = new ManagedAccount(session, userid);
                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedState m = new ManagedState(session);
                m.Create(c);
                session.Flush();
                return m.Id;
            }
        }

        /// <summary>
        /// Delete a state.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">state id</param>
        /// </summary>
        [WebMethod(Description = "Delete a state.")]
        public void DeleteState(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                // check permissions: userid must have admin rights to the Accounts table
                ManagedAccount user = new ManagedAccount(session, userid);
                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedState m = new ManagedState(session, id);
                m.Delete();
                session.Flush();
            }
        }

        /// <summary>
        /// Get state by id.
        /// </summary>
        /// <param name="id">state id</param>
        /// <returns></returns>
        [WebMethod(Description = "Get state by id.")]
        public TransitState GetStateById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return new ManagedState(session, id).TransitState;
            }
        }

        #endregion

        #region City

        /// <summary>
        /// Get cities count
        /// </summary>
        /// <returns>number of cities</returns>
        [WebMethod(Description = "Get cities count.")]
        public int GetCitiesCount()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery("SELECT COUNT(c) FROM City c").UniqueResult();
            }
        }

        /// <summary>
        /// Get all cities.
        /// </summary>
        /// <returns>list of cities</returns>
        [WebMethod(Description = "Get all cities.", CacheDuration = 60)]
        public List<TransitCity> GetCities(ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ICriteria c = session.CreateCriteria(typeof(City));
                if (options != null)
                {
                    c.SetFirstResult(options.FirstResult);
                    c.SetMaxResults(options.PageSize);
                }
                IList cities = c.List();
                List<TransitCity> result = new List<TransitCity>(cities.Count);
                foreach (City city in cities)
                {
                    result.Add(new ManagedCity(session, city).TransitCity);
                }
                return result;
            }
        }

        /// <summary>
        /// Get all cities with a matching name.
        /// </summary>
        /// <returns>list of cities</returns>
        [WebMethod(Description = "Get all cities with a matching name.", CacheDuration = 60)]
        public List<TransitCity> SearchCitiesByName(string name)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList cities = session.CreateCriteria(typeof(City))
                    .Add(Expression.Like("Name", string.Format("%{0}%", name)))
                    .List();
                List<TransitCity> result = new List<TransitCity>(cities.Count);
                foreach (City c in cities)
                {
                    result.Add(new ManagedCity(session, c).TransitCity);
                }
                return result;
            }
        }

        /// <summary>
        /// Get cities within a country and state.
        /// </summary>
        /// <param name="state">state name</param>
        /// <param name="country">country name</param>
        /// <returns>list of cities</returns>
        [WebMethod(Description = "Get cities within a country and state.", CacheDuration = 60)]
        public List<TransitCity> GetCitiesByLocation(string country, string state)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                List<TransitCity> result = new List<TransitCity>();

                if (string.IsNullOrEmpty(country))
                {
                    return result;
                }

                Country t_country = ManagedCountry.Find(session, country);

                ICriteria cr = session.CreateCriteria(typeof(City))
                    .Add(Expression.Eq("Country.Id", t_country.Id));

                IList cities = null;

                if (t_country.States != null && t_country.States.Count > 0 && string.IsNullOrEmpty(state))
                {
                    // no state specified but country has states
                    return result;
                }
                
                if (! string.IsNullOrEmpty(state))
                {
                    // state specified
                    State t_state = ManagedState.Find(session, state, country);
                    cr.Add(Expression.Eq("State.Id", t_state.Id));
                }

                cities = cr.List();

                foreach (City c in cities)
                {
                    result.Add(new ManagedCity(session, c).TransitCity);
                }

                return result;
            }
        }

        /// <summary>
        /// Add a city.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="c">transit city information</param>
        /// <returns>new city id</returns>
        /// </summary>
        [WebMethod(Description = "Add a city.")]
        public int AddCity(string ticket, TransitCity c)
        {
            int userid = ManagedAccount.GetAccountId(ticket);

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                // check permissions: userid must have admin rights to the Accounts table
                ManagedAccount user = new ManagedAccount(session, userid);
                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedCity m = new ManagedCity(session);
                m.Create(c);
                session.Flush();
                return m.Id;
            }
        }

        /// <summary>
        /// Delete a city.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">city id</param>
        /// </summary>
        [WebMethod(Description = "Delete a city.")]
        public void DeleteCity(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                // check permissions: userid must have admin rights to the Accounts table
                ManagedAccount user = new ManagedAccount(session, userid);
                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedCity m = new ManagedCity(session, id);
                m.Delete();
                session.Flush();
            }
        }

        /// <summary>
        /// Get city by id.
        /// </summary>
        /// <param name="id">city id</param>
        /// <returns></returns>
        [WebMethod(Description = "Get city by id.")]
        public TransitCity GetCityById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return new ManagedCity(session, id).TransitCity;
            }
        }

        /// <summary>
        /// Get city by tag.
        /// </summary>
        /// <param name="tag">city tag</param>
        /// <returns></returns>
        [WebMethod(Description = "Get city by tag.")]
        public TransitCity GetCityByTag(string tag)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                
                City c = (City) session.CreateCriteria(typeof(City))
                    .Add(Expression.Eq("Tag", tag))
                    .UniqueResult();

                if (c == null)
                {
                    return null;
                }

                return new ManagedCity(session, c).TransitCity;
            }
        }

        /// <summary>
        /// Merge cities.
        /// <param name="ticket">authentication ticket</param>
        /// </summary>
        [WebMethod(Description = "Merge cities.")]
        public int MergeCities(string ticket, int target_id, int merge_id)
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

                ManagedCity m = new ManagedCity(session, target_id);
                int result = m.Merge(merge_id);
                session.Flush();
                return result;
            }
        }

        #endregion

        #region Neighborhood


        /// <summary>
        /// Get neighborhoods count
        /// </summary>
        /// <returns>number of neighborhoods</returns>
        [WebMethod(Description = "Get neighborhoods count.")]
        public int GetNeighborhoodsCount()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery("SELECT COUNT(nh) FROM Neighborhood nh").UniqueResult();
            }
        }

        /// <summary>
        /// Get all neighborhoods.
        /// </summary>
        /// <returns>list of neighborhoods</returns>
        [WebMethod(Description = "Get all neighborhoods.", CacheDuration = 60)]
        public List<TransitNeighborhood> GetNeighborhoods(ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ICriteria cr = session.CreateCriteria(typeof(Neighborhood));
                if (options != null)
                {
                    cr.SetFirstResult(options.FirstResult);
                    cr.SetMaxResults(options.PageSize);
                }
                IList neighborhoods = cr.List();
                List<TransitNeighborhood> result = new List<TransitNeighborhood>(neighborhoods.Count);
                foreach (Neighborhood c in neighborhoods)
                {
                    result.Add(new ManagedNeighborhood(session, c).TransitNeighborhood);
                }
                return result;
            }
        }

        /// <summary>
        /// Get all neighborhoods with a matching name.
        /// </summary>
        /// <returns>list of neighborhoods</returns>
        [WebMethod(Description = "Get all neighborhoods with a matching name.", CacheDuration = 60)]
        public List<TransitNeighborhood> SearchNeighborhoodsByName(string name)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList neighborhoods = session.CreateCriteria(typeof(Neighborhood))
                    .Add(Expression.Like("Name", string.Format("%{0}%", name)))
                    .List();
                List<TransitNeighborhood> result = new List<TransitNeighborhood>(neighborhoods.Count);
                foreach (Neighborhood c in neighborhoods)
                {
                    result.Add(new ManagedNeighborhood(session, c).TransitNeighborhood);
                }
                return result;
            }
        }

        /// <summary>
        /// Get neighborhoods within a country, state and city.
        /// </summary>
        /// <param name="city">city name</param>
        /// <param name="state">state name</param>
        /// <param name="country">country name</param>
        /// <returns>list of neighborhoods</returns>
        [WebMethod(Description = "Get neighborhoods within a country, state and city.", CacheDuration = 60)]
        public List<TransitNeighborhood> GetNeighborhoodsByLocation(string country, string state, string city)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                List<TransitNeighborhood> result = new List<TransitNeighborhood>();

                if (string.IsNullOrEmpty(city) || string.IsNullOrEmpty(country))
                {
                    return result;
                }

                City t_city = ManagedCity.Find(session, city, state, country);

                IList neighborhoods = session.CreateCriteria(typeof(Neighborhood))
                    .Add(Expression.Eq("City.Id", t_city.Id))
                    .List();

                foreach (Neighborhood nh in neighborhoods)
                {
                    result.Add(new ManagedNeighborhood(session, nh).TransitNeighborhood);
                }

                return result;
            }
        }

        /// <summary>
        /// Add a neighborhood.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="c">transit neighborhood information</param>
        /// <returns>new neighborhood id</returns>
        /// </summary>
        [WebMethod(Description = "Add a neighborhood.")]
        public int AddNeighborhood(string ticket, TransitNeighborhood nh)
        {
            int userid = ManagedAccount.GetAccountId(ticket);

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                // check permissions: userid must have admin rights to the Accounts table
                ManagedAccount user = new ManagedAccount(session, userid);
                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedNeighborhood m = new ManagedNeighborhood(session);
                m.Create(nh);
                session.Flush();
                return m.Id;
            }
        }

        /// <summary>
        /// Delete a neighborhood.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">neighborhood id</param>
        /// </summary>
        [WebMethod(Description = "Delete a neighborhood.")]
        public void DeleteNeighborhood(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                // check permissions: userid must have admin rights to the Accounts table
                ManagedAccount user = new ManagedAccount(session, userid);
                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedNeighborhood m = new ManagedNeighborhood(session, id);
                m.Delete();
                session.Flush();
            }
        }

        /// <summary>
        /// Get neighborhood by id.
        /// </summary>
        /// <param name="id">neighborhood id</param>
        /// <returns></returns>
        [WebMethod(Description = "Get neighborhood by id.")]
        public TransitNeighborhood GetNeighborhoodById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return new ManagedNeighborhood(session, id).TransitNeighborhood;
            }
        }

        /// <summary>
        /// Merge neighborhoods.
        /// <param name="ticket">authentication ticket</param>
        /// </summary>
        [WebMethod(Description = "Merge neighborhoods.")]
        public int MergeNeighborhoods(string ticket, int target_id, int merge_id)
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

                ManagedNeighborhood m = new ManagedNeighborhood(session, target_id);
                int result = m.Merge(merge_id);
                session.Flush();
                return result;
            }
        }

        #endregion
    }
}