using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using FreeroamShared;

namespace Freeroam.Freemode.Phone.AppCollection
{
	class AppSettings : IPhoneApp
	{
		private struct Setting
		{
			public string Name { get; private set; }
			public Dictionary<string, Action> Items { get; private set; }

			public Setting(string name, Dictionary<string, Action> items)
			{
				Name = name;
				Items = items;
			}
		}

		private static class SettingsHolder
		{
			public static Setting[] Settings { get; } =
			{
				new Setting(Strings.PHONE_APP_SETTINGS_THEME, new Dictionary<string, Action>
				{
					{ Strings.PHONE_APP_SETTINGS_THEME_1, new Action(() => {
						PhoneState.PhoneTheme = 1;
						PhoneState.PhoneWallpaper = 10;
					}) },
					{ Strings.PHONE_APP_SETTINGS_THEME_2, new Action(() => {
						PhoneState.PhoneTheme = 2;
						PhoneState.PhoneWallpaper = 5;
					}) },
					{ Strings.PHONE_APP_SETTINGS_THEME_3, new Action(() => {
						PhoneState.PhoneTheme = 3;
						PhoneState.PhoneWallpaper = 6;
					}) },
					{ Strings.PHONE_APP_SETTINGS_THEME_4, new Action(() => {
						PhoneState.PhoneTheme = 4;
						PhoneState.PhoneWallpaper = 7;
					}) },
					{ Strings.PHONE_APP_SETTINGS_THEME_5, new Action(() => {
						PhoneState.PhoneTheme = 5;
						PhoneState.PhoneWallpaper = 8;
					}) },
					{ Strings.PHONE_APP_SETTINGS_THEME_6, new Action(() => {
						PhoneState.PhoneTheme = 6;
						PhoneState.PhoneWallpaper = 4;
					}) },
					{ Strings.PHONE_APP_SETTINGS_THEME_7, new Action(() => {
						PhoneState.PhoneTheme = 7;
						PhoneState.PhoneWallpaper = 4;
					}) }
				}),
				new Setting(Strings.PHONE_APP_SETTINGS_ABOUT, new Dictionary<string, Action>
				{
					{ Strings.PHONE_APP_SETTINGS_ABOUT_AUTHOR_PREFIX, new Action(() => { }) },
					{ Strings.PHONE_APP_SETTINGS_ABOUT_AUTHOR, new Action(() => { }) }
				})
			};
		}

		private Scaleform phoneScaleform;
		private int selected;
		private bool inSubMenu;
		private Setting selectedSettings;

		public void Init(Scaleform phoneScaleform)
		{
			this.phoneScaleform = phoneScaleform;
			phoneScaleform.CallFunction("SET_DATA_SLOT_EMPTY", 13);
		}

		public async Task OnTick()
		{
			await Task.FromResult(0);

			int slot = 0;
			if (inSubMenu)
				foreach (KeyValuePair<string, Action> item in selectedSettings.Items)
					phoneScaleform.CallFunction("SET_DATA_SLOT", 13, slot++, -1, item.Key);
			else
				foreach (Setting setting in SettingsHolder.Settings)
					phoneScaleform.CallFunction("SET_DATA_SLOT", 13, slot++, -1, setting.Name);
			phoneScaleform.CallFunction("SET_HEADER", inSubMenu ? selectedSettings.Name : Strings.PHONE_APP_SETTINGS);
			phoneScaleform.CallFunction("DISPLAY_VIEW", 13, selected);

			bool pressed = false;
			if (Game.IsControlJustPressed(0, Control.PhoneUp) && slot > 0)
			{
				if (--selected < 0)
					selected = slot - 1;
				pressed = true;
			}
			else if (Game.IsControlJustPressed(0, Control.PhoneDown) && slot > 0)
			{
				if (++selected > slot - 1)
					selected = 0;
				pressed = true;
			}
			else if (Game.IsControlJustPressed(0, Control.PhoneSelect) && slot > 0)
			{
				if (!inSubMenu)
				{
					inSubMenu = true;
					selectedSettings = SettingsHolder.Settings[selected];
					phoneScaleform.CallFunction("SET_DATA_SLOT_EMPTY", 13);
				}
				else
					selectedSettings.Items.ElementAt(selected).Value.Invoke();
				selected = 0;
				pressed = true;
			}
			else if (Game.IsControlJustPressed(0, Control.PhoneCancel))
			{
				if (!inSubMenu)
					PhoneAppStarter.MainApp();
				else
				{
					Audio.ReleaseSound(Audio.PlaySoundFrontend("Hang_Up", "Phone_SoundSet_Michael"));
					inSubMenu = false;
					phoneScaleform.CallFunction("SET_DATA_SLOT_EMPTY", 13);
				}

				if (pressed)
					Audio.ReleaseSound(Audio.PlaySoundFrontend("Menu_Navigate", "Phone_SoundSet_Default"));
			}
		}

		public void Stop()
		{

		}
	}
}
