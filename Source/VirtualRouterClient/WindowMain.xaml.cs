/*
* Virtual Router v1.0 - http://virtualrouter.codeplex.com
* Wifi Hot Spot for Windows 8, 7 and 2008 R2
* Copyright (c) 2013 Chris Pietschmann (http://pietschsoft.com)
* Licensed under the Microsoft Public License (Ms-PL)
* http://virtualrouter.codeplex.com/license
*/
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using VirtualRouterClient.VirtualRouterService;
using System.Diagnostics;
using System.Collections.Generic;
using VirtualRouterClient.Properties;
using VirtualRouterClient.AeroGlass;
using System.Reflection;

namespace VirtualRouterClient
{
    /// <summary>
    /// Interaction logic for WindowMain.xaml
    /// </summary>
    public partial class WindowMain : Window
    {
        private App myApp = (App)App.Current;
        private Thread threadUpdateUI;

        private WpfNotifyIcon trayIcon;

        public WindowMain()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(Window1_Loaded);
            this.Closing += new System.ComponentModel.CancelEventHandler(WindowMain_Closing);

            myApp.VirtualRouterServiceConnected += new EventHandler(myApp_VirtualRouterServiceConnected);
            myApp.VirtualRouterServiceDisconnected += new EventHandler(myApp_VirtualRouterServiceDisconnected);
        }

