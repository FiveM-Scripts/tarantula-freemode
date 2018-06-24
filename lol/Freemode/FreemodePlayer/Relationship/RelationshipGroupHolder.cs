using CitizenFX.Core;

namespace Freeroam.Freemode.FreemodePlayer.Relationship
{
	public class RelationshipGroupHolder : BaseScript
	{
		public static RelationshipGroup Player { get; } = World.AddRelationshipGroup("_PLAYER");
		public static RelationshipGroup NeutralEnemyPeds { get; } = World.AddRelationshipGroup("_AI_ENEMY");
		public static RelationshipGroup WarehousePeds { get; } = World.AddRelationshipGroup("_AI_WAREHOUSE");

		public RelationshipGroupHolder()
		{
			Player.SetRelationshipBetweenGroups(Player, CitizenFX.Core.Relationship.Neutral, true);
			NeutralEnemyPeds.SetRelationshipBetweenGroups(Player, CitizenFX.Core.Relationship.Dislike, true);
			WarehousePeds.SetRelationshipBetweenGroups(Player, CitizenFX.Core.Relationship.Respect, true);
		}
	}
}
