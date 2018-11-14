using System;
using System.Runtime.InteropServices;

namespace VirtualRouter.Wlan.WinAPI
{
    /// <summary>
        /// Contains an array of NIC information
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct WLAN_INTERFACE_INFO_LIST
        {
        /// <summary>
        /// Length of <see cref="InterfaceInfo"/> array
        /// </summary>
        public Int32 dwNumberOfItems;
        /// <summary>
        /// This member is not used by the wireless service. Applications can use this member when processing individual interfaces.
        /// </summary>
        public Int32 dwIndex;
        /// <summary>
        /// Array of WLAN interfaces.
        /// </summary>
        public WLAN_INTERFACE_INFO[] InterfaceInfo;

        /// <summary>
        /// Constructor for WLAN_INTERFACE_INFO_LIST.
        /// Constructor is needed because the InterfaceInfo member varies based on how many adapters are in the system.
        /// </summary>
        /// <param name="pList">the unmanaged pointer containing the list.</param>
        public WLAN_INTERFACE_INFO_LIST(IntPtr pList)
        {
            // The first 4 bytes are the number of WLAN_INTERFACE_INFO structures.
            dwNumberOfItems = Marshal.ReadInt32(pList, 0);

            // The next 4 bytes are the index of the current item in the unmanaged API.
            dwIndex = Marshal.ReadInt32(pList, 4);

            // Construct the array of WLAN_INTERFACE_INFO structures.
            InterfaceInfo = new WLAN_INTERFACE_INFO[dwNumberOfItems];

            for (int i = 0; i < dwNumberOfItems; i++)
            {
            // The offset of the array of structures is 8 bytes past the beginning. Then, take the index and multiply it by the number of bytes in the structure.
            // the length of the WLAN_INTERFACE_INFO structure is 532 bytes - this was determined by doing a sizeof(WLAN_INTERFACE_INFO) in an unmanaged C++ app.
            IntPtr pItemList = new IntPtr(pList.ToInt32() + (i * 532) + 8);

            // Construct the WLAN_INTERFACE_INFO structure, marshal the unmanaged structure into it, then copy it to the array of structures.
            WLAN_INTERFACE_INFO wii = new WLAN_INTERFACE_INFO();
            wii = (WLAN_INTERFACE_INFO)Marshal.PtrToStructure(pItemList, typeof(WLAN_INTERFACE_INFO));
            InterfaceInfo[i] = wii;
            }
        }
    }
}
