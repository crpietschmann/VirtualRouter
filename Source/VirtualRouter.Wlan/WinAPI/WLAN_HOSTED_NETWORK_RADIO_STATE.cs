using System.Runtime.InteropServices;

namespace VirtualRouter.Wlan.WinAPI
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct WLAN_HOSTED_NETWORK_RADIO_STATE
    {
        DOT11_RADIO_STATE dot11SoftwareRadioState;
        DOT11_RADIO_STATE dot11HardwareRadioState;
    }
}
