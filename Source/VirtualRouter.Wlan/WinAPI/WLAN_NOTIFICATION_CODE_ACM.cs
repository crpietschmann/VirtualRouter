
namespace VirtualRouter.Wlan.WinAPI
{
 /// <summary>
 /// Indicates the type of an ACM (<see cref="WlanNotificationSource.ACM"/>) notification.
 /// </summary>
 /// <remarks>
 /// The enumeration identifiers correspond to the native <c>wlan_notification_acm_</c> identifiers.
 /// On Windows XP SP2, only the <c>ConnectionComplete</c> and <c>Disconnected</c> notifications are available.
 /// </remarks>
 public enum WLAN_NOTIFICATION_CODE_ACM
 {
 AutoconfEnabled = 1,
 AutoconfDisabled,
 BackgroundScanEnabled,
 BackgroundScanDisabled,
 BssTypeChange,
 PowerSettingChange,
 ScanComplete,
 ScanFail,
 ConnectionStart,
 ConnectionComplete,
 ConnectionAttemptFail,
 FilterListChange,
 InterfaceArrival,
 InterfaceRemoval,
 ProfileChange,
 ProfileNameChange,
 ProfilesExhausted,
 NetworkNotAvailable,
 NetworkAvailable,
 Disconnecting,
 Disconnected,
 AdhocNetworkStateChange
 } 
}
