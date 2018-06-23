using CitizenFX.Core;
using System.Threading.Tasks;

namespace Freeroam.Freemode.Phone
{
	public interface IPhoneApp
	{
		void Init(Scaleform phoneScaleform);
		Task OnTick();
		void Stop();
	}
}
