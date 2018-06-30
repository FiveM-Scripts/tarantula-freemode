using CitizenFX.Core;
using CitizenFX.Core.Native;
using Freeroam.Freemode.Phone.AppCollection;
using System.Threading.Tasks;

namespace Freeroam.Freemode.Phone
{
	public class PhoneStarter : BaseScript
	{
		private static Scaleform phoneScaleform;

		public PhoneStarter()
		{
			Tick += OnTick;
		}

		private async Task OnTick()
		{
			await Task.FromResult(0);

			if (!PhoneState.IsShown)
			{
				if (Game.IsControlJustPressed(0, Control.Phone) && !PhoneState.IsShown && !PhoneState.Block
					&& !API.IsPauseMenuActive() && !Game.PlayerPed.IsDead)
				{
					phoneScaleform = new Scaleform("CELLPHONE_IFRUIT");
					PhoneState.PhoneScaleform = phoneScaleform;
					PhoneState.IsShown = true;
					Audio.PlaySoundFrontend("Pull_Out", "Phone_SoundSet_Default");
					API.SetMobilePhonePosition(58f, -21f, -60f);
					API.SetMobilePhoneRotation(-90f, 0f, 0f, 0);
					API.SetMobilePhoneScale(285f);
					API.CreateMobilePhone(0);

					PhoneAppStarter.MainApp();
				}
			}
			else if (PhoneState.Block || API.IsPauseMenuActive() || Game.PlayerPed.IsDead)
				StopPhone();

			if (PhoneState.IsShown)
			{
				int h = 0, m = 0, s = 0;
				API.NetworkGetServerTime(ref h, ref m, ref s);
				phoneScaleform.CallFunction("SET_TITLEBAR_TIME", h, m);
				phoneScaleform.CallFunction("SET_SLEEP_MODE", false);
				phoneScaleform.CallFunction("SET_BACKGROUND_IMAGE", 0);
				phoneScaleform.CallFunction("SET_THEME", 5);
				Vector3 playerPos = Game.PlayerPed.Position;
				phoneScaleform.CallFunction("SET_SIGNAL_STRENGTH", API.GetZoneScumminess(API.GetZoneAtCoords(playerPos.X, playerPos.Y, playerPos.Z)));
				
				int renderId = 0;
				API.GetMobilePhoneRenderId(ref renderId);
				API.SetTextRenderId(renderId);
				API.DrawScaleformMovie(phoneScaleform.Handle, 0.0998f, 0.1775f, 0.1983f, 0.364f, 255, 255, 255, 255, 0);
				API.SetTextRenderId(1);
			}
		}

		public static void StopPhone()
		{
			PhoneAppStarter.Stop();
			PhoneState.IsShown = false;
			API.DestroyMobilePhone();
			phoneScaleform.Dispose();
		}
	}
}
