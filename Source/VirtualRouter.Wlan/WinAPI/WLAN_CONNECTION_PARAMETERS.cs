using System.Runtime.InteropServices;

namespace VirtualRouter.Wlan.WinAPI
{
    //http://msdn.microsoft.com/en-us/library/ms706851%28VS.85%29.aspx
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct WLAN_CONNECTION_PARAMETERS
    {
        WLAN_CONNECTION_MODE wlanConnectionMode;
        string strProfile; // LPCWSTR
        DOT11_SSID pDot11Ssid;
        DOT11_BSSID_LIST pDesiredBssidList;
        DOT11_BSS_TYPE dot11BssType;
        uint dwFlags; // DWORD
    }
}
