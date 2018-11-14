/*
* Virtual Router v1.0 - http://virtualrouter.codeplex.com
* Wifi Hot Spot for Windows 8, 7 and 2008 R2
* Copyright (c) 2013 Chris Pietschmann (http://pietschsoft.com)
* Licensed under the Microsoft Public License (Ms-PL)
* http://virtualrouter.codeplex.com/license
*/

using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace VirtualRouterClient
{
    [RunInstaller(true)]
    public class VirtualRouterClientInstaller : Installer
    {
        public VirtualRouterClientInstaller()
        {
            //this.Committed += new InstallEventHandler(VirtualRouterClientInstaller_Committed);
            this.AfterInstall += new InstallEventHandler(VirtualRouterClientInstaller_AfterInstall);
        }

        void VirtualRouterClientInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            try
            {
                Directory.SetCurrentDirectory(Path.GetDirectoryName
                (Assembly.GetExecutingAssembly().Location));
                Process.Start(Path.GetDirectoryName(
                  Assembly.GetExecutingAssembly().Location) + "\\VirtualRouterClient.exe");
            }
            catch
            {
                // Do nothing... 
            }
        }

        //void VirtualRouterClientInstaller_Committed(object sender, InstallEventArgs e)
        //{
        //}

        public override void Install(System.Collections.IDictionary stateSaver)
        {
            base.Install(stateSaver);
        }

        public override void Commit(System.Collections.IDictionary savedState)
        {
            base.Commit(savedState);
        }

        public override void Rollback(System.Collections.IDictionary savedState)
        {
            base.Rollback(savedState);
        }
    }
}
