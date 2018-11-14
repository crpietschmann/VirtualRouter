/*
* Virtual Router v1.0 - http://virtualrouter.codeplex.com
* Wifi Hot Spot for Windows 8, 7 and 2008 R2
* Copyright (c) 2013 Chris Pietschmann (http://pietschsoft.com)
* Licensed under the Microsoft Public License (Ms-PL)
* http://virtualrouter.codeplex.com/license
*/
using System;
using System.Threading;
using System.Windows;
using System.Diagnostics;
using System.Collections.Generic;

namespace VirtualRouterClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Thread threadServiceChecker;

        public void Activate()
        {
            this.MainWindow.WindowState = WindowState.Normal;
            this.MainWindow.Activate();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.ConnectService();

            this.threadServiceChecker = new Thread(new ThreadStart(this.ServiceChecker));
            this.threadServiceChecker.Start();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            this.threadServiceChecker.Abort();
            base.OnExit(e);
        }

        private void ServiceChecker()
        {
            while (true)
            {
                if (this._VirtualRouter.State == System.ServiceModel.CommunicationState.Faulted)
                {
                    this._VirtualRouter = null;
                }

                if (this._VirtualRouter == null)
                {
                    this.ConnectService();
                }

                Thread.Sleep(5000);
            }
        }

        private void ConnectService()
        {
            if (this._VirtualRouter == null)
            {
                this._VirtualRouter = new VirtualRouterClient.VirtualRouterService.VirtualRouterHostClient();
                this._VirtualRouter.InnerChannel.Faulted += new EventHandler(InnerChannel_Faulted);
                this._VirtualRouter.InnerChannel.Closed += new EventHandler(InnerChannel_Closed);
                this._VirtualRouter.InnerChannel.Opened += new EventHandler(InnerChannel_Opened);
                try
                {
                    this._VirtualRouter.Open();
                }
                catch { }
            }
        }

        private VirtualRouterService.VirtualRouterHostClient _VirtualRouter;
        public VirtualRouterService.VirtualRouterHostClient VirtualRouter
        {
            get
            {
                return this._VirtualRouter;
            }
        }

        public event EventHandler VirtualRouterServiceConnected;
        public event EventHandler VirtualRouterServiceDisconnected;

        private void InvokeVirtualRouterServiceDisconnected()
        {
            if (this.VirtualRouterServiceDisconnected != null)
            {
                this.Dispatcher.Invoke(this.VirtualRouterServiceDisconnected, this, EventArgs.Empty);
            }
        }

        private void InnerChannel_Opened(object sender, EventArgs e)
        {
            this._IsVirtualRouterServiceConnected = true;
            if (this.VirtualRouterServiceConnected != null)
            {
                this.Dispatcher.Invoke(this.VirtualRouterServiceConnected, this, EventArgs.Empty);
            }
        }

        private void InnerChannel_Closed(object sender, EventArgs e)
        {
            this._IsVirtualRouterServiceConnected = false;
            InvokeVirtualRouterServiceDisconnected();
        }

        private void InnerChannel_Faulted(object sender, EventArgs e)
        {
            this._IsVirtualRouterServiceConnected = false;
            InvokeVirtualRouterServiceDisconnected();
        }

        private bool _IsVirtualRouterServiceConnected = false;
        public bool IsVirtualRouterServiceConnected
        {
            get
            {
                return this._IsVirtualRouterServiceConnected;
            }
        }
    }
}
