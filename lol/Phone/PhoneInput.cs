using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Threading.Tasks;

namespace Freeroam.Phone
{
	class PhoneInput : BaseScript
	{
		private Scaleform phoneScaleform;

		public PhoneInput()
		{
			EventHandlers["freemode:heyItsAPhoneScaleform!"] += new Action<int>(handle =>
			{
				phoneScaleform = new Scaleform("THIS IS NEVER GOING TO WORK!!!")
				{
					// Guess what... it did
					NativeValue = (ulong) handle
				};
				// Mind = blown
			});

			Tick += OnTick;
		}

		private async Task OnTick()
		{
			await Task.FromResult(0);

			if (PhoneState.IsShown)
			{
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
					phoneScaleform.CallFunction("SET_INPUT_EVENT", 1);
					pressed = true;
				}
				else if (Game.IsControlJustPressed(0, Control.PhoneRight))
				{
					phoneScaleform.CallFunction("SET_INPUT_EVENT", 2);
					pressed = true;
				}
				else if (Game.IsControlJustPressed(0, Control.PhoneDown))
				{
					phoneScaleform.CallFunction("SET_INPUT_EVENT", 3);
					pressed = true;
				}
				else if (Game.IsControlJustPressed(0, Control.PhoneLeft))
				{
					phoneScaleform.CallFunction("SET_INPUT_EVENT", 4);
					pressed = true;
				}

				if (pressed)
					Audio.PlaySoundFrontend("Menu_Navigate", "Phone_SoundSet_Default");
			}
		}
	}
}
