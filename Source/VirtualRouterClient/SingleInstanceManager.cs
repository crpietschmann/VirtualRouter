/*
* Virtual Router v1.0 - http://virtualrouter.codeplex.com
* Wifi Hot Spot for Windows 8, 7 and 2008 R2
* Copyright (c) 2013 Chris Pietschmann (http://pietschsoft.com)
* Licensed under the Microsoft Public License (Ms-PL)
* http://virtualrouter.codeplex.com/license
*/
using Microsoft.VisualBasic.ApplicationServices;
 
namespace VirtualRouterClient
{
    public class SingleInstanceManager : WindowsFormsApplicationBase
    {
        App app;

        public SingleInstanceManager()
        {
            this.IsSingleInstance = true;
        }

        protected override bool OnStartup(Microsoft.VisualBasic.ApplicationServices.StartupEventArgs e)
        {
            // First time app is launched
            app = new App();
            app.InitializeComponent();
            app.Run();
            return false;
        }

        protected override void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
        {
            // Subsequent launches
            base.OnStartupNextInstance(eventArgs);
            app.Activate();
        }
    }
}
