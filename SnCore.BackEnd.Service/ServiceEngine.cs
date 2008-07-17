using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceProcess;
using SnCore.BackEndServices;
using System.Threading;

namespace SnCore.BackEnd.Service
{
    public class ServiceEngine
    {
        private static SystemService[] CreateServices()
        {
            return new SystemService[] 
            { 
                new SystemMailMessageService(),
                new SystemTagWordService(),
                new SystemReminderService(),
                new SystemSyndicationService(),
                new SystemThumbnailService(),
                new SystemSMTPMessageService(),
            };
        }

        private SystemService[] _services = null;

        private SystemService[] Services
        {
            get
            {
                if (_services == null)
                {
                    _services = CreateServices();
                }

                return _services;
            }
        }

        public ServiceEngine()
        {

        }

        public void StopOnConsole()
        {
            // set stopping for graceful stop
            foreach (SystemService service in Services)
            {
                service.IsStopping = true;
            }

            // force terminate services
            foreach (SystemService service in Services)
            {
                Console.WriteLine("Stopping {0} ...", service.ServiceName);
                service.Stop();
            }
        }

        public void RunOnConsole()
        {
            SnCore.Data.Hibernate.Session.Initialize(false);
            foreach (SystemService service in Services)
            {
                Console.WriteLine("Starting {0} ...", service.ServiceName);
                service.Start();
            }

            foreach (SystemService service in Services)
            {
                service.Wait();
                Console.WriteLine("Service {0} stopped.", service.ServiceName);
            }
        }

        public void RunAsService()
        {
            SnCore.Data.Hibernate.Session.Initialize(false);            
            ServiceBase.Run(Services);
        }
    }
}
