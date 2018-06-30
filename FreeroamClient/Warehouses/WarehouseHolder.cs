using CitizenFX.Core;

namespace Freeroam.Warehouses
{
    public class Warehouse
    {
        public Vector4 EntryPoint { get; private set; }
        public WarehouseInterior WarehouseInterior { get; private set; }
		public Vector4 ImportExportPoint { get; private set; }

        public Warehouse(Vector4 entryPoint, WarehouseInterior warehouseInterior, Vector4 importExportPoint)
        {
            EntryPoint = entryPoint;
			WarehouseInterior = warehouseInterior;
			ImportExportPoint = importExportPoint;
        }
    }

	public struct WarehouseInterior
	{
		public Vector4 TeleportPoint { get; private set; }
		public Vector3 ExitPoint { get; private set; }

		public WarehouseInterior(Vector4 tpPoint, Vector3 exitPoint)
		{
			TeleportPoint = tpPoint;
			ExitPoint = exitPoint;
		}
	}

	public static class WarehouseInteriors
	{
		public static WarehouseInterior INTERIOR_1 { get; } = new WarehouseInterior(new Vector4(970.3f, -2991.2f, -39.6f, 172.7f),
			new Vector3(971.0f, -2988.0f, -39.6f));
	}

    public static class WarehouseHolder
    {
        public static Warehouse[] Warehouses { get; } =
        {
            new Warehouse(new Vector4(1180.7f, -3113.8f, 6.0f, 93.2f), WarehouseInteriors.INTERIOR_1, new Vector4(1189.4f, -3108.1f, 5.5f, 358.0f)),
			new Warehouse(new Vector4(883.9f, -1987.5f, 30.3f, 89.1f), WarehouseInteriors.INTERIOR_1, new Vector4(881.9f, -1980.4f, 30.3f, 85.3f)),
			new Warehouse(new Vector4(941.1f, -2142.9f, 30.3f, 172.5f), WarehouseInteriors.INTERIOR_1, new Vector4(954.4f, -2108.5f, 29.8f, 88.9f))
		};
    }
}
