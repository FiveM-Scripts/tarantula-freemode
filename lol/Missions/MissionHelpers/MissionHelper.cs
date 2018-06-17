using CitizenFX.Core.UI;

namespace Freeroam.Missions.MissionHelpers
{
	public static class MissionHelper
	{
		public static void DrawTaskSubtitle(string text)
		{
			Screen.ShowSubtitle(text, 5000);
		}
	}
}
