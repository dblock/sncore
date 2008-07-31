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

        private SystemService[] GetServices(string[] services)
        {
            List<SystemService> result = new List<SystemService>();
            foreach(SystemService service in Services)
            {
                if (services == null 
                    || Array.IndexOf<string>(services, service.ServiceName) >= 0
                    || Array.IndexOf<string>(services, service.GetType().Name) >= 0)
                    result.Add(service);
            }
            return result.ToArray();
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

        public void RunOnConsole(string[] services)
        {
            SnCore.Data.Hibernate.Session.Initialize(false);

            foreach (string service in services)
            {
                Console.WriteLine("Initializing {0} ...", service);
            }

            SystemService[] runservices = GetServices(services);

            if (runservices == null || runservices.Length == 0)
                throw new Exception("No Services to Run");

            foreach (SystemService service in runservices)
            {
                Console.WriteLine("Starting {0} ...", service.ServiceName);
                service.Start();
            }

            foreach (SystemService service in runservices)
            {
                service.Wait();
                Console.WriteLine("Service {0} stopped.", service.ServiceName);
            }
        }

        public void RunAsService(string[] services)
        {
            SnCore.Data.Hibernate.Session.Initialize(false);
            ServiceBase.Run(GetServices(services));
        }
    }
}
