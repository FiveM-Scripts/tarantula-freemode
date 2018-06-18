using CitizenFX.Core;
using CitizenFX.Core.Native;
using System.Threading.Tasks;

namespace Freeroam.Util
{
	public enum PedType
	{
		PED_TYPE_PLAYER_0,
		PED_TYPE_PLAYER_1,
		PED_TYPE_NETWORK_PLAYER,
		PED_TYPE_PLAYER_2,
		PED_TYPE_CIVMALE,
		PED_TYPE_CIVFEMALE,
		PED_TYPE_COP,
		PED_TYPE_GANG_ALBANIAN,
		PED_TYPE_GANG_BIKER_1,
		PED_TYPE_GANG_BIKER_2,
		PED_TYPE_GANG_ITALIAN,
		PED_TYPE_GANG_RUSSIAN,
		PED_TYPE_GANG_RUSSIAN_2,
		PED_TYPE_GANG_IRISH,
		PED_TYPE_GANG_JAMAICAN,
		PED_TYPE_GANG_AFRICAN_AMERICAN,
		PED_TYPE_GANG_KOREAN,
		PED_TYPE_GANG_CHINESE_JAPANESE,
		PED_TYPE_GANG_PUERTO_RICAN,
		PED_TYPE_DEALER,
		PED_TYPE_MEDIC,
		PED_TYPE_FIREMAN,
		PED_TYPE_CRIMINAL,
		PED_TYPE_BUM,
		PED_TYPE_PROSTITUTE,
		PED_TYPE_SPECIAL,
		PED_TYPE_MISSION,
		PED_TYPE_SWAT,
		PED_TYPE_ANIMAL,
		PED_TYPE_ARMY
	}

	public static class EntityUtil
	{
		public static async Task<Ped> CreatePed(Model model, PedType pedType, Vector3 pos, float heading = 0f, bool networked = true)
		{
			if (API.IsModelAPed((uint) model.Hash))
			{
				model.Request();
				while (!model.IsLoaded)
					await BaseScript.Delay(1);
				model.MarkAsNoLongerNeeded();

				return new Ped(API.CreatePed((int) pedType, (uint) model.Hash, pos.X, pos.Y, pos.Z, heading, networked, false));
			}
			return null;
		}

		public static async Task<Vehicle> CreateVehicle(Model model, Vector3 pos, float heading = 0f, bool networked = true)
		{
			if (API.IsModelAVehicle((uint) model.Hash))
			{
				model.Request();
				while (!model.IsLoaded)
					await BaseScript.Delay(1);

				return new Vehicle(API.CreateVehicle((uint) model.Hash, pos.X, pos.Y, pos.Z, heading, networked, false));
			}
			model.MarkAsNoLongerNeeded();

			return null;
		}

		public static void _StartScenario(this Ped ped, string scenario)
		{
			API.TaskStartScenarioInPlace(ped.Handle, scenario, 0, true);
		}
	}

	public static class VehicleUtil
	{
		public static bool _IsBroken(this Vehicle vehicle)
		{
			return vehicle.IsDead || vehicle.EngineHealth == 0f || vehicle.PetrolTankHealth == 0f;
		}

		public static string _GetLabel(this Vehicle vehicle)
		{
			return API.GetLabelText(API.GetDisplayNameFromVehicleModel((uint)vehicle.Model.Hash));
		}
	}
}
