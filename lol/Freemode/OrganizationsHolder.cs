using CitizenFX.Core;
using Freeroam.Util;
using FreeroamShared;
using System.Linq;
using System.Threading.Tasks;

namespace Freeroam.Freemode
{
	public class OrganizationsHolder : BaseScript
	{
		public OrganizationsHolder()
		{
			EntityDecoration.RegisterProperty(Decors.ORGANIZATION_TYPE, DecorationType.Int);
			EntityDecoration.RegisterProperty(Decors.ORGANIZATION_CEO, DecorationType.Bool);

			Tick += OnTick;
		}

		private async Task OnTick()
		{
			await Delay(100);

			OrganizationType currentOrganization = GetPlayerOrganization(Game.Player);
			if (currentOrganization != OrganizationType.NONE
				&& GetOrganizationPlayers(currentOrganization).Where(player => IsPlayerCeoOfOrganization(player, currentOrganization)).Count() == 0)
				SetPlayerOrganization(OrganizationType.NONE);
		}

		public static OrganizationType GetPlayerOrganization(Player player)
		{
			if (!player.Character._HasDecor(Decors.ORGANIZATION_TYPE))
				return OrganizationType.NONE;
			else
				return (OrganizationType) player.Character._GetDecor<int>(Decors.ORGANIZATION_TYPE);
		}

		public static bool IsPlayerCeoOfOrganization(Player player, OrganizationType organizationType)
		{
			if (organizationType == OrganizationType.NONE || !player.Character._HasDecor(Decors.ORGANIZATION_CEO) || !player.Character._HasDecor(Decors.ORGANIZATION_TYPE))
				return false;
			else
				return GetPlayerOrganization(player) == organizationType && player.Character._GetDecor<bool>(Decors.ORGANIZATION_CEO);
		}

		public static Player[] GetOrganizationPlayers(OrganizationType organizationType)
		{
			return new PlayerList().Where(player => GetPlayerOrganization(player) == organizationType).ToArray();
		}

		public static void SetPlayerOrganization(OrganizationType organizationType)
		{
			Game.PlayerPed._SetDecor(Decors.ORGANIZATION_TYPE, (int) organizationType);
			Game.PlayerPed._SetDecor(Decors.ORGANIZATION_CEO, organizationType != OrganizationType.NONE && GetOrganizationPlayers(organizationType).Count() == 1 ? true : false);
			RelationshipGroup newGroup = RelationshipsHolder.Player;
			switch (organizationType)
			{
				case OrganizationType.ONE:
					newGroup = RelationshipsHolder.PlayerOrganization1;
					break;
				case OrganizationType.TWO:
					newGroup = RelationshipsHolder.PlayerOrganization2;
					break;
				case OrganizationType.THREE:
					newGroup = RelationshipsHolder.PlayerOrganization3;
					break;
				case OrganizationType.FOUR:
					newGroup = RelationshipsHolder.PlayerOrganization4;
					break;
			}
			Game.PlayerPed.RelationshipGroup = newGroup;
		}
	}
}