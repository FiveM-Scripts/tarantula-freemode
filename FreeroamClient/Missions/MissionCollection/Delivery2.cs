using CitizenFX.Core;
using CitizenFX.Core.Native;
using Freeroam.Missions.MissionHelpers;
using Freeroam.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Freeroam.Missions.MissionCollection
{
	public class Delivery2 : IMission
	{
		private MissionMusic missionMusic;
		private DeliveryMissionHelper missionHelper;
		private Vehicle deliveryCar;
		private Blip deliveryCarBlip;
		private List<Vehicle> enemyVehicles;

		public async Task Prepare()
		{
			VehicleHash[] possibleVehicles =
			{
				VehicleHash.Voltic2,
				VehicleHash.Voltic,
				VehicleHash.Kuruma2,
				VehicleHash.Oppressor,
				VehicleHash.Lectro
			};
			deliveryCar = await MissionHelper.CreateRobustVehicle(possibleVehicles[API.GetRandomIntInRange(0, possibleVehicles.Length)],
				new Vector3(667.4f, -756f, 23.7f), 171.5f);

			missionMusic = new MissionMusic();
			missionMusic.PlayStartMusic();

			missionHelper = new DeliveryMissionHelper(deliveryCar, missionMusic);

			Ped enemy1 = await missionHelper.CreateNeutralEnemyPed(PedHash.BikeHire01, new Vector3(667.8f, -768.8f, 23.6f), 184.8f, WeaponHash.AssaultRifleMk2);
			enemy1._StartScenario("WORLD_HUMAN_AA_SMOKE");
			Ped enemy2 = await missionHelper.CreateNeutralEnemyPed(PedHash.Car3Guy1, new Vector3(668.1f, -773f, 23.5f), 6f, WeaponHash.AssaultRifleMk2);
			enemy2._StartScenario("WORLD_HUMAN_DRUG_DEALER");
			Ped enemy3 = await missionHelper.CreateNeutralEnemyPed(PedHash.Car3Guy2, new Vector3(667f, -754.4f, 31.3f), 0f, WeaponHash.AssaultRifleMk2);
			enemy3._StartScenario("WORLD_HUMAN_BINOCULARS");
			Ped enemy4 = await missionHelper.CreateNeutralEnemyPed(PedHash.CarBuyerCutscene, new Vector3(672.1f, -781.9f, 23.5f), 101.9f, WeaponHash.AssaultRifleMk2);
			enemy4._StartScenario("WORLD_HUMAN_DRINKING");
			Ped enemy5 = await missionHelper.CreateNeutralEnemyPed(PedHash.MexGang01GMY, new Vector3(656.8f, -793.8f, 23.5f), 1.6f, WeaponHash.AssaultRifleMk2);
			enemy5._StartScenario("WORLD_HUMAN_WELDING");
			enemyVehicles = new List<Vehicle>();
			Vehicle vehicle1 = await EntityUtil.CreateVehicle(VehicleHash.SabreGT, new Vector3(654.5f, -729.8f, 24.2f), 181.1f);
			enemyVehicles.Add(vehicle1);
			Ped enemy6 = await missionHelper.CreateNeutralEnemyPed(PedHash.MexGang01GMY, new Vector3(), 0f, WeaponHash.PistolMk2);
			enemy6.SetIntoVehicle(vehicle1, VehicleSeat.Driver);
			Vehicle vehicle2 = await EntityUtil.CreateVehicle(VehicleHash.Ninef, new Vector3(669.4f, -706.8f, 24.6f), 177.5f);
			enemyVehicles.Add(vehicle2);
			Ped enemy7 = await missionHelper.CreateNeutralEnemyPed(PedHash.MexGoon01GMY, new Vector3(), 0f, WeaponHash.PistolMk2);
			enemy7.SetIntoVehicle(vehicle2, VehicleSeat.Driver);
			Ped enemy8 = await missionHelper.CreateNeutralEnemyPed(PedHash.MexGoon02GMY, new Vector3(), 0f, WeaponHash.MicroSMG);
			enemy8.SetIntoVehicle(vehicle2, VehicleSeat.Passenger);
			Ped enemy9 = await missionHelper.CreateNeutralEnemyPed(PedHash.MexThug01AMY, new Vector3(677.3f, -861.9f, 23.5f), 182.4f, WeaponHash.AssaultRifleMk2);
			enemy9._StartScenario("WORLD_HUMAN_CONST_DRILL");
			Ped enemy10 = await missionHelper.CreateNeutralEnemyPed(PedHash.MexBoss01GMM, new Vector3(683.5f, -861.9f, 23.6f), 64.5f, WeaponHash.AssaultRifleMk2);
			enemy10._StartScenario("WORLD_HUMAN_COP_IDLES");
		}

		public void Start()
		{
			deliveryCarBlip = deliveryCar.AttachBlip();
			deliveryCarBlip.Color = BlipColor.Blue;
			deliveryCarBlip.ShowRoute = true;

			MissionHelper.DrawTaskSubtitle($"Steal the ~b~{deliveryCar._GetLabel()}~w~.");
		}

		public async Task OnTick()
		{
			await Task.FromResult(0);

			missionHelper.HandleMissionFailedCheck();
			if (!missionHelper.IsDeliveryTaskStarted())
			{
				if (Game.PlayerPed.CurrentVehicle == deliveryCar)
					missionHelper.CreateDeliveryTask();
			}
			else
				await missionHelper.HandleDeliveryDropOff();
		}

		public void Stop()
		{
			if (deliveryCar.Exists())
				deliveryCar.MarkAsNoLongerNeeded();
			if (deliveryCarBlip.Exists())
				deliveryCarBlip.Delete();
			foreach (Vehicle vehicle in enemyVehicles)
				vehicle.MarkAsNoLongerNeeded();
			missionHelper.DestroyEntities();

			missionMusic.PlayStopMusic();
		}
	}
}