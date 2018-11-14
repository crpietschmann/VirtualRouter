
namespace VirtualRouter.Wlan.WinAPI
{
    /// <summary>
    /// Represents an 802.11 Basic Service Set type
    /// </summary>
    public enum DOT11_BSS_TYPE
    {
        ///<summary>
        /// dot11_BSS_type_infrastructure -> 1
        ///</summary>
        dot11_BSS_type_infrastructure = 1,

        ///<summary>
        /// dot11_BSS_type_independent -> 2
        ///</summary>
        dot11_BSS_type_independent = 2,

        ///<summary>
        /// dot11_BSS_type_any -> 3
        ///</summary>
        dot11_BSS_type_any = 3,
    }
}
