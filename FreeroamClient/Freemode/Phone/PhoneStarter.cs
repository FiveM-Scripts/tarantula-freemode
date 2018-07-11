using CitizenFX.Core;
using CitizenFX.Core.Native;
using System.Threading.Tasks;

namespace Freeroam.Freemode.Phone
{
	public class PhoneStarter : BaseScript
	{
		private static Scaleform phoneScaleform;
		private float phonePullUpProgress;

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
					Audio.ReleaseSound(Audio.PlaySoundFrontend("Pull_Out", "Phone_SoundSet_Default"));
					API.SetMobilePhoneScale(285f);
					API.CreateMobilePhone(0);

					phonePullUpProgress = 20f;
					PhoneAppStarter.MainApp();
				}
			}
			else if (PhoneState.Block || API.IsPauseMenuActive() || Game.PlayerPed.IsDead)
				StopPhone();

			if (PhoneState.IsShown)
			{
				API.SetMobilePhonePosition(58f, -21f - phonePullUpProgress, -60f);
				API.SetMobilePhoneRotation(-90f, -phonePullUpProgress * 4, 0f, 0);
				phonePullUpProgress = phonePullUpProgress - 3 < 1 ? 0 : phonePullUpProgress - 3;

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
