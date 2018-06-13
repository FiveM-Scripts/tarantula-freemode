using CitizenFX.Core;

namespace Freeroam.Warehouses
{
	class WarehouseBlipPlacer : BaseScript
	{
		public WarehouseBlipPlacer()
		{
			foreach (Warehouse warehouse in WarehouseHolder.Warehouses)
			{
				Blip blip = World.CreateBlip(new Vector3(warehouse.EntryPoint.X, warehouse.EntryPoint.Y, warehouse.EntryPoint.Z));
				blip.Sprite = BlipSprite.Garage;
				blip.Name = "Warehouse";
				blip.IsShortRange = true;
			}
		}
	}
}
