using Freeroam.Missions.MissionCollection;
using System;

namespace Freeroam.Missions.MissionHolders
{
	public static class DeliveryMissionHolder
	{
		public static Type[] Missions { get; } =
		{
			typeof(Delivery1),
			typeof(Delivery2),
			//typeof(Delivery3)
		};
	}
}