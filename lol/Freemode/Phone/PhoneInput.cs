using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Threading.Tasks;

namespace Freeroam.Freemode.Phone
{
	class PhoneInput : BaseScript
	{
		private enum Direction
		{
			UP,
			RIGHT,
			DOWN,
			LEFT
		}

		private Scaleform phoneScaleform;
		private int selected;

		public PhoneInput()
		{
			EventHandlers["freemode:heyItsAPhoneScaleform!"] += new Action<int>(handle =>
			{
				phoneScaleform = new Scaleform("THIS IS NEVER GOING TO WORK!!!")
				{
					NativeValue = (ulong)handle // Guess what... it did
				}; // Mind = blown
				selected = 4;
			});

			Tick += OnTick;
		}

		private async Task OnTick()
		{
			await Task.FromResult(0);

			if (PhoneState.IsShown)
			{
				phoneScaleform.CallFunction("DISPLAY_VIEW", 1, selected);
				string appName = PhoneAppHolder.Apps[selected].AppName;
				if (PhoneAppHolder.Apps[selected].Disabled)
					appName = "";
				phoneScaleform.CallFunction("SET_HEADER", appName);

				bool pressed = false;
				if (Game.IsControlJustPressed(0, Control.PhoneCancel))
				{
					PhoneState.IsShown = false;
					Audio.PlaySoundFrontend("Hang_Up", "Phone_SoundSet_Michael");
					API.DestroyMobilePhone();
					phoneScaleform.Dispose();
				}
				else if (Game.IsControlJustPressed(0, Control.PhoneUp))
				{
					Navigate(Direction.UP);
					pressed = true;
				}
				else if (Game.IsControlJustPressed(0, Control.PhoneRight))
				{
					Navigate(Direction.RIGHT);
					pressed = true;
				}
				else if (Game.IsControlJustPressed(0, Control.PhoneDown))
				{
					Navigate(Direction.DOWN);
					pressed = true;
				}
				else if (Game.IsControlJustPressed(0, Control.PhoneLeft))
				{
					Navigate(Direction.LEFT);
					pressed = true;
				}
				else if (Game.IsControlJustPressed(0, Control.PhoneSelect))
				{
					pressed = true;
				}

				if (pressed)
					Audio.PlaySoundFrontend("Menu_Navigate", "Phone_SoundSet_Default");
			}
		}

		private void Navigate(Direction direction)
		{
			switch (direction)
			{
				case Direction.UP:
					selected -= 3;
					if (selected < 0)
						selected = 9 - Math.Abs(selected);
					break;
				case Direction.RIGHT:
					selected += 1;
					if (selected > 8)
						selected = 0;
					break;
				case Direction.DOWN:
					selected += 3;
					if (selected > 8)
						selected = selected - 9;
					break;
				case Direction.LEFT:
					selected -= 1;
					if (selected < 0)
						selected = 8;
					break;
			}
		}
	}
}
