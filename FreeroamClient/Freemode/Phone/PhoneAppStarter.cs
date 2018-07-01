using CitizenFX.Core;
using Freeroam.Freemode.Phone.AppCollection;
using System.Threading.Tasks;

namespace Freeroam.Freemode.Phone
{
	public class PhoneAppStarter : BaseScript
	{
		private static IPhoneApp currentApp;

		public PhoneAppStarter()
		{
			Tick += OnTick;
		}

		public static void InitApp(IPhoneApp app)
		{
			if (currentApp != null)
				currentApp.Stop();
			currentApp = app;
			app.Init(PhoneState.PhoneScaleform);
		}

		public static void MainApp()
		{
			Stop();
			InitApp(new AppMain());
		}

		public static void Stop()
		{
			if (currentApp != null)
			{
				currentApp.Stop();
				Audio.ReleaseSound(Audio.PlaySoundFrontend("Hang_Up", "Phone_SoundSet_Michael"));
				currentApp = null;
			}
		}

		private async Task OnTick()
		{
			await Task.FromResult(0);

			if (currentApp != null)
				await currentApp.OnTick();
		}
	}
}
