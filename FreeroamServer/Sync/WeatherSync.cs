using CitizenFX.Core;
using System;
using System.Threading.Tasks;

namespace FreeroamServer.Sync
{
	class WeatherSync : BaseScript
	{
		private int weatherSwitchTime;
		private string currentWeather;

		private string[] weatherTypes =
		{
			"CLEAR",
			"EXTRASUNNY",
			"CLOUDS",
			"OVERCAST",
			"RAIN",
			"CLEARING",
			"THUNDER",
			"SMOG",
			"FOGGY"
		};

		public WeatherSync()
		{
			Tick += OnTick;
		}

		private async Task OnTick()
		{
			await Delay(1000);

			weatherSwitchTime--;
			int transitionTime = 0;
			if (weatherSwitchTime < 1)
			{
				Random random = new Random();
				currentWeather = weatherTypes[random.Next(0, weatherTypes.Length - 1)];
				weatherSwitchTime = random.Next(120, 600);
				transitionTime = 60;
			}
			TriggerClientEvent("freeroam:weatherUpdate", currentWeather, transitionTime);
		}
	}
}
