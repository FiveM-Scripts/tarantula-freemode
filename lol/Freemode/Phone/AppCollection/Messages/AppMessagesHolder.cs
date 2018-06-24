using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;

namespace Freeroam.Freemode.Phone.AppCollection.Messages
{
	public struct PlayerMessage
	{
		public string SenderName { get; private set; }
		public string SenderMugshotTxd { get; private set; }
		public string SenderMessage { get; private set; }
		public TimeSpan Timestamp { get; private set; }

		public PlayerMessage(string senderName, string senderMugshotTxd, string message)
		{
			SenderName = senderName;
			SenderMugshotTxd = senderMugshotTxd;
			SenderMessage = message;
			int h = 0, m = 0, s = 0;
			API.NetworkGetServerTime(ref h, ref m, ref s);
			Timestamp = new TimeSpan(h, m, s);
		}
	}

	public class MessagesHolder : BaseScript
	{
		public static List<PlayerMessage> Messages { get; } = new List<PlayerMessage>();

		public MessagesHolder()
		{
			EventHandlers["freeroam:forwardPlayerMessage"] += new Action<int, string>(AddMessage);
		}

		public static async void AddMessage(int senderServerId, string msg)
		{
			Player sender = new Player(API.GetPlayerFromServerId(senderServerId));
			int senderHeadshotHandle = API.RegisterPedheadshot(sender.Character.Handle);
			while (!API.IsPedheadshotReady(senderHeadshotHandle))
				await Delay(1);
			string senderHeadshotTxd = API.GetPedheadshotTxdString(senderHeadshotHandle);
			API.SetNotificationTextEntry("STRING");
			API.AddTextComponentString(msg);
			API.SetNotificationMessage(senderHeadshotTxd, senderHeadshotTxd, true, 1, "New Message", sender.Name);
			API.DrawNotification(true, true);
			Audio.PlaySoundFrontend("Text_Arrive_Tone", "Phone_SoundSet_Default");

			Messages.Add(new PlayerMessage(sender.Name, senderHeadshotTxd, msg));
		}
	}
}
