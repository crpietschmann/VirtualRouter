using System.Runtime.InteropServices;

namespace VirtualRouter.Wlan.WinAPI
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct WLAN_HOSTED_NETWORK_STATE_CHANGE
    {
        public WLAN_HOSTED_NETWORK_STATE OldState;
        public WLAN_HOSTED_NETWORK_STATE NewState;
        public WLAN_HOSTED_NETWORK_REASON Reason; // NewState;
    }
}
