using CitizenFX.Core;
using CitizenFX.Core.Native;
using Freeroam.Missions.MissionHelpers;
using Freeroam.Util;
using FreeroamShared;
using System;
using System.Threading.Tasks;

namespace Freeroam.Missions.MissionCollection
{
	public class Delivery3 : IMission
	{
		private MissionMusic missionMusic;
		private DeliveryMissionHelper missionHelper;
		private Vehicle deliveryCar;
		private Blip deliveryCarBlip;

		public async Task Prepare()
		{
			deliveryCar = await MissionHelper.CreateRobustVehicle(VehicleHash.Schafter5, new Vector3(-2307.3f, 369.3f, 174.2f), 323.8f);

			missionMusic = new MissionMusic();
			missionMusic.PlayStartMusic();

			missionHelper = new DeliveryMissionHelper(deliveryCar, missionMusic);
			Ped enemy = await missionHelper.CreateNeutralEnemyPed(PedHash.Security01SMM, new Vector3(), 0f, WeaponHash.CarbineRifle);
			enemy.SetIntoVehicle(deliveryCar, VehicleSeat.Driver);
			enemy.Task.CruiseWithVehicle(deliveryCar, 30f, (int) DrivingStyle.Normal);
		}

		public void Start()
		{
			deliveryCarBlip = deliveryCar.AttachBlip();
			deliveryCarBlip.Color = BlipColor.Blue;
			deliveryCarBlip.Alpha = 0;

			MissionHelper.DrawTaskSubtitle(String.Format(Strings.MISSION_DELIVERY_LOCATE, deliveryCar._GetLabel()));
			BaseScript.TriggerEvent("mtracker:settargets", new int[] { deliveryCar.Handle });
			BaseScript.TriggerEvent("mtracker:start");
		}

		public async Task OnTick()
		{
			await Task.FromResult(0);

			missionHelper.HandleMissionFailedCheck();
			if (!missionHelper.IsDeliveryTaskStarted())
			{
				if (!API.IsPauseMenuActive())
					BaseScript.TriggerEvent("mtracker:start");
				if (Game.PlayerPed.CurrentVehicle == deliveryCar)
				{
					BaseScript.TriggerEvent("mtracker:removealltargets");
					BaseScript.TriggerEvent("mtracker:stop");
					Game.Player.WantedLevel = 3;
					missionHelper.CreateDeliveryTask();
				}
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
			missionHelper.DestroyEntities();

			missionMusic.PlayStopMusic();
			BaseScript.TriggerEvent("mtracker:removealltargets");
			BaseScript.TriggerEvent("mtracker:stop");
		}
	}
}