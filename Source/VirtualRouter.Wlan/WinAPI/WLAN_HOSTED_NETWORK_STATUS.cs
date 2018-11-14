using System;
using System.Runtime.InteropServices;

namespace VirtualRouter.Wlan.WinAPI
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct WLAN_HOSTED_NETWORK_STATUS
    {
        public WLAN_HOSTED_NETWORK_STATE_CHANGE HostedNetworkState;
        public Guid IPDeviceID;
        public DOT11_MAC_ADDRESS wlanHostedNetworkBSSID;
        public DOT11_PHY_TYPE dot11PhyType;
        public uint ulChannelFrequency; // ULONG
        public uint dwNumberOfPeers; // DWORD
        public WLAN_HOSTED_NETWORK_PEER_STATE[] PeerList;
    }
}
