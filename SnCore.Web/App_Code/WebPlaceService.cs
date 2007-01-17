using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using SnCore.Services;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Expression;
using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Design;
using System.Reflection;
using System.Web.Services.Protocols;
using SnCore.Tools.Web;
using System.Collections.Specialized;
using System.Net.Mail;

namespace SnCore.WebServices
{
    /// <summary>
    /// Place information services.
    /// </summary>
    [WebService(Namespace = "http://www.vestris.com/sncore/ns/", Name = "WebPlaceService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WebPlaceService : WebService
    {
        public WebPlaceService()
        {

        }

        #region PlaceType

        /// <summary>
        /// Create or update a place type.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="type">transit place type</param>
        [WebMethod(Description = "Create or update a place type.")]
        public int CreateOrUpdatePlaceType(string ticket, TransitPlaceType type)
        {
            return WebServiceImpl<TransitPlaceType, ManagedPlaceType, PlaceType>.CreateOrUpdate(
                ticket, type);
        }

        /// <summary>
        /// Get a place type.
        /// </summary>
        /// <returns>transit place type</returns>
        [WebMethod(Description = "Get a place type.")]
        public TransitPlaceType GetPlaceTypeById(string ticket, int id)
        {
            return WebServiceImpl<TransitPlaceType, ManagedPlaceType, PlaceType>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all place types.
        /// </summary>
        /// <returns>list of transit place types</returns>
        [WebMethod(Description = "Get all place types.", CacheDuration = 60)]
        public List<TransitPlaceType> GetPlaceTypes(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitPlaceType, ManagedPlaceType, PlaceType>.GetList(
                ticket, options);
        }

        /// <summary>
        /// Get all place types count.
        /// </summary>
        /// <returns>number of place types</returns>
        [WebMethod(Description = "Get all place types.", CacheDuration = 60)]
        public int GetPlaceTypesCount(string ticket)
        {
            return WebServiceImpl<TransitPlaceType, ManagedPlaceType, PlaceType>.GetCount(
                ticket);
        }

        /// <summary>
        /// Delete a place type
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a place type.")]
        public void DeletePlaceType(string ticket, int id)
        {
            WebServiceImpl<TransitPlaceType, ManagedPlaceType, PlaceType>.Delete(
                ticket, id);
        }

        #endregion

        #region Place

        /// <summary>
        /// Create or update a place.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="place">transit place </param>
        /// <returns>place id</returns>
        [WebMethod(Description = "Create or update a place.")]
        public int CreateOrUpdatePlace(string ticket, TransitPlace place)
        {
            return WebServiceImpl<TransitPlace, ManagedPlace, Place>.CreateOrUpdate(
                ticket, place);
        }

        /// <summary>
        /// Get a place.
        /// </summary>
        /// <returns>transit place </returns>
        [WebMethod(Description = "Get a place.")]
        public TransitPlace GetPlaceById(string ticket, int id)
        {
            return WebServiceImpl<TransitPlace, ManagedPlace, Place>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get places count.
        /// </summary>
        /// <returns>transit places count</returns>
        [WebMethod(Description = "Get all places count.", CacheDuration = 60)]
        public int GetPlacesCount(string ticket, TransitPlaceQueryOptions qopt)
        {
            return WebServiceImpl<TransitPlace, ManagedPlace, Place>.GetCount(
                ticket, qopt.CreateCountQuery());
        }

        /// <summary>
        /// Get all places.
        /// </summary>
        /// <returns>list of transit places</returns>
        [WebMethod(Description = "Get all places.", CacheDuration = 60)]
        public List<TransitPlace> GetPlaces(string ticket, TransitPlaceQueryOptions qopt, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitPlace, ManagedPlace, Place>.GetList(
                ticket, options, qopt.CreateQuery());
        }

        /// <summary>
        /// Delete a place.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a place.")]
        public void DeletePlace(string ticket, int id)
        {
            WebServiceImpl<TransitPlace, ManagedPlace, Place>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Find a place.
        /// </summary>
        /// <param name="citytag">city name or tag</param>
        /// <param name="name">place name</param>
        /// <returns></returns>
        [WebMethod(Description = "Find a place.", CacheDuration = 60)]
        public TransitPlace FindPlace(string ticket, string citytag, string name)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                City city = (City)session.CreateCriteria(typeof(City))
                    .Add(Expression.Eq("Tag", citytag))
                    .UniqueResult();

                if (city == null)
                {
                    return null;
                }

                Place p = (Place)session.CreateCriteria(typeof(Place))
                    .Add(Expression.Eq("Name", name))
                    .Add(Expression.Eq("City.Id", city.Id))
                    .SetMaxResults(1)
                    .UniqueResult();

                if (p == null)
                {
                    p = (Place)session.CreateQuery(string.Format(
                        "SELECT p FROM Place p, PlaceName n" +
                        " WHERE n.Name = '{0}' AND p.Id = n.Place.Id" +
                        " AND p.City.Id = {1}",
                        Renderer.SqlEncode(name),
                        city.Id)).SetMaxResults(1).UniqueResult();
                }

                if (p == null)
                {
                    return null;
                }

                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                return new ManagedPlace(session, p).GetTransitInstance(sec);
            }
        }

        /// <summary>
        /// Get new places.
        /// </summary>
        /// <returns>transit places</returns>
        [WebMethod(Description = "Get new places.", CacheDuration = 60)]
        public List<TransitPlace> GetNewPlaces(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitPlace, ManagedPlace, Place>.GetList(
                ticket, options, "FROM Place p WHERE EXISTS ELEMENTS(p.PlacePictures) ORDER BY p.Created DESC");
        }

        #endregion

        #region PlacePicture

        /// <summary>
        /// Create or update a place picture.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="placepicture">transit place picture</param>
        [WebMethod(Description = "Create or update a place picture.")]
        public int CreateOrUpdatePlacePicture(string ticket, TransitPlacePicture placepicture)
        {
            int result = WebServiceImpl<TransitPlacePicture, ManagedPlacePicture, PlacePicture>.CreateOrUpdate(
                ticket, placepicture);

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedPlace m_place = new ManagedPlace(session, result);

                // send a message to place owner

                if (sec.Account != null && sec.Account.Id != m_place.Instance.Account.Id)
                {
                    ManagedAccount acct = new ManagedAccount(session, m_place.Instance.Account);
                    if (acct.HasVerifiedEmail(sec))
                    {
                        ManagedSiteConnector.SendAccountEmailMessageUriAsAdmin(
                            session,
                            new MailAddress(acct.GetActiveEmailAddress(sec), acct.Name).ToString(),
                            string.Format("EmailPlacePicture.aspx?id={0}", result));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Get a place picture.
        /// </summary>
        /// <returns>transit place picture</returns>
        [WebMethod(Description = "Get a place picture.")]
        public TransitPlacePicture GetPlacePictureById(string ticket, int id)
        {
            return WebServiceImpl<TransitPlacePicture, ManagedPlacePicture, PlacePicture>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get place pictures count.
        /// </summary>
        [WebMethod(Description = "Get place pictures count.")]
        public int GetPlacePicturesCount(string ticket, int id)
        {
            return WebServiceImpl<TransitPlacePicture, ManagedPlacePicture, PlacePicture>.GetCount(
                ticket, string.Format("WHERE PlacePicture.Place.Id = {0}", id));
        }

        /// <summary>
        /// Get all place pictures.
        /// </summary>
        /// <param name="placeid">place id</param>
        /// <returns>list of transit place pictures</returns>
        [WebMethod(Description = "Get all place pictures.")]
        public List<TransitPlacePicture> GetPlacePictures(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Place.Id", id) };
            Order[] orders = { Order.Desc("Created") };
            return WebServiceImpl<TransitPlacePicture, ManagedPlacePicture, PlacePicture>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Delete a place picture
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a place picture.")]
        public void DeletePlacePicture(string ticket, int id)
        {
            WebServiceImpl<TransitPlacePicture, ManagedPlacePicture, PlacePicture>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Get a place picture picture if modified since.
        /// </summary>
        /// <param name="id">place picture id</param>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="ifModifiedSince">last update date/time</param>
        /// <returns>transit picture</returns>
        [WebMethod(Description = "Get place picture picture data if modified since.", BufferResponse = true)]
        public TransitPlacePicture GetPlacePictureIfModifiedSinceById(string ticket, int id, DateTime ifModifiedSince)
        {
            TransitPlacePicture t_instance = WebServiceImpl<TransitPlacePicture, ManagedPlacePicture, PlacePicture>.GetById(
                ticket, id);

            if (t_instance.Modified <= ifModifiedSince)
                return null;

            return t_instance;
        }

        #endregion

        #region AccountPlaceType

        /// <summary>
        /// Create or update a account place type.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="type">transit account place type</param>
        [WebMethod(Description = "Create or update a account place type.")]
        public int CreateOrUpdateAccountPlaceType(string ticket, TransitAccountPlaceType type)
        {
            return WebServiceImpl<TransitAccountPlaceType, ManagedAccountPlaceType, AccountPlaceType>.CreateOrUpdate(
                ticket, type);
        }

        /// <summary>
        /// Get a account place type.
        /// </summary>
        /// <returns>transit account place type</returns>
        [WebMethod(Description = "Get a account place type.")]
        public TransitAccountPlaceType GetAccountPlaceTypeById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountPlaceType, ManagedAccountPlaceType, AccountPlaceType>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all account place types.
        /// </summary>
        /// <returns>list of transit account place types</returns>
        [WebMethod(Description = "Get all account place types.")]
        public List<TransitAccountPlaceType> GetAccountPlaceTypes(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitAccountPlaceType, ManagedAccountPlaceType, AccountPlaceType>.GetList(
                ticket, options);
        }

        /// <summary>
        /// Get all account place types count.
        /// </summary>
        /// <returns>number of transit account place types</returns>
        [WebMethod(Description = "Get all account place types count.")]
        public int GetAccountPlaceTypesCount(string ticket)
        {
            return WebServiceImpl<TransitAccountPlaceType, ManagedAccountPlaceType, AccountPlaceType>.GetCount(
                ticket);
        }

        /// <summary>
        /// Delete a account place type
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a account place type.")]
        public void DeleteAccountPlaceType(string ticket, int id)
        {
            WebServiceImpl<TransitAccountPlaceType, ManagedAccountPlaceType, AccountPlaceType>.Delete(
                ticket, id);
        }

        #endregion

        #region AccountPlace

        /// <summary>
        /// Create or update a account place.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="account place">transit account place </param>
        /// <returns>account place id</returns>
        [WebMethod(Description = "Create or update an account place.")]
        public int CreateOrUpdateAccountPlace(string ticket, TransitAccountPlace accountplace)
        {
            return WebServiceImpl<TransitAccountPlace, ManagedAccountPlace, AccountPlace>.CreateOrUpdate(
                ticket, accountplace);
        }

        /// <summary>
        /// Get a account place.
        /// </summary>
        /// <returns>transit account place </returns>
        [WebMethod(Description = "Get an account place.")]
        public TransitAccountPlace GetAccountPlaceById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountPlace, ManagedAccountPlace, AccountPlace>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get account places count by place id.
        /// </summary>
        /// <returns>number of account places</returns>
        [WebMethod(Description = "Get account places count by place id.")]
        public int GetAccountPlacesCountByPlaceId(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountPlace, ManagedAccountPlace, AccountPlace>.GetCount(
                ticket, string.Format("WHERE AccountPlace.Place.Id = {0}", id));
        }

        /// <summary>
        /// Get account places by place id.
        /// </summary>
        /// <returns>transit account places</returns>
        [WebMethod(Description = "Get account places by place id.")]
        public List<TransitAccountPlace> GetAccountPlacesByPlaceId(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expression = { Expression.Eq("Place.Id", id) };
            return WebServiceImpl<TransitAccountPlace, ManagedAccountPlace, AccountPlace>.GetList(
                ticket, options, expression, null);
        }

        /// <summary>
        /// Get account places count by account id.
        /// </summary>
        /// <returns>number of account places</returns>
        [WebMethod(Description = "Get account places count by account id.")]
        public int GetAccountPlacesCount(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountPlace, ManagedAccountPlace, AccountPlace>.GetCount(
                ticket, string.Format("WHERE AccountPlace.Account.Id = {0}", id));
        }

        /// <summary>
        /// Get account places by account id.
        /// </summary>
        /// <returns>transit account places</returns>
        [WebMethod(Description = "Get account places by account id.")]
        public List<TransitAccountPlace> GetAccountPlaces(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expression = { Expression.Eq("Account.Id", id) };
            return WebServiceImpl<TransitAccountPlace, ManagedAccountPlace, AccountPlace>.GetList(
                ticket, options, expression, null);
        }

        /// <summary>
        /// Delete a account place.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a account place.")]
        public void DeleteAccountPlace(string ticket, int id)
        {
            WebServiceImpl<TransitAccountPlace, ManagedAccountPlace, AccountPlace>.Delete(
                ticket, id);
        }

        #endregion

        #region AccountPlaceRequest

        /// <summary>
        /// Create or update an account place request.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="request">transit place request</param>
        /// <returns>place id</returns>
        [WebMethod(Description = "Create or update an account place request.")]
        public int CreateOrUpdateAccountPlaceRequest(string ticket, TransitAccountPlaceRequest request)
        {
            return WebServiceImpl<TransitAccountPlaceRequest, ManagedAccountPlaceRequest, AccountPlaceRequest>.CreateOrUpdate(
                ticket, request);
        }

        /// <summary>
        /// Get a place request.
        /// </summary>
        /// <returns>transit place </returns>
        [WebMethod(Description = "Get a place request.")]
        public TransitAccountPlaceRequest GetAccountPlaceRequestById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountPlaceRequest, ManagedAccountPlaceRequest, AccountPlaceRequest>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get place requests count.
        /// </summary>
        /// <returns>number of transit place requests</returns>
        [WebMethod(Description = "Get place requests count.", CacheDuration = 60)]
        public int GetAccountPlaceRequestsCount(string ticket)
        {
            return WebServiceImpl<TransitAccountPlaceRequest, ManagedAccountPlaceRequest, AccountPlaceRequest>.GetCount(
                ticket);
        }

        /// <summary>
        /// Get place requests.
        /// </summary>
        /// <returns>list of transit place requests</returns>
        [WebMethod(Description = "Get place requests.", CacheDuration = 60)]
        public List<TransitAccountPlaceRequest> GetAccountPlaceRequests(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitAccountPlaceRequest, ManagedAccountPlaceRequest, AccountPlaceRequest>.GetList(
                ticket, options);
        }

        /// <summary>
        /// Get place requests count by place id.
        /// </summary>
        /// <returns>number of transit place requests</returns>
        [WebMethod(Description = "Get place requests count by place id.", CacheDuration = 60)]
        public int GetAccountPlaceRequestsCountByPlaceId(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountPlaceRequest, ManagedAccountPlaceRequest, AccountPlaceRequest>.GetCount(
                ticket, string.Format("WHERE AccountPlaceRequest.Place.Id = {0}", id));
        }

        /// <summary>
        /// Get place requests by place id.
        /// </summary>
        /// <returns>list of transit place requests</returns>
        [WebMethod(Description = "Get place requests by place id.", CacheDuration = 60)]
        public List<TransitAccountPlaceRequest> GetAccountPlaceRequestsByPlaceId(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Place.Id", id) };
            return WebServiceImpl<TransitAccountPlaceRequest, ManagedAccountPlaceRequest, AccountPlaceRequest>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Delete a place request.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a place request.")]
        public void DeleteAccountPlaceRequest(string ticket, int id)
        {
            WebServiceImpl<TransitAccountPlaceRequest, ManagedAccountPlaceRequest, AccountPlaceRequest>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Approve a place request.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Approve a place request.")]
        public void AcceptAccountPlaceRequest(string ticket, int id, string message)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedAccountPlaceRequest m_request = new ManagedAccountPlaceRequest(session, id);
                m_request.Accept(sec, message);
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Reject a place request.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Reject a place request.")]
        public void RejectAccountPlaceRequest(string ticket, int id, string message)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedAccountPlaceRequest m_request = new ManagedAccountPlaceRequest(session, id);
                m_request.Reject(sec, message);
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        #endregion

        #region AccountPlaceFavorite

        /// <summary>
        /// Create or update an account place favorite.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="apf">transit account place favorite</param>
        /// <returns>account place favorite id</returns>
        [WebMethod(Description = "Create or update an account place favorite.")]
        public int CreateOrUpdateAccountPlaceFavorite(string ticket, TransitAccountPlaceFavorite apf)
        {
            return WebServiceImpl<TransitAccountPlaceFavorite, ManagedAccountPlaceFavorite, AccountPlaceFavorite>.CreateOrUpdate(
                ticket, apf);
        }

        /// <summary>
        /// Is a place your favorite?
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="place_id">place id</param>
        /// <returns>account place favorite id</returns>
        [WebMethod(Description = "Is a place your favorite?")]
        public bool IsAccountPlaceFavorite(string ticket, int user_id, int place_id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                AccountPlaceFavorite apf = (AccountPlaceFavorite)session.CreateCriteria(typeof(AccountPlaceFavorite))
                    .Add(Expression.Eq("Account.Id", user_id))
                    .Add(Expression.Eq("Place.Id", place_id))
                    .UniqueResult();
                return (apf != null);
            }
        }

        /// <summary>
        /// Get an account place favorite.
        /// </summary>
        /// <returns>transit account place favorite</returns>
        [WebMethod(Description = "Get an account place favorite.")]
        public TransitAccountPlaceFavorite GetAccountPlaceFavoriteById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountPlaceFavorite, ManagedAccountPlaceFavorite, AccountPlaceFavorite>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get account place favorites count by place id.
        /// </summary>
        /// <returns>number of account place favorites</returns>
        [WebMethod(Description = "Get account place favorites count by place id.")]
        public int GetAccountPlaceFavoritesCount(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountPlaceFavorite, ManagedAccountPlaceFavorite, AccountPlaceFavorite>.GetCount(
                ticket, string.Format("WHERE AccountPlaceFavorite.Place.Id = {0}", id));
        }

        /// <summary>
        /// Get account place favorites by place id.
        /// </summary>
        /// <returns>transit account place favorites</returns>
        [WebMethod(Description = "Get account place favorites by place id.")]
        public List<TransitAccountPlaceFavorite> GetAccountPlaceFavorites(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Place.Id", id) };
            return WebServiceImpl<TransitAccountPlaceFavorite, ManagedAccountPlaceFavorite, AccountPlaceFavorite>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Get account place favorites count.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>transit account place favorites count</returns>
        [WebMethod(Description = "Get account place favorites count.", CacheDuration = 60)]
        public int GetAccountPlaceFavoritesCountByAccountId(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountPlaceFavorite, ManagedAccountPlaceFavorite, AccountPlaceFavorite>.GetCount(
                ticket, string.Format("WHERE AccountPlaceFavorite.Account.Id = {0}", id));
        }

        /// <summary>
        /// Get account place favorites by account id.
        /// </summary>
        /// <returns>transit account place favorites</returns>
        [WebMethod(Description = "Get account place favorites by account id.")]
        public List<TransitAccountPlaceFavorite> GetAccountPlaceFavoritesByAccountId(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", id) };
            return WebServiceImpl<TransitAccountPlaceFavorite, ManagedAccountPlaceFavorite, AccountPlaceFavorite>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Delete an account place favorite.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete an account place favorite.")]
        public void DeleteAccountPlaceFavorite(string ticket, int id)
        {
            WebServiceImpl<TransitAccountPlaceFavorite, ManagedAccountPlaceFavorite, AccountPlaceFavorite>.Delete(
                ticket, id);
        }

        #endregion

        #region Search

        protected IList<Place> InternalSearchPlaces(ISession session, string s, ServiceQueryOptions options)
        {
            int maxsearchresults = ManagedConfiguration.GetValue(session, "SnCore.MaxSearchResults", 128);
            IQuery query = session.CreateSQLQuery(

                    "CREATE TABLE #Results ( Place_Id int, RANK int )\n" +
                    "CREATE TABLE #Unique_Results ( Place_Id int, RANK int )\n" +

                    "INSERT #Results\n" +
                    "SELECT place.Place_Id, ft.[RANK] FROM Place place\n" +
                    "INNER JOIN FREETEXTTABLE (Place, ([Name], [Street], [Zip], [CrossStreet], [Description], [Phone], [Fax], [Email], [Website]), '" +
                        Renderer.SqlEncode(s) + "', " +
                        maxsearchresults.ToString() + ") AS ft ON place.Place_Id = ft.[KEY]\n" +

                    "INSERT #Results\n" +
                    "SELECT place.Place_Id, ft.[RANK] FROM Place place, PlaceName placename\n" +
                    "INNER JOIN FREETEXTTABLE (PlaceName, ([Name]), '" + Renderer.SqlEncode(s) + "', " +
                        maxsearchresults.ToString() + ") AS ft ON placename.PlaceName_Id = ft.[KEY] \n" +
                    "WHERE placename.Place_Id = place.Place_Id\n" +

                    "INSERT #Results\n" +
                    "SELECT place.Place_Id, ft.[RANK] FROM Place place, PlacePropertyValue placepropertyvalue\n" +
                    "INNER JOIN FREETEXTTABLE (PlacePropertyValue, ([Value]), '" + Renderer.SqlEncode(s) + "', " +
                        maxsearchresults.ToString() + ") AS ft ON placepropertyvalue.PlacePropertyValue_Id = ft.[KEY] \n" +
                    "WHERE placepropertyvalue.Place_Id = place.Place_Id\n" +

                    "INSERT #Unique_Results\n" +
                    "SELECT DISTINCT Place_Id, SUM(RANK)\n" +
                    "FROM #Results GROUP BY Place_Id\n" +
                    "ORDER BY SUM(RANK) DESC\n" +

                    "SELECT " + (options != null ? options.GetSqlQueryTop() : string.Empty) +
                    "{Place.*} FROM {Place}, #Unique_Results\n" +
                    "WHERE Place.Place_Id = #Unique_Results.Place_Id\n" +
                    "ORDER BY #Unique_Results.RANK DESC\n" +

                    "DROP TABLE #Results\n" +
                    "DROP TABLE #Unique_Results\n",

                    "Place",
                    typeof(Place));

            return WebServiceQueryOptions<Place>.Apply(options, query.List<Place>());
        }

        /// <summary>
        /// Search places.
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "Search places.", CacheDuration = 60)]
        public List<TransitPlace> SearchPlaces(string ticket, string s, ServiceQueryOptions options)
        {
            if (string.IsNullOrEmpty(s))
                return new List<TransitPlace>();

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                IList<Place> places = InternalSearchPlaces(session, s, options);
                List<TransitPlace> result = new List<TransitPlace>(places.Count);
                foreach (Place p in places)
                {
                    result.Add(new ManagedPlace(session, p).GetTransitInstance(sec));
                }
                return result;
            }
        }

        /// <summary>
        /// Return the number of places matching a query.
        /// </summary>
        /// <returns>number of places</returns>
        [WebMethod(Description = "Return the number of places matching a query.", CacheDuration = 60)]
        public int SearchPlacesCount(string ticket, string s)
        {
            if (string.IsNullOrEmpty(s))
                return 0;

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return InternalSearchPlaces(session, s, null).Count;
            }
        }

        #endregion

        #region PlaceName

        /// <summary>
        /// Create or update a place name.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="placename">transit place name</param>
        [WebMethod(Description = "Create or update a place name.")]
        public int CreateOrUpdatePlaceName(string ticket, TransitPlaceName placename)
        {
            return WebServiceImpl<TransitPlaceName, ManagedPlaceName, PlaceName>.CreateOrUpdate(
                ticket, placename);
        }

        /// <summary>
        /// Get a place name.
        /// </summary>
        /// <returns>transit place name</returns>
        [WebMethod(Description = "Get a place name.")]
        public TransitPlaceName GetPlaceNameById(string ticket, int id)
        {
            return WebServiceImpl<TransitPlaceName, ManagedPlaceName, PlaceName>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all place names count.
        /// </summary>
        /// <param name="placeid">place id</param>
        /// <returns>number of transit place names</returns>
        [WebMethod(Description = "Get all place names count.")]
        public int GetPlaceNamesCount(string ticket, int id)
        {
            return WebServiceImpl<TransitPlaceName, ManagedPlaceName, PlaceName>.GetCount(
                ticket, string.Format("WHERE PlaceName.Place.Id = {0}", id));
        }

        /// <summary>
        /// Get all place names.
        /// </summary>
        /// <param name="placeid">place id</param>
        /// <returns>list of transit place names</returns>
        [WebMethod(Description = "Get all place names.")]
        public List<TransitPlaceName> GetPlaceNames(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Place.Id", id) };
            return WebServiceImpl<TransitPlaceName, ManagedPlaceName, PlaceName>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Delete a place name
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a place name.")]
        public void DeletePlaceName(string ticket, int id)
        {
            WebServiceImpl<TransitPlaceName, ManagedPlaceName, PlaceName>.Delete(
                ticket, id);
        }

        #endregion

        #region PlacePropertyGroup

        /// <summary>
        /// Create or update a property group.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="PropertyGroup">transit property group</param>
        [WebMethod(Description = "Create or update a property group.")]
        public int CreateOrUpdatePlacePropertyGroup(string ticket, TransitPlacePropertyGroup pg)
        {
            return WebServiceImpl<TransitPlacePropertyGroup, ManagedPlacePropertyGroup, PlacePropertyGroup>.CreateOrUpdate(
                ticket, pg);
        }

        /// <summary>
        /// Get a property group.
        /// </summary>
        /// <returns>transit property group</returns>
        [WebMethod(Description = "Get a property group.")]
        public TransitPlacePropertyGroup GetPlacePropertyGroupById(string ticket, int id)
        {
            return WebServiceImpl<TransitPlacePropertyGroup, ManagedPlacePropertyGroup, PlacePropertyGroup>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all property groups count.
        /// </summary>
        /// <returns>number of transit property groups</returns>
        [WebMethod(Description = "Get all property groups count.")]
        public int GetPlacePropertyGroupsCount(string ticket)
        {
            return WebServiceImpl<TransitPlacePropertyGroup, ManagedPlacePropertyGroup, PlacePropertyGroup>.GetCount(
                ticket);
        }

        /// <summary>
        /// Get all property groups.
        /// </summary>
        /// <returns>list of transit property groups</returns>
        [WebMethod(Description = "Get all property groups.")]
        public List<TransitPlacePropertyGroup> GetPlacePropertyGroups(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitPlacePropertyGroup, ManagedPlacePropertyGroup, PlacePropertyGroup>.GetList(
                ticket, options);
        }

        /// <summary>
        /// Delete a property group
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a property group.")]
        public void DeletePlacePropertyGroup(string ticket, int id)
        {
            WebServiceImpl<TransitPlacePropertyGroup, ManagedPlacePropertyGroup, PlacePropertyGroup>.Delete(
                ticket, id);
        }

        #endregion

        #region PlaceProperty

        /// <summary>
        /// Create or update a property.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="property">transit property</param>
        [WebMethod(Description = "Create or update a property.")]
        public int CreateOrUpdatePlaceProperty(string ticket, TransitPlaceProperty p)
        {
            return WebServiceImpl<TransitPlaceProperty, ManagedPlaceProperty, PlaceProperty>.CreateOrUpdate(
                ticket, p);
        }

        /// <summary>
        /// Get a place property.
        /// </summary>
        /// <returns>transit property</returns>
        [WebMethod(Description = "Get a place property.")]
        public TransitPlaceProperty GetPlacePropertyById(string ticket, int id)
        {
            return WebServiceImpl<TransitPlaceProperty, ManagedPlaceProperty, PlaceProperty>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all properties count.
        /// </summary>
        /// <returns>number of transit properties</returns>
        [WebMethod(Description = "Get all properties count.")]
        public int GetPlacePropertiesCount(string ticket, int gid)
        {
            return WebServiceImpl<TransitPlaceProperty, ManagedPlaceProperty, PlaceProperty>.GetCount(
                ticket, string.Format("WHERE PlaceProperty.PlacePropertyGroup.Id = {0}", gid));
        }

        /// <summary>
        /// Get all properties.
        /// </summary>
        /// <returns>list of transit properties</returns>
        [WebMethod(Description = "Get all properties.")]
        public List<TransitPlaceProperty> GetPlaceProperties(string ticket, int gid, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("PlacePropertyGroup.Id", gid) };
            return WebServiceImpl<TransitPlaceProperty, ManagedPlaceProperty, PlaceProperty>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Delete a property
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a property.")]
        public void DeletePlaceProperty(string ticket, int id)
        {
            WebServiceImpl<TransitPlaceProperty, ManagedPlaceProperty, PlaceProperty>.Delete(
                ticket, id);
        }

        #endregion

        #region PlacePropertyValue

        /// <summary>
        /// Get distinct place property values.
        /// </summary>
        /// <returns>list of possible values</returns>
        [WebMethod(Description = "Get distinct place property values.")]
        public List<TransitDistinctPlacePropertyValue> GetDistinctPropertyValues(
            string ticket, string groupname, string propertyname, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                IQuery query = session.CreateQuery(
                   "SELECT ppv FROM PlacePropertyValue ppv, PlacePropertyGroup ppg, PlaceProperty pp" +
                   " WHERE pp.PlacePropertyGroup.Id = ppg.Id" +
                   " AND ppv.PlaceProperty.Id = pp.Id" +
                   " AND ppg.Name = '" + Renderer.SqlEncode(groupname) + "'" +
                   " AND pp.Name = '" + Renderer.SqlEncode(propertyname) + "'");

                if (options != null)
                {
                    query.SetFirstResult(options.FirstResult);
                    query.SetMaxResults(options.PageSize);
                }

                IList list = query.List();

                SortedDictionary<string, TransitDistinctPlacePropertyValue> dict = new SortedDictionary<string, TransitDistinctPlacePropertyValue>();

                foreach (PlacePropertyValue pv in list)
                {
                    StringCollection values = new StringCollection();

                    switch (pv.PlaceProperty.TypeName)
                    {
                        case "System.Array":
                            values.AddRange(pv.Value.Split("\"".ToCharArray()));
                            break;
                        default:
                            values.Add(pv.Value);
                            break;
                    }

                    foreach (string s in values)
                    {
                        if (! string.IsNullOrEmpty(s))
                        {
                            TransitDistinctPlacePropertyValue tdppv = null;
                            if (!dict.TryGetValue(s, out tdppv))
                            {
                                tdppv = new TransitDistinctPlacePropertyValue();
                                tdppv.Count = 1;
                                tdppv.Value = s;
                                dict.Add(s, tdppv);
                            }
                            else
                            {
                                tdppv.Count++;
                            }
                        }
                    }
                }

                List<TransitDistinctPlacePropertyValue> result = new List<TransitDistinctPlacePropertyValue>(dict.Count);
                foreach (KeyValuePair<string, TransitDistinctPlacePropertyValue> tv in dict)
                {
                    result.Add(tv.Value);
                }

                return result;
            }
        }

        /// <summary>
        /// Get places that match a property value by name.
        /// </summary>
        /// <returns>transit places</returns>
        [WebMethod(Description = "Get places that match a property value by name.")]
        public List<TransitPlace> GetPlacesByPropertyValue(string ticket, string groupname, string propertyname, string propertyvalue, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitPlace, ManagedPlace, Place>.GetList(
                ticket, options,
                   "SELECT {place.*} FROM PlaceProperty p, PlacePropertyGroup g, PlacePropertyValue v, Place {place}" +
                   " WHERE {place}.Place_Id = v.Place_Id" +
                   " AND v.Place_Id = {place}.Place_Id" +
                   " AND v.PlaceProperty_Id = p.PlaceProperty_Id" +
                   " AND p.PlacePropertyGroup_Id = g.PlacePropertyGroup_Id" +
                   " AND p.Name = '" + Renderer.SqlEncode(propertyname) + "'" +
                   " AND (" +
                   "  v.Value LIKE '" + Renderer.SqlEncode(propertyvalue) + "'" +
                   "  OR v.Value LIKE '%\"" + Renderer.SqlEncode(propertyvalue) + "\"%'" +
                   " ) AND g.Name = '" + Renderer.SqlEncode(groupname) + "'",
                   "place");
        }

        /// <summary>
        /// Get the number of places that match a property value by name.
        /// </summary>
        /// <returns>transit places</returns>
        [WebMethod(Description = "Get the number of places that match a property value by name.")]
        public int GetPlacesByPropertyValueCount(string ticket, string groupname, string propertyname, string propertyvalue)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                IQuery query = session.CreateQuery(
                   "SELECT COUNT(*) FROM PlaceProperty p, PlacePropertyGroup g, PlacePropertyValue v, Place place" +
                   " WHERE place.Id = v.Place.Id" +
                   " AND v.Place.Id = place.Id" +
                   " AND v.PlaceProperty.Id = p.Id" +
                   " AND p.PlacePropertyGroup.Id = g.Id" +
                   " AND p.Name = '" + Renderer.SqlEncode(propertyname) + "'" +
                   " AND (" +
                   "  v.Value LIKE '" + Renderer.SqlEncode(propertyvalue) + "'" +
                   "  OR v.Value LIKE '%\"" + Renderer.SqlEncode(propertyvalue) + "\"%'" +
                   " ) AND g.Name = '" + Renderer.SqlEncode(groupname) + "'");

                return (int)query.UniqueResult();
            }
        }

        /// <summary>
        /// Get a place property value by name.
        /// </summary>
        /// <returns>transit place property value</returns>
        [WebMethod(Description = "Get a place property value by group and name.")]
        public TransitPlacePropertyValue GetPlacePropertyValueByName(string ticket, int placeid, string groupname, string propertyname)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                PlacePropertyGroup ppg = (PlacePropertyGroup)session.CreateCriteria(typeof(PlacePropertyGroup))
                    .Add(Expression.Eq("Name", groupname))
                    .UniqueResult();

                if (ppg == null)
                {
                    throw new Exception(string.Format(
                        "No property group with the name \"{0}\" found.", groupname));
                }

                PlaceProperty pp = (PlaceProperty)session.CreateCriteria(typeof(PlaceProperty))
                    .Add(Expression.Eq("Name", propertyname))
                    .Add(Expression.Eq("PlacePropertyGroup.Id", ppg.Id))
                    .UniqueResult();

                if (pp == null)
                {
                    throw new Exception(string.Format(
                        "No property with the name \"{0}\" found.", propertyname));
                }

                PlacePropertyValue ppv = (PlacePropertyValue)session.CreateCriteria(typeof(PlacePropertyValue))
                    .Add(Expression.Eq("Place.Id", placeid))
                    .Add(Expression.Eq("PlaceProperty.Id", pp.Id))
                    .UniqueResult();

                if (ppv == null)
                {
                    throw new Exception(string.Format(
                        "No property value for \"{0}\" of place \"{0}\" of group \"{0}\" found.",
                        propertyname, placeid, groupname));
                }

                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                TransitPlacePropertyValue result = new ManagedPlacePropertyValue(session, ppv).GetTransitInstance(sec);
                return result;
            }
        }

        /// <summary>
        /// Create or update a place property value.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="type">transit place property value</param>
        [WebMethod(Description = "Create or update a place property value.")]
        public int CreateOrUpdatePlacePropertyValue(string ticket, TransitPlacePropertyValue propertyvalue)
        {
            return WebServiceImpl<TransitPlacePropertyValue, ManagedPlacePropertyValue, PlacePropertyValue>.CreateOrUpdate(
                ticket, propertyvalue);
        }

        /// <summary>
        /// Get a place property value.
        /// </summary>
        /// <returns>transit place property value</returns>
        [WebMethod(Description = "Get a place property value.")]
        public TransitPlacePropertyValue GetPlacePropertyValueById(string ticket, int id)
        {
            return WebServiceImpl<TransitPlacePropertyValue, ManagedPlacePropertyValue, PlacePropertyValue>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get place property values count.
        /// </summary>
        /// <returns>number of place property values</returns>
        [WebMethod(Description = "Get place property values count.", CacheDuration = 60)]
        public int GetPlacePropertyValuesCount(string ticket, int placeid, int groupid)
        {
            return WebServiceImpl<TransitPlacePropertyValue, ManagedPlacePropertyValue, PlacePropertyValue>.GetCount(
                ticket, string.Format(
                        "WHERE PlacePropertyValue.Place.Id = {0}" +
                        " AND PlacePropertyValue.PlaceProperty.PlacePropertyGroup.Id = {1}" +
                        " AND PlacePropertyValue.PlaceProperty.Publish = 1",
                    placeid, groupid));
        }

        /// <summary>
        /// Get place property values.
        /// </summary>
        /// <returns>list of place property values</returns>
        [WebMethod(Description = "Get place property values.", CacheDuration = 60)]
        public List<TransitPlacePropertyValue> GetPlacePropertyValues(string ticket, int placeid, int groupid, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitPlacePropertyValue, ManagedPlacePropertyValue, PlacePropertyValue>.GetList(
                ticket, options, string.Format(
                        "SELECT PlacePropertyValue FROM PlacePropertyValue PlacePropertyValue " +
                        " WHERE PlacePropertyValue.Place.Id = {0}" +
                        " AND PlacePropertyValue.PlaceProperty.PlacePropertyGroup.Id = {1}" +
                        " AND PlacePropertyValue.PlaceProperty.Publish = 1", placeid, groupid));
        }

        /// <summary>
        /// Get all place property values, including unfilled ones.
        /// </summary>
        /// <returns>list of place property values</returns>
        [WebMethod(Description = "Get all place property values, including unfilled ones.", CacheDuration = 60)]
        public List<TransitPlacePropertyValue> GetAllPlacePropertyValuesById(string ticket, int placeid, int groupid)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);

                ICriteria c = session.CreateCriteria(typeof(PlaceProperty));
                if (groupid > 0) c.Add(Expression.Eq("PlacePropertyGroup.Id", groupid));
                IList properties = c.List();

                List<TransitPlacePropertyValue> result = new List<TransitPlacePropertyValue>(properties.Count);

                foreach (PlaceProperty property in properties)
                {
                    PlacePropertyValue value = (PlacePropertyValue)session.CreateCriteria(typeof(PlacePropertyValue))
                        .Add(Expression.Eq("Place.Id", placeid))
                        .Add(Expression.Eq("PlaceProperty.Id", property.Id))
                        .UniqueResult();

                    if (value == null)
                    {
                        value = new PlacePropertyValue();
                        value.PlaceProperty = property;
                        value.Value = property.DefaultValue;
                        if (placeid > 0) value.Place = (Place)session.Load(typeof(Place), placeid);
                    }

                    ManagedPlacePropertyValue m_ppv = new ManagedPlacePropertyValue(session, value);
                    result.Add(m_ppv.GetTransitInstance(sec));
                }

                return result;
            }
        }

        /// <summary>
        /// Delete a place property value.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a place property value.")]
        public void DeletePlacePropertyValue(string ticket, int id)
        {
            WebServiceImpl<TransitPlacePropertyValue, ManagedPlacePropertyValue, PlacePropertyValue>.Delete(
                ticket, id);
        }

        #endregion

        #region Place Attribute
        /// <summary>
        /// Create or update a place attribute.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="type">transit place attribute</param>
        [WebMethod(Description = "Create or update a place attribute.")]
        public int CreateOrUpdatePlaceAttribute(string ticket, TransitPlaceAttribute attribute)
        {
            return WebServiceImpl<TransitPlaceAttribute, ManagedPlaceAttribute, PlaceAttribute>.CreateOrUpdate(
                ticket, attribute);
        }

        /// <summary>
        /// Get place attributes.
        /// </summary>
        /// <returns>transit place attribute</returns>
        [WebMethod(Description = "Get place attributes.")]
        public TransitPlaceAttribute GetPlaceAttributeById(string ticket, int id)
        {
            return WebServiceImpl<TransitPlaceAttribute, ManagedPlaceAttribute, PlaceAttribute>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get place attributes count.
        /// </summary>
        /// <returns>number of place attributes</returns>
        [WebMethod(Description = "Get place attributes count.", CacheDuration = 60)]
        public int GetPlaceAttributesCount(string ticket, int id)
        {
            return WebServiceImpl<TransitPlaceAttribute, ManagedPlaceAttribute, PlaceAttribute>.GetCount(
                ticket, string.Format("WHERE PlaceAttribute.Place.Id = {0}", id));
        }

        /// <summary>
        /// Get place attributes.
        /// </summary>
        /// <returns>list of place attributes</returns>
        [WebMethod(Description = "Get place attributes.", CacheDuration = 60)]
        public List<TransitPlaceAttribute> GetPlaceAttributes(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Place.Id", id) };
            return WebServiceImpl<TransitPlaceAttribute, ManagedPlaceAttribute, PlaceAttribute>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Delete a place attribute.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a place attribute.")]
        public void DeletePlaceAttribute(string ticket, int id)
        {
            WebServiceImpl<TransitPlaceAttribute, ManagedPlaceAttribute, PlaceAttribute>.Delete(
                ticket, id);
        }

        #endregion

        #region Popular Places

        /// <summary>
        /// Get favorite (popular) places count.
        /// </summary>
        /// <returns>list of transit places</returns>
        [WebMethod(Description = "Get favorite (popular) places count.", CacheDuration = 60)]
        public int GetFavoritePlacesCount(string ticket)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IQuery q = session.CreateQuery("SELECT COUNT(DISTINCT apf.Place) FROM AccountPlaceFavorite apf");
                return (int)(long)q.UniqueResult();
            }
        }

        /// <summary>
        /// Get favorite (popular) places.
        /// </summary>
        /// <returns>list of transit places</returns>
        [WebMethod(Description = "Get favorite (popular) places.", CacheDuration = 60)]
        public List<TransitPlace> GetFavoritePlaces(string ticket, ServiceQueryOptions serviceoptions)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                IQuery q = session.CreateSQLQuery(
                    "CREATE TABLE #fav (	[Id] [int],	[Score] [int] )\n" +
                    "INSERT INTO #fav ( [Id], [Score] ) " +
                    " SELECT Place_Id, 1 FROM AccountPlaceFavorite " +
                    "CREATE TABLE #pl (	[Id] [int],	[Score] [int] )\n" +
                    "INSERT INTO #pl ( [Id], [Score] )" +
                    " SELECT Id, SUM(Score) AS 'Score' FROM #fav " +
                    " GROUP BY Id\n" +
                    "SELECT " + (serviceoptions != null ? serviceoptions.GetSqlQueryTop() : string.Empty) +
                    " {Place.*} FROM {Place} INNER JOIN #pl" +
                    " ON #pl.Id = Place.Place_Id" +
                    " ORDER BY [Score] DESC\n" +
                    "DROP TABLE #pl\n" +
                    "DROP TABLE #fav ",
                    "Place",
                    typeof(Place));

                //if (serviceoptions != null)
                //{
                //    q.SetMaxResults(serviceoptions.PageSize);
                //    q.SetFirstResult(serviceoptions.FirstResult);
                //}

                IList<Place> list = WebServiceQueryOptions<Place>.Apply(serviceoptions, q.List<Place>());
                List<TransitPlace> result = new List<TransitPlace>(list.Count);

                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                foreach (Place p in list)
                {
                    result.Add(new ManagedPlace(session, p).GetTransitInstance(sec));
                }

                return result;
            }
        }

        #endregion

        #region Place Queue

        /// <summary>
        /// Create or update a place queue.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="queue">transit place queue</param>
        [WebMethod(Description = "Create or update a place queue.")]
        public int CreateOrUpdatePlaceQueue(string ticket, TransitPlaceQueue queue)
        {
            return WebServiceImpl<TransitPlaceQueue, ManagedPlaceQueue, PlaceQueue>.CreateOrUpdate(
                ticket, queue);
        }

        /// <summary>
        /// Get a place queue.
        /// </summary>
        /// <returns>transit place queue</returns>
        [WebMethod(Description = "Get a place queue.")]
        public TransitPlaceQueue GetPlaceQueueById(string ticket, int id)
        {
            return WebServiceImpl<TransitPlaceQueue, ManagedPlaceQueue, PlaceQueue>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get a place queue by name.
        /// </summary>
        /// <returns>transit place queue</returns>
        [WebMethod(Description = "Get a place queue by name.")]
        public TransitPlaceQueue GetPlaceQueueByName(string ticket, int user_id, string name)
        {
            try
            {
                ICriterion[] expressions = 
                { 
                    Expression.Eq("Name", name),
                    Expression.Eq("Account.Id", user_id)
                };

                return WebServiceImpl<TransitPlaceQueue, ManagedPlaceQueue, PlaceQueue>.GetByCriterion(
                    ticket, expressions);
            }
            catch (ObjectNotFoundException)
            {
                return null;
            }
        }

        /// <summary>
        /// Get or create a place queue by name, create if doesn't exist.
        /// </summary>
        /// <returns>transit place queue</returns>
        [WebMethod(Description = "Get a place queue by name, create if doesn't exist.")]
        public TransitPlaceQueue GetOrCreatePlaceQueueByName(string ticket, int user_id, string name)
        {
            TransitPlaceQueue t_queue = GetPlaceQueueByName(ticket, user_id, name);
            
            if (t_queue == null)
            {
                t_queue = new TransitPlaceQueue();
                t_queue.AccountId = user_id;
                t_queue.Name = name;
                t_queue.PublishAll = false;
                t_queue.PublishFriends = true;
                t_queue.Id = CreateOrUpdatePlaceQueue(ticket, t_queue);
            }

            return t_queue;
        }

        /// <summary>
        /// Get all place queues by user id.
        /// </summary>
        /// <returns>list of transit place queues</returns>
        [WebMethod(Description = "Get all place queues.", CacheDuration = 60)]
        public List<TransitPlaceQueue> GetPlaceQueues(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", id) };
            return WebServiceImpl<TransitPlaceQueue, ManagedPlaceQueue, PlaceQueue>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Get number of all place queues by user id.
        /// </summary>
        /// <returns>list of transit place queues</returns>
        [WebMethod(Description = "Get all place queues count.", CacheDuration = 60)]
        public int GetPlaceQueuesCount(string ticket, int id)
        {
            return WebServiceImpl<TransitPlaceQueue, ManagedPlaceQueue, PlaceQueue>.GetCount(
                ticket, string.Format("WHERE PlaceQueue.Account.Id = {0}", id));
        }

        /// <summary>
        /// Delete a place queue
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a place queue.")]
        public void DeletePlaceQueue(string ticket, int id)
        {
            WebServiceImpl<TransitPlaceQueue, ManagedPlaceQueue, PlaceQueue>.Delete(
                ticket, id);
        }

        #endregion

        #region Place Queue Item

        /// <summary>
        /// Create or update a place queue item.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="queueitem">transit place queue item</param>
        [WebMethod(Description = "Create or update a place queue item.")]
        public int CreateOrUpdatePlaceQueueItem(string ticket, TransitPlaceQueueItem queueitem)
        {
            return WebServiceImpl<TransitPlaceQueueItem, ManagedPlaceQueueItem, PlaceQueueItem>.CreateOrUpdate(
                ticket, queueitem);
        }

        /// <summary>
        /// Get a place queue item.
        /// </summary>
        /// <returns>transit place queue item</returns>
        [WebMethod(Description = "Get a place queue item.")]
        public TransitPlaceQueueItem GetPlaceQueueItemById(string ticket, int id)
        {
            return WebServiceImpl<TransitPlaceQueueItem, ManagedPlaceQueueItem, PlaceQueueItem>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all place queue items in a queue.
        /// </summary>
        /// <returns>list of transit place queue items</returns>
        [WebMethod(Description = "Get all place queue items.", CacheDuration = 60)]
        public List<TransitPlaceQueueItem> GetPlaceQueueItems(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("PlaceQueue.Id", id) };
            return WebServiceImpl<TransitPlaceQueueItem, ManagedPlaceQueueItem, PlaceQueueItem>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Get place queue items count.
        /// </summary>
        [WebMethod(Description = "Get all place queue items.", CacheDuration = 60)]
        public int GetPlaceQueueItemsCount(string ticket, int id)
        {
            return WebServiceImpl<TransitPlaceQueueItem, ManagedPlaceQueueItem, PlaceQueueItem>.GetCount(
                ticket, string.Format("WHERE PlaceQueueItem.PlaceQueue.Id = {0}", id));
        }

        /// <summary>
        /// Delete a place queueitem
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a place queueitem.")]
        public void DeletePlaceQueueItem(string ticket, int id)
        {
            WebServiceImpl<TransitPlaceQueueItem, ManagedPlaceQueueItem, PlaceQueueItem>.Delete(
                ticket, id);
        }

        #endregion

        #region Friends' Place Queue Items

        /// <summary>
        /// Get all place queue items count in a queue of self and friends.
        /// </summary>
        [WebMethod(Description = "Get all place queue items.", CacheDuration = 60)]
        public int GetFriendsPlaceQueueItemsCount(string ticket, int user_id)
        {
            return GetFriendsPlaceQueueItems(ticket, user_id, null).Count;
        }

        /// <summary>
        /// Get all place queue items in a queue of self and friends.
        /// </summary>
        /// <returns>list of transit place queue items</returns>
        [WebMethod(Description = "Get all place queue items.", CacheDuration = 60)]
        public List<TransitFriendsPlaceQueueItem> GetFriendsPlaceQueueItems(string ticket, int user_id, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                // todo: use security context
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                Dictionary<Place, List<Account>> favorites = new Dictionary<Place, List<Account>>();

                // add your own places
                // ManagedPlaceQueueItem.GetPlaces(acct, favorites);

                IList friends = session.CreateQuery(string.Format("SELECT FROM AccountFriend f " +
                        "WHERE (f.Account.Id = {0} OR f.Keen.Id = {0})", user_id)).List();

                foreach (AccountFriend friend in friends)
                {
                    ManagedPlaceQueueItem.GetPlaces(friend.Keen.Id == user_id ? friend.Account : friend.Keen, favorites);
                }

                List<TransitFriendsPlaceQueueItem> result = new List<TransitFriendsPlaceQueueItem>(favorites.Count);
                Dictionary<Place, List<Account>>.Enumerator enumerator = favorites.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    result.Add(new TransitFriendsPlaceQueueItem(
                        enumerator.Current.Key, enumerator.Current.Value));
                }

                return result;
            }
        }

        #endregion

        #region Neighborhoods
        /// <summary>
        /// Get all place neighborhoods.
        /// </summary>
        /// <returns>list of transit place neighborhoods</returns>
        [WebMethod(Description = "Get all place neighborhoods.", CacheDuration = 60)]
        public List<TransitDistinctPlaceNeighborhood> GetPlaceNeighborhoods(
            string ticket, string country, string state, string city, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                // todo: use the security context
                int city_id = 0;
                ManagedCity.TryGetCityId(session, city, state, country, out city_id);
                IQuery q = session.CreateQuery(
                    "SELECT n.Id, n.Name, COUNT(p) FROM Neighborhood n, Place p" +
                    " WHERE p.Neighborhood = n" +
                    " AND n.City.Id = '" + city_id.ToString() + "'" +
                    " AND p.City.Id = '" + city_id.ToString() + "'" +
                    " GROUP BY n.Id, n.Name ORDER BY n.Name");

                if (options != null)
                {
                    q.SetFirstResult(options.FirstResult);
                    q.SetMaxResults(options.PageSize);
                }

                IList neighborhoods = q.List();
                List<TransitDistinctPlaceNeighborhood> result = new List<TransitDistinctPlaceNeighborhood>(neighborhoods.Count);
                foreach (object[] nh in neighborhoods)
                {
                    TransitDistinctPlaceNeighborhood tnh = new TransitDistinctPlaceNeighborhood();
                    tnh.Name = (string)nh[1];
                    tnh.Count = (long)nh[2];
                    result.Add(tnh);
                }

                return result;
            }
        }

        #endregion
    }
}