using System.Runtime.InteropServices;

namespace VirtualRouter.Wlan.WinAPI
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct WLAN_ASSOCIATION_ATTRIBUTES
    {
        DOT11_SSID dot11Ssid;
        DOT11_BSS_TYPE dot11BssType;
        DOT11_MAC_ADDRESS dot11Bssid;
        DOT11_PHY_TYPE dot11PhyType;
        uint uDot11PhyIndex; //ULONG
        uint wlanSignalQuality; //WLAN_SIGNAL_QUALITY -> ULONG
        uint ulRxRate; //ULONG
        uint ulTxRate; //ULONG
    }
}
