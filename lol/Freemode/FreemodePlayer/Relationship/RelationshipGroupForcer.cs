using CitizenFX.Core;
using System.Threading.Tasks;

namespace Freeroam.Freemode.FreemodePlayer.Relationship
{
	class RelationshipGroupForcer : BaseScript
	{
		public RelationshipGroupForcer()
		{
			Tick += OnTick;
		}

		private async Task OnTick()
		{
			await Delay(100);

			Game.PlayerPed.RelationshipGroup = RelationshipGroupHolder.Player;
		}
	}
}
