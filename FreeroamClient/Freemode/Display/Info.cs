using CitizenFX.Core;
using CitizenFX.Core.UI;
using System.Threading.Tasks;

namespace Freeroam.Freemode.Display
{
	class Info : BaseScript
	{
		private bool extended;

		public Info()
		{
			Tick += OnTick;
		}

		private async Task OnTick()
		{
			await Task.FromResult(0);

			if (Game.IsControlPressed(0, Control.MultiplayerInfo))
			{
				//Screen.Hud.RadarZoom = 1000;
				Screen.Hud.ShowComponentThisFrame(HudComponent.MpCash);
				extended = true;
			}
			else if (extended)
			{
				//Screen.Hud.RadarZoom = 1;
				extended = false;
			}
		}
	}
}
