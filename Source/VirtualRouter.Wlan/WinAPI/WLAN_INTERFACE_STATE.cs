
namespace VirtualRouter.Wlan.WinAPI
{
    /// <summary>
    /// Defines the state of the interface. e.g. connected, disconnected.
    /// </summary>
    public enum WLAN_INTERFACE_STATE
    {
        /// <summary>
        /// wlan_interface_state_not_ready -> 0
        /// </summary>
        wlan_interface_state_not_ready = 0,

        /// <summary>
        /// wlan_interface_state_connected -> 1
        /// </summary>
        wlan_interface_state_connected = 1,

        /// <summary>
        /// wlan_interface_state_ad_hoc_network_formed -> 2
        /// </summary>
        wlan_interface_state_ad_hoc_network_formed = 2,

        /// <summary>
        /// wlan_interface_state_disconnecting -> 3
        /// </summary>
        wlan_interface_state_disconnecting = 3,

        /// <summary>
        /// wlan_interface_state_disconnected -> 4
        /// </summary>
        wlan_interface_state_disconnected = 4,

        /// <summary>
        /// wlan_interface_state_associating -> 5
        /// </summary>
        wlan_interface_state_associating = 5,

        /// <summary>
        /// wlan_interface_state_discovering -> 6
        /// </summary>
        wlan_interface_state_discovering = 6,

        /// <summary>
        /// wlan_interface_state_authenticating -> 7
        /// </summary>
        wlan_interface_state_authenticating = 7,
    }
}
