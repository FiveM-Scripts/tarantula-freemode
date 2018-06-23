using CitizenFX.Core;
using CitizenFX.Core.Native;
using System.Threading.Tasks;

namespace Freeroam.Phone
{
	class PhoneStarter : BaseScript
	{
		private Scaleform phoneScaleform;

		public PhoneStarter()
		{
			Tick += OnTick;
		}

		private async Task OnTick()
		{
			await Task.FromResult(0);

			if (Game.IsControlJustPressed(0, Control.Phone) && !PhoneState.IsShown && !PhoneState.Block)
			{
				phoneScaleform = new Scaleform("CELLPHONE_IFRUIT");
				TriggerEvent("freemode:heyItsAPhoneScaleform!", phoneScaleform.Handle);
				PhoneState.IsShown = true;
				Audio.PlaySoundFrontend("Pull_Out", "Phone_SoundSet_Default");
				API.SetMobilePhonePosition(58f, -21f, -60f);
				API.SetMobilePhoneRotation(-90f, 0f, 0f, 0);
				API.SetMobilePhoneScale(285f);
				API.CreateMobilePhone(0);
			}
			else if (PhoneState.Block && PhoneState.IsShown)
			{
				PhoneState.IsShown = false;
				API.DestroyMobilePhone();
				phoneScaleform.Dispose();
			}

			if (PhoneState.IsShown)
			{
				int h = 0, m = 0, s = 0;
				API.NetworkGetServerTime(ref h, ref m, ref s);
				phoneScaleform.CallFunction("SET_HEADER", "Freemode");
				phoneScaleform.CallFunction("SET_TITLEBAR_TIME", h, m);
				phoneScaleform.CallFunction("SET_SLEEP_MODE", false);
				phoneScaleform.CallFunction("SET_SOFT_KEYS", 3, true, 4);
				phoneScaleform.CallFunction("SET_BACKGROUND_IMAGE", 0);
				phoneScaleform.CallFunction("SET_THEME", 5);
				for (int i = 0; i < 9; i++)
					phoneScaleform.CallFunction("SET_DATA_SLOT", 1, i, 42);
				int renderId = 0;
				API.GetMobilePhoneRenderId(ref renderId);
				API.SetTextRenderId(renderId);
				API.DrawScaleformMovie(phoneScaleform.Handle, 0.0998f, 0.1775f, 0.1983f, 0.364f, 255, 255, 255, 255, 0);
				API.SetTextRenderId(1);
			}
		}
	}
}
