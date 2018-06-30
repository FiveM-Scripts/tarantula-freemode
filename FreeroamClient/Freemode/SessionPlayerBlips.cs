using CitizenFX.Core;
using CitizenFX.Core.Native;
using FreeroamShared;
using System.Linq;
using System.Threading.Tasks;

namespace Freeroam.Freemode
{
	class SessionPlayerBlips : BaseScript
	{
		public SessionPlayerBlips()
		{
			Tick += OnTick;
		}

		private async Task OnTick()
		{
			await Delay(100);

			foreach (Player player in Players.Where(player => Game.Player != player))
			{
				Ped playerPed = player.Character;
				Blip playerBlip = playerPed.AttachedBlip;
				if (playerBlip == null)
					playerBlip = playerPed.AttachBlip();
				playerBlip.Name = player.Name;
				playerBlip.Color = GetPlayerSuitableBlipColor(player);
				if (API.IsPauseMenuActive())
					playerBlip.Alpha = 255;
				else
					FadeBlipByDistance(playerBlip);
				API.ShowHeadingIndicatorOnBlip(playerBlip.Handle, true);
			}
		}

		private BlipColor GetPlayerSuitableBlipColor(Player player)
		{
			switch (OrganizationsHolder.GetPlayerOrganization(player))
			{
				case OrganizationType.ONE:
					return BlipColor.TrevorOrange;
				case OrganizationType.TWO:
					return BlipColor.FranklinGreen;
				case OrganizationType.THREE:
					return BlipColor.MichaelBlue;
				case OrganizationType.FOUR:
					return BlipColor.Yellow;
				default:
					return BlipColor.White;
			}
		}

		private void FadeBlipByDistance(Blip blip)
		{
			int distance = (int) World.GetDistance(Game.PlayerPed.Position, blip.Position);
			blip.Alpha = 255 - (distance < 255 * 2 ? distance / 2 : 255);
		}
	}
}
