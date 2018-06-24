﻿using CitizenFX.Core;
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

			int slot = 0;
			if (inSubMenu)
				phoneScaleform.CallFunction("SET_DATA_SLOT", 2, slot++, -1, "Send Message");
			else
				foreach (Player player in new PlayerList().Where(player => player != Game.Player))
					phoneScaleform.CallFunction("SET_DATA_SLOT", 2, slot++, -1, player.Name);
			phoneScaleform.CallFunction("SET_HEADER", inSubMenu ? selectedPlayer.Name : "Playerlist");
			phoneScaleform.CallFunction("DISPLAY_VIEW", 2, selected);

			phoneScaleform.CallFunction("SET_SOFT_KEYS", (int) PhoneSelectSlot.SLOT_RIGHT, true, (int) PhoneSelectIcon.ICON_BACK);
			phoneScaleform.CallFunction("SET_SOFT_KEYS", (int)PhoneSelectSlot.SLOT_LEFT, true,
				slot > 0 ? (int) PhoneSelectIcon.ICON_SELECT : (int) PhoneSelectIcon.ICON_BLANK);

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
					if (message != null)
					{
						message = message.Trim();
						if (message.Length == 0)
							Screen.ShowNotification("~r~Please enter a message.");
						else
						{
							BaseScript.TriggerServerEvent("freeroam:sendPlayerMessage", selectedPlayer.ServerId, message);
							Screen.ShowNotification("~g~Message sent.");
						}
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