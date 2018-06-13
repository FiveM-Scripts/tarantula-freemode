using CitizenFX.Core;
using System.Threading.Tasks;

namespace Freeroam.Missions
{
	public class MissionStarter : BaseScript
	{
		private static IMission currentMission;
		private static bool started;

		public MissionStarter()
		{
			Tick += OnTick;
		}

		private async Task OnTick()
		{
			if (currentMission != null && started)
			{
				await currentMission.OnTick();
			}
		}

		public static async Task<bool> RequestPrepareMission(IMission mission)
		{
			if (currentMission == null)
			{
				MissionState.MissionRunning = true;
				await mission.Prepare();
				currentMission = mission;
				return true;
			}
			return false;
		}

		public static bool RequestStartMission()
		{
			if (currentMission != null && !started)
			{
				currentMission.Start();
				started = true;
				
				return true;
			}
			return false;
		}

		public static bool IsMissionRunning()
		{
			return currentMission != null;
		}

		public static bool RequestStopCurrentMission()
		{
			if (currentMission != null && started)
			{
				currentMission.Stop();
				currentMission = null;
				MissionState.MissionRunning = false;
				started = false;
				return true;
			}
			return false;
		}
	}
}
