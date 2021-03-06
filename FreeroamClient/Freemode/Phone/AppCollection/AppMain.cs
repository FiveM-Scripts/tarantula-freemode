﻿using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using FreeroamShared;

namespace Freeroam.Freemode.Phone.AppCollection
{
	public class AppMain : IPhoneApp
	{
		private enum PhoneInputDirection
		{
			UP,
			RIGHT,
			DOWN,
			LEFT
		}

		private Scaleform phoneScaleform;
		private int selected;

		public void Init(Scaleform phoneScaleform)
		{
			this.phoneScaleform = phoneScaleform;
			selected = 4;
		}

		public async Task OnTick()
		{
			await Task.FromResult(0);

			for (int i = 0; i < 9; i++)
			{
				PhoneAppIcon appIcon = PhoneAppHolder.Apps[i].AppIcon;
				if (PhoneAppHolder.Apps[i].Disabled)
					appIcon = PhoneAppIcon.APP_EMPTY;
				phoneScaleform.CallFunction("SET_DATA_SLOT", 1, i, (int) appIcon);
			}

			phoneScaleform.CallFunction("DISPLAY_VIEW", 1, selected);
			phoneScaleform.CallFunction("SET_HEADER", PhoneAppHolder.Apps[selected].Disabled ? "" : PhoneAppHolder.Apps[selected].AppName);

			phoneScaleform.CallFunction("SET_SOFT_KEYS", (int) PhoneSelectSlot.SLOT_RIGHT, true, (int) PhoneSelectIcon.ICON_BACK);
			phoneScaleform.CallFunction("SET_SOFT_KEYS", (int) PhoneSelectSlot.SLOT_LEFT, true,
				PhoneAppHolder.Apps[selected].Disabled ? (int) PhoneSelectIcon.ICON_BLANK : (int) PhoneSelectIcon.ICON_SELECT);

			bool pressed = false;
			if (Game.IsControlJustPressed(0, Control.PhoneUp))
			{
				Navigate(PhoneInputDirection.UP);
				pressed = true;
			}
			else if (Game.IsControlJustPressed(0, Control.PhoneRight))
			{
				Navigate(PhoneInputDirection.RIGHT);
				pressed = true;
			}
			else if (Game.IsControlJustPressed(0, Control.PhoneDown))
			{
				Navigate(PhoneInputDirection.DOWN);
				pressed = true;
			}
			else if (Game.IsControlJustPressed(0, Control.PhoneLeft))
			{
				Navigate(PhoneInputDirection.LEFT);
				pressed = true;
			}
			else if (Game.IsControlJustPressed(0, Control.PhoneSelect))
			{
				if (!PhoneAppHolder.Apps[selected].Disabled)
					PhoneAppStarter.InitApp((IPhoneApp) Activator.CreateInstance(PhoneAppHolder.Apps[selected].AppHandler));
				pressed = true;
			}
			else if (Game.IsControlJustPressed(0, Control.PhoneCancel))
				PhoneStarter.StopPhone();

			if (pressed)
				Audio.ReleaseSound(Audio.PlaySoundFrontend("Menu_Navigate", "Phone_SoundSet_Default"));
		}

		public void Stop()
		{

		}

		private void Navigate(PhoneInputDirection direction)
		{
			switch (direction)
			{
				case PhoneInputDirection.UP:
					selected -= 3;
					if (selected < 0)
						selected = 9 - Math.Abs(selected);
					break;
				case PhoneInputDirection.RIGHT:
					if (++selected > 8)
						selected = 0;
					break;
				case PhoneInputDirection.DOWN:
					selected += 3;
					if (selected > 8)
						selected = selected - 9;
					break;
				case PhoneInputDirection.LEFT:
					if (--selected < 0)
						selected = 8;
					break;
			}
		}
	}
}
