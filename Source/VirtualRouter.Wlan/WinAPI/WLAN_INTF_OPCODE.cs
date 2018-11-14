
namespace VirtualRouter.Wlan.WinAPI
{
    public enum WLAN_INTF_OPCODE
    {

        /// wlan_intf_opcode_autoconf_start -> 0x000000000
        wlan_intf_opcode_autoconf_start = 0,

        wlan_intf_opcode_autoconf_enabled,

        wlan_intf_opcode_background_scan_enabled,

        wlan_intf_opcode_media_streaming_mode,

        wlan_intf_opcode_radio_state,

        wlan_intf_opcode_bss_type,

        wlan_intf_opcode_interface_state,

        wlan_intf_opcode_current_connection,

        wlan_intf_opcode_channel_number,

        wlan_intf_opcode_supported_infrastructure_auth_cipher_pairs,

        wlan_intf_opcode_supported_adhoc_auth_cipher_pairs,

        wlan_intf_opcode_supported_country_or_region_string_list,

        wlan_intf_opcode_current_operation_mode,

        wlan_intf_opcode_supported_safe_mode,

        wlan_intf_opcode_certified_safe_mode,

        /// wlan_intf_opcode_autoconf_end -> 0x0fffffff
        wlan_intf_opcode_autoconf_end = 268435455,

        /// wlan_intf_opcode_msm_start -> 0x10000100
        wlan_intf_opcode_msm_start = 268435712,

        wlan_intf_opcode_statistics,

        wlan_intf_opcode_rssi,

        /// wlan_intf_opcode_msm_end -> 0x1fffffff
        wlan_intf_opcode_msm_end = 536870911,

        /// wlan_intf_opcode_security_start -> 0x20010000
        wlan_intf_opcode_security_start = 536936448,

        /// wlan_intf_opcode_security_end -> 0x2fffffff
        wlan_intf_opcode_security_end = 805306367,

        /// wlan_intf_opcode_ihv_start -> 0x30000000
        wlan_intf_opcode_ihv_start = 805306368,

        /// wlan_intf_opcode_ihv_end -> 0x3fffffff
        wlan_intf_opcode_ihv_end = 1073741823,
    }
}
