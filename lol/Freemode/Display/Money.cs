using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Threading.Tasks;

namespace Freeroam.Freemode.Display
{
	public class Money : BaseScript
	{
		public static int money { get; private set; }

		public Money()
		{
			Tick += OnTick;
		}

		private async Task OnTick()
		{
			await Delay(100);

			API.StatSetInt((uint) Game.GenerateHash("MP0_WALLET_BALANCE"), money, true);
		}

		public static void AddMoney(int amount)
		{
			money += amount;
		}

		public static void RemoveMoney(int amount)
		{
			money -= amount;
		}
	}
}
