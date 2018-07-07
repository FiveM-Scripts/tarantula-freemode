using CitizenFX.Core;
using CitizenFX.Core.UI;
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
		private UIMenuItem toggleCeoItem;
		private UIMenuItem killYourselfItem;
		private bool menuVisible;

		public FreemodeMenu()
		{
			menuPool = new MenuPool();
			mainMenu = new UIMenu(Strings.INTERACTION_TITLE, Strings.INTERACTION_SUBTITLE)
			{
				MouseControlsEnabled = false,
				ControlDisablingEnabled = false
			};
			mainMenu.DisableInstructionalButtons(true);
			menuPool.Add(mainMenu);
			toggleCeoItem = new UIMenuItem(Strings.INTERACTION_ITEM_CEO);
			mainMenu.OnItemSelect += new ItemSelectEvent((menu, item, pos) =>
			{
				if (item == toggleCeoItem)
				{
					if (OrganizationsHolder.GetPlayerOrganization(Game.Player) == OrganizationType.NONE)
					{
						OrganizationType freeOrganizationType = OrganizationsHolder.GetNextEmptyOrganization();
						if (freeOrganizationType == OrganizationType.NONE)
							Screen.ShowNotification(Strings.INTERACTION_ITEM_CEO_NONAVAIL, true);
						else
							OrganizationsHolder.SetPlayerOrganization(freeOrganizationType);
					}
					else
						OrganizationsHolder.SetPlayerOrganization(OrganizationType.NONE);
				}
			});
			killYourselfItem = new UIMenuItem(Strings.INTERACTION_ITEM_KYS);
			mainMenu.OnItemSelect += new ItemSelectEvent((menu, item, pos) =>
			{
				if (item == killYourselfItem)
					Game.PlayerPed.Kill();
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
						quickBlipItem = new UIMenuListItem(Strings.INTERACTION_ITEM_QUICK_NAV, World.GetAllBlips().Select(blip => blip.Type as dynamic).ToList(), 0);
						quickBlipItem.OnListSelected += new ItemListEvent((sender, pos) =>
						{
							World.WaypointPosition = World.GetAllBlips().Where(blip => blip.Type == quickBlipItem.IndexToItem(pos)).First().Position;
						});
						mainMenu.AddItem(quickBlipItem);
						mainMenu.AddItem(toggleCeoItem);
						mainMenu.AddItem(killYourselfItem);
						mainMenu.CurrentSelection = 0;
					}
				}
			}
		}
	}
}