using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace Freeroam
{
	class PauseMenuTitleHandler : BaseScript
	{
		public PauseMenuTitleHandler()
		{
			API.AddTextEntry("FE_THDR_GTAO", "FiveM - Freeroam");
		}
	}
}
