using System.Runtime.InteropServices;

namespace VirtualRouter.Wlan.WinAPI
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct WLAN_HOSTED_NETWORK_SECURITY_SETTINGS
    {
        DOT11_AUTH_ALGORITHM dot11AuthAlgo;
        DOT11_CIPHER_ALGORITHM dot11CipherAlgo;
    }
}
