using CitizenFX.Core;
using CitizenFX.Core.UI;
using System.Linq;
using System.Threading.Tasks;

namespace Freeroam.Freemode.Phone.AppCollection
{
	class AppPlayerlist : IPhoneApp
	{
		private Scaleform phoneScaleform;
		private int selected;
		private bool inSubMenu;
		private Player selectedPlayer;

		public void Init(Scaleform phoneScaleform)
		{
			this.phoneScaleform = phoneScaleform;
		}

		public async Task OnTick()
		{
			await Task.FromResult(0);

			phoneScaleform.CallFunction("SET_SOFT_KEYS", 3, true, 4);
			phoneScaleform.CallFunction("DISPLAY_VIEW", 2, selected);

			int slot = 0;
			if (!inSubMenu)
				foreach (Player player in new PlayerList().Where(player => player != Game.Player))
					phoneScaleform.CallFunction("SET_DATA_SLOT", 2, slot++, -1, player.Name);
			else
			{
				phoneScaleform.CallFunction("SET_DATA_SLOT", 2, slot++, -1, "Send Message");
			}

			if (!inSubMenu)
				phoneScaleform.CallFunction("SET_HEADER", "Playerlist");
			else
				phoneScaleform.CallFunction("SET_HEADER", selectedPlayer.Name);

			bool pressed = false;
			if (Game.IsControlJustPressed(0, Control.PhoneUp) && slot > 0)
			{
				if (--selected < 0)
					selected = slot - 1;
				pressed = true;
			}
			else if (Game.IsControlJustPressed(0, Control.PhoneDown) && slot > 0)
			{
				if (++selected > slot - 1)
					selected = 0;
				pressed = true;
			}
			else if (Game.IsControlJustPressed(0, Control.PhoneSelect) && slot > 0)
			{
				
				if (!inSubMenu)
				{
					inSubMenu = true;
					int selectedTemp = selected;
					if (selected == Game.Player.Handle)
						selectedTemp++;
					selectedPlayer = new Player(selectedTemp);
				}
				else
				{
					string message = await Game.GetUserInput(60);
					if (message.Length == 0)
						Screen.ShowNotification("~r~Please enter a message.");
					else
					{
						BaseScript.TriggerServerEvent("freeroam:sendMessage", selectedPlayer.ServerId, message);
						Screen.ShowNotification("~g~Message sent.");
					}
				}
				selected = 0;
				pressed = true;
			}
			else if (Game.IsControlJustPressed(0, Control.PhoneCancel))
			{
				if (!inSubMenu)
					PhoneAppStarter.MainApp();
				else
				{
					Audio.PlaySoundFrontend("Hang_Up", "Phone_SoundSet_Michael");
					inSubMenu = false;
				}
			}

			if (pressed)
				Audio.PlaySoundFrontend("Menu_Navigate", "Phone_SoundSet_Default");
		}

		public void Stop()
		{
			
		}
	}
}
