using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Messaging;
using Microsoft.Web.Services3.Design;
using SnCore.Data.Hibernate;
using System.Web.Services.Protocols;

namespace SnCore.WebServices
{
    public class ServiceQueryOptions
    {
        public int PageSize = -1;
        public int PageNumber = 0;

        public int FirstResult
        {
            get
            {
                return PageSize * PageNumber;
            }
        }

        public ServiceQueryOptions()
        {
        }

        public ServiceQueryOptions(int pagesize, int pagenumber)
        {
            PageSize = pagesize;
            PageNumber = pagenumber;
        }
    };

    public class WebService : SoapService
    {
        public WebService()
        {

        }

        public static IDbConnection GetNewConnection()
        {
            return new SqlConnection(
             SnCore.Data.Hibernate.Session.Configuration.GetProperty(
              "hibernate.connection.connection_string"));
        }
    }
}
