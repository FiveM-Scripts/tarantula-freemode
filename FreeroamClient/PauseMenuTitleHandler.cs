using CitizenFX.Core;
using CitizenFX.Core.Native;
using FreeroamShared;

namespace Freeroam
{
	class PauseMenuTitleHandler : BaseScript
	{
		public PauseMenuTitleHandler()
		{
			API.AddTextEntry("FE_THDR_GTAO", Strings.FE_THDR_GTAO);
		}
	}
}
