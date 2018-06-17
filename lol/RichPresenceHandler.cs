using CitizenFX.Core;
using CitizenFX.Core.Native;
using Freeroam.Missions;
using Freeroam.Warehouses;
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
				API.SetRichPresence("Playing A Mission");
			else if (WarehouseState.IsInsideWarehouse)
				API.SetRichPresence("Inside Their Warehouse");
			else
				API.SetRichPresence("Freeroaming");
		}
	}
}
