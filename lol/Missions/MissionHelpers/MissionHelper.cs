using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using Freeroam.Freemode.FreemodePlayer.Relationship;
using Freeroam.Util;
using System.Threading.Tasks;

namespace Freeroam.Missions.MissionHelpers
{
	public static class MissionHelper
	{
		public static void DrawTaskSubtitle(string text)
		{
			Screen.ShowSubtitle(text, 10000);
		}

		public static async Task<Ped> CreateNeutralEnemyPed(Model model, Vector3 pos, float heading = 0f, WeaponHash weaponHash = WeaponHash.Unarmed)
		{
			Ped ped = await EntityUtil.CreatePed(model, PedType.PED_TYPE_MISSION, pos, heading);
			if (weaponHash != WeaponHash.Unarmed)
				ped.Weapons.Give(weaponHash, int.MaxValue, false, true);
			ped.RelationshipGroup = RelationshipGroupHolder.NeutralEnemyPeds;
			ped.IsEnemy = true;
			API.SetPedEnemyAiBlip(ped.Handle, true);
			API.HideSpecialAbilityLockonOperation(ped.Handle, false);
			return ped;
		}

		public static async Task<Vehicle> CreateRobustVehicle(Model model, Vector3 pos, float heading = 0f)
		{
			Vehicle vehicle = await EntityUtil.CreateVehicle(model, pos, heading);
			vehicle.CanTiresBurst = false;
			API.SetVehicleExplodesOnHighExplosionDamage(vehicle.Handle, false);
			return vehicle;
		}
	}
}
