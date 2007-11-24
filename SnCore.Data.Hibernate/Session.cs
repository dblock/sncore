using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using System.Web;
using System.Diagnostics;
using NHibernate.Dialect.Function;
using System.Data.SqlClient;

namespace SnCore.Data.Hibernate
{
    /// <summary>
    /// From Andrew Mayorov, http://wiki.nhibernate.org/display/NH/Using+NHibernate+with+ASP.Net
    /// </summary>
    public class Session
    {
        private static object _locker = new object();
        private static NHibernate.Cfg.Configuration _config = null;
        private static ISessionFactory _factory = null;
        private static ISessionStorage _sessionsource = null;
        private static DomainModel _model = null;
        private static bool http = false;

        static Session()
        {
            try
            {
                System.Web.ProcessModelInfo.GetCurrentProcessInfo();
                http = true;
            }
            catch (System.Web.HttpException) { }
            if (http)
                _sessionsource = new HttpSessionSource();
            else
                _sessionsource = new ThreadSessionSource();
        }

        /// <summary>
        /// Returns an instance of <see cref="NHibernate.Cfg.Configuration"/> object
        /// containing the configuration for current application.
        /// </summary>
        /// <remarks>The object is being created upon first request for it.</remarks>
        public static NHibernate.Cfg.Configuration Configuration
        {
            get
            {
                lock (_locker)
                {
                    if (_config == null)
                    {
                        CreateConfiguration();
                    }
                    return _config;
                }
            }
            set
            {
                lock (_locker)
                {
                    _config = value;
                }
            }
        }

        private static void CreateConfiguration()
        {
            _config = new NHibernate.Cfg.Configuration();
            if (_config.Properties["hibernate.dialect"] == null)
                _config.Properties.Add("hibernate.dialect", "NHibernate.Dialect.MsSql2005Dialect");
            if (_config.Properties["hibernate.connection.provider"] == null)
                _config.Properties.Add("hibernate.connection.provider", "NHibernate.Connection.DriverConnectionProvider");
            if (_config.Properties["hibernate.connection.driver_class"] == null)
                _config.Properties.Add("hibernate.connection.driver_class", "NHibernate.Driver.SqlClientDriver");
            if (_config.Properties["hibernate.connection.connection_string"] == null)
                _config.Properties.Add("hibernate.connection.connection_string", "Server=localhost;initial catalog=SnCore;Integrated Security=SSPI");
            _config.AddAssembly("SnCore.Data");
            _config.AddAssembly("SnCore.Data.Hibernate");
        }

        public static DomainModel Model
        {
            get
            {
                if (_model == null)
                {
                    lock (_locker)
                    {
                        if (_model == null)
                        {
                            _model = new DomainModel(Current);
                        }
                    }
                }

                return _model;
            }
            set
            {
                lock (_locker)
                {
                    _model = value;
                }
            }
        }

        /// <summary>
        /// Returns an object implementing <see cref="NHibernate.ISessionFactory"/>
        /// interface, used to create new sessions.
        /// </summary>
        /// <remarks>The factory is created upon first request for it.</remarks>
        public static ISessionFactory Factory
        {
            get
            {
                if (_factory == null)
                {
                    NHibernate.Cfg.Configuration config = Session.Configuration;
                    lock (_locker)
                    {
                        if (_factory == null)
                        {
                            _factory = config.BuildSessionFactory();
                            _factory.Dialect.Functions["count"] = new ClassicCountFunction();
                            _factory.Dialect.Functions["sum"] = new ClassicSumFunction();
                            _factory.Dialect.Functions["avg"] = new ClassicAvgFunction();
                        }
                    }
                }
                return _factory;
            }
            set
            {
                lock (_locker)
                {
                    _factory = value;
                }
            }
        }

        /// <summary>
        /// Initializes internal state for handling new request. Call this method at 
        /// the beginning of the ASP.NET request.
        /// </summary>
        public static void BeginRequest()
        {
            //if (HasCurrent())
            //{
            //    throw new ApplicationException("There mustn't be current session at the begining of a request.");
            //}
        }

        /// <summary>
        /// Finalizes the request. If current session exists the method flushes it and
        /// closes. Call this method at the end of ASP.NET request.
        /// </summary>
        public static void EndRequest()
        {
            if (HasCurrent())
                Close();
            _sessionsource.Set(null);
        }

        /// <summary>
        /// Returns a current session object.
        /// </summary>
        /// <remarks>The session object will be created upon first request for it.</remarks>
        public static ISession Current
        {
            get
            {
                ISession s = _sessionsource.Get();
                if (s == null)
                {
                    s = Factory.OpenSession((IDbConnection)null);
                    s.FlushMode = FlushMode.Never;
                    s.Disconnect();
                    _sessionsource.Set(s);
                }
                return s;
            }
        }

        /// <summary>
        /// Tells if we currently have a session object or not.
        /// </summary>
        /// <returns><b>true</b> if there is a current session and <b>false</b> otherwise.</returns>
        private static bool HasCurrent()
        {
            return _sessionsource.Get() != null;
        }

        public static IDisposable OpenConnection(IDbConnection connection)
        {
            return new SessionOpener(Current, connection);
        }

        public static IDisposable OpenConnection()
        {
            return OpenConnection(new SqlConnection(
                Configuration.GetProperty("hibernate.connection.connection_string")));
        }

        /// <summary>
        /// Flushes and closes current session.
        /// </summary>
        public static void CloseAndFlush()
        {
            ISession session = _sessionsource.Get();
            if (session != null)
            {
                session.Flush();
                session.Close();
                _sessionsource.Set(null);
            }
        }

        /// <summary>
        /// Closes current session.
        /// </summary>
        /// <remarks><b>N.B.</b> Close do not flush the session.</remarks>
        public static void Close()
        {
            ISession session = _sessionsource.Get();
            if (session != null)
            {
                session.Close();
                session.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                _sessionsource.Set(null);
            }
        }

        /// <summary>
        /// Flushes current session.
        /// </summary>
        public static void Flush()
        {
            ISession session = _sessionsource.Get();
            if (session != null)
                session.Flush();
        }

        /// <summary>
        /// Begins new COM+ transaction.
        /// </summary>
        /// <returns>Transaction object allowing to set transaction vote.</returns>
        /// <remarks>
        /// If current session exists, it will be closed first.<br/>
        /// Returned transaction object implements IDisposable and <b>must</b> be disposed
        /// after use in order to finish the transaction.
        /// </remarks>
        /// <example>
        /// <code>
        /// using( Transaction tx = Session.BeginTransaction() ) 
        /// {
        ///		...
        ///	}
        /// </code>
        /// </example>
        public static Transaction BeginTransaction()
        {
            // Close();
            return new Transaction();
        }

        private abstract class ISessionStorage
        {
            public abstract ISession Get();
            public abstract void Set(ISession value);
        }

        /// <summary>
        /// Stores a session in the <see cref="HttpContext.Items" /> collection.
        /// </summary>
        private class HttpSessionSource : ISessionStorage
        {
            public override ISession Get()
            {
                if (HttpContext.Current == null)
                    return null;

                return (ISession)HttpContext.Current.Items["sdf.Persist.session"];
            }

            public override void Set(ISession value)
            {
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Items["sdf.Persist.session"] = value;
                }
            }
        }

        /// <summary>
        /// Stores a session in the thread-static class member.
        /// </summary>
        private class ThreadSessionSource : ISessionStorage
        {
            [ThreadStatic]
            private static ISession _session = null;

            public override ISession Get()
            {
                return _session;
            }

            public override void Set(ISession value)
            {
                _session = value;
            }
        }
    }
}
