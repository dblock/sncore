using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using SnCore.Services;
using NHibernate;
using NHibernate.Expression;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.Services.Protocols;
using SnCore.Tools.Web;
using SnCore.Tools;
using System.Net.Mail;
using System.Text;
using System.Reflection;
using SnCore.Data.Hibernate;

namespace SnCore.WebServices
{
    /// <summary>
    /// Managed web business objects services.
    /// </summary>
    [WebService(Namespace = "http://www.vestris.com/sncore/ns/", Name = "WebObjectService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WebObjectService : WebService
    {
        public WebObjectService()
        {

        }

        #region Schedule

        /// <summary>
        /// Create or update a schedule.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="schedule">transit  schedule</param>
        [WebMethod(Description = "Create or update a schedule.")]
        public int CreateOrUpdateSchedule(string ticket, TransitSchedule schedule)
        {
            return WebServiceImpl<TransitSchedule, ManagedSchedule, Schedule>.CreateOrUpdate(
                ticket, schedule);
        }

        /// <summary>
        /// Get a schedule.
        /// </summary>
        /// <returns>transit  schedule</returns>
        [WebMethod(Description = "Get a schedule.")]
        public TransitSchedule GetScheduleById(string ticket, int id)
        {
            return WebServiceImpl<TransitSchedule, ManagedSchedule, Schedule>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all schedules.
        /// </summary>
        /// <returns>list of transit  schedules</returns>
        [WebMethod(Description = "Get all schedules.")]
        public List<TransitSchedule> GetSchedules(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitSchedule, ManagedSchedule, Schedule>.GetList(
                ticket, options);
        }

        /// <summary>
        /// Get all schedules count.
        /// </summary>
        /// <returns>list of transit  schedules</returns>
        [WebMethod(Description = "Get all schedules.")]
        public int GetSchedulesCount(string ticket)
        {
            return WebServiceImpl<TransitSchedule, ManagedSchedule, Schedule>.GetCount(
                ticket);
        }

        /// <summary>
        /// Delete a schedule
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        [WebMethod(Description = "Delete a schedule.")]
        public void DeleteSchedule(string ticket, int id)
        {
            WebServiceImpl<TransitSchedule, ManagedSchedule, Schedule>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Get a humanly readable representation of a schedule.
        /// </summary>
        [WebMethod(Description = "Get a humanly readable representation of a schedule.")]
        public string GetScheduleString(string ticket, TransitSchedule schedule, long offsetTicks)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedSchedule m_schedule = new ManagedSchedule(session, schedule.GetInstance(session, sec));
                return m_schedule.ToString(new TimeSpan(offsetTicks));
            }
        }

        #endregion

        #region Attribute

        /// <summary>
        /// Create or update an attribute.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="PropertyGroup">transit attribute</param>
        [WebMethod(Description = "Create or update an attribute.")]
        public int CreateOrUpdateAttribute(string ticket, TransitAttribute attr)
        {
            return WebServiceImpl<TransitAttribute, ManagedAttribute, Attribute>.CreateOrUpdate(
                ticket, attr);
        }

        /// <summary>
        /// Get an attribute.
        /// </summary>
        /// <returns>transit attribute</returns>
        [WebMethod(Description = "Get an attribute.")]
        public TransitAttribute GetAttributeById(string ticket, int id)
        {
            return WebServiceImpl<TransitAttribute, ManagedAttribute, Attribute>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get attribute data if modified since.
        /// </summary>
        /// <param name="id">attribute id</param>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="ifModifiedSince">last update date/time</param>
        /// <returns>transit attribute with bitmap</returns>
        [WebMethod(Description = "Get attribute data if modified since.", BufferResponse = true)]
        public TransitAttribute GetAttributeIfModifiedSinceById(string ticket, int id, DateTime ifModifiedSince)
        {
            TransitAttribute t_instance = WebServiceImpl<TransitAttribute, ManagedAttribute, Attribute>.GetById(
                ticket, id);

            if (t_instance.Modified <= ifModifiedSince)
                return null;

            return t_instance;
        }

        /// <summary>
        /// Get all attributes count.
        /// </summary>
        /// <returns>list of transit attributes</returns>
        [WebMethod(Description = "Get all attributes count.")]
        public int GetAttributesCount(string ticket)
        {
            return WebServiceImpl<TransitAttribute, ManagedAttribute, Attribute>.GetCount(
                ticket);
        }

        /// <summary>
        /// Get all attributes.
        /// </summary>
        /// <returns>list of transit attributes</returns>
        [WebMethod(Description = "Get all attributes.")]
        public List<TransitAttribute> GetAttributes(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitAttribute, ManagedAttribute, Attribute>.GetList(
                ticket, options);
        }

        /// <summary>
        /// Delete an attribute
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        [WebMethod(Description = "Delete an attribute.")]
        public void DeleteAttribute(string ticket, int id)
        {
            WebServiceImpl<TransitAttribute, ManagedAttribute, Attribute>.Delete(
                ticket, id);
        }

        #endregion

        #region SurveyQuestion

        /// <summary>
        /// Get survey questions.
        /// </summary>
        /// <param name="surveyid">survey id</param>
        /// <returns>transit survey questions</returns>
        [WebMethod(Description = "Get survey questions.")]
        public List<TransitSurveyQuestion> GetSurveyQuestions(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Survey.Id", id) };
            return WebServiceImpl<TransitSurveyQuestion, ManagedSurveyQuestion, SurveyQuestion>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Get survey questions count.
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [WebMethod(Description = "Get survey questions count.")]
        public int GetSurveyQuestionsCount(string ticket, int id)
        {
            ICriterion[] expressions = { Expression.Eq("Survey.Id", id) };
            return WebServiceImpl<TransitSurveyQuestion, ManagedSurveyQuestion, SurveyQuestion>.GetCount(
                ticket, expressions);
        }

        /// <summary>
        /// Get survey question by id.
        /// </summary>
        /// <param name="id">survey question id</param>
        /// <returns>transit survey question</returns>
        [WebMethod(Description = "Get survey question by id.")]
        public TransitSurveyQuestion GetSurveyQuestionById(string ticket, int id)
        {
            return WebServiceImpl<TransitSurveyQuestion, ManagedSurveyQuestion, SurveyQuestion>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Create or update a survey question.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="surveyquestion">new survey question</param>
        [WebMethod(Description = "Create or update a survey question.")]
        public int CreateOrUpdateSurveyQuestion(string ticket, TransitSurveyQuestion surveyquestion)
        {
            return WebServiceImpl<TransitSurveyQuestion, ManagedSurveyQuestion, SurveyQuestion>.CreateOrUpdate(
                ticket, surveyquestion);
        }

        /// <summary>
        /// Delete a survey question.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="surveyquestionid">survey question id</param>
        [WebMethod(Description = "Delete a survey question.")]
        public void DeleteSurveyQuestion(string ticket, int id)
        {
            WebServiceImpl<TransitSurveyQuestion, ManagedSurveyQuestion, SurveyQuestion>.Delete(
                ticket, id);
        }

        #endregion

        #region Survey

        /// <summary>
        /// Create or update a survey.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="c">transit survey information</param>
        /// <returns>new survey id</returns>
        [WebMethod(Description = "Add a survey.")]
        public int CreateOrUpdateSurvey(string ticket, TransitSurvey survey)
        {
            return WebServiceImpl<TransitSurvey, ManagedSurvey, Survey>.CreateOrUpdate(
                ticket, survey);
        }

        /// <summary>
        /// Delete a survey.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">survey id</param>
        [WebMethod(Description = "Delete a survey.")]
        public void DeleteSurvey(string ticket, int id)
        {
            WebServiceImpl<TransitSurvey, ManagedSurvey, Survey>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Get surveys.
        /// </summary>
        /// <returns>transit surveys</returns>
        [WebMethod(Description = "Get surveys.", CacheDuration = 60)]
        public List<TransitSurvey> GetSurveys(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitSurvey, ManagedSurvey, Survey>.GetList(
                ticket, options);
        }

        /// <summary>
        /// Get surveys count.
        /// </summary>
        /// <returns>number of transit surveys</returns>
        [WebMethod(Description = "Get surveys count.", CacheDuration = 60)]
        public int GetSurveysCount(string ticket)
        {
            return WebServiceImpl<TransitSurvey, ManagedSurvey, Survey>.GetCount(
                ticket);
        }

        /// <summary>
        /// Get a survey by id.
        /// </summary>
        /// <param name="id">survey id</param>
        /// <returns>transit survey</returns>
        [WebMethod(Description = "Get a survey by id.")]
        public TransitSurvey GetSurveyById(string ticket, int id)
        {
            return WebServiceImpl<TransitSurvey, ManagedSurvey, Survey>.GetById(
                ticket, id);
        }

        #endregion

        #region Picture

        /// <summary>
        /// Create or update a picture.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="picture">transit picture</param>
        [WebMethod(Description = "Create or update a picture.")]
        public int CreateOrUpdatePicture(string ticket, TransitPicture picture)
        {
            return WebServiceImpl<TransitPicture, ManagedPicture, Picture>.CreateOrUpdate(
                ticket, picture);
        }

        /// <summary>
        /// Get a picture.
        /// </summary>
        /// <returns>transit picture</returns>
        [WebMethod(Description = "Get a picture.", BufferResponse = true)]
        public TransitPicture GetPictureById(string ticket, int id)
        {
            return WebServiceImpl<TransitPicture, ManagedPicture, Picture>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get a picture if modified since.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">picture id</param>
        /// <param name="ifModifiedSince">timestamp</param>
        [WebMethod(Description = "Get picture data if modified since.", BufferResponse = true)]
        public TransitPicture GetPictureIfModifiedSinceById(string ticket, int id, DateTime ifModifiedSince)
        {
            TransitPicture t_instance = WebServiceImpl<TransitPicture, ManagedPicture, Picture>.GetById(
                ticket, id);

            if (t_instance.Modified <= ifModifiedSince)
                return null;

            return t_instance;
        }

        /// <summary>
        /// Get random picture.
        /// </summary>
        /// <param name="type">picture type</param>
        /// <returns>transit random picture, thumbnail only</returns>
        [WebMethod(Description = "Get random picture by type.", BufferResponse = true)]
        public TransitPicture GetRandomPictureByType(string ticket, string type)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                PictureType picturetype = ManagedPictureType.Find(session, type);

                if (picturetype == null)
                {
                    return null;
                }

                IList<Picture> list = session.CreateCriteria(typeof(Picture))
                    .Add(Expression.Eq("Type.Id", picturetype.Id))
                    .List<Picture>();

                if (list.Count == 0)
                {
                    return null;
                }

                ManagedPicture m_instance = new ManagedPicture();
                m_instance.SetInstance(session, list[new Random().Next() % list.Count]);
                return m_instance.GetTransitInstance(sec);
            }
        }

        /// <summary>
        /// Get all pictures.
        /// </summary>
        /// <returns>list of transit pictures</returns>
        [WebMethod(Description = "Get all pictures.", BufferResponse = true)]
        public List<TransitPicture> GetPictures(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitPicture, ManagedPicture, Picture>.GetList(
                ticket, options);
        }

        /// <summary>
        /// Get all pictures count.
        /// </summary>
        /// <returns>number of transit pictures</returns>
        [WebMethod(Description = "Get all pictures count.")]
        public int GetPicturesCount(string ticket)
        {
            return WebServiceImpl<TransitPicture, ManagedPicture, Picture>.GetCount(
                ticket);
        }

        /// <summary>
        /// Delete a picture
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        [WebMethod(Description = "Delete a picture.")]
        public void DeletePicture(string ticket, int id)
        {
            WebServiceImpl<TransitPicture, ManagedPicture, Picture>.Delete(
                ticket, id);
        }

        #endregion

        #region PictureType

        /// <summary>
        /// Create or update a picture type.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="picturetype">transit picture type</param>
        [WebMethod(Description = "Create or update a picture type.")]
        public int CreateOrUpdatePictureType(string ticket, TransitPictureType picturetype)
        {
            return WebServiceImpl<TransitPictureType, ManagedPictureType, PictureType>.CreateOrUpdate(
                ticket, picturetype);
        }

        /// <summary>
        /// Get a picture type.
        /// </summary>
        /// <returns>transit picture type</returns>
        [WebMethod(Description = "Get a picture type.")]
        public TransitPictureType GetPictureTypeById(string ticket, int id)
        {
            return WebServiceImpl<TransitPictureType, ManagedPictureType, PictureType>.GetById(
                ticket, id);
        }


        /// <summary>
        /// Get all picture types.
        /// </summary>
        /// <returns>list of transit picture types</returns>
        [WebMethod(Description = "Get all picture types.")]
        public List<TransitPictureType> GetPictureTypes(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitPictureType, ManagedPictureType, PictureType>.GetList(
                ticket, options);
        }

        /// <summary>
        /// Get all picture types count.
        /// </summary>
        /// <returns>number of transit picture types</returns>
        [WebMethod(Description = "Get all picture types count.")]
        public int GetPictureTypesCount(string ticket)
        {
            return WebServiceImpl<TransitPictureType, ManagedPictureType, PictureType>.GetCount(
                ticket);
        }

        /// <summary>
        /// Delete a picture type
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        [WebMethod(Description = "Delete a picture type.")]
        public void DeletePictureType(string ticket, int id)
        {
            WebServiceImpl<TransitPictureType, ManagedPictureType, PictureType>.Delete(
                ticket, id);
        }

        #endregion

        #region Reminder

        /// <summary>
        /// Create or update a reminder.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="reminder">transit reminder</param>
        [WebMethod(Description = "Create or update a reminder.")]
        public int CreateOrUpdateReminder(string ticket, TransitReminder reminder)
        {
            return WebServiceImpl<TransitReminder, ManagedReminder, Reminder>.CreateOrUpdate(
                ticket, reminder);
        }

        /// <summary>
        /// Get a reminder.
        /// </summary>
        /// <returns>transit reminder</returns>
        [WebMethod(Description = "Get a reminder.")]
        public TransitReminder GetReminderById(string ticket, int id)
        {
            return WebServiceImpl<TransitReminder, ManagedReminder, Reminder>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all  reminders.
        /// </summary>
        /// <returns>list of transit reminders</returns>
        [WebMethod(Description = "Get all  reminders.")]
        public List<TransitReminder> GetReminders(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitReminder, ManagedReminder, Reminder>.GetList(
                ticket, options);
        }

        /// <summary>
        /// Get all  reminders count.
        /// </summary>
        /// <returns>number of transit reminders</returns>
        [WebMethod(Description = "Get all  reminders.")]
        public int GetRemindersCount(string ticket)
        {
            return WebServiceImpl<TransitReminder, ManagedReminder, Reminder>.GetCount(
                ticket);
        }

        /// <summary>
        /// Delete a reminder
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        [WebMethod(Description = "Delete a reminder.")]
        public void DeleteReminder(string ticket, int id)
        {
            WebServiceImpl<TransitReminder, ManagedReminder, Reminder>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Test whether a reminder is being sent to an account.
        /// </summary>
        [WebMethod(Description = "Test whether a reminder is being sent to an account.")]
        public bool CanSendReminder(int reminder_id, int account_id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedReminder mr = new ManagedReminder(session, reminder_id);
                return mr.CanSend(account_id);
            }
        }

        #endregion

        #region ReminderAccountProperty

        /// <summary>
        /// Create or update a reminder account property.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="reminderaccountproperty">transit reminder account property</param>
        [WebMethod(Description = "Create or update a reminder account property.")]
        public int CreateOrUpdateReminderAccountProperty(string ticket, TransitReminderAccountProperty reminderaccountproperty)
        {
            return WebServiceImpl<TransitReminderAccountProperty, ManagedReminderAccountProperty, ReminderAccountProperty>.CreateOrUpdate(
                ticket, reminderaccountproperty);
        }

        /// <summary>
        /// Get a reminder account property.
        /// </summary>
        /// <returns>transit reminder account property</returns>
        [WebMethod(Description = "Get a reminderaccountproperty.")]
        public TransitReminderAccountProperty GetReminderAccountPropertyById(string ticket, int id)
        {
            return WebServiceImpl<TransitReminderAccountProperty, ManagedReminderAccountProperty, ReminderAccountProperty>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all reminder account properties count.
        /// </summary>
        [WebMethod(Description = "Get all reminder account properties count.")]
        public int GetReminderAccountPropertiesCount(string ticket, int id)
        {
            ICriterion[] expressions = { Expression.Eq("Reminder.Id", id) };
            return WebServiceImpl<TransitReminderAccountProperty, ManagedReminderAccountProperty, ReminderAccountProperty>.GetCount(
                ticket, expressions);
        }

        /// <summary>
        /// Get all reminder account properties.
        /// </summary>
        /// <returns>list of transit reminder account properties</returns>
        [WebMethod(Description = "Get all reminder account properties.")]
        public List<TransitReminderAccountProperty> GetReminderAccountProperties(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Reminder.Id", id) };
            return WebServiceImpl<TransitReminderAccountProperty, ManagedReminderAccountProperty, ReminderAccountProperty>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Delete a reminder account property
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        [WebMethod(Description = "Delete a reminder account property.")]
        public void DeleteReminderAccountProperty(string ticket, int id)
        {
            WebServiceImpl<TransitReminderAccountProperty, ManagedReminderAccountProperty, ReminderAccountProperty>.Delete(
                ticket, id);
        }

        #endregion

        #region ReminderEvent

        /// <summary>
        /// Create or update a reminder event.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="reminderevent">transit reminder event</param>
        [WebMethod(Description = "Create or update a reminder event.")]
        public int CreateOrUpdateReminderEvent(string ticket, TransitReminderEvent reminderevent)
        {
            return WebServiceImpl<TransitReminderEvent, ManagedReminderEvent, ReminderEvent>.CreateOrUpdate(
                ticket, reminderevent);
        }

        /// <summary>
        /// Get a reminder event.
        /// </summary>
        /// <returns>transit reminder event</returns>
        [WebMethod(Description = "Get a reminder event.")]
        public TransitReminderEvent GetReminderEventById(string ticket, int id)
        {
            return WebServiceImpl<TransitReminderEvent, ManagedReminderEvent, ReminderEvent>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all reminder events.
        /// </summary>
        /// <returns>list of transit reminder events</returns>
        [WebMethod(Description = "Get all reminder events.")]
        public List<TransitReminderEvent> GetReminderEvents(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitReminderEvent, ManagedReminderEvent, ReminderEvent>.GetList(
                ticket, options);
        }

        /// <summary>
        /// Get all reminder events count.
        /// </summary>
        /// <returns>number of transit reminder events</returns>
        [WebMethod(Description = "Get all reminder events count.")]
        public int GetReminderEventsCount(string ticket)
        {
            return WebServiceImpl<TransitReminderEvent, ManagedReminderEvent, ReminderEvent>.GetCount(
                ticket);
        }

        /// <summary>
        /// Delete a reminder event
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        [WebMethod(Description = "Delete a reminder event.")]
        public void DeleteReminderEvent(string ticket, int id)
        {
            WebServiceImpl<TransitReminderEvent, ManagedReminderEvent, ReminderEvent>.Delete(
                ticket, id);
        }

        #endregion

        #region DataObject

        /// <summary>
        /// Create or update a data object.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="data object">transit data object</param>
        [WebMethod(Description = "Create or update a data object.")]
        public int CreateOrUpdateDataObject(string ticket, TransitDataObject dataobject)
        {
            return WebServiceImpl<TransitDataObject, ManagedDataObject, DataObject>.CreateOrUpdate(
                ticket, dataobject);
        }

        /// <summary>
        /// Get a data object.
        /// </summary>
        /// <returns>transit data object</returns>
        [WebMethod(Description = "Get a data object.")]
        public TransitDataObject GetDataObjectById(string ticket, int id)
        {
            return WebServiceImpl<TransitDataObject, ManagedDataObject, DataObject>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all  data objects.
        /// </summary>
        /// <returns>list of transit data objects</returns>
        [WebMethod(Description = "Get all  data objects.")]
        public List<TransitDataObject> GetDataObjects(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitDataObject, ManagedDataObject, DataObject>.GetList(
                ticket, options);
        }

        /// <summary>
        /// Get all  data objects count.
        /// </summary>
        /// <returns>number of transit data objects</returns>
        [WebMethod(Description = "Get all  data objects.")]
        public int GetDataObjectsCount(string ticket)
        {
            return WebServiceImpl<TransitDataObject, ManagedDataObject, DataObject>.GetCount(
                ticket);
        }

        /// <summary>
        /// Delete a data object
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        [WebMethod(Description = "Delete a data object.")]
        public void DeleteDataObject(string ticket, int id)
        {
            WebServiceImpl<TransitDataObject, ManagedDataObject, DataObject>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Get data object fields.
        /// </summary>
        /// <returns>list of fields</returns>
        [WebMethod(Description = "Get data object fields.")]
        public List<string> GetDataObjectFieldsById(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                DataObject dataobject = session.Load<DataObject>(id);
                Type t = Assembly.GetAssembly(typeof(DataObject)).GetType(dataobject.Name, true);

                PropertyInfo[] pinfo = t.GetProperties();

                List<string> result = new List<string>(pinfo.Length);
                foreach (PropertyInfo property in pinfo)
                {
                    result.Add(property.Name);
                }

                return result;
            }
        }

        #endregion

        #region Bookmark

        /// <summary>
        /// Create or update a bookmark.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="bookmark">transit bookmark</param>
        [WebMethod(Description = "Create or update a bookmark.")]
        public int CreateOrUpdateBookmark(string ticket, TransitBookmark bookmark)
        {
            return WebServiceImpl<TransitBookmark, ManagedBookmark, Bookmark>.CreateOrUpdate(
                ticket, bookmark);
        }

        /// <summary>
        /// Get a bookmark.
        /// </summary>
        /// <returns>transit bookmark</returns>
        [WebMethod(Description = "Get a bookmark.", BufferResponse = true)]
        public TransitBookmark GetBookmarkById(string ticket, int id)
        {
            return WebServiceImpl<TransitBookmark, ManagedBookmark, Bookmark>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get a bookmark if modified since.
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="id"></param>
        /// <param name="ifModifiedSince"></param>
        /// <returns></returns>
        [WebMethod(Description = "Get a bookmark if modified since.", BufferResponse = true)]
        public TransitBookmark GetBookmarkIfModifiedSinceById(string ticket, int id, DateTime ifModifiedSince)
        {
            TransitBookmark t_instance = WebServiceImpl<TransitBookmark, ManagedBookmark, Bookmark>.GetById(
                ticket, id);

            if (t_instance.Modified <= ifModifiedSince)
                return null;

            return t_instance;
        }

        /// <summary>
        /// Get all  bookmarks.
        /// </summary>
        /// <returns>list of transit bookmarks</returns>
        [WebMethod(Description = "Get all  bookmarks.")]
        public List<TransitBookmark> GetBookmarks(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitBookmark, ManagedBookmark, Bookmark>.GetList(
                ticket, options);
        }

        /// <summary>
        /// Get all  bookmarks count.
        /// </summary>
        /// <returns>number of transit bookmarks</returns>
        [WebMethod(Description = "Get all  bookmarks.")]
        public int GetBookmarksCount(string ticket)
        {
            return WebServiceImpl<TransitBookmark, ManagedBookmark, Bookmark>.GetCount(
                ticket);
        }

        /// <summary>
        /// Delete a bookmark
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        [WebMethod(Description = "Delete a bookmark.")]
        public void DeleteBookmark(string ticket, int id)
        {
            WebServiceImpl<TransitBookmark, ManagedBookmark, Bookmark>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Get all bookmarks that have bitmaps associated.
        /// </summary>
        /// <returns>list of transit bookmarks</returns>
        [WebMethod(Description = "Get all bookmarks that have bitmaps associated.", CacheDuration = 60)]
        public List<TransitBookmark> GetBookmarksWithOptions(string ticket, BookmarkQueryOptions qopt, ServiceQueryOptions options)
        {
            List<TransitBookmark> bookmarks = WebServiceImpl<TransitBookmark, ManagedBookmark, Bookmark>.GetList(
                ticket, null);

            if (qopt != null)
            {
                for (int i = bookmarks.Count - 1; i >= 0; i--)
                {
                    if (qopt.WithFullBitmaps && !bookmarks[i].HasFullBitmap)
                    {
                        bookmarks.RemoveAt(i);
                        continue;
                    }

                    if (qopt.WithLinkedBitmaps && !bookmarks[i].HasLinkBitmap)
                    {
                        bookmarks.RemoveAt(i);
                        continue;
                    }
                }
            }

            if (options != null)
            {
                Collection<TransitBookmark>.ApplyServiceOptions(options.FirstResult, options.PageSize, bookmarks);
            }

            return bookmarks;
        }

        #endregion

        #region Feature

        /// <summary>
        /// Feature an object.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="tf">transit feature</param>
        [WebMethod(Description = "Create or update a feature.")]
        public int CreateOrUpdateFeature(string ticket, TransitFeature feature)
        {
            return WebServiceImpl<TransitFeature, ManagedFeature, Feature>.CreateOrUpdate(
                ticket, feature);
        }

        /// <summary>
        /// Get a feature.
        /// </summary>
        /// <returns>transit feature</returns>
        [WebMethod(Description = "Get a feature.")]
        public TransitFeature GetFeatureById(string ticket, int id)
        {
            return WebServiceImpl<TransitFeature, ManagedFeature, Feature>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get the number of features of a certain type.
        /// </summary>
        /// <returns>number of features</returns>
        [WebMethod(Description = "Get the number features of a certain type.", CacheDuration = 60)]
        public int GetFeaturesCount(string ticket, string featuretype)
        {
            return WebServiceImpl<TransitFeature, ManagedFeature, Feature>.GetCount(
                ticket, string.Format("WHERE Feature.DataObject.Name = '{0}'", Renderer.SqlEncode(featuretype)));
        }

        /// <summary>
        /// Get all features of a certain type.
        /// </summary>
        /// <returns>list of transit features</returns>
        [WebMethod(Description = "Get all features of a certain type.", CacheDuration = 60)]
        public List<TransitFeature> GetFeatures(string ticket, string featuretype, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitFeature, ManagedFeature, Feature>.GetList(
                ticket, options, string.Format("SELECT Feature FROM Feature Feature WHERE Feature.DataObject.Name = '{0}'" +
                    " ORDER BY Feature.Created DESC", featuretype));
        }

        /// <summary>
        /// Get latest feature of a certain type.
        /// </summary>
        /// <returns>transit features</returns>
        [WebMethod(Description = "Get the latest feature of a certain type.", CacheDuration = 60)]
        public TransitFeature GetLatestFeature(string ticket, string featuretype)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                int feature_id = ManagedFeature.GetLatestFeatureId(session, featuretype);

                if (feature_id == 0)
                    return null;

                return new ManagedFeature(session, feature_id).GetTransitInstance(sec);
            }
        }

        /// <summary>
        /// Find the latest feature of a certain type for an item.
        /// </summary>
        /// <returns>transit features</returns>
        [WebMethod(Description = "Find the latest feature of a certain type for an item.", CacheDuration = 60)]
        public TransitFeature FindLatestFeature(string ticket, string featuretype, int objectid)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                int feature_id = ManagedFeature.GetLatestFeatureId(session, featuretype, objectid);

                if (feature_id == 0)
                    return null;

                return new ManagedFeature(session, feature_id).GetTransitInstance(sec);
            }
        }

        /// <summary>
        /// Delete a feature.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        [WebMethod(Description = "Delete a feature.")]
        public void DeleteFeature(string ticket, int id)
        {
            WebServiceImpl<TransitFeature, ManagedFeature, Feature>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Delete all features of an object.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="feature">token transit feature</param>
        [WebMethod(Description = "Delete all feature of an object.")]
        public void DeleteAllFeatures(string ticket, TransitFeature token)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);

                if (!sec.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedFeature.Delete(session, token.DataObjectName, token.DataRowId);
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        #endregion
    }
}
