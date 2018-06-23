using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Threading.Tasks;

namespace Freeroam.Freemode.Display
{
	public class Money : BaseScript
	{
		public static int Amount { get; private set; }

		public Money()
		{
			Tick += OnTick;
		}

		private async Task OnTick()
		{
			await Delay(100);

			API.StatSetInt((uint) Game.GenerateHash("MP0_WALLET_BALANCE"), Amount, true);
		}

		public static void AddMoney(int amount)
		{
			Amount += amount;
		}

		public static void RemoveMoney(int amount)
		{
			Amount -= amount;
		}
	}
}
