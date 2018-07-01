using CitizenFX.Core;
using CitizenFX.Core.UI;
using Freeroam.Util;
using FreeroamShared;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Freeroam.Freemode
{
	public class OrganizationsHolder : BaseScript
	{
		private static Scaleform organizationUpdateScaleform;
		private static string organizationUpdateScaleformMessage;
		private static int organizationUpdateScaleformTime;

		public OrganizationsHolder()
		{
			EntityDecoration.RegisterProperty(Decors.ORGANIZATION_TYPE, DecorationType.Int);
			EntityDecoration.RegisterProperty(Decors.ORGANIZATION_CEO, DecorationType.Bool);

			Tick += OnTick;
			Tick += OnScaleformTick;
		}

		private async Task OnTick()
		{
			await Delay(100);

			OrganizationType currentOrganization = GetPlayerOrganization(Game.Player);
			if (currentOrganization != OrganizationType.NONE
				&& GetOrganizationPlayers(currentOrganization).Where(player => IsPlayerCeoOfOrganization(player, currentOrganization)).Count() == 0)
				SetPlayerOrganization(OrganizationType.NONE);
		}

		private async Task OnScaleformTick()
		{
			await Task.FromResult(0);

			if (organizationUpdateScaleform != null)
			{
				if (--organizationUpdateScaleformTime == 0)
				{
					organizationUpdateScaleform.Dispose();
					organizationUpdateScaleform = null;
				}
				else
				{
					organizationUpdateScaleform.CallFunction("SHOW_SHARD_CENTERED_TOP_MP_MESSAGE");
					organizationUpdateScaleform.CallFunction("SHARD_SET_TEXT", organizationUpdateScaleformMessage, "", 1);
					organizationUpdateScaleform.Render2D();
				}
			}
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
			RelationshipGroup newGroup;
			// TODO: Maybe do this better when I'm not lazy
			switch (organizationType)
			{
				case OrganizationType.ONE:
					newGroup = RelationshipsHolder.PlayerOrganization1;
					ShowOrganizationUpdateEffect(Strings.CLIENT_ORG_ENTER);
					break;
				case OrganizationType.TWO:
					newGroup = RelationshipsHolder.PlayerOrganization2;
					ShowOrganizationUpdateEffect(Strings.CLIENT_ORG_ENTER);
					break;
				case OrganizationType.THREE:
					newGroup = RelationshipsHolder.PlayerOrganization3;
					ShowOrganizationUpdateEffect(Strings.CLIENT_ORG_ENTER);
					break;
				case OrganizationType.FOUR:
					newGroup = RelationshipsHolder.PlayerOrganization4;
					ShowOrganizationUpdateEffect(Strings.CLIENT_ORG_ENTER);
					break;
				default:
					newGroup = RelationshipsHolder.Player;
					ShowOrganizationUpdateEffect(Strings.CLIENT_ORG_LEAVE);
					break;
			}
			Game.PlayerPed.RelationshipGroup = newGroup;
		}

		public static OrganizationType GetNextEmptyOrganization()
		{
			Player[] players = new PlayerList().ToArray();
			foreach (OrganizationType organizationType in ((OrganizationType[]) Enum.GetValues(typeof(OrganizationType)))
				.Where(organizationType => organizationType != OrganizationType.NONE))
			{
				if (players.Where(player => GetPlayerOrganization(player) == organizationType).Count() == 0)
					return organizationType;
			}
			return OrganizationType.NONE;
		}

		private static void ShowOrganizationUpdateEffect(string message)
		{
			organizationUpdateScaleform = new Scaleform("MP_BIG_MESSAGE_FREEMODE");
			organizationUpdateScaleformMessage = message;
			organizationUpdateScaleformTime = 100;
			Screen.Effects.Start(ScreenEffect.HeistCelebPass, 2000);
			Audio.ReleaseSound(Audio.PlaySoundFrontend("PROPERTY_PURCHASE", "HUD_AWARDS"));
		}
	}
}