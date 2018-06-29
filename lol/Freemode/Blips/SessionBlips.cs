using CitizenFX.Core;
using CitizenFX.Core.Native;
using System.Linq;
using System.Threading.Tasks;

namespace Freeroam.Freemode.Blips
{
	class SessionBlips : BaseScript
	{
		public SessionBlips()
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
				playerBlip.Color = BlipColor.White;
				if (API.IsPauseMenuActive())
					playerBlip.Alpha = 255;
				else
					FadeBlipByDistance(playerBlip);
				API.ShowHeadingIndicatorOnBlip(playerBlip.Handle, true);
			}
		}

		private void HandleBlipColor(Blip blip, Player player)
		{
			
		}

		private void FadeBlipByDistance(Blip blip)
		{
			int distance = (int) World.GetDistance(Game.PlayerPed.Position, blip.Position);
			blip.Alpha = 255 * 2 - distance > 255 * 2 ? 0 : 255;
		}
	}
}
