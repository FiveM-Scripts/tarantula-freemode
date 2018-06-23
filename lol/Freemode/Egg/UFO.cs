using CitizenFX.Core;
using CitizenFX.Core.Native;
using Freeroam.Util;
using System.Threading.Tasks;

namespace Freeroam.Freemode.Egg
{
	class UFO : BaseScript
	{
		private Prop ufo;

		public UFO()
		{
			//Tick += OnTick;
		}

		private async Task OnTick()
		{
			await Delay(100);

			if (API.NetworkIsHost())
			{
				if (ufo == null)
					ufo = await EntityUtil.CreateProp(API.GetHashKey("p_spinning_anus_s"), Game.PlayerPed.GetOffsetPosition(new Vector3(0f, 0f, 3000f)), true);
				else
				{
					ufo.Velocity = new Vector3(50f, 50f, 0f);
					if (World.GetDistance(ufo.Position, Game.PlayerPed.Position) > 6000f)
					{
						ufo.Delete();
						ufo = null;
					}
				}
			}
		}
	}
}
