/*
* Virtual Router v1.0 - http://virtualrouter.codeplex.com
* Wifi Hot Spot for Windows 8, 7 and 2008 R2
* Copyright (c) 2013 Chris Pietschmann (http://pietschsoft.com)
* Licensed under the Microsoft Public License (Ms-PL)
* http://virtualrouter.codeplex.com/license
*/
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceProcess;
using VirtualRouterHost;

namespace VirtualRouterService
{
    public partial class VirtualRouterWindowsService : ServiceBase
    {
        private ServiceHost serviceHost = null;
        private VirtualRouterHost.VirtualRouterHost virtualRouterHost = null;

        public VirtualRouterWindowsService()
        {
            InitializeComponent();

            this.AutoLog = true;
            this.CanShutdown = true;
            this.CanPauseAndContinue = true;
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                if (this.serviceHost != null)
                {
                    this.serviceHost.Close();
                }

                this.virtualRouterHost = new VirtualRouterHost.VirtualRouterHost();
                this.serviceHost = new ServiceHost(this.virtualRouterHost);

                LoadSavedState();
                
                if (this.serviceHost.State != CommunicationState.Opened)
                {
                    this.serviceHost.Open();
                }
            }
            catch (Exception ex)
            {
                WriteLog("Error Starting Service \n" + ex.ToString());
            }
        }

        protected override void OnStop()
        {
            if (virtualRouterHost != null)
            {
                SaveState();
                this.virtualRouterHost.Stop();
            }

            if (this.serviceHost != null)
            {
                this.serviceHost.Close();
                this.serviceHost = null;
            }
        }

        protected override void OnShutdown()
        {
            this.OnStop();
        }

        protected override void OnPause()
        {
            this.OnShutdown();
        }

        protected override void OnContinue()
        {
            this.OnStart(new string[0]);
        }

        string strStateFileName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory , "VirtualRouterService.savedstate");

        void LoadSavedState()
        {
            try
            {
                if (File.Exists(strStateFileName))
                {
                    VirtualRouterServiceState state;
                    using (var file = new StreamReader(strStateFileName))
                    {
                        var serializer = new DataContractSerializer(typeof(VirtualRouterServiceState));
                        state = (VirtualRouterServiceState)serializer.ReadObject(file.BaseStream);
                        file.Close();
                    }

                    if (state != null)
                    {
                        if (state.IsStarted)
                        {
                            // Set SSID and Password
                            this.virtualRouterHost.SetConnectionSettings(state.SSID, state.MaxPeerCount);
                            this.virtualRouterHost.SetPassword(state.Password);

                            // Get the Shared Connection
                            var conns = this.virtualRouterHost.GetSharableConnections();


                            SharableConnection sharedConnection = null;
                            if (!string.IsNullOrEmpty(state.SharedConnectionGuid))
                            {
                                sharedConnection = (from c in conns
                                                    where c.Guid.ToString() == new Guid(state.SharedConnectionGuid).ToString()
                                                    select c).FirstOrDefault();
                                if (sharedConnection == null)
                                {
                                    sharedConnection = conns.First();
                                }
                            }

                            this.virtualRouterHost.Start(sharedConnection);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog("Error Loading Service State - " + ex.ToString());
            }
        }

        void SaveState()
        {
#if DEBUG
            WriteLog("Start Saving State");
#endif

            StreamWriter sw = null;
            try
            {
                var state = new VirtualRouterServiceState();
                state.IsStarted = this.virtualRouterHost.IsStarted();

                var conn = this.virtualRouterHost.GetSharedConnection();
                if (conn != null)
                {
                    if (conn.Guid != null)
                    {
                        state.SharedConnectionGuid = this.virtualRouterHost.GetSharedConnection().Guid.ToString();
                    }
                }

                var connSettings = this.virtualRouterHost.GetConnectionSettings();
                state.SSID = connSettings.SSID;
                state.MaxPeerCount = connSettings.MaxPeerCount;

                state.Password = this.virtualRouterHost.GetPassword();



                sw = new StreamWriter(new FileStream(strStateFileName, FileMode.Create));
                var stream = sw.BaseStream;

                var serializer = new DataContractSerializer(typeof(VirtualRouterServiceState));
                serializer.WriteObject(stream, state);
            }
            catch (Exception ex)
            {
                WriteLog("Error Saving Service State - " + ex.ToString());
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                    sw.Dispose();
                }
            }
#if DEBUG
            WriteLog("State Saved");
#endif
        }

        private void WriteLog(string message)
        {
            EventLog.WriteEntry("Virtual Router Service", message, EventLogEntryType.Information);
        }
    }
}
