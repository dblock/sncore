using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
using SnCore.BackEndServices;

namespace SnCore.BackEnd.Service
{
    [RunInstaller(true)]
    public partial class ServiceInstaller : Installer
    {
        struct ServiceNameDescription
        {
            public string name;
            public string description;
            public ServiceNameDescription(string _name, string _description)
            {
                name = _name;
                description = _description;
            }
        };

        public ServiceInstaller()
        {
            ServiceNameDescription[] services = {
                new ServiceNameDescription( SystemMailMessageService.Name, SystemMailMessageService.Description ),
                new ServiceNameDescription( SystemReminderService.Name, SystemReminderService.Description ),
                new ServiceNameDescription( SystemSMTPMessageService.Name, SystemSMTPMessageService.Description ),
                new ServiceNameDescription( SystemTagWordService.Name, SystemTagWordService.Description ),
                new ServiceNameDescription( SystemSyndicationService.Name, SystemSyndicationService.Description ),
                new ServiceNameDescription( SystemThumbnailService.Name, SystemThumbnailService.Description )
            };

            foreach (ServiceNameDescription service in services)
            {
                System.ServiceProcess.ServiceInstaller serviceInstaller = new System.ServiceProcess.ServiceInstaller();
                serviceInstaller.ServiceName = service.name;
                serviceInstaller.DisplayName = service.name;
                serviceInstaller.Description = service.description;
                serviceInstaller.StartType = ServiceStartMode.Automatic;

                ServiceProcessInstaller serviceProcessInstaller = new ServiceProcessInstaller();
                serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
                serviceProcessInstaller.Username = null;
                serviceProcessInstaller.Password = null;
                serviceProcessInstaller.Installers.Add(serviceInstaller);

                Installers.Add(serviceProcessInstaller);
            }
        }
    }
}