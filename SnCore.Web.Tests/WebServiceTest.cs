using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace SnCore.Web.Tests
{
    public class UserInfo
    {
        public int id;
        public string email;
        public string password;
        public string ticket;
        public SnCore.WebServices.WebAccountService.TransitAccount account;

        public UserInfo()
        {

        }
    };

    public abstract class WebServiceTest
    {
        protected UserInfo CreateUserWithVerifiedEmailAddress()
        {
            string email = GetNewEmailAddress();
            string password = GetNewString();
            return CreateUserWithVerifiedEmailAddress(email, password);
        }

        protected UserInfo CreateUserWithVerifiedEmailAddress(string email, string password)
        {
            SnCore.WebServices.WebAccountService.WebAccountService endpoint = new SnCore.WebServices.WebAccountService.WebAccountService();

            UserInfo result = new UserInfo();
            result.email = email;
            result.password = password;

            result.id = CreateUser(email, password);
            Assert.IsTrue(result.id > 0);

            result.ticket = Login(email, password);
            Assert.IsNotEmpty(result.ticket);

            Assert.IsFalse(endpoint.HasVerifiedEmail(result.ticket, result.id));
            SnCore.WebServices.WebAccountService.TransitAccountEmailConfirmation[] confirmations = endpoint.GetAccountEmailConfirmations(
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
            SnCore.WebServices.WebAccountService.TransitAccount t_instance = new SnCore.WebServices.WebAccountService.TransitAccount();
            t_instance.Name = GetNewString();
            t_instance.Password = password;
            t_instance.Birthday = dateofbirth;
            SnCore.WebServices.WebAccountService.WebAccountService account_endpoint = new SnCore.WebServices.WebAccountService.WebAccountService();
            int id = account_endpoint.CreateAccount(string.Empty, email, t_instance);
            Console.WriteLine("Created user: {0}", id);
            Assert.IsTrue(id > 0);
            return id;
        }

        protected void DeleteUser(int id)
        {
            SnCore.WebServices.WebAccountService.WebAccountService account_endpoint = new SnCore.WebServices.WebAccountService.WebAccountService();
            account_endpoint.DeleteAccount(GetAdminTicket(), id);
        }

        public string Login(string email, string password)
        {
            SnCore.WebServices.WebAccountService.WebAccountService endpoint = new SnCore.WebServices.WebAccountService.WebAccountService();
            return endpoint.Login(email, password);
        }

        public SnCore.WebServices.WebAccountService.TransitAccount GetAdminAccount()
        {
            SnCore.WebServices.WebAccountService.WebAccountService endpoint = new SnCore.WebServices.WebAccountService.WebAccountService();
            return endpoint.GetAccount(GetAdminTicket(), true);
        }

        public string GetAdminTicket()
        {
            SnCore.WebServices.WebAccountService.WebAccountService endpoint = new SnCore.WebServices.WebAccountService.WebAccountService();
            return endpoint.Login("admin@localhost.com", "password");
        }

        public static string GetNewEmailAddress()
        {
            return string.Format("{0}@localhost.com", GetNewString());
        }

        public static string GetNewUri()
        {
            return string.Format("http://uri/{0}", GetNewString());
        }

        public static string GetNewString()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
