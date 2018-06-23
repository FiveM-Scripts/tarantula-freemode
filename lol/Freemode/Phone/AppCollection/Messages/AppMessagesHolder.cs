using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;

namespace Freeroam.Freemode.Phone.AppCollection.Messages
{
	public struct Message
	{
		public Player Sender { get; private set; }
		public string SenderMessage { get; private set; }

		public Message(Player sender, string message)
		{
			Sender = sender;
			SenderMessage = message;
		}
	}

	public class MessagesHolder : BaseScript
	{
		public static List<Message> Messages { get; } = new List<Message>();

		public MessagesHolder()
		{
			EventHandlers["freeroam:sendMessage"] += new Action<int, string>(AddMessage);
		}

		public static void AddMessage(int senderServerId, string msg)
		{
			Player sender = new Player(API.GetPlayerFromServerId(senderServerId));
			Messages.Add(new Message(sender, msg));

			Audio.PlaySoundFrontend("Text_Arrive_Tone", "Phone_SoundSet_Default");
			API.SetNotificationTextEntry("STRING");
			API.AddTextComponentString(msg);
			API.SetNotificationMessage("CHAR_MULTIPLAYER", "CHAR_MULTIPLAYER", true, 1, "New Message!", sender.Name);
			API.DrawNotification(true, true);
		}
	}
}
