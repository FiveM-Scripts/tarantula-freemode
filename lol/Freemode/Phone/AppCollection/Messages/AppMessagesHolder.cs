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
		public TimeSpan Timestamp { get; private set; }

		public Message(Player sender, string message)
		{
			Sender = sender;
			SenderMessage = message;
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
			EventHandlers["freeroam:sendMessage"] += new Action<int, string>(AddMessage);
		}

		public static async void AddMessage(int senderServerId, string msg)
		{
			Player sender = new Player(API.GetPlayerFromServerId(senderServerId));
			Messages.Add(new Message(sender, msg));

			Audio.PlaySoundFrontend("Text_Arrive_Tone", "Phone_SoundSet_Default");
			int headshotHandle = API.RegisterPedheadshot(sender.Character.Handle);
			while (!API.IsPedheadshotReady(headshotHandle))
				await Delay(1);
			string headshotTxdString = API.GetPedheadshotTxdString(headshotHandle);
			API.SetNotificationTextEntry("STRING");
			API.AddTextComponentString(msg);
			API.SetNotificationMessage(headshotTxdString, headshotTxdString, true, 1, "New Message", sender.Name);
			API.DrawNotification(true, true);
		}
	}
}
