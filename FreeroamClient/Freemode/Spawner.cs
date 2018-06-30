using CitizenFX.Core;
using CitizenFX.Core.UI;
using Freeroam.Util;
using FreeroamShared;
using System;
using System.Threading.Tasks;

namespace Freeroam.Freemode
{
	class Spawner : BaseScript
	{
		private Scaleform wastedScaleform;
		private bool died;

		public Spawner()
		{
			EventHandlers["onClientMapStart"] += new Action(async () =>
			{
				await Delay(100);
				Exports["spawnmanager"].setAutoSpawn(false);
				Exports["spawnmanager"].spawnPlayer();
			});

			Tick += OnTick;
			Tick += OnScaleformMessageDrawTick;
		}

		private async Task OnTick()
		{
			await Delay(100);

			if (Game.PlayerPed.IsDead)
			{
				wastedScaleform = new Scaleform("MP_BIG_MESSAGE_FREEMODE");
				died = true;
				Screen.Effects.Start(ScreenEffect.DeathFailMpIn);
				Audio.PlaySoundFrontend("Bed", "WastedSounds");
				await Delay(10000);
				Screen.Fading.FadeOut(500);
				await Delay(3000);
				Game.PlayerPed.Position = WorldUtil.GetClosestImmersiveStreetSpawn(Game.PlayerPed.Position, 100f);
				Game.PlayerPed.Resurrect();
				Screen.Fading.FadeIn(500);
				Screen.Effects.Stop(ScreenEffect.DeathFailMpIn);
				died = false;
				wastedScaleform.Dispose();
			}
		}

		private async Task OnScaleformMessageDrawTick()
		{
			await Task.FromResult(0);

			if (died)
			{
				wastedScaleform.CallFunction("SHOW_SHARD_WASTED_MP_MESSAGE", Strings.WASTED, "", -1, true, true);
				wastedScaleform.Render2D();
			}
		}
	}
}
