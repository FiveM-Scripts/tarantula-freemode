using CitizenFX.Core;
using CitizenFX.Core.Native;
using FreeroamShared;

namespace Freeroam
{
	class PauseMenuTitleHandler : BaseScript
	{
		public PauseMenuTitleHandler()
		{
			API.AddTextEntry("FE_THDR_GTAO", Strings.CLIENT_FE_THDR_GTAO);
		}
	}
}
