
namespace VirtualRouter.Wlan.WinAPI
{
    //http://msdn.microsoft.com/en-us/library/dd439501%28VS.85%29.aspx
    public enum WLAN_HOSTED_NETWORK_NOTIFICATION_CODE
    {
        /// <summary>
        /// The Hosted Network state has changed.
        /// </summary>
        wlan_hosted_network_state_change = 0x00001000,
        /// <summary>
        /// The Hosted Network peer state has changed.
        /// </summary>
        wlan_hosted_network_peer_state_change,
        /// <summary>
        /// The Hosted Network radio state has changed.
        /// </summary>
        wlan_hosted_network_radio_state_change
    }
}
