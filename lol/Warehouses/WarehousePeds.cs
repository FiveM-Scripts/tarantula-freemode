using CitizenFX.Core;
using CitizenFX.Core.Native;
using Freeroam.Freemode.FreemodePlayer.Freemode;
using Freeroam.Util;
using FreeroamShared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Freeroam.Warehouses
{
	class WarehousePeds : BaseScript
	{
		private List<Ped> peds;
		private bool pedsSpawned;
		
		public WarehousePeds()
		{
			EventHandlers[EventType.WAREHOUSE_IN] += new Action(SpawnWarehousePeds);
			EventHandlers[EventType.WAREHOUSE_OUT] += new Action(DespawnWarehousePeds);

			Tick += OnTick;
		}

		private async Task OnTick()
		{
			await Delay(1000);

			if (WarehouseState.IsInsideWarehouse)
			{
				foreach (Ped ped in peds)
				{
					if (API.GetRandomIntInRange(0, 50) == 25)
					{
						ped.PlayAmbientSpeech("CHAT_STATE", SpeechModifier.ForceShouted);
						if (API.IsAmbientSpeechPlaying(ped.Handle))
						{
							while (API.IsAmbientSpeechPlaying(ped.Handle))
								await Delay(100);
							if (API.GetRandomIntInRange(0, 100) < 50)
								peds[API.GetRandomIntInRange(0, peds.Count)].PlayAmbientSpeech("CHAT_RESP", SpeechModifier.ForceShouted);
						}
					}
				}
			}
		}

		private async void SpawnWarehousePeds()
		{
			if (WarehouseState.IsInsideWarehouse && !pedsSpawned)
			{
				peds = new List<Ped>();
				Ped ped1 = await CreateWarehousePed(PedHash.Autoshop01SMM, new Vector3(978.4f, -3018.0f, -39.6f), 2.5f);
				ped1._StartScenario("WORLD_HUMAN_CLIPBOARD");
				Ped ped2 = await CreateWarehousePed(PedHash.Autoshop01SMM, new Vector3(991.5f, -2988.4f, -39.6f), 356.4f);
				ped2._StartScenario("WORLD_HUMAN_WELDING");
				Ped ped3 = await CreateWarehousePed(PedHash.Autoshop02SMM, new Vector3(973.2f, -3026.3f, -39.2f), 281.6f);
				ped3._StartScenario("WORLD_HUMAN_HAMMERING");
				Ped ped4 = await CreateWarehousePed(PedHash.Autoshop02SMM, new Vector3(953.7f, -3013.3f, -39.6f), 112.7f);
				ped4._StartScenario("WORLD_HUMAN_HAMMERING");
				Ped ped5 = await CreateWarehousePed(PedHash.Car3Guy1, new Vector3(1012.1f, -3009.4f, -35.9f), 94.9f);
				ped5._StartScenario("WORLD_HUMAN_STAND_MOBILE_UPRIGHT");
				Ped ped6 = await CreateWarehousePed(PedHash.Autoshop02SMM, new Vector3(1006.4f, -3003.9f, -39.6f), 268.5f);
				ped6._StartScenario("WORLD_HUMAN_CLIPBOARD");
				Ped ped7 = await CreateWarehousePed(PedHash.Car3Guy2, new Vector3(1008.9f, -3011.9f, -39.6f), 271.1f);
				ped7._StartScenario("WORLD_HUMAN_WINDOW_SHOP_BROWSE");
				Ped ped8 = await CreateWarehousePed(PedHash.Autopsy01SMY, new Vector3(963.0f, -2995.1f, -39.6f), 274.5f);
				ped8._StartScenario("WORLD_HUMAN_WINDOW_SHOP_BROWSE");
				Ped ped9 = await CreateWarehousePed(PedHash.Autoshop01SMM, new Vector3(962.5f, -3002.6f, -39.6f), 185.6f);
				ped9._StartScenario("WORLD_HUMAN_STAND_MOBILE");
				Ped ped10 = await CreateWarehousePed(PedHash.Autopsy01SMY, new Vector3(1008.2f, -3020.1f, -39.6f), 8.4f);
				ped10._StartScenario("WORLD_HUMAN_STAND_IMPATIENT");
				Ped ped11 = await CreateWarehousePed(PedHash.Armymech01SMY, new Vector3(966.5f, -2992.7f, -39.6f), 94.2f);
				ped11._StartScenario("WORLD_HUMAN_SMOKING_POT");
				Ped ped12 = await CreateWarehousePed(PedHash.Armymech01SMY, new Vector3(953.5f, -3014.6f, -39.6f), 187.2f);
				ped12._StartScenario("WORLD_HUMAN_WELDING");

				pedsSpawned = true;
			}
		}

		private async Task<Ped> CreateWarehousePed(Model model, Vector3 pos, float heading = 0f)
		{
			model.Request();
			while (!model.IsLoaded)
				await Delay(1);

			Ped ped = await EntityUtil.CreatePed(model, PedType.PED_TYPE_SPECIAL, pos, heading, false);
			ped.RelationshipGroup = RelationshipGroupHolder.WarehousePeds;
			ped.CanRagdoll = false;
			ped.AlwaysKeepTask = true;
			peds.Add(ped);
			return ped;
		}

		private void DespawnWarehousePeds()
		{
			if (pedsSpawned)
			{
				foreach (Ped ped in peds)
					if (ped.Exists())
						ped.Delete();

				pedsSpawned = false;
			}
		}
	}
}
