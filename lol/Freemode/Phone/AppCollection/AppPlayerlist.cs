using CitizenFX.Core;
using CitizenFX.Core.Native;
using System.Threading.Tasks;

namespace Freeroam.Freemode.Phone.AppCollection
{
	class AppPlayerlist : IPhoneApp
	{
		private Scaleform phoneScaleform;
		private int selected;

		public void Init(Scaleform phoneScaleform)
		{
			this.phoneScaleform = phoneScaleform;
		}

		public async Task OnTick()
		{
			await Task.FromResult(0);

			phoneScaleform.CallFunction("SET_SOFT_KEYS", 3, true, 4);
			int slot = 0;
			for (int i = 0; i < PlayerList.MaxPlayers; i++)
			{
				if (API.NetworkIsPlayerActive(i))
				{
					Player player = new Player(i);
					phoneScaleform.CallFunction("SET_DATA_SLOT", 2, slot++, -1, player.Name);
				}
			}
			phoneScaleform.CallFunction("SET_DATA_SLOT", 2, slot++, -1, "Test");
			phoneScaleform.CallFunction("DISPLAY_VIEW", 2, selected);

			if (Game.IsControlJustPressed(0, Control.PhoneUp))
			{
				if (--selected < 0)
					selected = slot - 1;
				Audio.PlaySoundFrontend("Menu_Navigate", "Phone_SoundSet_Default");
			}
			else if (Game.IsControlJustPressed(0, Control.PhoneDown))
			{
				if (++selected > slot - 1)
					selected = 0;
				Audio.PlaySoundFrontend("Menu_Navigate", "Phone_SoundSet_Default");
			}
			else if (Game.IsControlJustPressed(0, Control.PhoneCancel))
				PhoneAppStarter.MainApp();
		}

		public void Stop()
		{
			
		}
	}
}
