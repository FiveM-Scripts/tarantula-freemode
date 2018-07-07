using CitizenFX.Core;
using CitizenFX.Core.Native;
using FreeroamShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Freeroam.Freemode.Phone.AppCollection
{
	public struct Message
	{
		public string SenderName { get; private set; }
		public string SenderMessage { get; private set; }
		public string SenderCharImg { get; private set; }
		public TimeSpan Timestamp { get; private set; }

		public Message(string senderName, string message, string senderCharImg)
		{
			SenderName = senderName;
			SenderMessage = message;
			SenderCharImg = senderCharImg;
			int h = 0, m = 0, s = 0;
			API.NetworkGetServerTime(ref h, ref m, ref s);
			Timestamp = new TimeSpan(h, m, s);
		}
	}

	public class MessagesHolder : BaseScript
	{
		public static List<Message> Messages { get; } = new List<Message>();

		public MessagesHolder()
		{
			EventHandlers[Events.MESSAGE_FORWARD] += new Action<string, string, string>(AddMessage);
			EventHandlers[Events.MESSAGE_FORWARD_PLAYER] += new Action<int, string>(AddPlayerMessage);
		}

		public static void AddMessage(string sender, string message, string charImg)
		{
			NotifyNewMessage(sender, message, charImg);
			Messages.Add(new Message(sender, message, charImg));
		}

		public static async void AddPlayerMessage(int senderServerId, string message)
		{
			Player sender = new Player(API.GetPlayerFromServerId(senderServerId));
			int senderHeadshotHandle = API.RegisterPedheadshot(sender.Character.Handle);
			while (!API.IsPedheadshotReady(senderHeadshotHandle))
				await Delay(1);
			string senderHeadshotTxd = API.GetPedheadshotTxdString(senderHeadshotHandle);
			NotifyNewMessage(sender.Name, message, senderHeadshotTxd);
			Messages.Add(new Message(sender.Name, message, senderHeadshotTxd));
		}

		private static void NotifyNewMessage(string sender, string message, string charImg)
		{
			API.SetNotificationTextEntry("STRING");
			API.AddTextComponentString(message);
			API.SetNotificationMessage(charImg, charImg, true, 1, Strings.PHONE_APP_MESSAGES_RECEIVE, sender);
			API.DrawNotification(true, true);
			Audio.ReleaseSound(Audio.PlaySoundFrontend("Text_Arrive_Tone", "Phone_SoundSet_Default"));
		}
	}

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

			int slot = 0;
			if (inSubMenu)
				phoneScaleform.CallFunction("SET_DATA_SLOT", 7, 0, selectedMessage.SenderName, selectedMessage.SenderMessage, selectedMessage.SenderCharImg);
			else
				foreach (Message message in Enumerable.Reverse(MessagesHolder.Messages))
					phoneScaleform.CallFunction("SET_DATA_SLOT", 6, slot++, message.Timestamp.Hours, message.Timestamp.Minutes, -1,
						message.SenderName, message.SenderMessage);
			phoneScaleform.CallFunction("DISPLAY_VIEW", inSubMenu ? 7 : 6, inSubMenu ? 0 : selected);

			phoneScaleform.CallFunction("SET_SOFT_KEYS", (int) PhoneSelectSlot.SLOT_RIGHT, true, (int) PhoneSelectIcon.ICON_BACK);
			phoneScaleform.CallFunction("SET_SOFT_KEYS", (int) PhoneSelectSlot.SLOT_LEFT, true,
				slot > 0 && !inSubMenu ? (int) PhoneSelectIcon.ICON_SELECT : (int) PhoneSelectIcon.ICON_BLANK);

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
					selectedMessage = MessagesHolder.Messages[MessagesHolder.Messages.Count - 1 - selected];
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
					Audio.ReleaseSound(Audio.PlaySoundFrontend("Hang_Up", "Phone_SoundSet_Michael"));
					inSubMenu = false;
				}
			}

			if (pressed)
				Audio.ReleaseSound(Audio.PlaySoundFrontend("Menu_Navigate", "Phone_SoundSet_Default"));
		}

		public void Stop()
		{
			
		}
	}
}
