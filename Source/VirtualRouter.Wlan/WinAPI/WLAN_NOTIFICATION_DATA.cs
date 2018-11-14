using System;
using System.Runtime.InteropServices;

namespace VirtualRouter.Wlan.WinAPI
{
    /// <summary>
    /// Contains information provided when registering for notifications.
    /// </summary>
    /// <remarks>
    /// Corresponds to the native <c>WLAN_NOTIFICATION_DATA</c> type.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct WLAN_NOTIFICATION_DATA
    {
        /// <summary>
        /// Specifies where the notification comes from.
        /// </summary>
        /// <remarks>
        /// On Windows XP SP2, this field must be set to <see cref="WlanNotificationSource.None"/>, <see cref="WlanNotificationSource.All"/> or <see cref="WlanNotificationSource.ACM"/>.
        /// </remarks>
        public WLAN_NOTIFICATION_SOURCE notificationSource;
        /// <summary>
        /// Indicates the type of notification. The value of this field indicates what type of associated data will be present in <see cref="dataPtr"/>.
        /// </summary>
        public int notificationCode;
        /// <summary>
        /// Indicates which interface the notification is for.
        /// </summary>
        public Guid interfaceGuid;
        /// <summary>
        /// Specifies the size of <see cref="dataPtr"/>, in bytes.
        /// </summary>
        public int dataSize;
        /// <summary>
        /// Pointer to additional data needed for the notification, as indicated by <see cref="notificationCode"/>.
        /// </summary>
        public IntPtr dataPtr;

        /// <summary>
        /// Gets the notification code (in the correct enumeration type) according to the notification source.
        /// </summary>
        public object NotificationCode
        {
            get
            {
                if (notificationSource == WLAN_NOTIFICATION_SOURCE.MSM)
                    return (WLAN_NOTIFICATION_CODE_MSM)notificationCode;
                else if (notificationSource == WLAN_NOTIFICATION_SOURCE.ACM)
                    return (WLAN_NOTIFICATION_CODE_ACM)notificationCode;
                else
                    return notificationCode;
            }

        }
    } 
}
