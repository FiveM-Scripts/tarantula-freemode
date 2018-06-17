using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using Freeroam.Freemode.Relationship;
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
			this.missionMusic = missionMusic;
		}

		public void HandleMissionFailedCheck()
		{
			if (Game.PlayerPed.IsDead)
			{
				MissionHelper.DrawTaskSubtitle("~r~You died.");
				MissionStarter.RequestStopCurrentMission();
			}
			else if (deliveryCar.IsBroken())
			{
				MissionHelper.DrawTaskSubtitle("~r~The Rocket Voltic was destroyed.");
				MissionStarter.RequestStopCurrentMission();
			}
		}

		public async Task<Ped> CreateNeutralEnemyPed(Model model, Vector3 pos, float heading = 0f, WeaponHash weaponHash = WeaponHash.Unarmed)
		{
			Ped ped = await EntityUtil.CreatePed(model, PedType.PED_TYPE_MISSION, pos, heading);
			if (weaponHash != WeaponHash.Unarmed)
				ped.Weapons.Give(weaponHash, int.MaxValue, false, true);
			ped.RelationshipGroup = RelationshipGroupHolder.NeutralEnemyPeds;
			ped.IsEnemy = true;
			API.SetPedEnemyAiBlip(ped.Handle, true);
			API.HideSpecialAbilityLockonOperation(ped.Handle, false);
			enemies.Add(ped);
			return ped;
		}

		public async Task HandleDeliveryDropOff()
		{
			if (Game.PlayerPed.CurrentVehicle == deliveryCar)
			{
				if (!insideCar)
				{
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
					deliveryCar.AttachedBlip.Alpha = 255;
					deliveryCar.AttachedBlip.ShowRoute = true;
					importBlip.Alpha = 0;
					importBlip.ShowRoute = false;
					insideCar = false;
				}
			}

			if (Game.Player.WantedLevel > 0)
			{
				if (!wantedLevelWarned)
				{
					Screen.ShowSubtitle("Lose your Wanted Level.", 5000);
					importBlip.Alpha = 0;
					importBlip.ShowRoute = false;
					wantedLevelWarned = true;
				}
			}
			else
			{
				if (wantedLevelWarned)
				{
					importBlip.Alpha = 255;
					importBlip.ShowRoute = true;
					wantedLevelWarned = false;
				}

				World.DrawMarker(MarkerType.VerticalCylinder, importPoint, Vector3.Zero, Vector3.Zero, new Vector3(3f, 3f, 3f),
						Color.FromArgb(127, 0, 0, 255));
				if (World.GetDistance(deliveryCar.Position, importPoint) < 5f)
				{
					deliveryCar.ApplyForce(-deliveryCar.Velocity / 3);
					Game.PlayerPed.Task.DriveTo(deliveryCar, importPoint, 1f, 1f);
					await WarehouseTeleporter.RequestTeleport(WarehouseTeleport.Inside);
					deliveryCar.Delete();
					// TODO: Properly save
					WarehouseState.VehicleAmount++;
					MissionStarter.RequestStopCurrentMission();
				}
			}
		}

		public void CreateDeliveryTask()
		{
			if (!delivering)
			{
				MissionHelper.DrawTaskSubtitle("Bring the ~b~Rocket Voltic~w~ to the ~g~Warehouse~w~.");
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
