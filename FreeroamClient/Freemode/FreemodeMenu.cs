using CitizenFX.Core;
using CitizenFX.Core.UI;
using Freeroam.Freemode.Display;
using Freeroam.Freemode.Phone;
using Freeroam.Warehouses;
using FreeroamShared;
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
		private UIMenuItem killYourselfItem;
		private bool menuVisible;

		public FreemodeMenu()
		{
			menuPool = new MenuPool();
			mainMenu = new UIMenu("Interaction", "Go get a Hobby")
			{
				MouseControlsEnabled = false,
				ControlDisablingEnabled = false
			};
			mainMenu.DisableInstructionalButtons(true);
			menuPool.Add(mainMenu);

			killYourselfItem = new UIMenuItem("Kill Yourself");
			killYourselfItem.SetRightLabel("$500");
			mainMenu.OnItemSelect += new ItemSelectEvent((menu, item, pos) =>
			{
				if (item == killYourselfItem)
				{
					if (Money.Amount < 500)
						Screen.ShowNotification("~r~You don't have enough money to kill yourself, faggot.");
					else
					{
						Game.PlayerPed.Kill();
						Money.RemoveMoney(500);
					}
				}
			});

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

			if (Game.PlayerPed.IsDead)
				mainMenu.Visible = false;
			else
			{
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
						mainMenu.AddItem(killYourselfItem);
						mainMenu.CurrentSelection = 0;
					}
				}
			}
		}
	}
}