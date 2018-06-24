using Freeroam.Freemode.Phone.AppCollection;
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
			new PhoneApp(PhoneAppIcon.APP_MESSAGING, "Messages", typeof(AppMessages)),
			new PhoneApp(PhoneAppIcon.APP_GROUP, "Playerlist", typeof(AppPlayerlist)),
			new PhoneApp(PhoneAppIcon.APP_TRACKIFY, "Trackify", null, true),
			new PhoneApp(PhoneAppIcon.APP_EMPTY, "", null, true),
			new PhoneApp(PhoneAppIcon.APP_EMPTY, "", null, true),
			new PhoneApp(PhoneAppIcon.APP_EMPTY, "", null, true),
			new PhoneApp(PhoneAppIcon.APP_EMPTY, "", null, true),
			new PhoneApp(PhoneAppIcon.APP_EMPTY, "", null, true),
			new PhoneApp(PhoneAppIcon.APP_EMPTY, "", null, true)
		};
	}
}
