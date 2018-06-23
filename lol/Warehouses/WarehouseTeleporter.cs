using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using Freeroam.Missions;
using System.Drawing;
using System.Threading.Tasks;

namespace Freeroam.Warehouses
{
	public enum WarehouseTeleport
	{
		Inside,
		Outside
	}

	public class WarehouseTeleporter : BaseScript
	{
		private static Warehouse insideWarehouse;

		public WarehouseTeleporter()
		{
			Tick += OnTick;
		}

		private async Task OnTick()
		{
			await Task.FromResult(0);

			if (!MissionState.MissionRunning)
			{
				Warehouse closestWarehouse = null;
				Vector4 tpPos = Vector4.Zero;
				string enterKeyText = null;
				foreach (Warehouse warehouse in WarehouseHolder.Warehouses)
				{
					Vector3 entryPointPos = new Vector3(warehouse.EntryPoint.X, warehouse.EntryPoint.Y, warehouse.EntryPoint.Z);
					Vector3 exitPointPos = warehouse.WarehouseInterior.ExitPoint;

					World.DrawMarker(MarkerType.VerticalCylinder, entryPointPos - new Vector3(0f, 0f, 1f), Vector3.Zero, Vector3.Zero,
						new Vector3(1f, 1f, 1f), Color.FromArgb(127, 0, 0, 255));
					World.DrawMarker(MarkerType.VerticalCylinder, exitPointPos - new Vector3(0f, 0f, 1f), Vector3.Zero, Vector3.Zero,
						new Vector3(1f, 1f, 1f), Color.FromArgb(127, 0, 0, 255));

					if (World.GetDistance(Game.PlayerPed.Position, entryPointPos) < 1f)
					{
						if (Game.Player.WantedLevel > 0)
							Screen.DisplayHelpTextThisFrame("You can't enter with a active Wanted Level.");
						else
						{
							enterKeyText = "Press ~INPUT_PICKUP~ to enter your warehouse.";
							closestWarehouse = warehouse;
							tpPos = closestWarehouse.WarehouseInterior.TeleportPoint;
						}
					}
					else if (World.GetDistance(Game.PlayerPed.Position, exitPointPos) < 1f)
					{
						enterKeyText = "Press ~INPUT_PICKUP~ to exit your warehouse.";
						closestWarehouse = warehouse;
						tpPos = closestWarehouse.EntryPoint;
					}
				}

				if (closestWarehouse != null && !Game.PlayerPed.IsInVehicle())
				{
					Screen.DisplayHelpTextThisFrame(enterKeyText);
					if (Game.IsControlJustPressed(0, Control.Pickup))
					{
						await OnWarehouseTeleport(closestWarehouse, tpPos);
					}
				}
			}
		}

		private async Task OnWarehouseTeleport(Warehouse warehouse, Vector4 tp)
		{
			if (tp == warehouse.WarehouseInterior.TeleportPoint)
			{
				API.RequestIpl("ex_exec_warehouse_placement");
				API.EnableInteriorProp(API.GetInteriorAtCoords(tp.X, tp.Y, tp.Z), "Basic_style_set");
				API.SetEmitterRadioStation("DLC_IE_Warehouse_Radio_01", "RADIO_21_DLC_XM17");
				insideWarehouse = warehouse;
				WarehouseState.LastWarehouse = warehouse;
				await RequestTeleport(WarehouseTeleport.Inside);
				TriggerEvent("freemode:warehouseIn");
			}
			else
			{
				await RequestTeleport(WarehouseTeleport.Outside);
				TriggerEvent("freemode:warehouseOut");
				insideWarehouse = null;
			}
		}

		public static async Task<bool> RequestTeleport(WarehouseTeleport teleport)
		{
			if (insideWarehouse != null)
			{
				if (!Screen.Fading.IsFadingOut && !Screen.Fading.IsFadedOut)
					Screen.Fading.FadeOut(1000);
				Game.PlayerPed.IsInvincible = true;
				await Delay(4000);
				while (!API.IsIplActive("ex_exec_warehouse_placement"))
					await Delay(1000);

				Vector4 tp;
				if (teleport == WarehouseTeleport.Inside)
					tp = insideWarehouse.WarehouseInterior.TeleportPoint;
				else
					tp = insideWarehouse.EntryPoint;
				Game.PlayerPed.Position = new Vector3(tp.X, tp.Y, tp.Z);
				Game.PlayerPed.Heading = tp.W;

				WarehouseState.IsInsideWarehouse = tp == insideWarehouse.WarehouseInterior.TeleportPoint;
				Screen.Fading.FadeIn(1000);
				Game.PlayerPed.IsInvincible = false;

				return true;
			}
			return false;
		}
	}
}
