using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
using SnCore.BackEndServices;
using Microsoft.Win32;

namespace SnCore.BackEnd.Service
{
    public class SnCoreServiceProcessInstaller : ServiceProcessInstaller
    {
        private string mServiceName;

        public string ServiceName
        {
            get
            {
                return mServiceName;
            }
        }

        public SnCoreServiceProcessInstaller(string name)
        {
            mServiceName = name;
        }

        public override void Install(System.Collections.IDictionary stateSaver)
        {
            base.Install(stateSaver);

            RegistryKey system = Registry.LocalMachine.OpenSubKey("System");
            RegistryKey currentControlSet = system.OpenSubKey("CurrentControlSet");
            RegistryKey services = currentControlSet.OpenSubKey("Services");
            RegistryKey service = services.OpenSubKey(ServiceName, true);
            RegistryKey config = service.CreateSubKey("Parameters");
            string args = string.Format("/s:\"{0}\"", ServiceName);
            config.SetValue("Arguments", args);
            string path = string.Format("{0} {1}", service.GetValue("ImagePath"), args);
            service.SetValue("ImagePath", path);
        }
    }


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

                SnCoreServiceProcessInstaller serviceProcessInstaller = new SnCoreServiceProcessInstaller(service.name);
                serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
                serviceProcessInstaller.Username = null;
                serviceProcessInstaller.Password = null;
                serviceProcessInstaller.Installers.Add(serviceInstaller);

                Installers.Add(serviceProcessInstaller);
            }
        }
    }
}