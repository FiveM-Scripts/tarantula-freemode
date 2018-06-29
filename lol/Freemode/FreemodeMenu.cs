using CitizenFX.Core;
using CitizenFX.Core.UI;
using Freeroam.Freemode.Phone;
using Freeroam.Util;
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
		private UIMenuItem joinItem;
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

			joinItem = new UIMenuItem("Toggle ORG");
			mainMenu.OnItemSelect += new ItemSelectEvent((menu, item, pos) =>
			{
				if (item == joinItem)
				{
					if (OrganizationsHolder.GetPlayerOrganization(Game.Player) == OrganizationType.ONE)
					{
						OrganizationsHolder.SetPlayerOrganization(OrganizationType.NONE);
						Screen.ShowNotification("No Org");
					}
					else
					{
						OrganizationsHolder.SetPlayerOrganization(OrganizationType.ONE);
						Screen.ShowNotification("Org One");
					}

					if (OrganizationsHolder.IsPlayerCeoOfOrganization(Game.Player, OrganizationType.ONE))
						Screen.ShowNotification("CEO!!!");
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
						//mainMenu.AddItem(joinItem);
					}
				}
			}
		}
	}
}
