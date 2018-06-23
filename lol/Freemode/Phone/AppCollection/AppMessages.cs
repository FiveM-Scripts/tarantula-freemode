using CitizenFX.Core;
using Freeroam.Freemode.Phone.AppCollection.Messages;
using System.Linq;
using System.Threading.Tasks;

namespace Freeroam.Freemode.Phone.AppCollection
{
	public class AppMessages : IPhoneApp
	{
		private Scaleform phoneScaleform;
		private int selected;
		private bool inSubMenu;
		private Message selectedMessage;

		public void Init(Scaleform phoneScaleform)
		{
			this.phoneScaleform = phoneScaleform;
		}

		public async Task OnTick()
		{
			await Task.FromResult(0);

			phoneScaleform.CallFunction("SET_SOFT_KEYS", 3, true, 4);
			int slot = 0;
			if (!inSubMenu)
			{
				foreach (Message message in Enumerable.Reverse(MessagesHolder.Messages))
					phoneScaleform.CallFunction("SET_DATA_SLOT", 6, slot++, message.Timestamp.Hours, message.Timestamp.Minutes, -1,
						message.Sender.Name, message.SenderMessage);
				phoneScaleform.CallFunction("DISPLAY_VIEW", 6, selected);
			}
			else
			{
				phoneScaleform.CallFunction("SET_DATA_SLOT", 7, 0, selectedMessage.Sender.Name, selectedMessage.SenderMessage, "CHAR_MULTIPLAYER");
				phoneScaleform.CallFunction("DISPLAY_VIEW", 7);
			}

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
					selectedMessage = MessagesHolder.Messages[selected];
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
