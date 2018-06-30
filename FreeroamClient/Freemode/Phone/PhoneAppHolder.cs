using Freeroam.Freemode.Phone.AppCollection;
using FreeroamShared;
using System;

namespace Freeroam.Freemode.Phone
{
	public struct PhoneApp
	{
		public PhoneAppIcon AppIcon { get; private set; }
		public string AppName { get; private set; }
		public Type AppHandler { get; private set; }
		public bool Disabled;

		public PhoneApp(PhoneAppIcon appIcon, string appName, Type appHandler = null, bool disabled = false)
		{
			AppIcon = appIcon;
			AppName = appName;
			AppHandler = appHandler;
			Disabled = disabled;
		}
	}

	public static class PhoneAppHolder
	{
		public static PhoneApp[] Apps { get; } =
		{
			new PhoneApp(PhoneAppIcon.APP_MESSAGING, Strings.CLIENT_PHONE_APP_MESSAGES, typeof(AppMessages)),
			new PhoneApp(PhoneAppIcon.APP_GROUP, Strings.CLIENT_PHONE_APP_PLAYERLIST, typeof(AppPlayerlist)),
            new PhoneApp(PhoneAppIcon.APP_CONTACTS, Strings.CLIENT_PHONE_APP_CONTACTS, typeof(AppContacts)),
            new PhoneApp(PhoneAppIcon.APP_RSS, Strings.CLIENT_PHONE_APP_RADIO, typeof(AppRadio)),
			new PhoneApp(PhoneAppIcon.APP_TRACKIFY, Strings.CLIENT_PHONE_APP_TRACKIFY, typeof(AppTrackify), true),
			new PhoneApp(PhoneAppIcon.APP_EMPTY, Strings.PLACEHOLDER, null, true),
			new PhoneApp(PhoneAppIcon.APP_EMPTY, Strings.PLACEHOLDER, null, true),
			new PhoneApp(PhoneAppIcon.APP_EMPTY, Strings.PLACEHOLDER, null, true),
			new PhoneApp(PhoneAppIcon.APP_EMPTY, Strings.PLACEHOLDER, null, true)
		};
	}
}
