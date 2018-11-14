using System;
using System.Runtime.InteropServices;

namespace VirtualRouter.Wlan.WinAPI
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    struct WLAN_AVAILABLE_NETWORK_LIST
    {
        internal uint dwNumberOfItems;
        internal uint dwIndex;
        internal WLAN_AVAILABLE_NETWORK[] wlanAvailableNetwork;

        internal WLAN_AVAILABLE_NETWORK_LIST(IntPtr ppAvailableNetworkList)
        {
            dwNumberOfItems = (uint)Marshal.ReadInt32(ppAvailableNetworkList);
            dwIndex = (uint)Marshal.ReadInt32(ppAvailableNetworkList, 4);
            wlanAvailableNetwork = new WLAN_AVAILABLE_NETWORK[dwNumberOfItems];

            for (int i = 0; i < dwNumberOfItems; i++)
            {
                IntPtr pWlanAvailableNetwork = new IntPtr(ppAvailableNetworkList.ToInt32() + i * Marshal.SizeOf(typeof(WLAN_AVAILABLE_NETWORK)) + 8);
                wlanAvailableNetwork[i] = (WLAN_AVAILABLE_NETWORK)Marshal.PtrToStructure(pWlanAvailableNetwork, typeof(WLAN_AVAILABLE_NETWORK));
            }
        }
    }
}
