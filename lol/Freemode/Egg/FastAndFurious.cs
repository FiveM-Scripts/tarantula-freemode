using CitizenFX.Core;
using Freeroam.Missions;
using System.Threading.Tasks;

namespace Freeroam.Freemode.Egg
{
	class FastAndFurious : BaseScript
	{
		private bool musicStarted;

		public FastAndFurious()
		{
			Tick += OnTick;
		}

		private async Task OnTick()
		{
			await Delay(100);

			Vehicle vehicle;
			if (Game.PlayerPed.IsInVehicle() && (vehicle = Game.PlayerPed.CurrentVehicle).Speed > 50f
				&& vehicle.ClassType != VehicleClass.Planes && vehicle.ClassType != VehicleClass.Helicopters
				&& vehicle.ClassType != VehicleClass.Boats)
			{
				if (!musicStarted)
				{
					MissionMusic.Play("AH3A_START");
					MissionMusic.Play("AH3A_START_ESCAPE");
					musicStarted = true;
				}
			}
			else if (musicStarted)
			{
				MissionMusic.Play("AC_STOP", false);
				musicStarted = false;
			}
		}
	}
}
