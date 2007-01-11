using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Services.Protocols;
using System.Reflection;
using NUnit.Framework;

namespace SnCore.Web.Soap.Tests
{
    public class WebServiceBaseTest<EndPointType>
        where EndPointType : new()
    {
        private EndPointType mEndPoint;
        private string mAdminTicket = string.Empty;
        private string mUserTicket = string.Empty;

        public WebServiceBaseTest()
        {
            mEndPoint = new EndPointType();
        }

        public EndPointType EndPoint
        {
            get
            {
                return mEndPoint;
            }
        }

        protected int CreateUser(string email, string password)
        {
            WebAccountService.TransitAccount t_instance = new WebAccountService.TransitAccount();
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.Password = password;
            t_instance.Birthday = DateTime.UtcNow.AddYears(-10);
            WebAccountService.WebAccountService account_endpoint = new WebAccountService.WebAccountService();
            int id = account_endpoint.CreateAccount(string.Empty, email, t_instance);
            Console.WriteLine("Created user: {0}", id);
            Assert.IsTrue(id > 0);
            return id;
        }

        protected void DeleteUser(int id)
        {
            WebAccountService.WebAccountService account_endpoint = new WebAccountService.WebAccountService();
            account_endpoint.DeleteAccountById(GetAdminTicket(), id, null); 
        }

        private void CreateUserAccount()
        {
            CreateUser("user@localhost.com", "password");
        }

        public string Login(string email, string password)
        {
            WebAuthService.WebAuthService endpoint = new WebAuthService.WebAuthService();
            return endpoint.Login(email, password);
        }

        public string GetUserTicket()
        {
            if (string.IsNullOrEmpty(mUserTicket))
            {
                try
                {
                    mUserTicket = Login("user@localhost.com", "password");
                }
                catch (SoapException)
                {
                    CreateUserAccount();
                    mUserTicket = Login("user@localhost.com", "password");
                }
            }

            return mUserTicket;
        }

        public WebAuthService.TransitAccount GetUserAccount()
        {
            WebAuthService.WebAuthService endpoint = new WebAuthService.WebAuthService();
            return endpoint.GetAccount(GetUserTicket());
        }

        public WebAuthService.TransitAccount GetAdminAccount()
        {
            WebAuthService.WebAuthService endpoint = new WebAuthService.WebAuthService();
            return endpoint.GetAccount(GetAdminTicket());
        }

        public string GetAdminTicket()
        {
            if (string.IsNullOrEmpty(mAdminTicket))
            {
                WebAuthService.WebAuthService endpoint = new WebAuthService.WebAuthService();
                mAdminTicket = endpoint.Login("admin@localhost.com", "password");
            }

            return mAdminTicket;
        }

        public object GetServiceQueryOptions(int page, int size)
        {
            try
            {
                string assembly = mEndPoint.GetType().Assembly.GetName().Name;
                string type = mEndPoint.GetType().Name;
                if (type.EndsWith("NoCache")) type = mEndPoint.GetType().BaseType.Name;

                object options = Activator.CreateInstance(mEndPoint.GetType().Assembly.GetType(
                    string.Format("{0}.{1}.ServiceQueryOptions", assembly, type)));

                object[] pagenumber_args = { page };
                options.GetType().InvokeMember("PageNumber", BindingFlags.SetProperty, null, options, pagenumber_args);

                object[] pagesize_args = { size };
                options.GetType().InvokeMember("PageSize", BindingFlags.SetProperty, null, options, pagesize_args);

                return options;
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }
    }
}