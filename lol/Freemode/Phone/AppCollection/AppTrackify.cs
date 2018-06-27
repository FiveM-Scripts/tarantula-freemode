using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace Freeroam.Freemode.Phone.AppCollection
{
	public class AppTrackify : IPhoneApp
	{
		private Scaleform phoneScaleform;

		public void Init(Scaleform phoneScaleform)
		{
			this.phoneScaleform = phoneScaleform;
		}

		public async Task OnTick()
		{
			await Task.FromResult(0);

			phoneScaleform.CallFunction("DISPLAY_VIEW", 23);
			phoneScaleform.CallFunction("SET_DATA_SLOT", 23, 0, -99, 0, 100, 1, false);
			phoneScaleform.CallFunction("SET_DATA_SLOT", 23, 1, 50, 100, 25, 105);
			phoneScaleform.CallFunction("SET_SOFT_KEYS", (int) PhoneSelectSlot.SLOT_LEFT, true, (int) PhoneSelectIcon.ICON_BLANK);

			if (Game.IsControlJustPressed(0, Control.PhoneCancel))
				PhoneAppStarter.MainApp();
		}

		public void Stop()
		{
			
		}
	}
}
