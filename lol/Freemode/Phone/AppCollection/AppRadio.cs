using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace Freeroam.Freemode.Phone.AppCollection
{
	class AppRadioBlocker : BaseScript
	{
		public AppRadioBlocker()
		{
			Tick += OnTick;
		}

		private async Task OnTick()
		{
			await Delay(100);

			for (int i = 0; i < PhoneAppHolder.Apps.Length; i++)
				if (PhoneAppHolder.Apps[i].AppHandler == typeof(AppRadio))
					PhoneAppHolder.Apps[i].Disabled = Game.PlayerPed.IsInVehicle();
		}
	}

	public class AppRadio : IPhoneApp
	{
		private struct AppRadioStationEntry
		{
			public string RadioName { get; private set; }
			public RadioStation Radio { get; private set; }

			public AppRadioStationEntry(string radioName, RadioStation radio)
			{
				RadioName = radioName;
				Radio = radio;
			}
		}
		private static AppRadioStationEntry[] radioStationEntries { get; } =
		{
			new AppRadioStationEntry("Los Santos Rock Radio", RadioStation.LosSantosRockRadio),
			new AppRadioStationEntry("Non-Stop-Pop FM", RadioStation.NonStopPopFM),
			new AppRadioStationEntry("Radio Los Santos", RadioStation.RadioLosSantos),
			new AppRadioStationEntry("Channel X", RadioStation.ChannelX),
			new AppRadioStationEntry("West Coast Talk Radio", RadioStation.WestCoastTalkRadio),
			new AppRadioStationEntry("Rebel Radio", RadioStation.RebelRadio),
			new AppRadioStationEntry("Soulwax FM", RadioStation.SoulwaxFM),
			new AppRadioStationEntry("East Los FM", RadioStation.EastLosFM),
			new AppRadioStationEntry("West Coast Classics", RadioStation.WestCoastClassics),
			new AppRadioStationEntry("The Blue Ark", RadioStation.TheBlueArk),
			new AppRadioStationEntry("WorldWide FM", RadioStation.WorldWideFM),
			new AppRadioStationEntry("Flylo FM", RadioStation.FlyloFM),
			new AppRadioStationEntry("The Lowdown", RadioStation.TheLowdown),
			new AppRadioStationEntry("The Lab", RadioStation.TheLab),
			new AppRadioStationEntry("Radio Mirror Park", RadioStation.RadioMirrorPark),
			new AppRadioStationEntry("Space", RadioStation.Space),
			new AppRadioStationEntry("Vinewood Boulevard Radio", RadioStation.VinewoodBoulevardRadio),
			new AppRadioStationEntry("Blonded Los Santos FM", (RadioStation) 17)
		};

		private Scaleform phoneScaleform;
		private int selected;

		public void Init(Scaleform phoneScaleform)
		{
			this.phoneScaleform = phoneScaleform;
			phoneScaleform.CallFunction("SET_DATA_SLOT_EMPTY", 13);
		}

		public async Task OnTick()
		{
			await Task.FromResult(0);

			if (Game.PlayerPed.IsInVehicle())
			{
				PhoneAppStarter.MainApp();
				return;
			}

			int slot = 0;
			phoneScaleform.CallFunction("SET_DATA_SLOT", 13, slot++, -1, "Stop");
			foreach (AppRadioStationEntry radioStationEntry in radioStationEntries)
				phoneScaleform.CallFunction("SET_DATA_SLOT", 13, slot++, -1, radioStationEntry.RadioName);
			phoneScaleform.CallFunction("DISPLAY_VIEW", 13, selected);

			phoneScaleform.CallFunction("SET_SOFT_KEYS", (int)PhoneSelectSlot.SLOT_RIGHT, true, (int)PhoneSelectIcon.ICON_BACK);
			phoneScaleform.CallFunction("SET_SOFT_KEYS", (int)PhoneSelectSlot.SLOT_LEFT, true,
				slot > 0 ? (int)PhoneSelectIcon.ICON_SELECT : (int)PhoneSelectIcon.ICON_BLANK);

			bool pressed = false;
			if (Game.IsControlJustPressed(0, Control.PhoneUp))
			{
				if (--selected < 0)
					selected = slot - 1;
				pressed = true;
			}
			else if (Game.IsControlJustPressed(0, Control.PhoneDown))
			{
				if (++selected > slot - 1)
					selected = 0;
				pressed = true;
			}
			else if (Game.IsControlJustPressed(0, Control.PhoneSelect))
			{
				if (selected == 0)
					StopRadio();
				else
				{
					API.SetMobileRadioEnabledDuringGameplay(true);
					API.SetMobilePhoneRadioState(true);
					API.SetRadioToStationIndex((int) radioStationEntries[selected - 1].Radio);
				}
				pressed = true;
			}
			else if (Game.IsControlJustPressed(0, Control.PhoneCancel))
				PhoneAppStarter.MainApp();

			if (pressed)
				Audio.PlaySoundFrontend("Menu_Navigate", "Phone_SoundSet_Default");
		}

		public void Stop()
		{
			StopRadio();
		}

		private void StopRadio()
		{
			API.SetMobileRadioEnabledDuringGameplay(false);
			API.SetMobilePhoneRadioState(false);
		}
	}
}
