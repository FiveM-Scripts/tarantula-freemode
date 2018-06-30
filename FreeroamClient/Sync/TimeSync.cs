using CitizenFX.Core;
using CitizenFX.Core.Native;
using System.Threading.Tasks;

namespace Freeroam.Sync
{
	class TimeSync : BaseScript
	{
		public TimeSync()
		{
			Tick += OnTick;
		}

		private async Task OnTick()
		{
			await Delay(1000);

			int h = 0, m = 0, s = 0;
			API.NetworkGetServerTime(ref h, ref m, ref s);
			API.NetworkOverrideClockTime(h, m, s);
		}
	}
}
