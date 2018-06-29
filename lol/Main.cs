using CitizenFX.Core;
using FreeroamShared;

namespace Freeroam
{
	class Main : BaseScript
	{
		public Main()
		{
			TriggerServerEvent(Events.INIT);
		}
	}
}
