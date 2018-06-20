using CitizenFX.Core;
using Freeroam.Phone;
using System;
using System.Threading.Tasks;

namespace Freeroam.Warehouses
{
	class WarehouseBehaviour : BaseScript
	{
		public WarehouseBehaviour()
		{
			EventHandlers["freemode:warehouseIn"] += new Action(delegate
			{
				PhoneState.Block = true;
			});
			EventHandlers["freemode:warehouseOut"] += new Action(delegate
			{
				PhoneState.Block = false;
			});

			Tick += OnTick;
		}

		private async Task OnTick()
		{
			await Task.FromResult(0);

			if (WarehouseState.IsInsideWarehouse)
			{
				Game.Player.DisableFiringThisFrame();
				Game.Player.WantedLevel = 0;
			}
		}
	}
}
