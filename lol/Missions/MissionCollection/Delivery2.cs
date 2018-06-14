using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using Freeroam.Freemode.Relationship;
using Freeroam.Util;
using Freeroam.Warehouses;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace Freeroam.Missions.MissionCollection
{
	public class Delivery2 : IMission
	{
		private Vehicle deliveryCar;
		private Blip deliveryCarBlip;
		private RelationshipGroup enemyRelationshipGroup;
		private List<Ped> enemies;
		private List<Vehicle> enemyVehicles;
		private bool enteredCar;
		private Blip warehouseImportBlip;

		public async Task Prepare()
		{
			deliveryCar = await EntityUtil.CreateVehicle(VehicleHash.Voltic2, new Vector3(667.4f, -756f, 23.7f), 171.5f);

			enemyRelationshipGroup = World.AddRelationshipGroup("_DELIVERY2_ENEMIES");
			enemyRelationshipGroup.SetRelationshipBetweenGroups(RelationshipGroupHolder.PlayerRelationship, Relationship.Dislike, true);

			enemies = new List<Ped>();
			Ped enemy1 = await CreateEnemyPed(PedHash.BikeHire01, new Vector3(667.8f, -768.8f, 23.6f), 184.8f);
			enemy1._StartScenario("WORLD_HUMAN_AA_SMOKE");
			Ped enemy2 = await CreateEnemyPed(PedHash.Car3Guy1, new Vector3(668.1f, -773f, 23.5f), 6f);
			enemy2._StartScenario("WORLD_HUMAN_DRUG_DEALER");
			Ped enemy3 = await CreateEnemyPed(PedHash.Car3Guy2, new Vector3(667f, -754.4f, 31.3f));
			enemy3._StartScenario("WORLD_HUMAN_BINOCULARS");
			Ped enemy4 = await CreateEnemyPed(PedHash.CarBuyerCutscene, new Vector3(672.1f, -781.9f, 23.5f), 101.9f);
			enemy4._StartScenario("WORLD_HUMAN_DRINKING");
			Ped enemy5 = await CreateEnemyPed(PedHash.MexGang01GMY, new Vector3(656.8f, -793.8f, 23.5f), 1.6f);
			enemy5._StartScenario("WORLD_HUMAN_WELDING");
			enemyVehicles = new List<Vehicle>();
			Vehicle vehicle1 = await EntityUtil.CreateVehicle(VehicleHash.SabreGT, new Vector3(654.5f, -729.8f, 24.2f), 181.1f);
			enemyVehicles.Add(vehicle1);
			Ped enemy6 = await CreateEnemyPed(PedHash.MexGang01GMY, new Vector3());
			enemy6.Weapons.Give(WeaponHash.SMGMk2, int.MaxValue, false, true);
			enemy6.SetIntoVehicle(vehicle1, VehicleSeat.Driver);
			Vehicle vehicle2 = await EntityUtil.CreateVehicle(VehicleHash.Ninef, new Vector3(669.4f, -706.8f, 24.6f), 177.5f);
			enemyVehicles.Add(vehicle2);
			Ped enemy7 = await CreateEnemyPed(PedHash.MexGoon01GMY, new Vector3());
			enemy7.Weapons.Give(WeaponHash.MicroSMG, int.MaxValue, false, true);
			enemy7.SetIntoVehicle(vehicle2, VehicleSeat.Driver);
			Ped enemy8 = await CreateEnemyPed(PedHash.MexGoon02GMY, new Vector3());
			enemy8.Weapons.Give(WeaponHash.MicroSMG, int.MaxValue, false, true);
			enemy8.SetIntoVehicle(vehicle2, VehicleSeat.Passenger);
			Ped enemy9 = await CreateEnemyPed(PedHash.MexThug01AMY, new Vector3(677.3f, -861.9f, 23.5f), 182.4f);
			enemy9._StartScenario("WORLD_HUMAN_CONST_DRILL");
			Ped enemy10 = await CreateEnemyPed(PedHash.MexBoss01GMM, new Vector3(683.5f, -861.9f, 23.6f), 64.5f);
			enemy10._StartScenario("WORLD_HUMAN_COP_IDLES");

			MissionMusic.Play("BIKER_SYG_START");

			await Task.FromResult(0);
		}

		private async Task<Ped> CreateEnemyPed(Model model, Vector3 pos, float heading = 0f)
		{
			Ped ped = await EntityUtil.CreatePed(model, PedType.PED_TYPE_MISSION, pos, heading);
			ped.Weapons.Give(WeaponHash.AssaultRifleMk2, int.MaxValue, false, true);
			ped.RelationshipGroup = enemyRelationshipGroup;
			ped.IsEnemy = true;
			API.SetPedEnemyAiBlip(ped.Handle, true);
			API.HideSpecialAbilityLockonOperation(ped.Handle, false);
			enemies.Add(ped);
			return ped;
		}

		public void Start()
		{
			deliveryCarBlip = deliveryCar.AttachBlip();
			deliveryCarBlip.Color = BlipColor.Blue;
			deliveryCarBlip.ShowRoute = true;

			Screen.ShowSubtitle("Steal the ~b~Rocket Voltic~w~.", 10000);
		}

		public async Task OnTick()
		{
			if (deliveryCar.EngineHealth <= 0f)
			{
				Screen.ShowSubtitle("~r~The Rocket Voltic was destroyed.", 10000);
				MissionStarter.RequestStopCurrentMission();
			}
			else
			{
				Vector4 importPointOrigin = WarehouseState.LastWarehouse.ImportExportPoint;
				Vector3 importPoint = new Vector3(importPointOrigin.X, importPointOrigin.Y, importPointOrigin.Z - 1);
				if (!enteredCar)
				{
					if (Game.PlayerPed.CurrentVehicle == deliveryCar)
					{
						Screen.ShowSubtitle("Bring the ~b~Rocket Voltic~w~ to the ~g~Warehouse~w~.");
						warehouseImportBlip = World.CreateBlip(importPoint);
						warehouseImportBlip.Color = BlipColor.Green;
						warehouseImportBlip.ShowRoute = true;
						MissionMusic.Play("BIKER_SYG_ATTACKED");

						foreach (Ped enemy in enemies)
							enemy.Task.FightAgainst(Game.PlayerPed);

						enteredCar = true;
					}
				}
				else
				{
					World.DrawMarker(MarkerType.VerticalCylinder, importPoint, Vector3.Zero, Vector3.Zero, new Vector3(3f, 3f, 3f),
						Color.FromArgb(127, 0, 0, 255));
					if (World.GetDistance(deliveryCar.Position, importPoint) < 5f)
					{
						deliveryCar.ApplyForce(-deliveryCar.Velocity);
						Game.PlayerPed.Task.DriveTo(deliveryCar, Game.PlayerPed.Position, 0f, 0f);
						await WarehouseTeleporter.RequestTeleport(WarehouseTeleport.Inside);
						deliveryCar.Delete();
						// TODO: Properly save
						WarehouseState.VehicleAmount++;
						MissionStarter.RequestStopCurrentMission();
					}
				}
			}

			await Task.FromResult(0);
		}

		public void Stop()
		{
			if (deliveryCar.Exists())
				deliveryCar.MarkAsNoLongerNeeded();
			if (deliveryCarBlip.Exists())
				deliveryCarBlip.Delete();
			foreach (Ped enemy in enemies)
				enemy.MarkAsNoLongerNeeded();
			foreach (Vehicle vehicle in enemyVehicles)
				vehicle.MarkAsNoLongerNeeded();
			if (warehouseImportBlip != null)
				warehouseImportBlip.Delete();

			MissionMusic.Play("BIKER_MP_MUSIC_STOP", false);
		}
	}
}
