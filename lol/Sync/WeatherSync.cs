using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;

namespace Freeroam.Sync
{
	class WeatherSync : BaseScript
	{
		private string currentWeather;
		private bool transitioning;

		public WeatherSync()
		{
			EventHandlers["freeroam:weatherUpdate"] += new Action<string, int>(OnWeatherUpdate);
		}

		private async void OnWeatherUpdate(string weather, int transitionTime)
		{
			if (!transitioning)
			{
				if (currentWeather == null || weather == currentWeather)
				{
					API.SetWeatherTypeNow(weather);
					currentWeather = weather;
				}
				else
				{
					transitioning = true;
					API.SetWeatherTypeOverTime(weather, transitionTime);
					await Delay(transitionTime);
					API.SetWeatherTypeNowPersist(weather);
					currentWeather = weather;
					transitioning = false;
				}
			}
		}
	}
}
