using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace Freeroam.Missions
{
	public static class MissionMusic
	{
		public static void Play(string music, bool disableMusicEvents = true)
		{
			API.TriggerMusicEvent(music);
			Audio.SetAudioFlag("DisableFlightMusic", disableMusicEvents);
			Audio.SetAudioFlag("WantedMusicDisabled", disableMusicEvents);
		}
	}
}
