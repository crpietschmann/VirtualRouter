/*
* Virtual Router v1.0 - http://virtualrouter.codeplex.com
* Wifi Hot Spot for Windows 8, 7 and 2008 R2
* Copyright (c) 2013 Chris Pietschmann (http://pietschsoft.com)
* Licensed under the Microsoft Public License (Ms-PL)
* http://virtualrouter.codeplex.com/license
*/
using System;
using System.Threading;
using System.Windows.Controls;
using VirtualRouterClient.VirtualRouterService;
using System.Windows.Media;

namespace VirtualRouterClient
{
    /// <summary>
    /// Interaction logic for PeerDevice.xaml
    /// </summary>
    public partial class PeerDevice : UserControl
    {
        Thread thread;

        public PeerDevice(ConnectedPeer peer)
        {
            InitializeComponent();

            this.ContextMenu = new ContextMenu();
            var propertiesMenuItem = new MenuItem();
            propertiesMenuItem.Header = "_Properties...";
            propertiesMenuItem.Click+=new System.Windows.RoutedEventHandler(PropertiesMenuItem_Click);
            this.ContextMenu.Items.Add(propertiesMenuItem);


            this.Peer = peer;
        }

        private ConnectedPeer _Peer;
        public ConnectedPeer Peer {
            get
            {
                return this._Peer;
            }
            private set
            {
                this._Peer = value;

                lblDisplayName.Content = lblDisplayName.ToolTip = this._Peer.MacAddress;

                UpdateDeviceIcon();

                lblMACAddress.Content = "";
                lblIPAddress.Content = "";
                
                // TODO - Need to get IP Address
                lblMACAddress.Content = "Retrieving Host Name...";
                lblIPAddress.Content = "Retrieving IP Address...";

                thread = new Thread(new ParameterizedThreadStart(this.getIPInfo));
                thread.Start(this);
            }
        }

        public void UpdateDeviceIcon()
        {   
            var icon = DeviceIconManager.LoadIcon(this._Peer.MacAddress);
            var resourceName = icon.Icon.ToResourceName();
            imgDeviceIcon.Source = (ImageSource)FindResource(resourceName);
        }

        public string IPAddress { get; set; }
        public string HostName { get; set; }

        private void SetIPInfoDisplay(IPInfo ipinfo)
        {
            if (ipinfo.HostName == ipinfo.IPAddress)
            {
                lblDisplayName.Content = lblDisplayName.ToolTip = ipinfo.HostName;
            }
            else
            {
                if (string.IsNullOrEmpty(ipinfo.HostName))
                {
                    lblDisplayName.Content = lblDisplayName.ToolTip = ipinfo.IPAddress;
                }
                else
                {
                    lblDisplayName.Content = lblDisplayName.ToolTip = ipinfo.HostName;
                }
            }

            this.lblMACAddress.Content = "MAC: " + ipinfo.MacAddress;
            this.lblIPAddress.Content = "IP: " + ipinfo.IPAddress;

            this.IPAddress = ipinfo.IPAddress;
            this.HostName = ipinfo.HostName;
        }

        private void SetIPNotFound()
        {
            this.lblMACAddress.Content = "Host Name could not be found.";
            this.lblIPAddress.Content = "IP Address could not be found.";
        }

        private void getIPInfo(object data)
        {
            var ipinfoFound = false;
            var count = 0;

            // count is used to only check a certain number of times before giving up
            // This will keep the thread from preventing the app from exiting when the user closes the main window.

            while (!ipinfoFound && count < 10)
            {
                try
                {
                    var pd = data as PeerDevice;
                    var ipinfo = IPInfo.GetIPInfo(pd.Peer.MacAddress.Replace(":", "-"));
                    if (ipinfo != null)
                    {
                        var hostname = ipinfo.HostName;
                        this.Dispatcher.Invoke((Action)delegate()
                        {
                            this.SetIPInfoDisplay(ipinfo);
                        });
                        ipinfoFound = true;
                    }
                    else
                    {
                        Thread.Sleep(1000);
                        ipinfoFound = false;
                    }
                }
                catch
                {
                    ipinfoFound = false;
                }
                count++;
            }
            if (!ipinfoFound)
            {
                this.Dispatcher.Invoke((Action)delegate()
                {
                    this.SetIPNotFound();
                });
            }
        }

        private void UserControl_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ShowPropertiesDialog();
        }

        private void PropertiesMenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowPropertiesDialog();
        }

        public void ShowPropertiesDialog()
        {
            var window = new PeerDeviceProperties(this);
            window.Owner = App.Current.MainWindow;
            window.ShowDialog();
        }

    }
}
