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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VirtualRouterClient.AeroGlass;

namespace VirtualRouterClient
{
    /// <summary>
    /// Interaction logic for DeviceIconPicker.xaml
    /// </summary>
    public partial class DeviceIconPicker : Window
    {
        public DeviceIconPicker(string deviceName)
        {
            InitializeComponent();

            lblInstructions.Content = "Choose the icon to display for " + deviceName + ".";

            this.Loaded += new RoutedEventHandler(DeviceIconPicker_Loaded);


            var icons = new List<PickerIcon>();

            var iconNames = Enum.GetNames(typeof(DeviceIconEnum));
            foreach (var n in iconNames)
            {
                var e = (DeviceIconEnum)Enum.Parse(typeof(DeviceIconEnum), n);
                icons.Add(new PickerIcon() { Value = e, IconName = e.ToDescriptionString(), IconPath = e.ToResourceName() });
            }

            listIcons.ItemsSource = icons;
        }

        public DeviceIconEnum SelectedIcon
        {
            get
            {
                return ((PickerIcon)listIcons.SelectedItem).Value;
            }
            set
            {
                foreach (var item in listIcons.Items)
                {
                    var icon = item as PickerIcon;
                    if (icon.Value == value)
                    {
                        listIcons.SelectedItem = item;
                    }
                }
            }
        }


        void DeviceIconPicker_Loaded(object sender, RoutedEventArgs e)
        {
            AeroGlassHelper.ExtendGlass(this, (int)windowContent.Margin.Left, (int)windowContent.Margin.Right, (int)windowContent.Margin.Top, (int)windowContent.Margin.Bottom);
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        public class PickerIcon
        {
            public string IconPath { get; set; }
            public string IconName { get; set; }
            public DeviceIconEnum Value { get; set; }
        }

        private void listIcons_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnOK_Click(sender, new RoutedEventArgs());
        }
    }
}
