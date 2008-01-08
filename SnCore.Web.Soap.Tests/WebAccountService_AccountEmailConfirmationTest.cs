using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountEmailConfirmationTest : WebServiceBaseTest<WebAccountServiceNoCache>
    {
        public AccountEmailConfirmationTest()
        {

        }

        [Test]
        public void CreateAccountEmailTest()
        {
            // create a user
            string email = GetNewEmailAddress();
            string password = GetNewString();
            int user_id = CreateUser(email, password);
            Assert.IsTrue(user_id > 0);
            string ticket = Login(email, password);
            Assert.IsNotEmpty(ticket);
            // check that the e-mail is part of e-mails
            WebAccountService.TransitAccountEmail[] emails = EndPoint.GetAccountEmails(ticket, user_id, null);
            Assert.AreEqual(1, emails.Length);
            Assert.AreEqual(email, emails[0].Address, "E-mail addresses don't match.");
            Assert.IsFalse(emails[0].Verified, "E-mail address hasn't been verified.");

            // make sure the user himself can't see the e-mail confirmations
            try
            {
                int invalid_count = EndPoint.GetAccountEmailConfirmationsCount(ticket, user_id);
                Console.WriteLine("Confirmations count: {0}", invalid_count);
                WebAccountService.TransitAccountEmailConfirmation[] invalid_confirmations = EndPoint.GetAccountEmailConfirmations(ticket, user_id, null);
                Assert.IsTrue(false, "User shouldn't be able to see e-mail confirmations.");
            }
            catch (Exception)
            {
            }

            // find the e-mail confirmation
            int count = EndPoint.GetAccountEmailConfirmationsCount(GetAdminTicket(), user_id);
            Console.WriteLine("Confirmations count: {0}", count);
            Assert.AreEqual(count, 1);

            // find the e-mail confirmation
            WebAccountService.TransitAccountEmailConfirmation[] confirmations = EndPoint.GetAccountEmailConfirmations(GetAdminTicket(), user_id, null);
            Assert.AreEqual(confirmations.Length, 1);

            // verify the e-mail
            string verifiedemail = EndPoint.VerifyAccountEmail(confirmations[0].Id, confirmations[0].Code);
            Console.WriteLine("Verified e-mail: {0}", verifiedemail);
            Assert.AreEqual(verifiedemail, email);

            // check verified e-mail
            WebAccountService.TransitAccountEmail[] emails2 = EndPoint.GetAccountEmails(ticket, user_id, null);
            Assert.AreEqual(1, emails2.Length);
            Assert.AreEqual(email, emails2[0].Address, "E-mail addresses don't match.");
            Assert.IsTrue(emails2[0].Verified, "E-mail address hasn't been verified.");

            // check that the confirmation was deleted
            int count2 = EndPoint.GetAccountEmailConfirmationsCount(GetAdminTicket(), user_id);
            Console.WriteLine("Confirmations count: {0}", count2);
            Assert.AreEqual(count2, 0);

            DeleteUser(user_id);
        }

        [Test]
        public void VerifyEmailTest()
        {
            // create a user
            string email = GetNewEmailAddress();
            string password = GetNewString();
            int user_id = CreateUser(email, password);
            Assert.IsTrue(user_id > 0);
            string ticket = Login(email, password);
            Assert.IsNotEmpty(ticket);
            // verify e-mail
            WebAccountService.TransitAccountEmailConfirmation[] confirmations = EndPoint.GetAccountEmailConfirmations(GetAdminTicket(), user_id, null);
            string verifiedemail = EndPoint.VerifyAccountEmail(confirmations[0].Id, confirmations[0].Code);
            Console.WriteLine("Verified e-mail: {0}", verifiedemail);
            Assert.AreEqual(verifiedemail, email);
            // add an e-mail
            WebAccountService.TransitAccountEmail t_instance = new WebAccountService.TransitAccountEmail();
            t_instance.Address = GetNewEmailAddress();
            t_instance.Id = EndPoint.CreateOrUpdateAccountEmail(ticket, t_instance);
            Assert.IsTrue(t_instance.Id > 0);
            // verify e-mail
            WebAccountService.TransitAccountEmailConfirmation[] confirmations2 = EndPoint.GetAccountEmailConfirmations(GetAdminTicket(), user_id, null);
            string verifiedemail2 = EndPoint.VerifyAccountEmail(confirmations2[0].Id, confirmations2[0].Code);
            Assert.AreEqual(verifiedemail2, t_instance.Address);
            Console.WriteLine("Verified e-mail: {0}", verifiedemail2);
            DeleteUser(user_id);
        }

        [Test]
        public void SetPrincipalTest()
        {
            // create a user
            string email = GetNewEmailAddress();
            string password = GetNewString();
            int user_id = CreateUser(email, password);
            Assert.IsTrue(user_id > 0);
            string ticket = Login(email, password);
            Assert.IsNotEmpty(ticket);
            // verify e-mail
            WebAccountService.TransitAccountEmailConfirmation[] confirmations = EndPoint.GetAccountEmailConfirmations(GetAdminTicket(), user_id, null);
            string verifiedemail = EndPoint.VerifyAccountEmail(confirmations[0].Id, confirmations[0].Code);
            Console.WriteLine("Verified e-mail: {0}", verifiedemail);
            Assert.AreEqual(verifiedemail, email);
            // add an e-mail
            WebAccountService.TransitAccountEmail t_instance = new WebAccountService.TransitAccountEmail();
            t_instance.Address = GetNewEmailAddress();
            t_instance.Id = EndPoint.CreateOrUpdateAccountEmail(ticket, t_instance);
            Assert.IsTrue(t_instance.Id > 0);
            // verify e-mail
            WebAccountService.TransitAccountEmailConfirmation[] confirmations2 = EndPoint.GetAccountEmailConfirmations(GetAdminTicket(), user_id, null);
            string verifiedemail2 = EndPoint.VerifyAccountEmail(confirmations2[0].Id, confirmations2[0].Code);
            Assert.AreEqual(verifiedemail2, t_instance.Address);
            Console.WriteLine("Verified e-mail: {0}", verifiedemail2);
            // set the second e-mail principal
            t_instance.Principal = true;
            EndPoint.CreateOrUpdateAccountEmail(ticket, t_instance);
            // verify that only one e-mail (this one) is principal
            WebAccountService.TransitAccountEmail[] emails = EndPoint.GetAccountEmails(ticket, user_id, null);
            Assert.AreEqual(emails.Length, 2);
            WebAccountService.TransitAccountEmail t_instance_first = null;
            foreach (WebAccountService.TransitAccountEmail instance in emails)
            {
                if (instance.Id == t_instance.Id)
                {
                    Assert.IsTrue(instance.Principal);
                }
                else
                {
                    Assert.IsFalse(instance.Principal);
                    t_instance_first = instance;
                }
            }
            Assert.IsNotNull(t_instance_first);
            // make the first e-mail principal
            t_instance_first.Principal = true;
            EndPoint.CreateOrUpdateAccountEmail(ticket, t_instance_first);
            // verify that the first one is principal
            WebAccountService.TransitAccountEmail[] emails2 = EndPoint.GetAccountEmails(ticket, user_id, null);
            Assert.AreEqual(emails2.Length, 2);
            foreach (WebAccountService.TransitAccountEmail instance in emails2)
            {
                if (instance.Id == t_instance.Id)
                    Assert.IsFalse(instance.Principal);
                else if (instance.Id == t_instance_first.Id)
                    Assert.IsTrue(instance.Principal);
                else
                    Assert.IsTrue(false, "Found an invalid e-mail."); 
            }

            DeleteUser(user_id);
        }

        [Test]
        public void ConfirmAccountEmailTest()
        {
            // create a user
            string email = GetNewEmailAddress();
            string password = GetNewString();
            int user_id = CreateUser(email, password);
            Assert.IsTrue(user_id > 0);
            string ticket = Login(email, password);
            Assert.IsNotEmpty(ticket);
            // get the e-mail that needs to be re-confirmed
            WebAccountService.TransitAccountEmail[] emails = EndPoint.GetAccountEmails(ticket, user_id, null);
            Console.WriteLine("Emails: {0}", emails.Length);
            Assert.AreEqual(1, emails.Length);
            Assert.AreEqual(emails[0].Address, email);
            // verify that there's one confirmation pending
            WebAccountService.TransitAccountEmailConfirmation[] confirmations1 = EndPoint.GetAccountEmailConfirmations(GetAdminTicket(), user_id, null);
            Console.WriteLine("Confirmations: {0}", confirmations1.Length);
            Assert.AreEqual(1, confirmations1.Length);
            Assert.AreEqual(confirmations1[0].AccountEmail.Address, email);
            // resend a confirmation
            EndPoint.ConfirmAccountEmail(ticket, emails[0].Id);
            // verify that there's still one confirmation pending
            WebAccountService.TransitAccountEmailConfirmation[] confirmations2 = EndPoint.GetAccountEmailConfirmations(GetAdminTicket(), user_id, null);
            Console.WriteLine("Confirmations: {0}", confirmations2.Length);
            Assert.AreEqual(1, confirmations2.Length);
            Assert.AreEqual(confirmations2[0].AccountEmail.Address, email);
            // add an e-mail
            WebAccountService.TransitAccountEmail t_email = new WebAccountService.TransitAccountEmail();
            t_email.AccountId = user_id;
            t_email.Address = GetNewEmailAddress();
            t_email.Id = EndPoint.CreateOrUpdateAccountEmail(ticket, t_email);
            // verify that there're two confirmations pending
            WebAccountService.TransitAccountEmailConfirmation[] confirmations3 = EndPoint.GetAccountEmailConfirmations(GetAdminTicket(), user_id, null);
            Console.WriteLine("Confirmations: {0}", confirmations3.Length);
            Assert.AreEqual(2, confirmations3.Length);
            // resend a confirmation
            EndPoint.ConfirmAccountEmail(ticket, emails[0].Id);
            // verify that there's still two confirmation pending
            WebAccountService.TransitAccountEmailConfirmation[] confirmations4 = EndPoint.GetAccountEmailConfirmations(GetAdminTicket(), user_id, null);
            Console.WriteLine("Confirmations: {0}", confirmations4.Length);
            Assert.AreEqual(2, confirmations4.Length);
            DeleteUser(user_id);
        }

        [Test]
        public void VerifyFailedEmailTest()
        {
            // create a user
            string email = GetNewEmailAddress();
            string password = GetNewString();
            int user_id = CreateUser(email, password);
            Assert.IsTrue(user_id > 0);
            string ticket = Login(email, password);
            Assert.IsNotEmpty(ticket);
            // verify e-mail
            WebAccountService.TransitAccountEmailConfirmation[] confirmations = EndPoint.GetAccountEmailConfirmations(GetAdminTicket(), user_id, null);
            string verifiedemail = EndPoint.VerifyAccountEmail(confirmations[0].Id, confirmations[0].Code);
            Console.WriteLine("Verified e-mail: {0}", verifiedemail);
            Assert.AreEqual(verifiedemail, email);
            // fail e-mail
            int email_id = confirmations[0].AccountEmail.Id;
            Console.WriteLine("Email id: {0}", email_id);
            WebAccountService.TransitAccountEmail t_email_before = EndPoint.GetAccountEmailById(ticket, email_id);
            Assert.IsFalse(t_email_before.Failed);
            Assert.IsTrue(string.IsNullOrEmpty(t_email_before.LastError));
            t_email_before.Failed = true;
            t_email_before.LastError = GetNewString();
            EndPoint.CreateOrUpdateAccountEmail(ticket, t_email_before);
            // verify that the e-mail was failed
            WebAccountService.TransitAccountEmail t_email_after = EndPoint.GetAccountEmailById(ticket, email_id);
            Console.WriteLine("Email failure: {0}", t_email_after.LastError);
            Assert.IsTrue(t_email_after.Failed);
            Assert.IsFalse(string.IsNullOrEmpty(t_email_after.LastError));
            Assert.AreEqual(t_email_after.LastError, t_email_before.LastError);
            // resend e-mail confirmation
            EndPoint.ConfirmAccountEmail(ticket, t_email_after.Id);
            // verify e-mail again
            WebAccountService.TransitAccountEmailConfirmation[] confirmations_after = EndPoint.GetAccountEmailConfirmations(GetAdminTicket(), user_id, null);
            string verifiedemail_after = EndPoint.VerifyAccountEmail(confirmations_after[0].Id, confirmations_after[0].Code);
            Console.WriteLine("Verified e-mail: {0}", verifiedemail_after);
            Assert.AreEqual(verifiedemail_after, email);
            // verify that failure was cleared
            WebAccountService.TransitAccountEmail t_email_final = EndPoint.GetAccountEmailById(ticket, email_id);
            Assert.IsFalse(t_email_final.Failed);
            Assert.IsTrue(string.IsNullOrEmpty(t_email_final.LastError));
            // delete user
            DeleteUser(user_id);
        }

    }
}
