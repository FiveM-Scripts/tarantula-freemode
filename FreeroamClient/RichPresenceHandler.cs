using CitizenFX.Core;
using CitizenFX.Core.Native;
using Freeroam.Missions;
using Freeroam.Warehouses;
using FreeroamShared;
using System.Threading.Tasks;

namespace Freeroam
{
	class RichPresenceHandler : BaseScript
	{
		public RichPresenceHandler()
		{
			Tick += OnTick;
		}

		private async Task OnTick()
		{
			await Delay(5000);

			if (MissionState.MissionRunning)
				API.SetRichPresence(Strings.CLIENT_RP_MISSION);
			else if (WarehouseState.IsInsideWarehouse)
				API.SetRichPresence(Strings.CLIENT_RP_WAREHOUSE);
			else
				API.SetRichPresence(Strings.CLIENT_RP_FREEROAM);
		}
	}
}
