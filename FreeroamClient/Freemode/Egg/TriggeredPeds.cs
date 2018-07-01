using CitizenFX.Core;
using CitizenFX.Core.Native;
using System.Threading.Tasks;

namespace Freeroam.Freemode.Egg
{
	class TriggeredPeds : BaseScript
	{
		public TriggeredPeds()
		{
			Tick += OnTick;
		}

		private async Task OnTick()
		{
			await Delay(100);

			if (Game.Player.IsAiming && Game.PlayerPed.IsInVehicle() && Game.PlayerPed.Weapons.Current == WeaponHash.Unarmed)
			{
				int aimedEntityHandle = 0;
				if (API.GetEntityPlayerIsFreeAimingAt(Game.Player.Handle, ref aimedEntityHandle) && API.IsEntityAPed(aimedEntityHandle))
				{
					Ped aimedPed = new Ped(aimedEntityHandle);
					string response;
					switch (API.GetRandomIntInRange(0, 3))
					{
						case 0:
							response = "FIGHT";
							break;
						case 1:
							response = "GENERIC_INSULT_MED";
							break;
						default:
							response = "GENERIC_INSULT_HIGH";
							break;
					}

					aimedPed.PlayAmbientSpeech(response, SpeechModifier.ForceShouted);
				}
			}
		}
	}
}
