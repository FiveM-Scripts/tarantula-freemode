using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace Freeroam.Missions
{
	public class MissionMusic
	{
		private struct Music
		{
			public string StartMusic { get; private set; }
			public string ActionMusic { get; private set; }
			public string StopMusic { get; private set; }

			public Music(string startMusic, string actionMusic, string stopMusic)
			{
				StartMusic = startMusic;
				ActionMusic = actionMusic;
				StopMusic = stopMusic;
			}
		}

		private static Music[] musicCollection { get; } =
		{
			new Music("IE_START_MUSIC", "IE_DELIVERING_ATTACK", "IE_END_MUSIC"),
			new Music("BIKER_SYG_START", "BIKER_SYG_ATTACKED", "BIKER_MP_MUSIC_STOP")
		};
		private Music currentMission;

		public MissionMusic()
		{
			currentMission = musicCollection[API.GetRandomIntInRange(0, musicCollection.Length)];
		}

		public void PlayStartMusic()
		{
			API.TriggerMusicEvent(currentMission.StartMusic);
			DisableMusicEvents(true);
		}

		public void PlayActionMusic()
		{
			API.TriggerMusicEvent(currentMission.ActionMusic);
			DisableMusicEvents(true);
		}

		public void PlayStopMusic()
		{
			API.TriggerMusicEvent(currentMission.StopMusic);
			DisableMusicEvents(false);
		}

		public static void Play(string music, bool disableMusicEvents = true)
		{
			API.TriggerMusicEvent(music);
			DisableMusicEvents(disableMusicEvents);
		}

		private static void DisableMusicEvents(bool state)
		{
			Audio.SetAudioFlag("DisableFlightMusic", state);
			Audio.SetAudioFlag("WantedMusicDisabled", state);
		}
	}
}
