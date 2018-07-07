using CitizenFX.Core;
using CitizenFX.Core.Native;
using Freeroam.Util;
using FreeroamShared;
using System.Linq;
using System.Threading.Tasks;

namespace Freeroam.Freemode.Egg
{
	class BetterPolice : BaseScript
	{
		public BetterPolice()
		{
			EntityDecoration.RegisterProperty(Decors.COP_WEAPONIZED, DecorationType.Bool);

			Tick += OnTick;
		}

		private async Task OnTick()
		{
			await Delay(100);

			if (Game.Player.WantedLevel > 0)
				foreach (Ped ped in World.GetAllPeds().Where(ped => !ped.IsPlayer))
				{
					int pedType = API.GetPedType(ped.Handle);
					if (pedType == 6 || pedType == 27 || pedType == 29)
					{
						ped.FiringPattern = FiringPattern.FullAuto;
						ped.ShootRate = 1;

						if (!ped._HasDecor(Decors.COP_WEAPONIZED))
						{
							if (API.GetRandomIntInRange(0, 101) < 50)
							{
								WeaponHash[] possibleWeapons;
								if (pedType == 6)
									possibleWeapons = new WeaponHash[] {WeaponHash.PistolMk2, WeaponHash.Pistol50, WeaponHash.CombatPistol, WeaponHash.HeavyPistol,
										WeaponHash.VintagePistol, WeaponHash.APPistol, WeaponHash.StunGun, WeaponHash.BullpupShotgun, WeaponHash.SMG, WeaponHash.SMGMk2,
										WeaponHash.AssaultSMG, WeaponHash.CombatPDW};
								else if (pedType == 27)
									possibleWeapons = new WeaponHash[] {WeaponHash.APPistol, WeaponHash.SMGMk2, WeaponHash.CarbineRifleMk2, WeaponHash.SpecialCarbine,
										WeaponHash.PumpShotgun, WeaponHash.BullpupRifle, WeaponHash.AdvancedRifle, WeaponHash.MarksmanRifle, WeaponHash.AssaultShotgun,
										WeaponHash.HeavyShotgun, WeaponHash.SniperRifle, WeaponHash.HeavySniper, WeaponHash.HeavySniperMk2};
								else
									possibleWeapons = new WeaponHash[] {WeaponHash.PumpShotgun, WeaponHash.AssaultShotgun, WeaponHash.HeavyShotgun, WeaponHash.CombatPDW,
										WeaponHash.AssaultRifle, WeaponHash.AssaultRifleMk2, WeaponHash.CarbineRifleMk2, WeaponHash.SpecialCarbine, WeaponHash.AdvancedRifle,
										WeaponHash.MG, WeaponHash.CombatMG, WeaponHash.CombatMGMk2, WeaponHash.Minigun, WeaponHash.RPG};
								ped.Weapons.Give(possibleWeapons[API.GetRandomIntInRange(0, possibleWeapons.Count())], int.MaxValue, false, true);
							}
							ped._SetDecor(Decors.COP_WEAPONIZED, true);
						}
					}
				}
		}
	}
}