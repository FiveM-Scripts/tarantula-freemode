using CitizenFX.Core;
using System.Threading.Tasks;

namespace Freeroam.Freemode.Phone.AppCollection
{
	class AppMessages : IPhoneApp
	{
		private Scaleform phoneScaleform;

		public void Init(Scaleform phoneScaleform)
		{
			this.phoneScaleform = phoneScaleform;
		}

		public async Task OnTick()
		{
			await Task.FromResult(0);

			phoneScaleform.CallFunction("SET_SOFT_KEYS", 3, true, 4);
			for (int i = 0; i < 9; i++)
				phoneScaleform.CallFunction("SET_DATA_SLOT", 6, i, "Hi", "Hi");
			phoneScaleform.CallFunction("DISPLAY_VIEW", 6, 0);

			if (Game.IsControlJustPressed(0, Control.PhoneCancel))
				PhoneAppStarter.MainApp();
		}

		public void Stop()
		{
			
		}
	}
}
