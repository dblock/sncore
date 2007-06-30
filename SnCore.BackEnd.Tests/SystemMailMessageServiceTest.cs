using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SnCore.BackEndServices;
using SnCore.Data.Tests;
using SnCore.Services;
using NHibernate.Expression;

namespace SnCore.BackEnd.Tests
{
    [TestFixture]
    public class SystemMailMessageServicesTest : NHibernateTest
    {
        private SystemMailMessageService service = null;

        [SetUp]
        public override void SetUp()
        {
            service = new SystemMailMessageService();
            base.SetUp();
        }

        [Test]
        public void TestEmailQueue()
        {
            IList<AccountEmailMessage> unprocessed_queue = Session.CreateCriteria(typeof(AccountEmailMessage))
                .List<AccountEmailMessage>();

            Console.WriteLine("Unprocessed queue: {0} elements", unprocessed_queue.Count);
            service.RunEmailQueue(Session, ManagedAccount.GetAdminSecurityContext(Session));

            IList<AccountEmailMessage> processed_queue = Session.CreateCriteria(typeof(AccountEmailMessage))
                .List<AccountEmailMessage>();

            Console.WriteLine("Processed queue: {0} elements", processed_queue.Count);

            IList<AccountEmailMessage> error_queue = Session.CreateCriteria(typeof(AccountEmailMessage))
                .Add(Expression.IsNotNull("SendError"))
                .List<AccountEmailMessage>();

            Console.WriteLine("Error queue: {0} elements", error_queue.Count);
            foreach (AccountEmailMessage message in error_queue)
            {
                Console.WriteLine("{0} - {1}: {2}", message.Id, message.MailTo, message.SendError);
            }
        }

        [Test]
        public void TestMarketingCampaign()
        {
            service.RunMarketingCampaign(Session, ManagedAccount.GetAdminSecurityContext(Session));
        }
    }
}
