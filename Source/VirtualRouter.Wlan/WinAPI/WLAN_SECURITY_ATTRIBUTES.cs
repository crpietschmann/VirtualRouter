using System.Runtime.InteropServices;

namespace VirtualRouter.Wlan.WinAPI
{
    //http://msdn.microsoft.com/en-us/library/ms707400%28VS.85%29.aspx
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct WLAN_SECURITY_ATTRIBUTES
    {
        bool bSecurityEnabled;
        bool bOneXEnabled;
        DOT11_AUTH_ALGORITHM dot11AuthAlgorithm;
        DOT11_CIPHER_ALGORITHM dot11CipherAlgorithm;
    }
}
