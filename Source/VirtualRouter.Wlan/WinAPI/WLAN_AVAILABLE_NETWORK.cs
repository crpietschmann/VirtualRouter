using System.Runtime.InteropServices;

namespace VirtualRouter.Wlan.WinAPI
{
    //http://msdn.microsoft.com/en-us/library/ms707403%28VS.85%29.aspx
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct WLAN_AVAILABLE_NETWORK
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string strProfileName; // WCHAR[256]
        public DOT11_SSID dot11Ssid;
        public DOT11_BSS_TYPE dot11BssType;
        public uint uNumberOfBssids; // ULONG
        public bool bNetworkConnectable; // BOOL
        public uint wlanNotConnectableReason; // WLAN_REASON_CODE
        public uint uNumberOfPhyTypes; // ULONG
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public DOT11_PHY_TYPE[] dot11PhyTypes;
        public bool bMorePhyTypes; // BOOL
        public uint wlanSignalQuality; // WLAN_SIGNAL_QUALITY
        public bool bSecurityEnabled; // BOOL
        public DOT11_AUTH_ALGORITHM dot11DefaultAuthAlgorithm;
        public DOT11_CIPHER_ALGORITHM dot11DefaultCipherAlgorithm;
        public uint dwFlags; // DWORD
        public uint dwReserved; // DWORD
    }
}
