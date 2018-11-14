/*
* Virtual Router v1.0 - http://virtualrouter.codeplex.com
* Wifi Hot Spot for Windows 8, 7 and 2008 R2
* Copyright (c) 2013 Chris Pietschmann (http://pietschsoft.com)
* Licensed under the Microsoft Public License (Ms-PL)
* http://virtualrouter.codeplex.com/license
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualRouter.Wlan;
using IcsMgr;
using System.ServiceModel;
using VirtualRouter.Wlan.WinAPI;
using System.Threading;

namespace VirtualRouterHost
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class VirtualRouterHost : IVirtualRouterHost
    {
        private WlanManager wlanManager;
        private IcsManager icsManager;

        private SharableConnection currentSharedConnection;

        public VirtualRouterHost()
        {
            this.wlanManager = new WlanManager();
            this.icsManager = new IcsManager();
        }

        #region IVirtualRouterHost Members

        private string _lastErrorMessage;
        public string GetLastError()
        {
            return this._lastErrorMessage;
        }
        
        public bool Start(SharableConnection sharedConnection)
        {
            try
            {
                this.Stop();

                this.wlanManager.StartHostedNetwork();

                Thread.Sleep(1000);

                if (sharedConnection != null)
                {
                    if (sharedConnection.Guid != Guid.Empty)
                    {
                        if (this.icsManager.SharingInstalled)
                        {
                            var privateConnectionGuid = this.wlanManager.HostedNetworkInterfaceGuid;

                            if (privateConnectionGuid == Guid.Empty)
                            {
                                // If the GUID for the Hosted Network Adapter isn't return properly,
                                // then retrieve it by the DeviceName.

                                privateConnectionGuid = (from c in this.icsManager.Connections
                                                         where c.props.DeviceName.ToLowerInvariant().Contains("microsoft virtual wifi miniport adapter") // Windows 7
                                                         || c.props.DeviceName.ToLowerInvariant().Contains("microsoft hosted network virtual adapter") // Windows 8
                                                         select c.Guid).FirstOrDefault();
                                // Note: For some reason the DeviceName can have different names, currently it checks for the ones that I have identified thus far.

                                if (privateConnectionGuid == Guid.Empty)
                                {
                                    // Device still now found, so throw exception so the message gets raised up to the client.
                                    throw new Exception("Virtual Wifi device not found!\n\nNeither \"Microsoft Hosted Network Virtual Adapter\" or \"Microsoft Virtual Wifi Miniport Adapter\" were found.");
                                }
                            }

                            this.icsManager.EnableIcs(sharedConnection.Guid, privateConnectionGuid);

                            this.currentSharedConnection = sharedConnection;
                        }
                    }
                }
                else
                {
                    this.currentSharedConnection = null;
                }

                

                return true;
            }
            catch(Exception ex)
            {
                this._lastErrorMessage = ex.Message;
                return false;
            }
        }

        public bool Stop()
        {
            try
            {
                if (this.icsManager.SharingInstalled)
                {
                    this.icsManager.DisableIcsOnAll();
                }

                this.wlanManager.StopHostedNetwork();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SetConnectionSettings(string ssid, int maxNumberOfPeers)
        {
            try
            {
                this.wlanManager.SetConnectionSettings(ssid, maxNumberOfPeers);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public ConnectionSettings GetConnectionSettings()
        {
            try
            {
                string ssid;
                int maxNumberOfPeers;

                var r = this.wlanManager.QueryConnectionSettings(out ssid, out maxNumberOfPeers);

                return new ConnectionSettings()
                {
                    SSID = ssid,
                    MaxPeerCount = maxNumberOfPeers
                };
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<SharableConnection> GetSharableConnections()
        {
            List<IcsConnection> connections;
            try
            {
                connections = this.icsManager.Connections;
            }
            catch
            {
                connections = new List<IcsConnection>();
            }

            // Empty item to signify No Connection Sharing
            yield return new SharableConnection() { DeviceName = "None", Guid = Guid.Empty, Name = "None" };

            if (connections != null)
            {
                foreach (var conn in connections)
                {
                    if (conn.IsConnected && conn.IsSupported)
                    {
                        yield return new SharableConnection(conn);
                    }
                }
            }
        }

        public bool SetPassword(string password)
        {
            try
            {
                this.wlanManager.SetSecondaryKey(password);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetPassword()
        {
            try
            {
                string passKey = string.Empty;
                bool isPassPhrase;
                bool isPersistent;

                var r = this.wlanManager.QuerySecondaryKey(out passKey, out isPassPhrase, out isPersistent);

                return passKey;
            }
            catch
            {
                return null;
            }
        }

        public bool IsStarted()
        {
            try
            {
                return wlanManager.IsHostedNetworkStarted;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<ConnectedPeer> GetConnectedPeers()
        {
            foreach (var v in wlanManager.Stations)
            {
                yield return new ConnectedPeer(v.Value);
            }
        }

        public SharableConnection GetSharedConnection()
        {
            return this.currentSharedConnection;
        }

        #endregion
    }
}
