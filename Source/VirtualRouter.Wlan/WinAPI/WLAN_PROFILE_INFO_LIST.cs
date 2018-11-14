using System;
using System.Runtime.InteropServices;

namespace VirtualRouter.Wlan.WinAPI
{
    public struct WLAN_PROFILE_INFO_LIST
    {
        public uint dwNumberOfItems;
        public uint dwIndex;
        public WLAN_PROFILE_INFO[] ProfileInfo;

        public WLAN_PROFILE_INFO_LIST(IntPtr ppProfileList)
        {
            dwNumberOfItems = (uint)Marshal.ReadInt32(ppProfileList);
            dwIndex = (uint)Marshal.ReadInt32(ppProfileList, 4);
            ProfileInfo = new WLAN_PROFILE_INFO[dwNumberOfItems];
            IntPtr ppProfileListTemp = new IntPtr(ppProfileList.ToInt32() + 8);

            for (int i = 0; i < dwNumberOfItems; i++)
            {
                ppProfileList = new IntPtr(ppProfileListTemp.ToInt32() + i * Marshal.SizeOf(typeof(WLAN_PROFILE_INFO)));
                ProfileInfo[i] = (WLAN_PROFILE_INFO)Marshal.PtrToStructure(ppProfileList, typeof(WLAN_PROFILE_INFO));
            }
        }
    }
}
