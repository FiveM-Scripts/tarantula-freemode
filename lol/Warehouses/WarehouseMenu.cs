﻿using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using Freeroam.Freemode.Phone;
using Freeroam.Missions;
using Freeroam.Missions.MissionHolders;
using NativeUI;
using System;
using System.Threading.Tasks;

namespace Freeroam.Warehouses
{
	class WarehouseMenu : BaseScript
	{
		private MenuPool menuPool;
		private UIMenu actionMenu;
		private UIMenuItem stealVehicleItem;

		public WarehouseMenu()
		{
			menuPool = new MenuPool();
			actionMenu = new UIMenu("Business Menu", "Actions");
			stealVehicleItem = new UIMenuItem("Steal New Vehicle");
			actionMenu.AddItem(stealVehicleItem);
			menuPool.Add(actionMenu);

			actionMenu.OnItemSelect += new ItemSelectEvent(async (menu, item, pos) =>
			{
				switch (pos)
				{
					case 0:
						actionMenu.Visible = false;
						Screen.Fading.FadeOut(1000);
						await MissionStarter.RequestPrepareMission((IMission) Activator.CreateInstance(
							DeliveryMissionHolder.Missions[API.GetRandomIntInRange(0, DeliveryMissionHolder.Missions.Length)]));
						await WarehouseTeleporter.RequestTeleport(WarehouseTeleport.Outside);
						await Delay(1000);
						MissionStarter.RequestStartMission();
						break;
				}
			});

			Tick += OnTick;
		}

		private async Task OnTick()
		{
			await Task.FromResult(0);

			if (WarehouseState.IsInsideWarehouse)
			{
				menuPool.ProcessMenus();
				if (Game.IsControlJustPressed(0, Control.InteractionMenu) && !MissionState.MissionRunning)
				{
					actionMenu.Visible = !actionMenu.Visible;
					PhoneState.Block = actionMenu.Visible;
				}
			}
		}
	}
}
