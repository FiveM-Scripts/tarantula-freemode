using Freeroam.Freemode.Phone.AppCollection;
using System;

namespace Freeroam.Freemode.Phone
{
	public enum PhoneAppIcon
	{
		APP_CAMERA = 1,
		APP_CHAT,
		APP_EMPTY,
		APP_MESSAGING = 4,
		APP_CONTACTS,
		APP_INTERNET,
		APP_CONTACTS_PLUS = 11,
		APP_TASKS,
		APP_GROUP = 14,
		APP_SETTINGS = 24,
		APP_WARNING = 27,
		APP_GAMES = 35,
		APP_RIGHT_ARROW = 38,
		APP_TASKS_2,
		APP_TARGET,
		APP_TRACKIFY = 42,
		APP_CLOUD,
		APP_BROADCAST = 49,
		APP_VLSI = 54,
		APP_BENNYS = 56,
		APP_SECUROSERV,
		APP_COORDS,
		APP_RSS
	}

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
			new PhoneApp(PhoneAppIcon.APP_TRACKIFY, "Trackify", null, true),
			new PhoneApp(PhoneAppIcon.APP_EMPTY, "", null, true),
			new PhoneApp(PhoneAppIcon.APP_EMPTY, "", null, true),
			new PhoneApp(PhoneAppIcon.APP_EMPTY, "", null, true),
			new PhoneApp(PhoneAppIcon.APP_EMPTY, "", null, true),
			new PhoneApp(PhoneAppIcon.APP_EMPTY, "", null, true),
			new PhoneApp(PhoneAppIcon.APP_EMPTY, "", null, true),
			new PhoneApp(PhoneAppIcon.APP_EMPTY, "", null, true)
		};
	}
}
