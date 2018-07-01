using CitizenFX.Core;
using CitizenFX.Core.Native;
using Freeroam.Util;
using FreeroamShared;
using System.Threading.Tasks;

namespace Freeroam.Freemode.Egg
{
	class TriggeredPeds : BaseScript
	{
		public TriggeredPeds()
		{
			EntityDecoration.RegisterProperty(Decors.TRIGGERED_AMOUNT, DecorationType.Int);

			Tick += OnTick;
		}

		private async Task OnTick()
		{
			await Delay(1000);

			if (Game.Player.IsAiming && Game.PlayerPed.IsInVehicle() && Game.PlayerPed.Weapons.Current == WeaponHash.Unarmed)
			{
				int aimedEntityHandle = 0;
				if (API.GetEntityPlayerIsFreeAimingAt(Game.Player.Handle, ref aimedEntityHandle) && API.IsEntityAPed(aimedEntityHandle)
					&& !API.IsPedAPlayer(aimedEntityHandle))
				{
					Ped aimedPed = new Ped(aimedEntityHandle);
					string response = "GENERIC_INSULT_HIGH";
					switch (API.GetRandomIntInRange(0, 3))
					{
						case 0:
							response = "FIGHT";
							break;
						case 1:
							response = "GENERIC_INSULT_MED";
							break;
					}
					aimedPed.PlayAmbientSpeech(response, SpeechModifier.ForceShouted);

					int newTriggeredAmount;
					if (!aimedPed._HasDecor(Decors.TRIGGERED_AMOUNT))
						newTriggeredAmount = 1;
					else
						newTriggeredAmount = aimedPed._GetDecor<int>(Decors.TRIGGERED_AMOUNT) + 1;
					aimedPed._SetDecor(Decors.TRIGGERED_AMOUNT, newTriggeredAmount);
					if (newTriggeredAmount > 2)
						aimedPed.Task.FightAgainst(Game.PlayerPed);
				}
			}
		}
	}
}
