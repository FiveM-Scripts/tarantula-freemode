using CitizenFX.Core;

namespace Freeroam.Warehouses
{
    public class Warehouse
    {
        public Vector4 EntryPoint { get; private set; }
        public Vector4 TeleportPoint { get; private set; }
        public Vector3 ExitPoint { get; private set; }
		public Vector4 ImportExportPoint { get; private set; }

        public Warehouse(Vector4 entryPoint, Vector4 tpPoint, Vector3 exitPoint, Vector4 importExportPoint)
        {
            EntryPoint = entryPoint;
            TeleportPoint = tpPoint;
            ExitPoint = exitPoint;
			ImportExportPoint = importExportPoint;
        }
    }

    public static class WarehouseHolder
    {
        public static Warehouse[] Warehouses { get; } =
        {
            new Warehouse(new Vector4(1180.7f, -3113.8f, 6.0f, 93.2f), new Vector4(970.3f, -2991.2f, -39.6f, 172.7f), new Vector3(971.0f, -2988.0f, -39.6f),
				new Vector4(1189.4f, -3108.1f, 5.5f, 358.0f))
        };
    }
}
