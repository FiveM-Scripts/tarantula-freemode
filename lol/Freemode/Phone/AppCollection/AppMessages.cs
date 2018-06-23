using CitizenFX.Core;
using Freeroam.Freemode.Phone.AppCollection.Messages;
using System.Threading.Tasks;

namespace Freeroam.Freemode.Phone.AppCollection
{
	public class AppMessages : IPhoneApp
	{
		private Scaleform phoneScaleform;

		public void Init(Scaleform phoneScaleform)
		{
			this.phoneScaleform = phoneScaleform;
		}

		public async Task OnTick()
		{
			await Task.FromResult(0);

			phoneScaleform.CallFunction("SET_SOFT_KEYS", 3, true, 4);
			int slot = 0;
			foreach (Message message in MessagesHolder.Messages)
				phoneScaleform.CallFunction("SET_DATA_SLOT", 6, slot++, -1, -1, -1, message.Sender.Name, message.SenderMessage);
			phoneScaleform.CallFunction("DISPLAY_VIEW", 6, 0);

			if (Game.IsControlJustPressed(0, Control.PhoneCancel))
				PhoneAppStarter.MainApp();
		}

		public void Stop()
		{
			
		}
	}
}
