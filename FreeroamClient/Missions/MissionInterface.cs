using System.Threading.Tasks;

namespace Freeroam.Missions
{
	public interface IMission
	{
		Task Prepare();
		void Start();
		Task OnTick();
		void Stop();
	}
}
