using CitizenFX.Core;
using CitizenFX.Core.UI;
using System.Drawing;
using System.Threading.Tasks;

namespace Freeroam.Warehouses
{
	class WarehouseScreenElements : BaseScript
	{
		Text vehiclesText;
		Rectangle vehiclesTextBackground;

		public WarehouseScreenElements()
		{
			vehiclesText = new Text($"Vehicles: {WarehouseState.VehicleAmount}", new PointF(1150f, 600f), 0.7f, Color.FromArgb(255, 255, 255, 255), Font.Pricedown, Alignment.Center, true, true);
			vehiclesTextBackground = new Rectangle(new PointF(1150f, 615f), new SizeF(200f, 100f), Color.FromArgb(127, 0, 0, 0), true);

			Tick += OnTick;
		}

		private async Task OnTick()
		{
			await Task.FromResult(0);

			if (WarehouseState.IsInsideWarehouse)
			{
				vehiclesText.Caption = $"Vehicles: {WarehouseState.VehicleAmount}";
				vehiclesText.Draw();
				vehiclesTextBackground.Draw();
			}
		}
	}
}
