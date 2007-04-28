using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Services.Protocols;
using System.Reflection;
using NUnit.Framework;
using SnCore.Tools.Drawing;

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
            return CreateUser(email, password, DateTime.UtcNow.AddYears(-10));
        }

        protected int CreateUser(string email, string password, DateTime dateofbirth)
        {
            WebAccountService.TransitAccount t_instance = new WebAccountService.TransitAccount();
            t_instance.Name = GetNewString();
            t_instance.Password = password;
            t_instance.Birthday = dateofbirth;
            WebAccountService.WebAccountService account_endpoint = new WebAccountService.WebAccountService();
            int id = account_endpoint.CreateAccount(string.Empty, email, t_instance);
            Console.WriteLine("Created user: {0}", id);
            Assert.IsTrue(id > 0);
            return id;
        }

        protected void DeleteUser(int id)
        {
            WebAccountService.WebAccountService account_endpoint = new WebAccountService.WebAccountService();
            account_endpoint.DeleteAccount(GetAdminTicket(), id, null);
        }

        private void CreateUserAccount()
        {
            CreateUser("user@localhost.com", "password");
        }

        public string Login(string email, string password)
        {
            WebAccountService.WebAccountService endpoint = new WebAccountService.WebAccountService();
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

        public WebAccountService.TransitAccount GetUserAccount()
        {
            WebAccountService.WebAccountService endpoint = new WebAccountService.WebAccountService();
            return endpoint.GetAccount(GetUserTicket(), true);
        }

        public WebAccountService.TransitAccount GetAdminAccount()
        {
            WebAccountService.WebAccountService endpoint = new WebAccountService.WebAccountService();
            return endpoint.GetAccount(GetAdminTicket(), true);
        }

        public string GetAdminTicket()
        {
            if (string.IsNullOrEmpty(mAdminTicket))
            {
                WebAccountService.WebAccountService endpoint = new WebAccountService.WebAccountService();
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

        public void CreateUserWithVerifiedEmail(out WebAccountService.TransitAccount t_account, out string ticket)
        {
            WebAccountService.WebAccountService endpoint = new WebAccountService.WebAccountService();
            string email = GetNewEmailAddress();
            string password = GetNewString();
            int userid = CreateUser(email, password);
            ticket = Login(email, password);
            t_account = endpoint.GetAccountById(ticket, userid);
            WebAccountService.TransitAccountEmailConfirmation[] confirmations = endpoint.GetAccountEmailConfirmations(
                GetAdminTicket(), userid, null);
            Assert.AreEqual(confirmations.Length, 1);
            string verifiedemail = endpoint.VerifyAccountEmail(password, confirmations[0].Id, confirmations[0].Code);
            Console.WriteLine("Verified e-mail: {0}", verifiedemail);
            Assert.AreEqual(verifiedemail, email);
        }

        public static string GetNewEmailAddress()
        {
            return string.Format("{0}@localhost.com", GetNewString());
        }

        public static string GetNewUri()
        {
            return string.Format("http://uri/{0}", GetNewString());
        }

        public static byte[] GetNewBitmap()
        {
            return ThumbnailBitmap.GetBitmapDataFromText(GetNewString(), 12, 240, 100);
        }

        public static string GetNewString()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
