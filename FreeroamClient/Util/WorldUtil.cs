using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace Freeroam.Util
{
	public static class WorldUtil
	{
		public static Vector3 GetClosestImmersiveStreetSpawn(Vector3 currentPos, float minDistance = 0f)
		{
			int nth = 0;
			Vector3 newPos = currentPos;
			while (IsPosNearbyPlayer(newPos) || World.GetDistance(currentPos, newPos) < minDistance)
				API.GetNthClosestVehicleNode(currentPos.X, currentPos.Y, currentPos.Z, nth++, ref newPos, 0, 0, 0);
			return newPos;
		}

		public static Vector3 GetClosestImmersiveStreetSpawnWithHeading(Vector3 currentPos, ref float heading, float minDistance = 0f)
		{
			int nth = 0;
			int unused = 0;
			Vector3 newPos = currentPos;
			while (IsPosNearbyPlayer(newPos) || World.GetDistance(currentPos, newPos) < minDistance)
				API.GetNthClosestVehicleNodeWithHeading(currentPos.X, currentPos.Y, currentPos.Z, nth++, ref newPos, ref heading,
					ref unused, 9, 3f, 2.5f);
			return newPos;
		}

		public static Vector3 GetClosestMajorStreetSpawn(Vector3 currentPos)
		{
			Vector3 newPos = currentPos;
			API.GetClosestMajorVehicleNode(currentPos.X, currentPos.Y, currentPos.Z, ref newPos, 3f, 0);
			return newPos;
		}

		public static Vector3 GetRandomStreetSpawn(Vector3 currentPos, float radius)
		{
			Vector3 newPos = currentPos;
			int nodeId = 0;
			API.GetRandomVehicleNode(currentPos.X, currentPos.Y, currentPos.Z, radius, true, true, true, ref newPos, ref nodeId);
			return newPos;
		}

		public static bool IsPosNearbyPlayer(Vector3 pos)
		{
			bool nearbyPlayer = false;
			foreach (Player player in new PlayerList())
				if (World.GetDistance(player.Character.Position, pos) < 100)
					nearbyPlayer = true;
			return nearbyPlayer;
		}
	}
}
