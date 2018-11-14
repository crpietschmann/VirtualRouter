
namespace VirtualRouter.Wlan.WinAPI
{
    public enum DOT11_AUTH_ALGORITHM
    {

        /// DOT11_AUTH_ALGO_80211_OPEN -> 1
        DOT11_AUTH_ALGO_80211_OPEN = 1,

        /// DOT11_AUTH_ALGO_80211_SHARED_KEY -> 2
        DOT11_AUTH_ALGO_80211_SHARED_KEY = 2,

        /// DOT11_AUTH_ALGO_WPA -> 3
        DOT11_AUTH_ALGO_WPA = 3,

        /// DOT11_AUTH_ALGO_WPA_PSK -> 4
        DOT11_AUTH_ALGO_WPA_PSK = 4,

        /// DOT11_AUTH_ALGO_WPA_NONE -> 5
        DOT11_AUTH_ALGO_WPA_NONE = 5,

        /// DOT11_AUTH_ALGO_RSNA -> 6
        DOT11_AUTH_ALGO_RSNA = 6,

        /// DOT11_AUTH_ALGO_RSNA_PSK -> 7
        DOT11_AUTH_ALGO_RSNA_PSK = 7,

        /// DOT11_AUTH_ALGO_IHV_START -> 0x80000000
        DOT11_AUTH_ALGO_IHV_START = -2147483648,

        /// DOT11_AUTH_ALGO_IHV_END -> 0xffffffff
        DOT11_AUTH_ALGO_IHV_END = -1,
    }
}
