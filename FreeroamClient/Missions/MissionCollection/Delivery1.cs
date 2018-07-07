using CitizenFX.Core;
using CitizenFX.Core.Native;
using Freeroam.Missions.MissionHelpers;
using Freeroam.Util;
using FreeroamShared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Freeroam.Missions.MissionCollection
{
	public class Delivery1 : IMission
	{
		private MissionMusic missionMusic;
		private DeliveryMissionHelper missionHelper;
		private Vehicle deliveryCar;
		private Blip deliveryCarBlip;
		private List<Vehicle> enemyVehicles;
		private Vehicle heli;
		private Blip heliBlip;

		public async Task Prepare()
		{
			VehicleHash[] possibleVehicles =
			{
				VehicleHash.Voltic2,
				VehicleHash.Kuruma2,
				VehicleHash.Oppressor,
				VehicleHash.Dukes2,
				VehicleHash.Comet3
			};
			deliveryCar = await MissionHelper.CreateRobustVehicle(possibleVehicles[API.GetRandomIntInRange(0, possibleVehicles.Length)],
				new Vector3(666.4f, 676.9f, 128.5f), 193.4f);

			missionMusic = new MissionMusic();
			missionMusic.PlayStartMusic();

			missionHelper = new DeliveryMissionHelper(deliveryCar, missionMusic);

			Ped enemy1 = await missionHelper.CreateNeutralEnemyPed(PedHash.Korean01GMY, new Vector3(663.1f, 662.5f, 128.9f), 252.3f, WeaponHash.PistolMk2);
			enemy1._StartScenario("WORLD_HUMAN_AA_SMOKE");
			Ped enemy2 = await missionHelper.CreateNeutralEnemyPed(PedHash.Korean02GMY, new Vector3(667.5f, 661.1f, 128.9f), 70.3f, WeaponHash.PistolMk2);
			enemy2._StartScenario("WORLD_HUMAN_GUARD_PATROL");
			Ped enemy3 = await missionHelper.CreateNeutralEnemyPed(PedHash.Korean02GMY, new Vector3(780.2f, 571.4f, 127.5f), 330.4f, WeaponHash.PistolMk2);
			enemy3._StartScenario("WORLD_HUMAN_SMOKING");
			enemyVehicles = new List<Vehicle>();
			Vehicle vehicle1 = await EntityUtil.CreateVehicle(VehicleHash.Sentinel, new Vector3(851.3f, 504f, 125.9f), 343f);
			enemyVehicles.Add(vehicle1);
			Ped enemy4 = await missionHelper.CreateNeutralEnemyPed(PedHash.KorLieut01GMY, new Vector3(), 0f, WeaponHash.PistolMk2);
			enemy4.SetIntoVehicle(vehicle1, VehicleSeat.Driver);
			Ped enemy5 = await missionHelper.CreateNeutralEnemyPed(PedHash.KorBoss01GMM, new Vector3(), 0f, WeaponHash.MicroSMG);
			enemy5.SetIntoVehicle(vehicle1, VehicleSeat.Passenger);
			Ped enemy6 = await missionHelper.CreateNeutralEnemyPed(PedHash.KorLieut01GMY, new Vector3(903.5f, 541.6f, 123.2f), 271.7f, WeaponHash.PistolMk2);
			enemy6._StartScenario("WORLD_HUMAN_SMOKING");
			Ped enemy7 = await missionHelper.CreateNeutralEnemyPed(PedHash.Korean01GMY, new Vector3(906.7f, 541.7f, 123.2f), 93.5f, WeaponHash.PistolMk2);
			enemy7._StartScenario("WORLD_HUMAN_GUARD_PATROL");
			Ped enemy8 = await missionHelper.CreateNeutralEnemyPed(PedHash.KorBoss01GMM, new Vector3(902.2f, 510.7f, 121.9f), 296f, WeaponHash.PistolMk2);
			enemy8._StartScenario("WORLD_HUMAN_STAND_IMPATIENT");
		}

		public void Start()
		{
			deliveryCarBlip = deliveryCar.AttachBlip();
			deliveryCarBlip.Color = BlipColor.Blue;
			deliveryCarBlip.ShowRoute = true;

			MissionHelper.DrawTaskSubtitle(String.Format(Strings.MISSION_DELIVERY_STEAL, deliveryCar._GetLabel()));
		}

		public async Task OnTick()
		{
			await Task.FromResult(0);

			missionHelper.HandleMissionFailedCheck();
			if (!missionHelper.IsDeliveryTaskStarted())
			{
				if (Game.PlayerPed.CurrentVehicle == deliveryCar)
				{
					heli = await EntityUtil.CreateVehicle(VehicleHash.Maverick, new Vector3(1509.8f, -224.2f, 892.1f), 145.3f);
					enemyVehicles.Add(heli);
					heliBlip = heli.AttachBlip();
					heliBlip.Sprite = BlipSprite.HelicopterAnimated;
					heliBlip.Color = BlipColor.Red;
					Ped enemy1 = await missionHelper.CreateNeutralEnemyPed(PedHash.Korean01GMY, new Vector3());
					enemy1.SetIntoVehicle(heli, VehicleSeat.Driver);
					Ped enemy2 = await missionHelper.CreateNeutralEnemyPed(PedHash.KorLieut01GMY, new Vector3(), 0f, WeaponHash.AssaultRifleMk2);
					enemy2.SetIntoVehicle(heli, VehicleSeat.LeftRear);
					Ped enemy3 = await missionHelper.CreateNeutralEnemyPed(PedHash.Korean02GMY, new Vector3(), 0f, WeaponHash.AssaultRifleMk2);
					enemy3.SetIntoVehicle(heli, VehicleSeat.RightRear);

					missionHelper.CreateDeliveryTask();
				}
			}
			else
			{
				if (heli.IsDead && heliBlip.Exists())
					heliBlip.Delete();

				await missionHelper.HandleDeliveryDropOff();
			}
		}

		public void Stop()
		{
			if (deliveryCar.Exists())
				deliveryCar.MarkAsNoLongerNeeded();
			if (deliveryCarBlip.Exists())
				deliveryCarBlip.Delete();
			foreach (Vehicle vehicle in enemyVehicles)
				vehicle.MarkAsNoLongerNeeded();
			if (heliBlip != null)
				heliBlip.Delete();
			missionHelper.DestroyEntities();

			missionMusic.PlayStopMusic();
		}
	}
}