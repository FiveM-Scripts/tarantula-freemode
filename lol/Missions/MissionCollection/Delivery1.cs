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
	public class Delivery1 : IMission
	{
		private Vehicle deliveryCar;
		private Blip deliveryCarBlip;
		private RelationshipGroup enemyRelationshipGroup;
		private List<Ped> enemies;
		private List<Vehicle> enemyVehicles;
		private Vehicle heli;
		private Blip heliBlip;
		private bool enteredCar;
		private Blip warehouseImportBlip;

		public async Task Prepare()
		{
			deliveryCar = await EntityUtil.CreateVehicle(VehicleHash.Kuruma2, new Vector3(666.4f, 676.9f, 128.5f), 193.4f);

			enemyRelationshipGroup = World.AddRelationshipGroup("_DELIVERY1_ENEMIES");
			enemyRelationshipGroup.SetRelationshipBetweenGroups(RelationshipGroupHolder.PlayerRelationship, Relationship.Dislike, true);

			enemies = new List<Ped>();
			Ped enemy1 = await CreateEnemyPed(PedHash.Korean01GMY, new Vector3(663.1f, 662.5f, 128.9f), 252.3f);
			enemy1._StartScenario("WORLD_HUMAN_AA_SMOKE");
			Ped enemy2 = await CreateEnemyPed(PedHash.Korean02GMY, new Vector3(667.5f, 661.1f, 128.9f), 70.3f);
			enemy2._StartScenario("WORLD_HUMAN_GUARD_PATROL");
			Ped enemy3 = await CreateEnemyPed(PedHash.Korean02GMY, new Vector3(780.2f, 571.4f, 127.5f), 330.4f);
			enemy3._StartScenario("WORLD_HUMAN_SMOKING");
			enemyVehicles = new List<Vehicle>();
			Vehicle vehicle1 = await EntityUtil.CreateVehicle(VehicleHash.Sentinel, new Vector3(851.3f, 504f, 125.9f), 343f);
			enemyVehicles.Add(vehicle1);
			Ped enemy4 = await CreateEnemyPed(PedHash.KorLieut01GMY, new Vector3());
			enemy4.SetIntoVehicle(vehicle1, VehicleSeat.Driver);
			Ped enemy5 = await CreateEnemyPed(PedHash.KorBoss01GMM, new Vector3());
			enemy5.SetIntoVehicle(vehicle1, VehicleSeat.Passenger);
			enemy5.Weapons.Give(WeaponHash.MicroSMG, int.MaxValue, true, true);
			Ped enemy6 = await CreateEnemyPed(PedHash.KorLieut01GMY, new Vector3(903.5f, 541.6f, 123.2f), 271.7f);
			enemy6._StartScenario("WORLD_HUMAN_SMOKING");
			Ped enemy7 = await CreateEnemyPed(PedHash.Korean01GMY, new Vector3(906.7f, 541.7f, 123.2f), 93.5f);
			enemy7._StartScenario("WORLD_HUMAN_GUARD_PATROL");
			Ped enemy8 = await CreateEnemyPed(PedHash.KorBoss01GMM, new Vector3(902.2f, 510.7f, 121.9f), 296f);
			enemy8._StartScenario("WORLD_HUMAN_STAND_IMPATIENT");

			MissionMusic.Play("IE_START_MUSIC");

			await Task.FromResult(0);
		}

		private async Task<Ped> CreateEnemyPed(Model model, Vector3 pos, float heading = 0f)
		{
			Ped ped = await EntityUtil.CreatePed(model, PedType.PED_TYPE_MISSION, pos, heading);
			ped.Weapons.Give(WeaponHash.PistolMk2, int.MaxValue, false, true);
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

			Screen.ShowSubtitle("Steal the ~b~Armored Kuruma~w~.", 10000);
		}

		public async Task OnTick()
		{
			if (deliveryCar.EngineHealth <= 0f)
			{
				Screen.ShowSubtitle("~r~The Armored Kuruma was destroyed.", 10000);
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
						Screen.ShowSubtitle("Bring the ~b~Armored Kuruma~w~ to the ~g~Warehouse~w~.");
						warehouseImportBlip = World.CreateBlip(importPoint);
						warehouseImportBlip.Color = BlipColor.Green;
						warehouseImportBlip.ShowRoute = true;
						MissionMusic.Play("IE_DELIVERING_ATTACK");

						heli = await EntityUtil.CreateVehicle(VehicleHash.Maverick, new Vector3(1509.8f, -224.2f, 892.1f), 145.3f);
						enemyVehicles.Add(heli);
						heliBlip = heli.AttachBlip();
						heliBlip.Sprite = BlipSprite.HelicopterAnimated;
						heliBlip.Color = BlipColor.Red;
						Ped enemy1 = await CreateEnemyPed(PedHash.Korean01GMY, new Vector3());
						enemy1.SetIntoVehicle(heli, VehicleSeat.Driver);
						Ped enemy2 = await CreateEnemyPed(PedHash.KorLieut01GMY, new Vector3());
						enemy2.Weapons.Give(WeaponHash.AssaultRifleMk2, int.MaxValue, true, true);
						enemy2.SetIntoVehicle(heli, VehicleSeat.LeftRear);
						Ped enemy3 = await CreateEnemyPed(PedHash.Korean02GMY, new Vector3());
						enemy3.Weapons.Give(WeaponHash.AssaultRifleMk2, int.MaxValue, true, true);
						enemy3.SetIntoVehicle(heli, VehicleSeat.RightRear);

						foreach (Ped enemy in enemies)
							enemy.Task.FightAgainst(Game.PlayerPed);

						enteredCar = true;
					}
				}
				else
				{
					if (heli.IsDead && heliBlip.Exists())
						heliBlip.Delete();

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
			if (heliBlip != null)
				heliBlip.Delete();
			if (warehouseImportBlip != null)
				warehouseImportBlip.Delete();

			MissionMusic.Play("IE_END_MUSIC", false);
		}
	}
}
