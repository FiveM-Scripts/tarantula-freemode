﻿using CitizenFX.Core;
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
			EventHandlers["freeroam:sendPlayerMessage"] += new Action<Player, int, string>(SendPlayerMessage);
			EventHandlers["freeroam:sendIdiotMessage"] += new Action<Player, string>(SendIdiotMessage);
		}

		private async void SendIdiotMessage([FromSource] Player player, string message)
		{
			HttpClient httpClient = new HttpClient();
			HttpResponseMessage response = await httpClient.PostAsync("http://94.130.180.216:8081/idiot",
				new StringContent(JsonConvert.SerializeObject(new Dictionary<string, string> { ["message"] = message }), Encoding.UTF8, "application/json"));
			if (response.StatusCode == HttpStatusCode.OK)
				TriggerClientEvent(player, "freeroam:forwardMessage", "Assistant", (string) JObject.Parse(await response.Content.ReadAsStringAsync())["response"],
					"CHAR_MP_BIKER_BOSS");
		}

		private void SendPlayerMessage([FromSource] Player player, int targetServerId, string message)
		{
			TriggerClientEvent(Players[targetServerId], "freeroam:forwardPlayerMessage", player.Handle, message);
		}
	}
}
