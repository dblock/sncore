using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using SnCore.Services;
using System.Collections;
using SnCore.Tools.Web;
using System.Collections.Specialized;
using NHibernate;

namespace SnCore.WebServices
{
    [WebService(Namespace = "http://www.vestris.com/sncore/ns/", Name = "FacebookGraphService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class FacebookGraphService : WebService
    {
        public FacebookGraphService()
        {

        }

        [WebMethod]
        public Facebook.Schema.user GetUser(string[] cookieNames, string[] cookieValues)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                Facebook.Session.ConnectSession facebookSession = new Facebook.Session.ConnectSession(
                    ManagedConfiguration.GetValue(session, "Facebook.APIKey", ""),
                    ManagedConfiguration.GetValue(session, "Facebook.Secret", ""));

                TransitAccount ta = new TransitAccount();
                NameValueCollectionSerializer facebookCookies = new NameValueCollectionSerializer(cookieNames, cookieValues);
                facebookSession.SessionKey = facebookCookies.Collection["session_key"];
                facebookSession.UserId = long.Parse(facebookCookies.Collection["user"]);
                
                Facebook.Rest.Api facebookAPI = new Facebook.Rest.Api(facebookSession);
                return facebookAPI.Users.GetInfo();
            }
        }
    }
}

