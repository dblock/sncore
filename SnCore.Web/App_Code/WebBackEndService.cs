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
using System.Web.Services.Protocols;

namespace SnCore.BackEndServices
{
    /// <summary>
    /// Managed web back-end services.
    /// </summary>
    [WebService(Namespace = "http://www.vestris.com/sncore/ns/", Name = "WebBackEndService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WebBackEndService : WebService
    {
        public WebBackEndService()
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

    }
}

