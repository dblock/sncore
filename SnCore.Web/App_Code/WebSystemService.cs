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

namespace SnCore.WebServices
{
    /// <summary>
    /// System information services.
    /// </summary>
    /// 
    [WebService(Namespace = "http://www.vestris.com/sncore/ns/", Name = "WebSystemService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WebSystemService : WebService
    {
        public WebSystemService()
        {

        }

        #region System
        /// <summary>
        /// System version.
        /// </summary>
        [WebMethod(Description = "System version.", CacheDuration = 120)]
        public string GetVersion()
        {
            return ManagedSystem.Version;
        }

        /// <summary>
        /// System title.
        /// </summary>
        [WebMethod(Description = "System title.", CacheDuration = 120)]
        public string GetTitle()
        {
            return ManagedSystem.Title;
        }

        /// <summary>
        /// Product copyright.
        /// </summary>
        [WebMethod(Description = "Product copyright.", CacheDuration = 120)]
        public string GetCopyright()
        {
            return ManagedSystem.Copyright;
        }

        /// <summary>
        /// Product description.
        /// </summary>
        [WebMethod(Description = "Product description.", CacheDuration = 120)]
        public string GetDescription()
        {
            return ManagedSystem.Description;
        }

        /// <summary>
        /// Product uptime in ticks.
        /// </summary>
        [WebMethod(Description = "Product uptime in ticks.")]
        public long GetUptime()
        {
            return ((TimeSpan)(DateTime.UtcNow - Global.Started)).Ticks;
        }       

        #endregion

        #region Configuration
        /// <summary>
        /// Get all configurations.
        /// </summary>
        /// <returns>list of configurations</returns>
        [WebMethod(Description = "Get all configurations.", CacheDuration = 60)]
        public List<TransitConfiguration> GetConfigurations()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList configurations = session.CreateCriteria(typeof(Configuration)).List();
                List<TransitConfiguration> result = new List<TransitConfiguration>(configurations.Count);
                foreach (Configuration c in configurations)
                {
                    TransitConfiguration tc = new ManagedConfiguration(session, c).TransitConfiguration;
                    if (tc.Password)
                    {
                        tc.Value = "[hidden]";
                    }
                    result.Add(tc);
                }
                return result;
            }
        }

        /// <summary>
        /// Add a configuration.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="c">transit configuration information</param>
        /// <returns>new configuration id</returns>
        /// </summary>
        [WebMethod(Description = "Add a configuration.")]
        public int AddConfiguration(string ticket, TransitConfiguration c)
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

                Configuration n = c.GetConfiguration(session);
                session.Save(n);
                session.Flush();
                return n.Id;
            }
        }

        /// <summary>
        /// Delete a configuration.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">configuration id</param>
        /// </summary>
        [WebMethod(Description = "Delete a configuration.")]
        public void DeleteConfiguration(string ticket, int id)
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

                ManagedConfiguration m = new ManagedConfiguration(session, id);
                m.Delete();
                session.Flush();
            }
        }

        /// <summary>
        /// Get configuration by id.
        /// </summary>
        /// <param name="id">configuratoin id</param>
        /// <returns></returns>
        [WebMethod(Description = "Get configuration by id.")]
        public TransitConfiguration GetConfigurationById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitConfiguration tc = new ManagedConfiguration(session, id).TransitConfiguration;
                if (tc.Password)
                {
                    tc.Value = "[hidden]";
                }
                return tc;
            }
        }

        /// <summary>
        /// Get configuration by name.
        /// </summary>
        /// <param name="name">configuration name</param>
        /// <returns></returns>
        [WebMethod(Description = "Get configuration by name.")]
        public TransitConfiguration GetConfigurationByName(string name)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitConfiguration tc = ManagedConfiguration.GetConfigurationByName(session, name).TransitConfiguration;
                if (tc.Password)
                {
                    tc.Value = "[hidden]";
                }
                return tc;
            }
        }

        /// <summary>
        /// Get configuration value with default.
        /// </summary>
        /// <param name="name">configuration name</param>
        /// <returns></returns>
        [WebMethod(Description = "Get configuration value.", CacheDuration = 60)]
        public TransitConfiguration GetConfigurationByNameWithDefault(string name, string def)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                Configuration c = ManagedConfiguration.GetConfiguration(session, name, false);
                if (c == null)
                {
                    TransitConfiguration tc = new TransitConfiguration();
                    tc.Name = name;
                    tc.Password = false;
                    tc.Value = def;
                    return tc;
                }
                else
                {
                    TransitConfiguration tc = new ManagedConfiguration(session, c).TransitConfiguration;
                    if (tc.Password)
                    {
                        tc.Value = "[hidden]";
                    }
                    return tc;
                }
            }
        }

        #endregion

        #region Survey
        /// <summary>
        /// Get survey questions.
        /// </summary>
        /// <param name="surveyid">survey id</param>
        /// <returns>transit survey questions</returns>
        [WebMethod(Description = "Get survey questions.")]
        public List<TransitSurveyQuestion> GetSurveyQuestions(int surveyid)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList list = session.CreateCriteria(typeof(SurveyQuestion))
                    .Add(Expression.Eq("Survey.Id", surveyid))
                    .List();

                List<TransitSurveyQuestion> result = new List<TransitSurveyQuestion>(list.Count);
                foreach (SurveyQuestion e in list)
                {
                    result.Add(new TransitSurveyQuestion(e));
                }
                return result;
            }
        }

        /// <summary>
        /// Get survey question by id.
        /// </summary>
        /// <param name="id">survey question id</param>
        /// <returns>transit survey question</returns>
        [WebMethod(Description = "Get survey question by id.")]
        public TransitSurveyQuestion GetSurveyQuestionById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return new ManagedSurveyQuestion(session, id).TransitSurveyQuestion;
            }
        }

        /// <summary>
        /// Add a survey question.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="surveyquestion">new survey question</param>
        [WebMethod(Description = "Add a survey question.")]
        public void AddSurveyQuestion(string ticket, TransitSurveyQuestion surveyquestion)
        {
            int id = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount a = new ManagedAccount(session, id);
                if (!a.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedSurveyQuestion mq = new ManagedSurveyQuestion(session);
                mq.Create(surveyquestion);
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Delete a survey question.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="surveyquestionid">survey question id</param>
        [WebMethod(Description = "Delete a survey question.")]
        public void DeleteSurveyQuestion(string ticket, int surveyquestionid)
        {
            int id = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSurveyQuestion mq = new ManagedSurveyQuestion(session, surveyquestionid);
                mq.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Add a survey.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="c">transit survey information</param>
        /// <returns>new survey id</returns>
        /// </summary>
        [WebMethod(Description = "Add a survey.")]
        public int AddSurvey(string ticket, TransitSurvey c)
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

                ManagedSurvey m = new ManagedSurvey(session);
                m.Create(c);
                session.Flush();
                return m.Id;
            }
        }

        /// <summary>
        /// Delete a survey.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">survey id</param>
        /// </summary>
        [WebMethod(Description = "Delete a survey.")]
        public void DeleteSurvey(string ticket, int id)
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

                ManagedSurvey m = new ManagedSurvey(session, id);
                m.Delete();
                session.Flush();
            }
        }

        /// <summary>
        /// Get surveys.
        /// </summary>
        /// <returns>transit survey questions</returns>
        [WebMethod(Description = "Get surveys.", CacheDuration = 60)]
        public List<TransitSurvey> GetSurveys()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList list = session.CreateCriteria(typeof(Survey))
                    .List();

                List<TransitSurvey> result = new List<TransitSurvey>(list.Count);
                foreach (Survey s in list)
                {
                    result.Add(new TransitSurvey(s));
                }
                return result;
            }
        }

        /// <summary>
        /// Get a survey by id.
        /// </summary>
        /// <param name="id">survey id</param>
        /// <returns>transit survey</returns>
        [WebMethod(Description = "Get a survey by id.")]
        public TransitSurvey GetSurveyById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return new ManagedSurvey(session, id).TransitSurvey;
            }
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
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedPicture m_picture = new ManagedPicture(session);
                m_picture.CreateOrUpdate(picture);
                SnCore.Data.Hibernate.Session.Flush();
                return m_picture.Id;
            }
        }

        /// <summary>
        /// Get a picture.
        /// </summary>
        /// <returns>transit picture</returns>
        [WebMethod(Description = "Get a picture.")]
        public TransitPicture GetPictureById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitPicture result = new ManagedPicture(session, id).TransitPicture;
                return result;
            }
        }


        /// <summary>
        /// Get all pictures.
        /// </summary>
        /// <returns>list of transit pictures</returns>
        [WebMethod(Description = "Get all pictures.")]
        public List<TransitPicture> GetPictures()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList pictures = session.CreateCriteria(typeof(Picture)).List();
                List<TransitPicture> result = new List<TransitPicture>(pictures.Count);
                foreach (Picture picture in pictures)
                {
                    result.Add(new ManagedPicture(session, picture).TransitPicture);
                }
                return result;
            }
        }

        /// <summary>
        /// Delete a picture
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a picture.")]
        public void DeletePicture(string ticket, int id)
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

                ManagedPicture m_picture = new ManagedPicture(session, id);
                m_picture.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
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
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedPictureType m_picturetype = new ManagedPictureType(session);
                m_picturetype.CreateOrUpdate(picturetype);
                SnCore.Data.Hibernate.Session.Flush();
                return m_picturetype.Id;
            }
        }

        /// <summary>
        /// Get a picture type.
        /// </summary>
        /// <returns>transit picture type</returns>
        [WebMethod(Description = "Get a picture type.")]
        public TransitPictureType GetPictureTypeById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitPictureType result = new ManagedPictureType(session, id).TransitPictureType;
                return result;
            }
        }


        /// <summary>
        /// Get all picture types.
        /// </summary>
        /// <returns>list of transit picture types</returns>
        [WebMethod(Description = "Get all picture types.")]
        public List<TransitPictureType> GetPictureTypes()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList picturetypes = session.CreateCriteria(typeof(PictureType)).List();
                List<TransitPictureType> result = new List<TransitPictureType>(picturetypes.Count);
                foreach (PictureType picturetype in picturetypes)
                {
                    result.Add(new ManagedPictureType(session, picturetype).TransitPictureType);
                }
                return result;
            }
        }

        /// <summary>
        /// Delete a picture type
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a picture type.")]
        public void DeletePictureType(string ticket, int id)
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

                ManagedPictureType m_picturetype = new ManagedPictureType(session, id);
                m_picturetype.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        #endregion

        #region Picture with Bitmaps

        /// <summary>
        /// Get picture data.
        /// </summary>
        /// <param name="id">picture id</param>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit picture</returns>
        [WebMethod(Description = "Get picture data.", BufferResponse = true)]
        public TransitPictureWithBitmap GetPictureWithBitmapById(string ticket, int id)
        {
            // todo: check permissions with ticket
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedPicture a = new ManagedPicture(session, id);
                return a.TransitPictureWithBitmap;
            }
        }

        /// <summary>
        /// Get picture data if modified since.
        /// </summary>
        /// <param name="id">picture id</param>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="ifModifiedSince">last update date/time</param>
        /// <returns>transit picture</returns>
        [WebMethod(Description = "Get picture data if modified since.", BufferResponse = true)]
        public TransitPictureWithBitmap GetPictureWithBitmapByIdIfModifiedSince(string ticket, int id, DateTime ifModifiedSince)
        {
            // todo: check permissions with ticket
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedPicture p = new ManagedPicture(session, id);

                if (p.Modified <= ifModifiedSince)
                {
                    return null;
                }

                return p.TransitPictureWithBitmap;
            }
        }

        /// <summary>
        /// Get picture thumbnail.
        /// </summary>
        /// <param name="id">picture id</param>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit picture, thumbnail only</returns>
        [WebMethod(Description = "Get picture thumbnail.", BufferResponse = true)]
        public TransitPictureWithThumbnail GetPictureWithThumbnailById(string ticket, int id)
        {
            // todo: check permissions with ticket
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedPicture p = new ManagedPicture(session, id);
                return p.TransitPictureWithThumbnail;
            }
        }

        /// <summary>
        /// Get random picture thumbnail.
        /// </summary>
        /// <param name="type">picture type</param>
        /// <returns>transit random picture, thumbnail only</returns>
        [WebMethod(Description = "Get random picture thumbnail.", BufferResponse = true)]
        public TransitPictureWithThumbnail GetRandomPictureWithThumbnailByType(string type)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                PictureType picturetype = ManagedPictureType.Find(session, type);

                if (picturetype == null)
                {
                    return null;
                }

                IList list = session.CreateCriteria(typeof(Picture))
                    .Add(Expression.Eq("Type.Id", picturetype.Id))
                    .List();

                if (list.Count == 0)
                {
                    return null;
                }

                return new ManagedPicture(session, (Picture)
                    list[new Random().Next() % list.Count]).TransitPictureWithThumbnail;
            }
        }

        /// <summary>
        /// Get picture thumbnail.
        /// </summary>
        /// <param name="id">picture id</param>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="ifModifiedSince">last update date/time</param>
        /// <returns>transit picture, thumbnail only</returns>
        [WebMethod(Description = "Get picture thumbnail if modified since.", BufferResponse = true)]
        public TransitPictureWithThumbnail GetPictureWithThumbnailByIdIfModifiedSince(string ticket, int id, DateTime ifModifiedSince)
        {
            // todo: check permissions with ticket
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedPicture p = new ManagedPicture(session, id);

                if (p.Modified <= ifModifiedSince)
                {
                    return null;
                }

                return p.TransitPictureWithThumbnail;
            }
        }

        #endregion

        #region Reminder

        /// <summary>
        /// Create or update a reminder.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="reminder">transit  reminder</param>
        [WebMethod(Description = "Create or update a reminder.")]
        public int CreateOrUpdateReminder(string ticket, TransitReminder reminder)
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

                ManagedReminder m_reminder = new ManagedReminder(session);
                m_reminder.CreateOrUpdate(reminder);
                SnCore.Data.Hibernate.Session.Flush();
                return m_reminder.Id;
            }
        }

        /// <summary>
        /// Get a reminder.
        /// </summary>
        /// <returns>transit  reminder</returns>
        [WebMethod(Description = "Get a reminder.")]
        public TransitReminder GetReminderById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitReminder result = new ManagedReminder(session, id).TransitReminder;
                return result;
            }
        }

        /// <summary>
        /// Get all  reminders.
        /// </summary>
        /// <returns>list of transit  reminders</returns>
        [WebMethod(Description = "Get all  reminders.")]
        public List<TransitReminder> GetReminders()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList reminders = session.CreateCriteria(typeof(Reminder)).List();
                List<TransitReminder> result = new List<TransitReminder>(reminders.Count);
                foreach (Reminder reminder in reminders)
                {
                    result.Add(new ManagedReminder(session, reminder).TransitReminder);
                }
                return result;
            }
        }

        /// <summary>
        /// Delete a reminder
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a reminder.")]
        public void DeleteReminder(string ticket, int id)
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

                ManagedReminder m_reminder = new ManagedReminder(session, id);
                m_reminder.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Test whether a reminder is being sent to an account.
        /// </summary>
        [WebMethod(Description = "Test whether a reminder is being sent to an account.")]
        public bool CanSendReminder(int reminder_id, int account_id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedReminder mr = new ManagedReminder(session, reminder_id);
                return mr.CanSend(account_id);
            }
        }

        #endregion

        #region Reminder Account Property

        /// <summary>
        /// Create or update a reminder account property.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="reminderaccountproperty">transit reminder account property</param>
        [WebMethod(Description = "Create or update a reminder account property.")]
        public int CreateOrUpdateReminderAccountProperty(string ticket, TransitReminderAccountProperty reminderaccountproperty)
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

                ManagedReminderAccountProperty m_reminderaccountproperty = new ManagedReminderAccountProperty(session);
                m_reminderaccountproperty.CreateOrUpdate(reminderaccountproperty);
                SnCore.Data.Hibernate.Session.Flush();
                return m_reminderaccountproperty.Id;
            }
        }

        /// <summary>
        /// Get a reminder account property.
        /// </summary>
        /// <returns>transit reminder account property</returns>
        [WebMethod(Description = "Get a reminderaccountproperty.")]
        public TransitReminderAccountProperty GetReminderAccountPropertyById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitReminderAccountProperty result = new ManagedReminderAccountProperty(session, id).TransitReminderAccountProperty;
                return result;
            }
        }

        /// <summary>
        /// Get all reminder account properties count.
        /// </summary>
        [WebMethod(Description = "Get all reminder account properties count.")]
        public int GetReminderAccountPropertiesCountById(int reminder_id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int) session.CreateQuery(string.Format(
                    "SELECT COUNT(*) FROM ReminderAccountProperty r WHERE r.Reminder.Id = {0}",
                    reminder_id)).UniqueResult();
            }
        }

        /// <summary>
        /// Get all reminder account properties.
        /// </summary>
        /// <returns>list of transit reminder account properties</returns>
        [WebMethod(Description = "Get all reminder account properties.")]
        public List<TransitReminderAccountProperty> GetReminderAccountPropertiesById(
            int reminder_id, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ICriteria c = session.CreateCriteria(typeof(ReminderAccountProperty))
                    .Add(Expression.Eq("Reminder.Id", reminder_id));

                if (options != null)
                {
                    c.SetFirstResult(options.FirstResult);
                    c.SetMaxResults(options.PageSize);
                }

                IList reminderaccountproperties = c.List();

                List<TransitReminderAccountProperty> result = new List<TransitReminderAccountProperty>(reminderaccountproperties.Count);
                foreach (ReminderAccountProperty reminderaccountproperty in reminderaccountproperties)
                {
                    result.Add(new ManagedReminderAccountProperty(session, reminderaccountproperty)
                        .TransitReminderAccountProperty);
                }

                return result;
            }
        }

        /// <summary>
        /// Delete a reminder account property
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a reminder account property.")]
        public void DeleteReminderAccountProperty(string ticket, int id)
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

                ManagedReminderAccountProperty m_reminderaccountproperty = new ManagedReminderAccountProperty(session, id);
                m_reminderaccountproperty.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
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
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedReminderEvent m_reminderevent = new ManagedReminderEvent(session);
                m_reminderevent.CreateOrUpdate(reminderevent);
                SnCore.Data.Hibernate.Session.Flush();
                return m_reminderevent.Id;
            }
        }

        /// <summary>
        /// Get a reminderevent.
        /// </summary>
        /// <returns>transit reminder event</returns>
        [WebMethod(Description = "Get a reminder event.")]
        public TransitReminderEvent GetReminderEventById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitReminderEvent result = new ManagedReminderEvent(session, id).TransitReminderEvent;
                return result;
            }
        }

        /// <summary>
        /// Get all reminder events.
        /// </summary>
        /// <returns>list of transit reminder events</returns>
        [WebMethod(Description = "Get all reminder events.")]
        public List<TransitReminderEvent> GetReminderEvents()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList reminderevents = session.CreateCriteria(typeof(ReminderEvent)).List();
                List<TransitReminderEvent> result = new List<TransitReminderEvent>(reminderevents.Count);
                foreach (ReminderEvent reminderevent in reminderevents)
                {
                    result.Add(new ManagedReminderEvent(session, reminderevent).TransitReminderEvent);
                }
                return result;
            }
        }

        /// <summary>
        /// Delete a reminder event
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a reminder event.")]
        public void DeleteReminderEvent(string ticket, int id)
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

                ManagedReminderEvent m_reminderevent = new ManagedReminderEvent(session, id);
                m_reminderevent.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        #endregion

        #region DataObject

        /// <summary>
        /// Get all data objects.
        /// </summary>
        /// <returns>list of transit data objects</returns>
        [WebMethod(Description = "Get all data objects.")]
        public List<TransitDataObject> GetDataObjects()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList dataobjects = session.CreateCriteria(typeof(DataObject)).List();
                List<TransitDataObject> result = new List<TransitDataObject>(dataobjects.Count);
                foreach (DataObject dataobject in dataobjects)
                {
                    result.Add(new ManagedDataObject(session, dataobject).TransitDataObject);
                }
                return result;
            }
        }

        /// <summary>
        /// Get data object fields.
        /// </summary>
        /// <returns>list of fields</returns>
        [WebMethod(Description = "Get data object fields.")]
        public List<string> GetDataObjectFieldsById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                DataObject dataobject = (DataObject)session.Load(typeof(DataObject), id);
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

        #region Invitations
        /// <summary>
        /// Get all account invitations.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit account invitations</returns>
        [WebMethod(Description = "Get all account invitations.", CacheDuration = 60)]
        public List<TransitAccountInvitation> GetAccountInvitations(string ticket)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount acct = new ManagedAccount(session, userid);

                if (!acct.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                IList list = session.CreateCriteria(typeof(AccountInvitation))
                    .AddOrder(Order.Desc("Created"))
                    .List();
                List<TransitAccountInvitation> result = new List<TransitAccountInvitation>(list.Count);
                foreach (AccountInvitation e in list)
                {
                    TransitAccountInvitation i = new TransitAccountInvitation(e);
                    i.Code = string.Empty;
                    result.Add(i);
                }
                return result;
            }
        }

        #endregion

        #region Bookmark

        /// <summary>
        /// Create or update a social bookmark.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="bookmark">transit bookmark</param>
        [WebMethod(Description = "Create or update a bookmark.")]
        public int CreateOrUpdateBookmark(string ticket, TransitBookmark bookmark)
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

                ManagedBookmark m_bookmark = new ManagedBookmark(session);
                m_bookmark.CreateOrUpdate(bookmark);
                SnCore.Data.Hibernate.Session.Flush();
                return m_bookmark.Id;
            }
        }

        /// <summary>
        /// Get a bookmark.
        /// </summary>
        /// <returns>transit bookmark</returns>
        [WebMethod(Description = "Get a bookmark.")]
        public TransitBookmark GetBookmarkById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitBookmark result = new ManagedBookmark(session, id).TransitBookmark;
                return result;
            }
        }


        /// <summary>
        /// Get all bookmarks.
        /// </summary>
        /// <returns>list of transit bookmarks</returns>
        [WebMethod(Description = "Get all bookmarks.", CacheDuration = 60)]
        public List<TransitBookmark> GetBookmarks()
        {
            return GetBookmarksWithOptions(null);
        }

        /// <summary>
        /// Get all bookmarks that have bitmaps associated.
        /// </summary>
        /// <returns>list of transit bookmarks</returns>
        [WebMethod(Description = "Get all bookmarks that have bitmaps associated.", CacheDuration = 60)]
        public List<TransitBookmark> GetBookmarksWithOptions(BookmarkQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList bookmarks = session.CreateCriteria(typeof(Bookmark)).List();
                List<TransitBookmark> result = new List<TransitBookmark>(bookmarks.Count);
                foreach (Bookmark bookmark in bookmarks)
                {
                    if (options != null && options.WithFullBitmaps && bookmark.FullBitmap == null)
                        continue;

                    if (options != null && options.WithLinkedBitmaps && bookmark.LinkBitmap == null)
                        continue;

                    result.Add(new ManagedBookmark(session, bookmark).TransitBookmark);
                }
                return result;
            }
        }

        /// <summary>
        /// Delete a bookmark.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a bookmark.")]
        public void DeleteBookmark(string ticket, int id)
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

                ManagedBookmark m_bookmark = new ManagedBookmark(session, id);
                m_bookmark.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        #endregion

        #region Bookmark with Bitmaps

        /// <summary>
        /// Get bookmark data.
        /// </summary>
        /// <param name="id">bookmark id</param>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit bookmark</returns>
        [WebMethod(Description = "Get bookmark data.", BufferResponse = true)]
        public TransitBookmarkWithBitmaps GetBookmarkWithBitmapsById(string ticket, int id)
        {
            // todo: check permissions with ticket
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedBookmark a = new ManagedBookmark(session, id);
                return a.TransitBookmarkWithBitmaps;
            }
        }

        /// <summary>
        /// Get bookmark data if modified since.
        /// </summary>
        /// <param name="id">bookmark id</param>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="ifModifiedSince">last update date/time</param>
        /// <returns>transit bookmark</returns>
        [WebMethod(Description = "Get bookmark data if modified since.", BufferResponse = true)]
        public TransitBookmarkWithBitmaps GetBookmarkWithBitmapsByIdIfModifiedSince(string ticket, int id, DateTime ifModifiedSince)
        {
            // todo: check permissions with ticket
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedBookmark p = new ManagedBookmark(session, id);

                if (p.Modified <= ifModifiedSince)
                {
                    return null;
                }

                return p.TransitBookmarkWithBitmaps;
            }
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
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedFeature m_feature = new ManagedFeature(session);
                m_feature.CreateOrUpdate(feature);
                SnCore.Data.Hibernate.Session.Flush();
                return m_feature.Id;
            }
        }

        /// <summary>
        /// Get a feature.
        /// </summary>
        /// <returns>transit feature</returns>
        [WebMethod(Description = "Get a feature.")]
        public TransitFeature GetFeatureById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitFeature result = new ManagedFeature(session, id).TransitFeature;
                return result;
            }
        }

        /// <summary>
        /// Get the number of features of a certain type.
        /// </summary>
        /// <returns>number of features</returns>
        [WebMethod(Description = "Get the number features of a certain type.", CacheDuration = 60)]
        public int GetFeaturesCount(string featuretype)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int) session.CreateQuery(string.Format(
                    "SELECT COUNT(*) FROM Feature f WHERE f.DataObject.Id = {0}",
                    ManagedDataObject.Find(session, featuretype))).UniqueResult();
            }
        }

        /// <summary>
        /// Get all features of a certain type.
        /// </summary>
        /// <returns>list of transit features</returns>
        [WebMethod(Description = "Get all features of a certain type.", CacheDuration = 60)]
        public List<TransitFeature> GetFeatures(string featuretype, ServiceQueryOptions serviceoptions)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ICriteria c = session.CreateCriteria(typeof(Feature))
                    .Add(Expression.Eq("DataObject.Id", ManagedDataObject.Find(session, featuretype)))
                    .AddOrder(Order.Desc("Created"));

                if (serviceoptions != null)
                {
                    c.SetMaxResults(serviceoptions.PageSize);
                    c.SetFirstResult(serviceoptions.PageNumber * serviceoptions.PageSize);
                }

                IList list = c.List();

                List<TransitFeature> result = new List<TransitFeature>(list.Count);
                foreach (Feature feature in list)
                {
                    result.Add(new ManagedFeature(session, feature).TransitFeature);
                }

                return result;
            }
        }

        /// <summary>
        /// Get latest feature of a certain type.
        /// </summary>
        /// <returns>transit features</returns>
        [WebMethod(Description = "Get the latest feature of a certain type.", CacheDuration = 60)]
        public TransitFeature GetLatestFeature(string featuretype)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                int feature_id = ManagedFeature.GetLatestFeatureId(session, featuretype);

                if (feature_id == 0)
                    return null;

                return new ManagedFeature(session, feature_id).TransitFeature;
            }
        }

        /// <summary>
        /// Find the latest feature of a certain type for an item.
        /// </summary>
        /// <returns>transit features</returns>
        [WebMethod(Description = "Find the latest feature of a certain type for an item.", CacheDuration = 60)]
        public TransitFeature FindLatestFeature(string featuretype, int objectid)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                int feature_id = ManagedFeature.GetLatestFeatureId(session, featuretype, objectid);

                if (feature_id == 0)
                    return null;

                return new ManagedFeature(session, feature_id).TransitFeature;
            }
        }

        /// <summary>
        /// Delete a feature.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a feature.")]
        public void DeleteFeature(string ticket, int id)
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

                ManagedFeature m_feature = new ManagedFeature(session, id);
                m_feature.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Delete all features of an object.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="feature">token transit feature</param>
        /// </summary>
        [WebMethod(Description = "Delete all feature of an object.")]
        public void DeleteAllFeatures(string ticket, TransitFeature token)
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

                ManagedFeature.Delete(session, token.DataObjectName, token.DataRowId);
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        #endregion

        #region Schedule

        /// <summary>
        /// Create or update a schedule.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="schedule">transit  schedule</param>
        [WebMethod(Description = "Create or update a schedule.")]
        public int CreateOrUpdateSchedule(string ticket, TransitSchedule schedule)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);
                int result = user.CreateOrUpdate(schedule);
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get a schedule.
        /// </summary>
        /// <returns>transit  schedule</returns>
        [WebMethod(Description = "Get a schedule.")]
        public TransitSchedule GetScheduleById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitSchedule result = new ManagedSchedule(session, id).TransitSchedule;
                return result;
            }
        }

        /// <summary>
        /// Get all  schedules.
        /// </summary>
        /// <returns>list of transit  schedules</returns>
        [WebMethod(Description = "Get all schedules.")]
        public List<TransitSchedule> GetSchedules()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList schedules = session.CreateCriteria(typeof(Schedule)).List();
                List<TransitSchedule> result = new List<TransitSchedule>(schedules.Count);
                foreach (Schedule schedule in schedules)
                {
                    result.Add(new ManagedSchedule(session, schedule).TransitSchedule);
                }
                return result;
            }
        }

        /// <summary>
        /// Delete a schedule
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a schedule.")]
        public void DeleteSchedule(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);
                ManagedSchedule m_schedule = new ManagedSchedule(session, id);

                if (m_schedule.AccountId != userid && !user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                m_schedule.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Get a humanly readable representation of a schedule.
        /// </summary>
        [WebMethod(Description = "Get a humanly readable representation of a schedule.")]
        public string GetScheduleString(TransitSchedule schedule, int offset)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSchedule m_schedule = new ManagedSchedule(session, schedule.GetSchedule(session));
                return m_schedule.ToString(offset);
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
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedAttribute m_attribute = new ManagedAttribute(session);
                m_attribute.CreateOrUpdate(attr);
                SnCore.Data.Hibernate.Session.Flush();
                return m_attribute.Id;
            }
        }

        /// <summary>
        /// Get an attribute.
        /// </summary>
        /// <returns>transit attribute</returns>
        [WebMethod(Description = "Get an attribute.")]
        public TransitAttribute GetAttributeById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitAttribute result = new ManagedAttribute(session, id).TransitAttribute;
                return result;
            }
        }

        /// <summary>
        /// Get an attribute with bitmap.
        /// </summary>
        /// <returns>transit attribute with bitmap</returns>
        [WebMethod(Description = "Get an attribute.")]
        public TransitAttributeWithBitmap GetAttributeWithBitmapById(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitAttributeWithBitmap result = new ManagedAttribute(session, id).TransitAttributeWithBitmap;
                return result;
            }
        }

        /// <summary>
        /// Get attribute data if modified since.
        /// </summary>
        /// <param name="id">attribute id</param>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="ifModifiedSince">last update date/time</param>
        /// <returns>transit attribute with bitmap</returns>
        [WebMethod(Description = "Get attribute data if modified since.", BufferResponse = true)]
        public TransitAttributeWithBitmap GetAttributeWithBitmapByIdIfModifiedSince(string ticket, int id, DateTime ifModifiedSince)
        {
            // todo: check permissions with ticket
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAttribute attribute = new ManagedAttribute(session, id);

                if (attribute.Modified <= ifModifiedSince)
                {
                    return null;
                }

                return attribute.TransitAttributeWithBitmap;
            }
        }

        /// <summary>
        /// Get all attributes.
        /// </summary>
        /// <returns>list of transit attributes</returns>
        [WebMethod(Description = "Get all attributes.")]
        public List<TransitAttribute> GetAttributes()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList attrs = session.CreateCriteria(typeof(Attribute)).List();
                List<TransitAttribute> result = new List<TransitAttribute>(attrs.Count);
                foreach (Attribute attr in attrs)
                {
                    result.Add(new ManagedAttribute(session, attr).TransitAttribute);
                }
                return result;
            }
        }

        /// <summary>
        /// Delete an attribute
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete an attribute.")]
        public void DeleteAttribute(string ticket, int id)
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

                ManagedAttribute m_attribute = new ManagedAttribute(session, id);
                m_attribute.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        #endregion

    }
}