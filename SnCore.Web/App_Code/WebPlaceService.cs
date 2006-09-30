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
    /// 
    [WebService(Namespace = "http://www.vestris.com/sncore/ns/", Name = "WebPlaceService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WebPlaceService : WebService
    {
        public WebPlaceService()
        {

        }

        #region Place Type

        /// <summary>
        /// Create or update a place type.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="type">transit place type</param>
        [WebMethod(Description = "Create or update a place type.")]
        public int CreateOrUpdatePlaceType(string ticket, TransitPlaceType type)
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

                ManagedPlaceType m_type = new ManagedPlaceType(session);
                m_type.CreateOrUpdate(type);
                SnCore.Data.Hibernate.Session.Flush();
                return m_type.Id;
            }
        }

        /// <summary>
        /// Get a place type.
        /// </summary>
        /// <returns>transit place type</returns>
        [WebMethod(Description = "Get a place type.")]
        public TransitPlaceType GetPlaceTypeById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitPlaceType result = new ManagedPlaceType(session, id).TransitPlaceType;
                return result;
            }
        }

        /// <summary>
        /// Get all place types.
        /// </summary>
        /// <returns>list of transit place types</returns>
        [WebMethod(Description = "Get all place types.", CacheDuration = 60)]
        public List<TransitPlaceType> GetPlaceTypes()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList types = session.CreateCriteria(typeof(PlaceType)).List();
                List<TransitPlaceType> result = new List<TransitPlaceType>(types.Count);
                foreach (PlaceType type in types)
                {
                    result.Add(new ManagedPlaceType(session, type).TransitPlaceType);
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete a place type
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a place type.")]
        public void DeletePlaceType(string ticket, int id)
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

                ManagedPlaceType m_type = new ManagedPlaceType(session, id);
                m_type.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
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
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.HasVerifiedEmail)
                    throw new ManagedAccount.NoVerifiedEmailException();

                if ((place.Id != 0) && (!user.IsAdministrator()))
                {
                    ManagedPlace m_place = new ManagedPlace(session, place.Id);
                    if (!m_place.CanWrite(userid))
                    {
                        throw new ManagedAccount.AccessDeniedException();
                    }
                }

                if (place.Id == 0) place.AccountId = user.Id;
                int result = user.CreateOrUpdate(place);

                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get a place.
        /// </summary>
        /// <returns>transit place </returns>
        [WebMethod(Description = "Get a place.")]
        public TransitPlace GetPlaceById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitPlace result = new ManagedPlace(session, id).TransitPlace;
                return result;
            }
        }

        /// <summary>
        /// Get places count.
        /// </summary>
        /// <returns>transit places count</returns>
        [WebMethod(Description = "Get all places count.", CacheDuration = 60)]
        public int GetPlacesCount(TransitPlaceQueryOptions queryoptions)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)queryoptions.CreateCountQuery(session).UniqueResult();
            }
        }

        /// <summary>
        /// Get all places.
        /// </summary>
        /// <returns>list of transit places</returns>
        [WebMethod(Description = "Get all places.", CacheDuration = 60)]
        public List<TransitPlace> GetPlaces(TransitPlaceQueryOptions queryoptions, ServiceQueryOptions serviceoptions)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IQuery q = queryoptions.CreateQuery(session);

                if (serviceoptions != null)
                {
                    q.SetMaxResults(serviceoptions.PageSize);
                    q.SetFirstResult(serviceoptions.PageNumber * serviceoptions.PageSize);
                }

                IList list = q.List();

                List<TransitPlace> result = new List<TransitPlace>(list.Count);
                foreach (Place p in list)
                {
                    result.Add(new ManagedPlace(session, p).TransitPlace);
                }

                return result;
            }
        }

        /// <summary>
        /// Delete a place.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a place.")]
        public void DeletePlace(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccount user = new ManagedAccount(session, userid);

                ManagedPlace m_place = new ManagedPlace(session, id);

                if (! m_place.CanWrite(userid) && !user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                m_place.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Find a place.
        /// </summary>
        /// <param name="citytag">city name or tag</param>
        /// <param name="name">place name</param>
        /// <returns></returns>
        [WebMethod(Description = "Find a place.", CacheDuration = 60)]
        public TransitPlace FindPlace(string citytag, string name)
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
                    p = (Place) session.CreateQuery(string.Format(
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

                return new ManagedPlace(session, p).TransitPlace;
            }
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
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);

                Place place = (Place) session.Load(typeof(Place), placepicture.PlaceId);

                if ((placepicture.Id != 0) && (!user.IsAdministrator()))
                {
                    ManagedPlace m_place = new ManagedPlace(session, place);
                    if (!m_place.CanWrite(userid))
                    {
                        throw new ManagedAccount.AccessDeniedException();
                    }
                }

                ManagedPlacePicture m_placepicture = new ManagedPlacePicture(session);
                m_placepicture.CreateOrUpdate(placepicture);

                // send a message to place owner TODO: a place picture should have an AccountId of who posted it

                if (user.Id != place.Account.Id)
                {
                    ManagedAccount acct = new ManagedAccount(session, place.Account);
                    if (acct.HasVerifiedEmail)
                    {
                        ManagedSiteConnector.SendAccountEmailMessageUriAsAdmin(
                            session,
                            new MailAddress(acct.ActiveEmailAddress, acct.Name).ToString(),
                            string.Format("EmailPlacePicture.aspx?id={0}", m_placepicture.Id));
                    }
                }

                SnCore.Data.Hibernate.Session.Flush();
                return m_placepicture.Id;
            }
        }

        /// <summary>
        /// Get a place picture.
        /// </summary>
        /// <returns>transit place picture</returns>
        [WebMethod(Description = "Get a place picture.")]
        public TransitPlacePicture GetPlacePictureById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitPlacePicture result = new ManagedPlacePicture(session, id).TransitPlacePicture;
                return result;
            }
        }

        /// <summary>
        /// Get place pictures count.
        /// </summary>
        [WebMethod(Description = "Get place pictures count.")]
        public int GetPlacePicturesCountById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery(string.Format(
                    "SELECT COUNT(p) FROM PlacePicture p WHERE p.Place.Id = {0}",
                    id)).UniqueResult();
            }
        }

        /// <summary>
        /// Get all place pictures.
        /// </summary>
        /// <param name="placeid">place id</param>
        /// <returns>list of transit place pictures</returns>
        [WebMethod(Description = "Get all place pictures.")]
        public List<TransitPlacePicture> GetPlacePicturesById(int placeid, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ICriteria c = session.CreateCriteria(typeof(PlacePicture))
                    .Add(Expression.Eq("Place.Id", placeid));

                if (options != null)
                {
                    c.SetFirstResult(options.FirstResult);
                    c.SetMaxResults(options.PageSize);
                }

                IList placepictures = c.List();
                List<TransitPlacePicture> result = new List<TransitPlacePicture>(placepictures.Count);
                foreach (PlacePicture placepicture in placepictures)
                {
                    result.Add(new ManagedPlacePicture(session, placepicture).TransitPlacePicture);
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete a place picture
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a place picture.")]
        public void DeletePlacePicture(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccount user = new ManagedAccount(session, userid);
                ManagedPlacePicture m_placepicture = new ManagedPlacePicture(session, id);

                if (!user.IsAdministrator())
                {
                    ManagedPlace place = new ManagedPlace(session, m_placepicture.Place.Id);
                    if (!place.CanWrite(userid))
                    {
                        throw new ManagedAccount.AccessDeniedException();
                    }
                }

                m_placepicture.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        #endregion

        #region Place Picture with Bitmaps

        /// <summary>
        /// Get place picture data.
        /// </summary>
        /// <param name="id">place picture id</param>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit place picture</returns>
        [WebMethod(Description = "Get place picture data.", BufferResponse = true)]
        public TransitPlacePictureWithBitmap GetPlacePictureWithBitmapById(string ticket, int id)
        {
            // todo: check permissions with ticket
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedPlacePicture a = new ManagedPlacePicture(session, id);
                return a.TransitPlacePictureWithBitmap;
            }
        }

        /// <summary>
        /// Get place picture data if modified since.
        /// </summary>
        /// <param name="id">place picture id</param>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="ifModifiedSince">last update date/time</param>
        /// <returns>transit place picture</returns>
        [WebMethod(Description = "Get place picture data if modified since.", BufferResponse = true)]
        public TransitPlacePictureWithBitmap GetPlacePictureWithBitmapByIdIfModifiedSince(string ticket, int id, DateTime ifModifiedSince)
        {
            // todo: check permissions with ticket
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedPlacePicture p = new ManagedPlacePicture(session, id);

                if (p.Modified <= ifModifiedSince)
                {
                    return null;
                }

                return p.TransitPlacePictureWithBitmap;
            }
        }

        /// <summary>
        /// Get place picture thumbnail.
        /// </summary>
        /// <param name="id">place picture id</param>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit place picture, thumbnail only</returns>
        [WebMethod(Description = "Get place picture thumbnail.", BufferResponse = true)]
        public TransitPlacePictureWithThumbnail GetPlacePictureWithThumbnailById(string ticket, int id)
        {
            // todo: check permissions with ticket
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedPlacePicture p = new ManagedPlacePicture(session, id);
                return p.TransitPlacePictureWithThumbnail;
            }
        }

        /// <summary>
        /// Get place picture thumbnail.
        /// </summary>
        /// <param name="id">place picture id</param>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="ifModifiedSince">last update date/time</param>
        /// <returns>transit place picture, thumbnail only</returns>
        [WebMethod(Description = "Get place picture thumbnail if modified since.", BufferResponse = true)]
        public TransitPlacePictureWithThumbnail GetPlacePictureWithThumbnailByIdIfModifiedSince(string ticket, int id, DateTime ifModifiedSince)
        {
            // todo: check permissions with ticket
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedPlacePicture p = new ManagedPlacePicture(session, id);

                if (p.Modified <= ifModifiedSince)
                {
                    return null;
                }

                return p.TransitPlacePictureWithThumbnail;
            }
        }

        #endregion

        #region Account Place Type

        /// <summary>
        /// Create or update a account place type.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="type">transit account place type</param>
        [WebMethod(Description = "Create or update a account place type.")]
        public int CreateOrUpdateAccountPlaceType(string ticket, TransitAccountPlaceType type)
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

                ManagedAccountPlaceType m_type = new ManagedAccountPlaceType(session);
                m_type.CreateOrUpdate(type);
                SnCore.Data.Hibernate.Session.Flush();
                return m_type.Id;
            }
        }

        /// <summary>
        /// Get a account place type.
        /// </summary>
        /// <returns>transit account place type</returns>
        [WebMethod(Description = "Get a account place type.")]
        public TransitAccountPlaceType GetAccountPlaceTypeById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitAccountPlaceType result = new ManagedAccountPlaceType(session, id).TransitAccountPlaceType;
                return result;
            }
        }

        /// <summary>
        /// Get all account place types.
        /// </summary>
        /// <returns>list of transit account place types</returns>
        [WebMethod(Description = "Get all account place types.")]
        public List<TransitAccountPlaceType> GetAccountPlaceTypes()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList types = session.CreateCriteria(typeof(AccountPlaceType)).List();
                List<TransitAccountPlaceType> result = new List<TransitAccountPlaceType>(types.Count);
                foreach (AccountPlaceType type in types)
                {
                    result.Add(new ManagedAccountPlaceType(session, type).TransitAccountPlaceType);
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete a account place type
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a account place type.")]
        public void DeleteAccountPlaceType(string ticket, int id)
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

                ManagedAccountPlaceType m_type = new ManagedAccountPlaceType(session, id);
                m_type.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
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
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);
                ManagedPlace place = (accountplace.Id == 0) ? null : new ManagedPlace(session, accountplace.Id);

                if ((accountplace.AccountId != 0) && (userid != accountplace.AccountId) && (!user.IsAdministrator()))
                {
                    if (place == null || !place.CanWrite(userid))
                    {
                        // one can only edit his own place or a place that he has rights to
                        throw new ManagedAccount.AccessDeniedException();
                    }
                }

                AccountPlaceType type = ManagedAccountPlaceType.Find(session, accountplace.Type);

                if (type.CanWrite && !user.IsAdministrator())
                {
                    if (place == null || !place.CanWrite(userid))
                    {
                        // only administrators can assign or edit r/w types
                        throw new ManagedAccount.AccessDeniedException();
                    }
                }

                if (accountplace.AccountId == 0) accountplace.AccountId = user.Id;
                ManagedAccount account = new ManagedAccount(session, accountplace.AccountId);
                int result = account.CreateOrUpdate(accountplace);

                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get a account place.
        /// </summary>
        /// <returns>transit account place </returns>
        [WebMethod(Description = "Get a account place.")]
        public TransitAccountPlace GetAccountPlaceById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitAccountPlace result = new ManagedAccountPlace(session, id).TransitAccountPlace;
                return result;
            }
        }

        /// <summary>
        /// Get account places count by place id.
        /// </summary>
        /// <returns>number of account places</returns>
        [WebMethod(Description = "Get account places count by place id.")]
        public int GetAccountPlacesCountByPlaceId(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery(string.Format(
                    "SELECT COUNT(s) FROM AccountPlace s WHERE s.Place.Id = {0}",
                    id)).UniqueResult();
            }
        }

        /// <summary>
        /// Get account places by place id.
        /// </summary>
        /// <returns>transit account places</returns>
        [WebMethod(Description = "Get account places by place id.")]
        public List<TransitAccountPlace> GetAccountPlacesByPlaceId(int id, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ICriteria c = session.CreateCriteria(typeof(AccountPlace))
                    .Add(Expression.Eq("Place.Id", id));

                if (options != null)
                {
                    c.SetMaxResults(options.PageSize);
                    c.SetFirstResult(options.FirstResult);
                }

                IList places = c.List();

                List<TransitAccountPlace> result = new List<TransitAccountPlace>(places.Count);
                foreach (AccountPlace place in places)
                {
                    result.Add(new ManagedAccountPlace(session, place).TransitAccountPlace);
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get account places count.
        /// </summary>
        [WebMethod(Description = "Get account places count.")]
        public int GetAccountPlacesCount(string ticket)
        {
            return GetAccountPlacesCountByAccountId(ManagedAccount.GetAccountId(ticket));
        }

        /// <summary>
        /// Get account places count by account id.
        /// </summary>
        [WebMethod(Description = "Get account places count by account id.")]
        public int GetAccountPlacesCountByAccountId(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery(string.Format(
                    "SELECT COUNT(s) FROM AccountPlace s WHERE s.Account.Id = {0}",
                    id)).UniqueResult();
            }
        }

        /// <summary>
        /// Get account places.
        /// </summary>
        /// <returns>transit account places</returns>
        [WebMethod(Description = "Get account places.")]
        public List<TransitAccountPlace> GetAccountPlaces(string ticket, ServiceQueryOptions options)
        {
            return GetAccountPlacesByAccountId(ManagedAccount.GetAccountId(ticket), options);
        }

        /// <summary>
        /// Get account places by account id.
        /// </summary>
        /// <returns>transit account places</returns>
        [WebMethod(Description = "Get account places by account id.")]
        public List<TransitAccountPlace> GetAccountPlacesByAccountId(int id, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ICriteria c = session.CreateCriteria(typeof(AccountPlace))
                    .Add(Expression.Eq("Account.Id", id));

                if (options != null)
                {
                    c.SetFirstResult(options.FirstResult);
                    c.SetMaxResults(options.PageSize);
                }

                IList places = c.List();

                List<TransitAccountPlace> result = new List<TransitAccountPlace>(places.Count);
                foreach (AccountPlace place in places)
                {
                    result.Add(new ManagedAccountPlace(session, place).TransitAccountPlace);
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete a account place.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a account place.")]
        public void DeleteAccountPlace(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccount user = new ManagedAccount(session, userid);
                ManagedAccountPlace m_accountplace = new ManagedAccountPlace(session, id);

                if (m_accountplace.Account.Id != userid && !user.IsAdministrator())
                {
                    ManagedPlace place = new ManagedPlace(session, m_accountplace.Place.Id);
                    if (!place.CanWrite(userid))
                    {
                        throw new ManagedAccount.AccessDeniedException();
                    }
                }

                m_accountplace.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        #endregion

        #region AccountPlace Request

        /// <summary>
        /// Create or update an account place request.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="request">transit place request</param>
        /// <returns>place id</returns>
        [WebMethod(Description = "Create or update an account place request.")]
        public int CreateOrUpdateAccountPlaceRequest(string ticket, TransitAccountPlaceRequest request)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);

                if ((request.Id != 0) && (!user.IsAdministrator()))
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                if (request.Id == 0) request.AccountId = user.Id;
                int result = user.CreateOrUpdate(request);
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get a place request.
        /// </summary>
        /// <returns>transit place </returns>
        [WebMethod(Description = "Get a place request.")]
        public TransitAccountPlaceRequest GetAccountPlaceRequestById(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);
                ManagedAccountPlaceRequest request = new ManagedAccountPlaceRequest(session, id);

                if (!user.IsAdministrator())
                {
                    ManagedPlace place = new ManagedPlace(session, request.Place.Id);
                    if (!place.CanWrite(userid))
                    {
                        throw new ManagedAccount.AccessDeniedException();
                    }
                }

                TransitAccountPlaceRequest result = request.TransitAccountPlaceRequest;
                return result;
            }
        }

        /// <summary>
        /// Get place requests.
        /// </summary>
        /// <returns>list of transit place requests</returns>
        [WebMethod(Description = "Get place requests.", CacheDuration = 60)]
        public List<TransitAccountPlaceRequest> GetAccountPlaceRequests(string ticket)
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

                IList list = session.CreateCriteria(typeof(AccountPlaceRequest))
                    .List();

                List<TransitAccountPlaceRequest> result = new List<TransitAccountPlaceRequest>(list.Count);
                foreach (AccountPlaceRequest p in list)
                {
                    result.Add(new ManagedAccountPlaceRequest(session, p).TransitAccountPlaceRequest);
                }

                return result;
            }
        }

        /// <summary>
        /// Get place requests by place id.
        /// </summary>
        /// <returns>list of transit place requests</returns>
        [WebMethod(Description = "Get place requests by place id.")]
        public List<TransitAccountPlaceRequest> GetAccountPlaceRequestsById(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedPlace place = new ManagedPlace(session, id);
                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator() && !place.CanWrite(userid))
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                IList list = session.CreateCriteria(typeof(AccountPlaceRequest))
                    .Add(Expression.Eq("Place.Id", id))
                    .List();

                List<TransitAccountPlaceRequest> result = new List<TransitAccountPlaceRequest>(list.Count);
                foreach (AccountPlaceRequest p in list)
                {
                    result.Add(new ManagedAccountPlaceRequest(session, p).TransitAccountPlaceRequest);
                }

                return result;
            }
        }


        /// <summary>
        /// Delete a place request.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a place request.")]
        public void DeleteAccountPlaceRequest(string ticket, int id)
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

                ManagedAccountPlaceRequest m_request = new ManagedAccountPlaceRequest(session, id);
                m_request.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Approve a place request.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Approve a place request.")]
        public void AcceptAccountPlaceRequest(string ticket, int id, string message)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);
                ManagedAccountPlaceRequest m_request = new ManagedAccountPlaceRequest(session, id);

                if (!user.IsAdministrator())
                {
                    ManagedPlace m_place = new ManagedPlace(session, m_request.Place.Id);
                    if (!m_place.CanWrite(userid))
                    {
                        throw new ManagedAccount.AccessDeniedException();
                    }
                }

                m_request.Accept(message);
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
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);
                ManagedAccountPlaceRequest m_request = new ManagedAccountPlaceRequest(session, id);

                if (!user.IsAdministrator())
                {
                    ManagedPlace m_place = new ManagedPlace(session, m_request.Place.Id);
                    if (!m_place.CanWrite(userid))
                    {
                        throw new ManagedAccount.AccessDeniedException();
                    }
                }

                m_request.Reject(message);
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Get new places.
        /// </summary>
        /// <returns>transit places</returns>
        [WebMethod(Description = "Get new places.", CacheDuration = 60)]
        public List<TransitPlace> GetNewPlaces(int max)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList list = session.CreateQuery(
                    "FROM Place p WHERE EXISTS ELEMENTS(p.PlacePictures) ORDER BY p.Created DESC")
                    .SetMaxResults(max)
                    .List();

                List<TransitPlace> result = new List<TransitPlace>(list.Count);
                foreach (Place p in list)
                {
                    result.Add(new ManagedPlace(session, p).TransitPlace);
                }

                return result;
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
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);

                if (apf.AccountId == 0) apf.AccountId = user.Id;
                if ((userid != apf.AccountId) && (!user.IsAdministrator()))
                {
                    // one can only edit his own place or a place that he has rights to
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedAccount account = new ManagedAccount(session, apf.AccountId);
                int result = account.CreateOrUpdate(apf);

                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Is a place your favorite?
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="place_id">place id</param>
        /// <returns>account place favorite id</returns>
        [WebMethod(Description = "Is a place your favorite?")]
        public bool IsAccountPlaceFavorite(string ticket, int place_id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                AccountPlaceFavorite apf = (AccountPlaceFavorite) session.CreateCriteria(typeof(AccountPlaceFavorite))
                    .Add(Expression.Eq("Account.Id", userid))
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
        public TransitAccountPlaceFavorite GetAccountPlaceFavoriteById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitAccountPlaceFavorite result = new ManagedAccountPlaceFavorite(session, id).TransitAccountPlaceFavorite;
                return result;
            }
        }

        /// <summary>
        /// Get account place favorites count by place id.
        /// </summary>
        /// <returns>number of account place favorites</returns>
        [WebMethod(Description = "Get account place favorites count by place id.")]
        public int GetAccountPlaceFavoritesCountByPlaceId(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery(string.Format(
                    "SELECT COUNT(s) FROM AccountPlaceFavorite s WHERE s.Place.Id = {0}",
                    id)).UniqueResult();
            }
        }

        /// <summary>
        /// Get account place favorites by place id.
        /// </summary>
        /// <returns>transit account place favorites</returns>
        [WebMethod(Description = "Get account place favorites by place id.")]
        public List<TransitAccountPlaceFavorite> GetAccountPlaceFavoritesByPlaceId(int id, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ICriteria c = session.CreateCriteria(typeof(AccountPlaceFavorite))
                    .Add(Expression.Eq("Place.Id", id));

                if (options != null)
                {
                    c.SetFirstResult(options.FirstResult);
                    c.SetMaxResults(options.PageSize);
                }

                IList places = c.List();

                List<TransitAccountPlaceFavorite> result = new List<TransitAccountPlaceFavorite>(places.Count);
                foreach (AccountPlaceFavorite place in places)
                {
                    result.Add(new ManagedAccountPlaceFavorite(session, place).TransitAccountPlaceFavorite);
                }

                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get account place favorites count.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>number of account place favorites</returns>
        [WebMethod(Description = "Get account place favorites count.")]
        public int GetAccountPlaceFavoritesCount(string ticket)
        {
            return GetAccountPlaceFavoritesCountById(ManagedAccount.GetAccountId(ticket));
        }

        /// <summary>
        /// Get account place favorites count.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>transit account place favorites count</returns>
        [WebMethod(Description = "Get account place favorites count.", CacheDuration = 60)]
        public int GetAccountPlaceFavoritesCountById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery(string.Format(
                    "SELECT COUNT(s) FROM AccountPlaceFavorite s WHERE s.Account.Id = {0}",
                    id)).UniqueResult();
            }
        }

        /// <summary>
        /// Get account place favorites.
        /// </summary>
        /// <returns>transit account place favorites</returns>
        [WebMethod(Description = "Get account place favorites.")]
        public List<TransitAccountPlaceFavorite> GetAccountPlaceFavorites(string ticket, ServiceQueryOptions options)
        {
            return GetAccountPlaceFavoritesByAccountId(ManagedAccount.GetAccountId(ticket), options);
        }

        /// <summary>
        /// Get account place favorites by account id.
        /// </summary>
        /// <returns>transit account place favorites</returns>
        [WebMethod(Description = "Get account place favorites by account id.")]
        public List<TransitAccountPlaceFavorite> GetAccountPlaceFavoritesByAccountId(int id, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ICriteria c = session.CreateCriteria(typeof(AccountPlaceFavorite))
                    .Add(Expression.Eq("Account.Id", id))
                    .AddOrder(Order.Desc("Created"));

                if (options != null)
                {
                    c.SetFirstResult(options.FirstResult);
                    c.SetMaxResults(options.PageSize);
                }

                IList places = c.List();

                List<TransitAccountPlaceFavorite> result = new List<TransitAccountPlaceFavorite>(places.Count);
                foreach (AccountPlaceFavorite place in places)
                {
                    result.Add(new ManagedAccountPlaceFavorite(session, place).TransitAccountPlaceFavorite);
                }

                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete an account place favorite.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete an account place favorite.")]
        public void DeleteAccountPlaceFavorite(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccount user = new ManagedAccount(session, userid);
                ManagedAccountPlaceFavorite m_AccountPlaceFavorite = new ManagedAccountPlaceFavorite(session, id);

                if (m_AccountPlaceFavorite.Account.Id != userid && !user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                m_AccountPlaceFavorite.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        #endregion

        #region Search

        protected IList InternalSearchPlaces(ISession session, string s, ServiceQueryOptions options)
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

                    "SELECT {Place.*} FROM {Place}, #Unique_Results\n" +
                    "WHERE Place.Place_Id = #Unique_Results.Place_Id\n" +
                    "ORDER BY #Unique_Results.RANK DESC\n" +

                    "DROP TABLE #Results\n" +
                    "DROP TABLE #Unique_Results\n",

                    "Place",
                    typeof(Place));

            if (options != null)
            {
                query.SetFirstResult(options.FirstResult);
                query.SetMaxResults(options.PageSize);
            }

            return query.List();
        }

        /// <summary>
        /// Search places.
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "Search places.", CacheDuration = 60)]
        public List<TransitPlace> SearchPlaces(string s, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList places = InternalSearchPlaces(session, s, options);

                List<TransitPlace> result = new List<TransitPlace>(places.Count);
                foreach (Place p in places)
                {
                    result.Add(new ManagedPlace(session, p).TransitPlace);
                }

                return result;
            }
        }

        /// <summary>
        /// Return the number of places matching a query.
        /// </summary>
        /// <returns>number of places</returns>
        [WebMethod(Description = "Return the number of places matching a query.", CacheDuration = 60)]
        public int SearchPlacesCount(string s)
        {
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
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);

                if ((placename.Id != 0) && (!user.IsAdministrator()))
                {
                    ManagedPlace place = new ManagedPlace(session, placename.PlaceId);
                    if (!place.CanWrite(userid))
                    {
                        throw new ManagedAccount.AccessDeniedException();
                    }
                }

                ManagedPlaceName m_placename = new ManagedPlaceName(session);
                m_placename.CreateOrUpdate(placename);
                SnCore.Data.Hibernate.Session.Flush();
                return m_placename.Id;
            }
        }

        /// <summary>
        /// Get a place name.
        /// </summary>
        /// <returns>transit place name</returns>
        [WebMethod(Description = "Get a place name.")]
        public TransitPlaceName GetPlaceNameById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitPlaceName result = new ManagedPlaceName(session, id).TransitPlaceName;
                return result;
            }
        }


        /// <summary>
        /// Get all place names.
        /// </summary>
        /// <param name="placeid">place id</param>
        /// <returns>list of transit place names</returns>
        [WebMethod(Description = "Get all place names.")]
        public List<TransitPlaceName> GetPlaceNames(int placeid)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList placenames = session.CreateCriteria(typeof(PlaceName))
                    .Add(Expression.Eq("Place.Id", placeid))
                    .List();
                List<TransitPlaceName> result = new List<TransitPlaceName>(placenames.Count);
                foreach (PlaceName placename in placenames)
                {
                    result.Add(new ManagedPlaceName(session, placename).TransitPlaceName);
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete a place name
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a place name.")]
        public void DeletePlaceName(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccount user = new ManagedAccount(session, userid);
                ManagedPlaceName m_placename = new ManagedPlaceName(session, id);

                if (!user.IsAdministrator())
                {
                    ManagedPlace place = new ManagedPlace(session, m_placename.Place.Id);
                    if (!place.CanWrite(userid))
                    {
                        throw new ManagedAccount.AccessDeniedException();
                    }
                }

                m_placename.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        #endregion

        #region Place Property Group

        /// <summary>
        /// Create or update a property group.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="PropertyGroup">transit property group</param>
        [WebMethod(Description = "Create or update a property group.")]
        public int CreateOrUpdatePlacePropertyGroup(string ticket, TransitPlacePropertyGroup pg)
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

                ManagedPlacePropertyGroup m_propertygroup = new ManagedPlacePropertyGroup(session);
                m_propertygroup.CreateOrUpdate(pg);
                SnCore.Data.Hibernate.Session.Flush();
                return m_propertygroup.Id;
            }
        }

        /// <summary>
        /// Get a property group.
        /// </summary>
        /// <returns>transit property group</returns>
        [WebMethod(Description = "Get a property group.")]
        public TransitPlacePropertyGroup GetPlacePropertyGroupById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitPlacePropertyGroup result = new ManagedPlacePropertyGroup(session, id).TransitPlacePropertyGroup;
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }


        /// <summary>
        /// Get all property groups.
        /// </summary>
        /// <returns>list of transit property groups</returns>
        [WebMethod(Description = "Get all property groups.")]
        public List<TransitPlacePropertyGroup> GetPlacePropertyGroups()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList pgs = session.CreateCriteria(typeof(PlacePropertyGroup)).List();
                List<TransitPlacePropertyGroup> result = new List<TransitPlacePropertyGroup>(pgs.Count);
                foreach (PlacePropertyGroup pg in pgs)
                {
                    result.Add(new ManagedPlacePropertyGroup(session, pg).TransitPlacePropertyGroup);
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete a property group
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a property group.")]
        public void DeletePlacePropertyGroup(string ticket, int id)
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

                ManagedPlacePropertyGroup m_propertygroup = new ManagedPlacePropertyGroup(session, id);
                m_propertygroup.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        #endregion

        #region Place Property

        /// <summary>
        /// Create or update a property.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="property">transit property</param>
        [WebMethod(Description = "Create or update a property.")]
        public int CreateOrUpdatePlaceProperty(string ticket, TransitPlaceProperty p)
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

                ManagedPlaceProperty m_property = new ManagedPlaceProperty(session);
                m_property.CreateOrUpdate(p);
                SnCore.Data.Hibernate.Session.Flush();
                return m_property.Id;
            }
        }

        /// <summary>
        /// Get a property.
        /// </summary>
        /// <returns>transit property</returns>
        [WebMethod(Description = "Get a property.")]
        public TransitPlaceProperty GetPlacePropertyById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitPlaceProperty result = new ManagedPlaceProperty(session, id).TransitPlaceProperty;
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }


        /// <summary>
        /// Get all properties.
        /// </summary>
        /// <returns>list of transit properties</returns>
        [WebMethod(Description = "Get all properties.")]
        public List<TransitPlaceProperty> GetPlaceProperties(int gid)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList ps = session.CreateCriteria(typeof(PlaceProperty))
                    .Add(Expression.Eq("PlacePropertyGroup.Id", gid))
                    .List();

                List<TransitPlaceProperty> result = new List<TransitPlaceProperty>(ps.Count);
                foreach (PlaceProperty p in ps)
                {
                    result.Add(new ManagedPlaceProperty(session, p).TransitPlaceProperty);
                }

                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete a property
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a property.")]
        public void DeletePlaceProperty(string ticket, int id)
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

                ManagedPlaceProperty m_property = new ManagedPlaceProperty(session, id);
                m_property.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        #endregion

        #region Place Property Value

        /// <summary>
        /// Get distinct place property values.
        /// </summary>
        /// <returns>list of possible values</returns>
        [WebMethod(Description = "Get distinct place property values.")]
        public List<TransitDistinctPlacePropertyValue> GetDistinctPropertyValues(string groupname, string propertyname)
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

                IList list = query.List();

                SortedDictionary<string, TransitDistinctPlacePropertyValue> dict = new SortedDictionary<string, TransitDistinctPlacePropertyValue>();
                
                foreach (PlacePropertyValue pv in list)
                {
                    StringCollection values = new StringCollection();

                    switch(pv.PlaceProperty.TypeName)
                    {
                        case "System.Array":
                            values.AddRange(pv.Value.Split("\"".ToCharArray()));
                            break;
                        default:
                            values.Add(pv.Value);
                            break;
                    }

                    foreach(string s in values)
                    {
                        if (s.Length > 0)
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
        public List<TransitPlace> GetPlacesByPropertyValue(
            string groupname, string propertyname, string propertyvalue, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                IQuery query = session.CreateSQLQuery(
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
                   "place",
                   typeof(Place));

                if (options != null)
                {
                    query.SetFirstResult(options.FirstResult);
                    query.SetMaxResults(options.PageSize);
                }

                IList list = query.List();

                List<TransitPlace> result = new List<TransitPlace>(list.Count);

                foreach (Place place in list)
                {
                    result.Add(new ManagedPlace(session, place).TransitPlace);
                }

                return result;
            }
        }

        /// <summary>
        /// Get the number of places that match a property value by name.
        /// </summary>
        /// <returns>transit places</returns>
        [WebMethod(Description = "Get the number of places that match a property value by name.")]
        public int GetPlacesByPropertyValueCount(
            string groupname, string propertyname, string propertyvalue)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                IQuery query = session.CreateQuery(
                   "SELECT COUNT(place) FROM PlaceProperty p, PlacePropertyGroup g, PlacePropertyValue v, Place place" +
                   " WHERE place.Id = v.Place.Id" +
                   " AND v.Place.Id = place.Id" +
                   " AND v.PlaceProperty.Id = p.Id" +
                   " AND p.PlacePropertyGroup.Id = g.Id" +
                   " AND p.Name = '" + Renderer.SqlEncode(propertyname) + "'" +
                   " AND (" +
                   "  v.Value LIKE '" + Renderer.SqlEncode(propertyvalue) + "'" +
                   "  OR v.Value LIKE '%\"" + Renderer.SqlEncode(propertyvalue) + "\"%'" +
                   " ) AND g.Name = '" + Renderer.SqlEncode(groupname) + "'");

                return (int) query.UniqueResult();
            }
        }

        /// <summary>
        /// Get a place property value by name.
        /// </summary>
        /// <returns>transit place property value</returns>
        [WebMethod(Description = "Get a place property value by group and name.")]
        public TransitPlacePropertyValue GetPlacePropertyValueByName(int placeid, string groupname, string propertyname)
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

                TransitPlacePropertyValue result = new ManagedPlacePropertyValue(session, ppv).TransitPlacePropertyValue;
                SnCore.Data.Hibernate.Session.Flush();
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
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);
                ManagedPlace m_place = new ManagedPlace(session, propertyvalue.PlaceId);

                if (!m_place.CanWrite(userid) && !user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                int result = m_place.CreateOrUpdate(propertyvalue);

                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get a place property value.
        /// </summary>
        /// <returns>transit place property value</returns>
        [WebMethod(Description = "Get a place property value.")]
        public TransitPlacePropertyValue GetPlacePropertyValueById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitPlacePropertyValue result = new ManagedPlacePropertyValue(session, id).TransitPlacePropertyValue;
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get place property values.
        /// </summary>
        /// <returns>list of place property values</returns>
        [WebMethod(Description = "Get place property values.", CacheDuration = 60)]
        public List<TransitPlacePropertyValue> GetPlacePropertyValuesById(int placeid, int groupid)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList propertyvalues = session.CreateCriteria(typeof(PlacePropertyValue))
                    .Add(Expression.Eq("Place.Id", placeid))
                    // .Add(Expression.Eq("PlaceProperty.PlacePropertyGroup.Id", groupid))
                    // .Add(Expression.Eq("PlaceProperty.Publish", true))
                    .List();

                List<TransitPlacePropertyValue> result = new List<TransitPlacePropertyValue>(propertyvalues.Count);
                foreach (PlacePropertyValue propertyvalue in propertyvalues)
                {
                    if ((propertyvalue.PlaceProperty.PlacePropertyGroup.Id == groupid)
                        && propertyvalue.PlaceProperty.Publish)
                    {
                        result.Add(new ManagedPlacePropertyValue(session, propertyvalue).TransitPlacePropertyValue);
                    }
                }

                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get all place property values, including unfilled ones.
        /// </summary>
        /// <returns>list of place property values</returns>
        [WebMethod(Description = "Get all place property values, including unfilled ones.", CacheDuration = 60)]
        public List<TransitPlacePropertyValue> GetAllPlacePropertyValuesById(int placeid, int groupid)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

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
                        value.Place = (Place)session.Load(typeof(Place), placeid);
                    }

                    result.Add(new TransitPlacePropertyValue(value));
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
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);
                ManagedPlacePropertyValue m_propertyvalue = new ManagedPlacePropertyValue(session, id);

                ManagedPlace m_place = new ManagedPlace(session, m_propertyvalue.PlaceId);

                if (!m_place.CanWrite(userid) && !user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                m_propertyvalue.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
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
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedPlace place = new ManagedPlace(session, attribute.PlaceId);

                PlaceAttribute a = attribute.GetPlaceAttribute(session);
                if (a.Id == 0) a.Created = DateTime.UtcNow;
                session.Save(a);

                SnCore.Data.Hibernate.Session.Flush();
                return a.Id;
            }
        }

        /// <summary>
        /// Get place attributes.
        /// </summary>
        /// <returns>transit place attribute</returns>
        [WebMethod(Description = "Get place attributes.")]
        public TransitPlaceAttribute GetPlaceAttributeById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitPlaceAttribute result = new ManagedPlaceAttribute(session, id).TransitPlaceAttribute;
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }


        /// <summary>
        /// Get place attributes count.
        /// </summary>
        /// <returns>number of place attributes</returns>
        [WebMethod(Description = "Get place attributes count.", CacheDuration = 60)]
        public int GetPlaceAttributesCountById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery(string.Format(
                    "SELECT COUNT(a) FROM PlaceAttribute a WHERE a.Place.Id = {0}",
                    id)).UniqueResult();
            }
        }

        /// <summary>
        /// Get place attributes.
        /// </summary>
        /// <returns>list of place attributes</returns>
        [WebMethod(Description = "Get place attributes.", CacheDuration = 60)]
        public List<TransitPlaceAttribute> GetPlaceAttributesById(int placeid, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ICriteria c = session.CreateCriteria(typeof(PlaceAttribute))
                    .Add(Expression.Eq("Place.Id", placeid));

                if (options != null)
                {
                    c.SetFirstResult(options.FirstResult);
                    c.SetMaxResults(options.PageSize);
                }

                IList attributes = c.List();

                List<TransitPlaceAttribute> result = new List<TransitPlaceAttribute>(attributes.Count);
                foreach (PlaceAttribute attribute in attributes)
                {
                    result.Add(new ManagedPlaceAttribute(session, attribute).TransitPlaceAttribute);
                }

                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete a place attribute.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a place attribute.")]
        public void DeletePlaceAttribute(string ticket, int id)
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

                ManagedPlaceAttribute m_attribute = new ManagedPlaceAttribute(session, id);
                m_attribute.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }
        #endregion
    }
}       