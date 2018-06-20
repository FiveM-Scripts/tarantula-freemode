﻿using CitizenFX.Core;
using Freeroam.Phone;
using Freeroam.Warehouses;
using NativeUI;
using System.Linq;
using System.Threading.Tasks;

namespace Freeroam.Freemode
{
	class FreemodeMenu : BaseScript
	{
		private MenuPool menuPool;
		private UIMenu mainMenu;
		private UIMenuListItem quickBlipItem;
		private bool menuVisible;

		public FreemodeMenu()
		{
			menuPool = new MenuPool();
			mainMenu = new UIMenu(Game.Player.Name, "Interaction");
			mainMenu.MouseControlsEnabled = false;
			mainMenu.ControlDisablingEnabled = false;
			mainMenu.DisableInstructionalButtons(true);
			menuPool.Add(mainMenu);

			Tick += OnTick;
		}

		private async Task OnTick()
		{
			await Task.FromResult(0);

			if (PhoneState.IsShown || WarehouseState.IsInsideWarehouse)
				mainMenu.Visible = false;
			else
			{
				if (mainMenu.Visible)
					PhoneState.Block = true;
				else if (menuVisible)
				{
					PhoneState.Block = false;
					menuVisible = false;
				}
			}

			menuPool.ProcessMenus();
			if (Game.IsControlJustPressed(0, Control.InteractionMenu) && !PhoneState.IsShown && !WarehouseState.IsInsideWarehouse)
			{
				mainMenu.Visible = !mainMenu.Visible;
				if (mainMenu.Visible)
				{
					menuVisible = true;
					mainMenu.Clear();
					quickBlipItem = new UIMenuListItem("Quick Waypoint", World.GetAllBlips().Select(blip => blip.Type as dynamic).ToList(), 0);
					quickBlipItem.OnListSelected += new ItemListEvent((sender, pos) =>
					{
						World.WaypointPosition = World.GetAllBlips().Where(blip => blip.Type == quickBlipItem.IndexToItem(pos)).First().Position;
					});
					mainMenu.AddItem(quickBlipItem);
				}
			}
		}
	}
}
