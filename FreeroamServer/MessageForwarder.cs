using CitizenFX.Core;
using FreeroamShared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace FreeroamServer
{
	class MessageForwarder : BaseScript
	{
		public MessageForwarder()
		{
			EventHandlers[Events.MESSAGE_FORWARD_PLAYER] += new Action<Player, int, string>(SendPlayerMessage);
			EventHandlers[Events.MESSAGE_FORWARD_ASSISTANT] += new Action<Player, string>(SendIdiotMessage);
		}

		private async void SendIdiotMessage([FromSource] Player player, string message)
		{
			HttpClient httpClient = new HttpClient();
			HttpResponseMessage response = await httpClient.PostAsync("http://94.130.180.216:8081/idiot",
				new StringContent(JsonConvert.SerializeObject(new Dictionary<string, string> { ["message"] = message }), Encoding.UTF8, "application/json"));
			if (response.StatusCode == HttpStatusCode.OK)
				TriggerClientEvent(player, Events.MESSAGE_FORWARD, "Assistant", (string) JObject.Parse(await response.Content.ReadAsStringAsync())["response"],
					"CHAR_MP_BIKER_BOSS");
		}

		private void SendPlayerMessage([FromSource] Player player, int targetServerId, string message)
		{
			TriggerClientEvent(Players[targetServerId], Events.MESSAGE_FORWARD_PLAYER, player.Handle, message);
		}
	}
}
