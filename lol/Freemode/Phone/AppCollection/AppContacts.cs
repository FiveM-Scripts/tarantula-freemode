using CitizenFX.Core;
using CitizenFX.Core.UI;
using FreeroamShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Freeroam.Freemode.Phone.AppCollection
{
	public class AppContacts : IPhoneApp
	{
		private struct Contact
		{
			public string Name { get; private set; }
			public Dictionary<string, Action> Items { get; private set; }

			public Contact(string name, Dictionary<string, Action> items)
			{
				Name = name;
				Items = items;
			}
		}

		private static class ContactsHolder
		{
			public static Contact[] Contacts { get; } =
			{
				new Contact("Assistant", new Dictionary<string, Action>
				{
					{ "Send Message", new Action(async () =>
						{
							inputBlocked = true;
							string message = await Game.GetUserInput(60);
							if (message != null)
							{
								message = message.Trim();
								if (message.Length == 0)
									Screen.ShowNotification("~r~Please enter a message.");
								else
								{
									BaseScript.TriggerServerEvent(EventType.MESSAGE_FORWARD_ASSISTANT, message);
									Screen.ShowNotification("~g~Message sent.");
								}
							}
							inputBlocked = false;
						}
					)}
				})
			};
		}

		private Scaleform phoneScaleform;
		private int selected;
		private bool inSubMenu;
		private Contact selectedContact;
		private static bool inputBlocked;

		public void Init(Scaleform phoneScaleform)
		{
			this.phoneScaleform = phoneScaleform;
			phoneScaleform.CallFunction("SET_DATA_SLOT_EMPTY", 13);
		}

		public async Task OnTick()
		{
			await Task.FromResult(0);

			int slot = 0;
			if (inSubMenu)
				foreach (KeyValuePair<string, Action> item in selectedContact.Items)
					phoneScaleform.CallFunction("SET_DATA_SLOT", 2, slot++, -1, item.Key);
			else
				foreach (Contact contact in ContactsHolder.Contacts)
					phoneScaleform.CallFunction("SET_DATA_SLOT", 13, slot++, -1, contact.Name);
			phoneScaleform.CallFunction("SET_HEADER", inSubMenu ? selectedContact.Name : "Contacts");
			phoneScaleform.CallFunction("DISPLAY_VIEW", inSubMenu ? 2 : 13, selected);

			if (!inputBlocked)
			{
				bool pressed = false;
				if (Game.IsControlJustPressed(0, Control.PhoneUp) && slot > 0)
				{
					if (--selected < 0)
						selected = slot - 1;
					pressed = true;
				}
				else if (Game.IsControlJustPressed(0, Control.PhoneDown) && slot > 0)
				{
					if (++selected > slot - 1)
						selected = 0;
					pressed = true;
				}
				else if (Game.IsControlJustPressed(0, Control.PhoneSelect) && slot > 0)
				{
					if (!inSubMenu)
					{
						inSubMenu = true;
						selectedContact = ContactsHolder.Contacts[selected];
					}
					else
						selectedContact.Items.ElementAt(selected).Value.Invoke();
					selected = 0;
					pressed = true;
				}
				else if (Game.IsControlJustPressed(0, Control.PhoneCancel))
				{
					if (!inSubMenu)
						PhoneAppStarter.MainApp();
					else
					{
						Audio.PlaySoundFrontend("Hang_Up", "Phone_SoundSet_Michael");
						inSubMenu = false;
					}
				}

				if (pressed)
					Audio.PlaySoundFrontend("Menu_Navigate", "Phone_SoundSet_Default");
			}
		}

		public void Stop()
		{
			
		}
	}
}
