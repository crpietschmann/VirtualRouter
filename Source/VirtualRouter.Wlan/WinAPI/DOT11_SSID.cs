using System.Runtime.InteropServices;

namespace VirtualRouter.Wlan.WinAPI
{
    [StructLayout(LayoutKind.Sequential)] //, CharSet = CharSet.Ansi)]
    public struct DOT11_SSID
    {

        /// ULONG->unsigned int
        public int uSSIDLength; //uint

        /// UCHAR[]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string ucSSID;
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        //public byte[] ucSSID;
    }
}
