/*
* Virtual Router v1.0 - http://virtualrouter.codeplex.com
* Wifi Hot Spot for Windows 8, 7 and 2008 R2
* Copyright (c) 2013 Chris Pietschmann (http://pietschsoft.com)
* Licensed under the Microsoft Public License (Ms-PL)
* http://virtualrouter.codeplex.com/license
*/
using System.Collections.Generic;
using System.Linq;
using VirtualRouterClient.Properties;

namespace VirtualRouterClient
{
    public static class DeviceIconManager
    {
        public static DeviceIcon LoadIcon(string macAddress)
        {
            //if (Settings.Default.DeviceIcons == null)
            //{
            //    Settings.Default.DeviceIcons = new System.Collections.ArrayList();
            //}

            ////retrieve setting
            //DeviceIcon di = (from obj in Settings.Default.DeviceIcons.ToArray()
            //                 where (obj as DeviceIcon).MacAddress.Replace(":", "-").ToLowerInvariant() == macAddress.Replace(":", "-").ToLowerInvariant()
            //                 select obj as DeviceIcon).FirstOrDefault();
            //if (di == null)
            //    return new DeviceIcon(macAddress, DeviceIconEnum.Default);
            //else
            //    return di;

            var icons = GetDeviceIcons();

            //retrieve setting
            DeviceIcon di = (from obj in icons
                             where obj.MacAddress.Replace(":", "-").ToLowerInvariant() == macAddress.Replace(":", "-").ToLowerInvariant()
                             select obj).FirstOrDefault();
            if (di == null)
                return new DeviceIcon(macAddress, DeviceIconEnum.Default);
            else
                return di;
        }

        public static void SaveIcon(string macAddress, DeviceIconEnum icon)
        {
            //if (Settings.Default.DeviceIcons == null)
            //{
            //    Settings.Default.DeviceIcons = new System.Collections.ArrayList();

            //}
            //// Remove existing setting
            //Settings.Default.DeviceIcons.Remove(
            //        (from obj in Settings.Default.DeviceIcons.ToArray()
            //         where (obj as DeviceIcon).MacAddress.Replace(":", "-").ToLowerInvariant() == macAddress.Replace(":", "-").ToLowerInvariant()
            //             select obj).FirstOrDefault()
            //    );

            ////Save new setting
            //Settings.Default.DeviceIcons.Add(new DeviceIcon(macAddress, icon));
            //Settings.Default.Save();

            var icons = GetDeviceIcons();

            foreach(var item in
                    (from i in icons
                    where i.MacAddress.Replace(":", "-").ToLowerInvariant() == macAddress.Replace(":", "-").ToLowerInvariant()
                    select i).ToArray()
                ) {
                        icons.Remove(item);
            }

            icons.Add(new DeviceIcon(macAddress, icon));
            SaveDeviceIcons(icons);
        }

        private static List<DeviceIcon> GetDeviceIcons()
        {
            if (string.IsNullOrEmpty(Settings.Default.DeviceIcons))
            {
                return new List<DeviceIcon>();
            }
            else
            {
                try
                {
                    return JSONHelper.Deserialize<List<DeviceIcon>>(Settings.Default.DeviceIcons);
                }
                catch
                {
                    return new List<DeviceIcon>();
                }
            }
        }

        private static void SaveDeviceIcons(List<DeviceIcon> icons)
        {
            Settings.Default.DeviceIcons = JSONHelper.Serialize<List<DeviceIcon>>(icons);
            Settings.Default.Save();
        }
    }

    public class DeviceIcon
    {
        public DeviceIcon() { }
        public DeviceIcon(string macAddress, DeviceIconEnum icon)
        {
            this.MacAddress = macAddress;
            this.Icon = icon;
        }
        public string MacAddress { get; set; }
        public DeviceIconEnum Icon { get; set; }
    }
}
