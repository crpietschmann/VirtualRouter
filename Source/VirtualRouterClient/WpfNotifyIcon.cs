/*
* Virtual Router v1.0 - http://virtualrouter.codeplex.com
* Wifi Hot Spot for Windows 8, 7 and 2008 R2
* Copyright (c) 2013 Chris Pietschmann (http://pietschsoft.com)
* Licensed under the Microsoft Public License (Ms-PL)
* http://virtualrouter.codeplex.com/license
*/
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace VirtualRouterClient
{
    public partial class WpfNotifyIcon : Component
    {
        public WpfNotifyIcon()
        {
            InitializeComponent();
        }

        public WpfNotifyIcon(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public EventHandler DoubleClick;

        private void notifyIcon1_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.DoubleClick != null)
            {
                this.DoubleClick(sender, e);
            }
        }

        public void Show()
        {
            this.notifyIcon1.Visible = true;
        }

        public void Hide()
        {
            this.notifyIcon1.Visible = false;
        }

        public ContextMenuStrip ContextMenuStrip
        {
            get
            {
                return this.notifyIcon1.ContextMenuStrip;
            }
            set
            {
                this.notifyIcon1.ContextMenuStrip = value;
            }
        }

        public System.Drawing.Icon Icon
        {
            get
            {
                return this.notifyIcon1.Icon;
            }
            set
            {
                this.notifyIcon1.Icon = value;
            }
        }

        public string Text
        {
            get
            {
                return this.notifyIcon1.Text;
            }
            set
            {
                this.notifyIcon1.Text = value;
            }
        }
    }
}