        private void WindowMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Settings.Default.SSID = txtSSID.Text;
            Settings.Default.Password = txtPassword.Text;
            Settings.Default.Save();
        }

        private void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            AeroGlassHelper.ExtendGlass(this, (int)windowContent.Margin.Left, (int)windowContent.Margin.Right, (int)windowContent.Margin.Top, (int)windowContent.Margin.Bottom);

            txtSSID.Text = Settings.Default.SSID;
            txtPassword.Text = Settings.Default.Password;

            // This line is for testing purposes
            //panelConnections.Children.Add(new PeerDevice(new ConnectedPeer() { MacAddress = "AA-22-33-EE-EE-FF" }));

            var args = System.Environment.GetCommandLineArgs();
            var minarg = (from a in args
                          where a.ToLowerInvariant().Contains("/min")
                          select a).FirstOrDefault();
            if (!string.IsNullOrEmpty(minarg))
            {
                this.WindowState = WindowState.Minimized;
                this.ShowInTaskbar = false;
            }

            this.AddSystemMenuItems();

            this.threadUpdateUI = new Thread(new ThreadStart(this.UpdateUIThread));
            this.threadUpdateUI.Start();

            this.Closed += new EventHandler(Window1_Closed);


            // Show System Tray Icon
            var stream = Application.GetResourceStream(new Uri("icons/virtualrouterdisabled.ico", UriKind.Relative)).Stream;
            var icon = new System.Drawing.Icon(stream);
            this.trayIcon = new WpfNotifyIcon();
            this.trayIcon.Icon = icon;
            this.trayIcon.Show();
            this.trayIcon.Text = "Virtual Router (Disabled)";
            this.trayIcon.DoubleClick += new EventHandler(trayIcon_DoubleClick);

            var trayMenu = new System.Windows.Forms.ContextMenuStrip();
            trayMenu.Items.Add("&Manage Virtual Router...", null, new EventHandler(this.TrayIcon_Menu_Manage));
            trayMenu.Items.Add(new System.Windows.Forms.ToolStripSeparator());
            trayMenu.Items.Add("Check for &Updates...", null, new EventHandler(this.TrayIcon_Menu_Update));
            trayMenu.Items.Add("&About...", null, new EventHandler(this.TrayIcon_Menu_About));
            this.trayIcon.ContextMenuStrip = trayMenu;

            this.StateChanged += new EventHandler(WindowMain_StateChanged);

            UpdateDisplay();
        }

        void TrayIcon_Menu_Update(object sender, EventArgs e)
        {
            CheckUpdates();
        }

        public static void CheckUpdates()
        {
            Process.Start("http://virtualrouter.codeplex.com");
        }

        void TrayIcon_Menu_About(object sender, EventArgs e)
        {
            ShowAboutBox();
        }

        void TrayIcon_Menu_Manage(object sender, EventArgs e)
        {
            this.WindowState = WindowState.Normal;
        }

        void WindowMain_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.ShowInTaskbar = false;
            }
            else
            {
                this.ShowInTaskbar = true;
            }
        }

        void trayIcon_DoubleClick(object sender, EventArgs e)
        {
            this.WindowState = WindowState.Normal;
        }

        private void Window1_Closed(object sender, EventArgs e)
        {
            this.threadUpdateUI.Abort();
            this.trayIcon.Hide();
            this.trayIcon.Dispose();
        }

        void UpdateUIThread()
        {
            while (true)
            {
                this.Dispatcher.Invoke(new Action(this.UpdateDisplay));
                Thread.Sleep(5000); // 5 Seconds
            }
        }

        void myApp_VirtualRouterServiceDisconnected(object sender, EventArgs e)
        {
            lblStatus.Content = "Can not manage Virtual Router. The Service is not running.";
            this.trayIcon.Text = "Virtual Router (Disabled)";
            UpdateDisplay();
        }

        void myApp_VirtualRouterServiceConnected(object sender, EventArgs e)
        {
            lblStatus.Content = "Virtual Router can now be managed.";
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            UpdateUIDisplay();

            RefreshSharableConnectionsDisplay();

            if (myApp.IsVirtualRouterServiceConnected)
            {
                //panelConnections.Children.Clear();
                var peers = myApp.VirtualRouter.GetConnectedPeers();
                groupBoxPeersConnected.Header = "Peers Connected (" + peers.Count().ToString() + "):";
                foreach (var p in peers)
                {
                    if (!this.isPeerAlreadyConnected(p))
                    {
                        panelConnections.Children.Add(new PeerDevice(p));
                    }
                }
                this.removeDisconnectedPeers(peers);
            }
            else
            {
                groupBoxPeersConnected.Header = "Peers Connected (0):";
            }
        }

        private bool isPeerAlreadyConnected(ConnectedPeer peer)
        {
            foreach (var element in panelConnections.Children)
            {
                var elem = element as PeerDevice;
                if (elem != null)
                {
                    if (elem.Peer.MacAddress.ToLowerInvariant() == peer.MacAddress.ToLowerInvariant())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private void removeDisconnectedPeers(ConnectedPeer[] peers)
        {
            List<PeerDevice> peersToRemove = new List<PeerDevice>();

            foreach (var element in panelConnections.Children)
            {
                var elem = element as PeerDevice;
                if (elem != null)
                {
                    var exists = false;
                    foreach (var p in peers)
                    {
                        if (p.MacAddress.ToLowerInvariant() == elem.Peer.MacAddress.ToLowerInvariant())
                        {
                            exists = true;
                            break;
                        }
                    }
                    if (!exists)
                    {
                        peersToRemove.Add(elem);
                    }
                }
            }
            foreach (var elem in peersToRemove)
            {
                panelConnections.Children.Remove(elem);
            }
        }

        #region "System Menu Stuff"

        #region Win32 API Stuff

        // Define the Win32 API methods we are going to use
        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        private static extern bool InsertMenu(IntPtr hMenu, Int32 wPosition, Int32 wFlags, Int32 wIDNewItem, string lpNewItem);

        /// Define our Constants we will use
        public const Int32 WM_SYSCOMMAND = 0x112;
        public const Int32 MF_SEPARATOR = 0x800;
        public const Int32 MF_BYPOSITION = 0x400;
        public const Int32 MF_STRING = 0x0;

        #endregion 

        private const int _AboutSysMenuID = 1001;
        private const int _UpdateSysMenuID = 1002;

        private void AddSystemMenuItems()
        {
            IntPtr windowHandle = new WindowInteropHelper(this).Handle;
            IntPtr systemMenu = GetSystemMenu(windowHandle, false);

            InsertMenu(systemMenu, 5, MF_BYPOSITION | MF_SEPARATOR, 0, string.Empty);
            InsertMenu(systemMenu, 6, MF_BYPOSITION, _UpdateSysMenuID, "Check for Updates...");
            InsertMenu(systemMenu, 7, MF_BYPOSITION, _AboutSysMenuID, "About...");

            HwndSource source = HwndSource.FromHwnd(windowHandle);
            source.AddHook(new HwndSourceHook(WndProc));
        }

        private static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // Check if a System Command has been executed
            if (msg == WM_SYSCOMMAND)
            {
                // Execute the appropriate code for the System Menu item that was clicked
                switch (wParam.ToInt32())
                {
                    case _AboutSysMenuID:
                        ShowAboutBox();
                        handled = true;
                        break;
                    case _UpdateSysMenuID:
                        CheckUpdates();
                        handled = true;
                        break;
                }
            }

            return IntPtr.Zero;
        }

        #endregion

        static void ShowAboutBox()
        {
            System.Windows.MessageBox.Show(
                            AssemblyAttributes.AssemblyProduct + " " + AssemblyAttributes.AssemblyVersion + Environment.NewLine
                            + Environment.NewLine + AssemblyAttributes.AssemblyDescription + Environment.NewLine
                            + Environment.NewLine + "Licensed under the Microsoft Public License (Ms-PL)" + Environment.NewLine
                            + Environment.NewLine + AssemblyAttributes.AssemblyCopyright + Environment.NewLine
                            + Environment.NewLine + "http://virtualrouter.codeplex.com"
                            
                            , "About " + AssemblyAttributes.AssemblyProduct + "...");
        }

        private void btnToggleHostedNetwork_Click(object sender, RoutedEventArgs e)
        {
            if (myApp.IsVirtualRouterServiceConnected)
            {
                if (myApp.VirtualRouter.IsStarted())
                {
                    myApp.VirtualRouter.Stop();
                }
                else
                {
                    if (this.ValidateFields())
                    {

                        myApp.VirtualRouter.SetConnectionSettings(txtSSID.Text, 100);
                        myApp.VirtualRouter.SetPassword(txtPassword.Text);

                        if (!myApp.VirtualRouter.Start((SharableConnection)cbSharedConnection.SelectedItem))
                        {
                            string strMessage = myApp.VirtualRouter.GetLastError() ?? "Virtual Router Could Not Be Started!";
                            lblStatus.Content = strMessage;
                            MessageBox.Show(strMessage, this.Title);
                        }
                        else
                        {
                            lblStatus.Content = "Virtual Router Started...";
                        }

                    }
                }
            }

            UpdateUIDisplay();
        }

        private bool ValidateFields()
        {
            var errorMessage = string.Empty;

            if (txtSSID.Text.Length <= 0)
            {
                errorMessage += "Network Name (SSID) is required." + Environment.NewLine;
            }

            if (txtSSID.Text.Length > 32)
            {
                errorMessage += "Network Name (SSID) can not be longer than 32 characters." + Environment.NewLine;
            }

            if (txtPassword.Text.Length < 8)
            {
                errorMessage += "Password must be at least 8 characters." + Environment.NewLine;
            }

            if (txtPassword.Text.Length > 64)
            {
                errorMessage += "Password can not be longer than 64 characters." + Environment.NewLine;
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                MessageBox.Show(errorMessage, "", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private bool isVirtualRouterRunning = false;
        private void UpdateUIDisplay()
        {
            var enableToggleButton = false;
            var enableSettingsFields = false;

            if (myApp.IsVirtualRouterServiceConnected)
            {
                enableToggleButton = true;
                try
                {
                    btnToggleHostedNetwork.IsEnabled = true;
                    if (myApp.VirtualRouter.IsStarted())
                    {
                        enableSettingsFields = false;
                        btnToggleHostedNetwork.Content = "Stop Virtual Router";
                        this.trayIcon.Text = "Virtual Router (Running)";
                        this.trayIcon.Icon = new System.Drawing.Icon(
                            Application.GetResourceStream(new Uri("icons/virtualrouterenabled.ico", UriKind.Relative)).Stream
                            );

                        if (!isVirtualRouterRunning)
                        {
                            this.Icon = this.imgIcon.Source = BitmapFrame.Create(Application.GetResourceStream(new Uri("Icons/VirtualRouterEnabled.ico", UriKind.Relative)).Stream);
                        }
                        isVirtualRouterRunning = true;

                        txtSSID.Text = myApp.VirtualRouter.GetConnectionSettings().SSID;
                        txtPassword.Text = myApp.VirtualRouter.GetPassword();
                    }
                    else
                    {
                        enableSettingsFields = true;
                        btnToggleHostedNetwork.Content = "Start Virtual Router";
                        this.trayIcon.Text = "Virtual Router (Stopped)";
                        this.trayIcon.Icon = new System.Drawing.Icon(
                            Application.GetResourceStream(new Uri("icons/virtualrouterdisabled.ico", UriKind.Relative)).Stream
                            );

                        if (isVirtualRouterRunning)
                        {
                            this.Icon = this.imgIcon.Source = BitmapFrame.Create(Application.GetResourceStream(new Uri("Icons/VirtualRouterDisabled.ico", UriKind.Relative)).Stream);
                        }
                        isVirtualRouterRunning = false;
                    }
                }
                catch
                {
                    enableToggleButton = false;
                    enableSettingsFields = false;
                }
            }

            btnToggleHostedNetwork.IsEnabled = enableToggleButton;
            gbVirtualRouterSettings.IsEnabled = enableSettingsFields;
        }

        private void RefreshSharableConnectionsDisplay()
        {
            if (myApp.IsVirtualRouterServiceConnected)
            {
                cbSharedConnection.DisplayMemberPath = "Name";
                
                var connections = myApp.VirtualRouter.GetSharableConnections();

                Guid selectedId = Guid.Empty;
                if (myApp.VirtualRouter.IsStarted())
                {
                    var sharedConn = myApp.VirtualRouter.GetSharedConnection();
                    if (sharedConn != null)
                    {
                        selectedId = sharedConn.Guid;
                    }
                }
                else
                {
                    var previousItem = cbSharedConnection.SelectedItem as SharableConnection;
                    if (previousItem != null)
                    {
                        selectedId = previousItem.Guid;
                    }
                }

                cbSharedConnection.Items.Clear();
                foreach (var c in connections)
                {
                    cbSharedConnection.Items.Add(c);
                    if (c.Guid == selectedId)
                    {
                        cbSharedConnection.SelectedItem = c;
                    }
                }
            }
        }

        private void btnRefreshSharableConnections_Click(object sender, RoutedEventArgs e)
        {
            RefreshSharableConnectionsDisplay();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
