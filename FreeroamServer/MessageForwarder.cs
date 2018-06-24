using CitizenFX.Core;
using System;

namespace FreeroamServer
{
	class MessageForwarder : BaseScript
	{
		public MessageForwarder()
		{
			EventHandlers["freeroam:sendPlayerMessage"] += new Action<Player, int, string>(SendMessage);
		}

		private void SendMessage([FromSource] Player player, int targetServerId, string message)
		{
			TriggerClientEvent(Players[targetServerId], "freeroam:forwardPlayerMessage", player.Handle, message);
		}
	}
}
