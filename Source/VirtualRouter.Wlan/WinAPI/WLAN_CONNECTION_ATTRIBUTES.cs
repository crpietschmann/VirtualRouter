using System.Runtime.InteropServices;

namespace VirtualRouter.Wlan.WinAPI
{
    //http://msdn.microsoft.com/en-us/library/ms706842%28VS.85%29.aspx
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct WLAN_CONNECTION_ATTRIBUTES
    {
        WLAN_INTERFACE_STATE isState;
        WLAN_CONNECTION_MODE wlanCOnnectionMode;
        string strProfileMode; //WCHAR[256];
        WLAN_ASSOCIATION_ATTRIBUTES wlanAssociationAttributes;
        WLAN_SECURITY_ATTRIBUTES wlanSecurityAttributes;
    }
}
