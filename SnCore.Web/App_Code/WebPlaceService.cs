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

                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedPlace m_place = new ManagedPlace(session, id);
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

                    string url = string.Format(
                        "{0}/PlacePictureView.aspx?id={1}",
                        ManagedConfiguration.GetValue(session, "SnCore.WebSite.Url", "http://localhost/SnCore"),
                        m_placepicture.Id);

                    string messagebody =
                        "<html>" +
                        "<style>body { font-size: .80em; font-family: Verdana; }</style>" +
                        "<body>" +
                        "Dear " + Renderer.Render(acct.Name) + ",<br>" +
                        "<br>A new picture has been uploaded to <b>" + Renderer.Render(place.Name) + "</b> that you have suggested." +
                        "<br><br>" +
                        "<blockquote>" +
                        "<a href=\"" + url + "\">View</a> this picture." +
                        "</blockquote>" +
                        "</body>" +
                        "</html>";

                    acct.SendAccountMailMessage(
                        ManagedConfiguration.GetValue(session, "SnCore.Admin.EmailAddress", "admin@localhost.com"),
                        acct.ActiveEmailAddress,
                        string.Format("{0}: a new picture has been added to {1}.",
                            ManagedConfiguration.GetValue(session, "SnCore.Name", "SnCore"),
                            place.Name),
                        messagebody,
                        true);
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
        /// Get all place pictures.
        /// </summary>
        /// <param name="placeid">place id</param>
        /// <returns>list of transit place pictures</returns>
        [WebMethod(Description = "Get all place pictures.")]
        public List<TransitPlacePicture> GetPlacePictures(int placeid)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList placepictures = session.CreateCriteria(typeof(PlacePicture))
                    .Add(Expression.Eq("Place.Id", placeid))
                    .List();
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
        /// Get account places by place id.
        /// </summary>
        /// <returns>transit account places</returns>
        [WebMethod(Description = "Get account place by place id.")]
        public List<TransitAccountPlace> GetAccountPlacesByPlaceId(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList places = session.CreateCriteria(typeof(AccountPlace))
                    .Add(Expression.Eq("Place.Id", id))
                    .List();

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
        /// Get account places by account id.
        /// </summary>
        /// <returns>transit account places</returns>
        [WebMethod(Description = "Get account place by account id.")]
        public List<TransitAccountPlace> GetAccountPlacesByAccountId(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList places = session.CreateCriteria(typeof(AccountPlace))
                    .Add(Expression.Eq("Account.Id", id))
                    .List();

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
        /// Get account place favorites by place id.
        /// </summary>
        /// <returns>transit account place favorites</returns>
        [WebMethod(Description = "Get account place favorites by place id.")]
        public List<TransitAccountPlaceFavorite> GetAccountPlaceFavoritesByPlaceId(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList places = session.CreateCriteria(typeof(AccountPlaceFavorite))
                    .Add(Expression.Eq("Place.Id", id))
                    .List();

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
        /// Get account place favorites by account id.
        /// </summary>
        /// <returns>transit account place favorites</returns>
        [WebMethod(Description = "Get account place favorites by account id.")]
        public List<TransitAccountPlaceFavorite> GetAccountPlaceFavoritesByAccountId(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList places = session.CreateCriteria(typeof(AccountPlaceFavorite))
                    .Add(Expression.Eq("Account.Id", id))
                    .List();

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

                IQuery query = session.CreateSQLQuery(
                        "SELECT {Place.*} FROM Place {Place} WHERE Place.Place_Id IN (" +
                         "SELECT p.Place_Id FROM Place p WHERE FREETEXT ((Name, Street, Zip, CrossStreet, Description, Phone, Fax, Email, Website), '" + Renderer.SqlEncode(s) + "')" +
                         " UNION " + 
                         "SELECT p.Place_Id FROM Place p, PlaceName pn" +
                         " WHERE p.Place_Id = pn.Place_Id" +
                         " AND FREETEXT (pn.Name, '" + Renderer.SqlEncode(s) + "')" +
                        ")",                        
                        "Place",
                        typeof(Place));

                if (options != null)
                {
                    query.SetFirstResult(options.FirstResult);
                    query.SetMaxResults(options.PageSize);
                }

                IList places = query.List();

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

                IQuery query = session.CreateSQLQuery(
                        "SELECT {Place.*} FROM Place {Place} WHERE Place.Place_Id IN (" +
                         "SELECT p.Place_Id FROM Place p WHERE FREETEXT ((Name, Street, Zip, CrossStreet, Description, Phone, Fax, Email, Website), '" + Renderer.SqlEncode(s) + "')" +
                         " UNION " +
                         "SELECT p.Place_Id FROM Place p, PlaceName pn" +
                         " WHERE p.Place_Id = pn.Place_Id" +
                         " AND FREETEXT (pn.Name, '" + Renderer.SqlEncode(s) + "')" +
                        ")",
                        "Place",
                        typeof(Place));

                return query.List().Count;
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
    }
}