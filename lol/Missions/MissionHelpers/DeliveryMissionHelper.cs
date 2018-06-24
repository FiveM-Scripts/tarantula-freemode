using CitizenFX.Core;
using CitizenFX.Core.Native;
using Freeroam.Freemode.Display;
using Freeroam.Util;
using Freeroam.Warehouses;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace Freeroam.Missions.MissionHelpers
{
	public class DeliveryMissionHelper
	{
		private Vector3 importPoint;
		private Vehicle deliveryCar;
		private string vehicleLabel;
		private MissionMusic missionMusic;
		private Blip importBlip;
		private List<Ped> enemies;
		private bool wantedLevelWarned;
		private bool insideCar;
		private bool delivering;

		public DeliveryMissionHelper(Vehicle deliveryCar, MissionMusic missionMusic)
		{
			Vector4 importPointOrigin = WarehouseState.LastWarehouse.ImportExportPoint;
			importPoint = new Vector3(importPointOrigin.X, importPointOrigin.Y, importPointOrigin.Z - 1);

			enemies = new List<Ped>();

			this.deliveryCar = deliveryCar;
			vehicleLabel = deliveryCar._GetLabel();
			this.missionMusic = missionMusic;
		}

		public void HandleMissionFailedCheck()
		{
			if (deliveryCar._IsBroken())
			{
				MissionHelper.DrawTaskSubtitle($"~r~The {vehicleLabel} was destroyed.");
				MissionStarter.RequestStopCurrentMission();
			}
		}

		public async Task<Ped> CreateNeutralEnemyPed(Model model, Vector3 pos, float heading = 0f, WeaponHash weaponHash = WeaponHash.Unarmed)
		{
			Ped enemy = await MissionHelper.CreateNeutralEnemyPed(model, pos, heading, weaponHash);
			enemies.Add(enemy);
			return enemy;
		}

		public async Task HandleDeliveryDropOff()
		{
			if (Game.Player.WantedLevel > 0)
			{
				if (!wantedLevelWarned)
				{
					MissionHelper.DrawTaskSubtitle("Lose your Wanted Level.");
					importBlip.Alpha = 0;
					importBlip.ShowRoute = false;
					wantedLevelWarned = true;
				}
			}
			else
			{
				if (wantedLevelWarned)
				{
					MissionHelper.DrawTaskSubtitle($"Bring the ~b~{vehicleLabel}~w~ to the ~g~Warehouse~w~.");
					importBlip.Alpha = 255;
					importBlip.ShowRoute = true;
					wantedLevelWarned = false;
				}
				else if (Game.PlayerPed.CurrentVehicle == deliveryCar)
				{
					if (!insideCar)
					{
						MissionHelper.DrawTaskSubtitle($"Bring the ~b~{vehicleLabel}~w~ to the ~g~Warehouse~w~.");
						deliveryCar.AttachedBlip.Alpha = 0;
						deliveryCar.AttachedBlip.ShowRoute = false;
						importBlip.Alpha = 255;
						importBlip.ShowRoute = true;
						insideCar = true;
					}
				}
				else
				{
					if (insideCar)
					{
						MissionHelper.DrawTaskSubtitle($"Get back into the ~b~{vehicleLabel}~w~.");
						deliveryCar.AttachedBlip.Alpha = 255;
						deliveryCar.AttachedBlip.ShowRoute = true;
						importBlip.Alpha = 0;
						importBlip.ShowRoute = false;
						insideCar = false;
					}
				}

				World.DrawMarker(MarkerType.VerticalCylinder, importPoint, Vector3.Zero, Vector3.Zero, new Vector3(3f, 3f, 3f),
						Color.FromArgb(127, 0, 0, 255));
				if (World.GetDistance(deliveryCar.Position, importPoint) < 5f)
				{
					API.SetVehicleHalt(deliveryCar.Handle, 3f, 1, true);
					await WarehouseTeleporter.RequestTeleport(WarehouseTeleport.Inside);
					deliveryCar.Delete();
					// TODO: Properly save
					WarehouseState.VehicleAmount++;
					Money.AddMoney(10000);
					MissionStarter.RequestStopCurrentMission();
				}
			}
		}

		public void CreateDeliveryTask()
		{
			if (!delivering)
			{
				MissionHelper.DrawTaskSubtitle($"Bring the ~b~{vehicleLabel}~w~ to the ~g~Warehouse~w~.");
				importBlip = World.CreateBlip(importPoint);
				importBlip.Color = BlipColor.Green;
				importBlip.ShowRoute = true;
				missionMusic.PlayActionMusic();

				foreach (Ped enemy in enemies)
					enemy.Task.FightAgainst(Game.PlayerPed);

				delivering = true;
			}
		}

		public bool IsDeliveryTaskStarted()
		{
			return delivering;
		}

		public void DestroyEntities()
		{
			if (importBlip != null)
				importBlip.Delete();

			foreach (Ped enemy in enemies)
				enemy.MarkAsNoLongerNeeded();
		}
	}
}
