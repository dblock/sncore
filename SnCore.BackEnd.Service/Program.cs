using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using SnCore.Data.Hibernate;
using SnCore.BackEndServices;

namespace SnCore.BackEnd.Service
{
    static class Program
    {
        static void Main()
        {
            // initialize nhibernate session, no http
            SnCore.Data.Hibernate.Session.Initialize(false);
            // start services
            ServiceBase[] services = new ServiceBase[] { 
                new SystemMailMessageService(),
                new SystemTagWordService(),
                new SystemReminderService(),
                new SystemSyndicationService(),
                new SystemThumbnailService(),
                new SystemSMTPMessageService(),
            };

            ServiceBase.Run(services);
        }
    }
}