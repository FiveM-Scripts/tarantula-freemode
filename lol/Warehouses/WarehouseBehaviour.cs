using CitizenFX.Core;
using System.Threading.Tasks;

namespace Freeroam.Warehouses
{
	class WarehouseBehaviour : BaseScript
	{
		public WarehouseBehaviour()
		{
			Tick += OnTick;
		}

		private async Task OnTick()
		{
			await Task.FromResult(0);

			if (WarehouseState.IsInsideWarehouse)
			{
				Game.Player.DisableFiringThisFrame();
			}
		}
	}
}
