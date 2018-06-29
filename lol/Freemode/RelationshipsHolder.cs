using CitizenFX.Core;

namespace Freeroam.Freemode
{
	public class RelationshipsHolder : BaseScript
	{
		public static RelationshipGroup Player { get; } = World.AddRelationshipGroup("_PLAYER");
		public static RelationshipGroup PlayerOrganization1 { get; } = World.AddRelationshipGroup("_PLAYER_ORG1");
		public static RelationshipGroup PlayerOrganization2 { get; } = World.AddRelationshipGroup("_PLAYER_ORG2");
		public static RelationshipGroup PlayerOrganization3 { get; } = World.AddRelationshipGroup("_PLAYER_ORG3");
		public static RelationshipGroup PlayerOrganization4 { get; } = World.AddRelationshipGroup("_PLAYER_ORG4");
		public static RelationshipGroup NeutralEnemyPeds { get; } = World.AddRelationshipGroup("_AI_ENEMY");
		public static RelationshipGroup WarehousePeds { get; } = World.AddRelationshipGroup("_AI_WAREHOUSE");

		public RelationshipsHolder()
		{
			Player.SetRelationshipBetweenGroups(Player, Relationship.Neutral, true);

			PlayerOrganization1.SetRelationshipBetweenGroups(Player, Relationship.Hate, true);
			PlayerOrganization1.SetRelationshipBetweenGroups(PlayerOrganization2, Relationship.Hate, true);
			PlayerOrganization1.SetRelationshipBetweenGroups(PlayerOrganization3, Relationship.Hate, true);
			PlayerOrganization1.SetRelationshipBetweenGroups(PlayerOrganization4, Relationship.Hate, true);

			PlayerOrganization2.SetRelationshipBetweenGroups(Player, Relationship.Hate, true);
			PlayerOrganization2.SetRelationshipBetweenGroups(PlayerOrganization1, Relationship.Hate, true);
			PlayerOrganization2.SetRelationshipBetweenGroups(PlayerOrganization3, Relationship.Hate, true);
			PlayerOrganization2.SetRelationshipBetweenGroups(PlayerOrganization4, Relationship.Hate, true);

			PlayerOrganization3.SetRelationshipBetweenGroups(Player, Relationship.Hate, true);
			PlayerOrganization3.SetRelationshipBetweenGroups(PlayerOrganization1, Relationship.Hate, true);
			PlayerOrganization3.SetRelationshipBetweenGroups(PlayerOrganization2, Relationship.Hate, true);
			PlayerOrganization3.SetRelationshipBetweenGroups(PlayerOrganization4, Relationship.Hate, true);

			PlayerOrganization4.SetRelationshipBetweenGroups(Player, Relationship.Hate, true);
			PlayerOrganization4.SetRelationshipBetweenGroups(PlayerOrganization1, Relationship.Hate, true);
			PlayerOrganization4.SetRelationshipBetweenGroups(PlayerOrganization2, Relationship.Hate, true);
			PlayerOrganization4.SetRelationshipBetweenGroups(PlayerOrganization3, Relationship.Hate, true);

			NeutralEnemyPeds.SetRelationshipBetweenGroups(Player, Relationship.Dislike, true);
			WarehousePeds.SetRelationshipBetweenGroups(Player, Relationship.Respect, true);
		}
	}
}
