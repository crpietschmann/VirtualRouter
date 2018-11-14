/*
* Virtual Router v1.0 - http://virtualrouter.codeplex.com
* Wifi Hot Spot for Windows 8, 7 and 2008 R2
* Copyright (c) 2013 Chris Pietschmann (http://pietschsoft.com)
* Licensed under the Microsoft Public License (Ms-PL)
* http://virtualrouter.codeplex.com/license
*/
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace VirtualRouterService
{
    [RunInstaller(true)]
    public class VirtualRouterWindowsServiceInstaller : Installer
    {
        string strServiceName = "Virtual Router";

        public VirtualRouterWindowsServiceInstaller()
        {
            var processInstaller = new ServiceProcessInstaller();
            var serviceInstaller = new ServiceInstaller();

            processInstaller.Account = ServiceAccount.LocalSystem;
            processInstaller.Username = null;
            processInstaller.Password = null;

            serviceInstaller.DisplayName = AssemblyAttributes.AssemblyTitle;
            serviceInstaller.Description = AssemblyAttributes.AssemblyDescription;
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            serviceInstaller.ServiceName = strServiceName;

            this.Installers.Add(processInstaller);
            this.Installers.Add(serviceInstaller);

            this.Committed += new InstallEventHandler(VirtualRouterWindowsServiceInstaller_Committed);
        }

        void VirtualRouterWindowsServiceInstaller_Committed(object sender, InstallEventArgs e)
        {
            // Auto Start the Service Once Installation is Finished.
            var controller = new ServiceController(strServiceName);
            controller.Start();
        }
    }
}
