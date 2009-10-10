using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using NUnit.Framework;
using SnCore.Tools.Drawing;

namespace SnCore.Web.Soap.Tests
{
    public class UserInfo
    {
        public int id;
        public string email;
        public string password;
        public string ticket;
        public WebAccountService.TransitAccount account;

        public UserInfo()
        {

        }
    };

    public class WebServiceBaseTest<EndPointType>
        where EndPointType : new()
    {
        private EndPointType mEndPoint;
        private string mAdminTicket = string.Empty;

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

        protected UserInfo CreateUserWithVerifiedEmailAddress()
        {
            Console.WriteLine("CreateUserWithVerifiedEmailAddress");
            string email = GetNewEmailAddress();
            string password = GetNewString();
            return CreateUserWithVerifiedEmailAddress(email, password);
        }

        protected UserInfo CreateUserWithVerifiedEmailAddress(string email, string password)
        {
            WebAccountService.WebAccountService endpoint = new WebAccountService.WebAccountService();

            UserInfo result = new UserInfo();
            result.email = email;
            result.password = password;

            result.id = CreateUser(email, password);
            Assert.IsTrue(result.id > 0);

            result.ticket = Login(email, password);
            Assert.IsNotEmpty(result.ticket);

            Assert.IsFalse(endpoint.HasVerifiedEmail(result.ticket, result.id));
            WebAccountService.TransitAccountEmailConfirmation[] confirmations = endpoint.GetAccountEmailConfirmations(
                GetAdminTicket(), result.id, null);

            string verifiedemail = endpoint.VerifyAccountEmail(
                confirmations[0].Id, confirmations[0].Code);
            
            Console.WriteLine("Verified: {0}", verifiedemail);
            Assert.AreEqual(verifiedemail, result.email);
            Assert.IsTrue(endpoint.HasVerifiedEmail(result.ticket, result.id));

            return result;
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
            account_endpoint.DeleteAccount(GetAdminTicket(), id);
        }

        private void CreateUserAccount()
        {
            CreateUserWithVerifiedEmailAddress("user@localhost.com", "password");
        }

        public string Login(string email, string password)
        {
            WebAccountService.WebAccountService endpoint = new WebAccountService.WebAccountService();
            return endpoint.Login(email, password);
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
