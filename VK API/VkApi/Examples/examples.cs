#region Usings.

// Methods for handling message sending.
using vkapi.methods;

// Longpoll for handling longpoll.
using vkapi.longpoll;

// Events for handling events. 
using vkapi.longpoll.events;

#endregion

namespace vkapi
{
    namespace examples
    {
        /// Examples namespace. Includes examples classes.
        /// You should include this, and create any class that you want to use.
        /// This namespace is dont includes any for working with VK API!

        class EchoBot
        {
            // Fields.

            // API for using in callback methods.
            private readonly VkApiBotLongpoll _Api;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="accessToken">Access token from group</param>
            /// <param name="groupIndex">Group index</param>
            public EchoBot(string accessToken, int groupIndex)
            {
                // Creating API object.
                _Api = new VkApiBotLongpoll(accessToken, groupIndex);

                // Subscribing on new messages (As there we may get any other events that we dont expect to use).
                _Api.EventSubscribeType(EventMessageNew.SubscriptionEventName);

                // Starting listening with our callback for message.
                _Api.ListenLongpoll(CallbackMessageEvent);
            }

            /// <summary>
            /// Callback for message event (As we subscibed only for new messages.
            /// </summary>
            /// <param name="updateEvent">Event given from library</param>
            private void CallbackMessageEvent(IEvent updateEvent)
            {
                // Upcasting update event with type IEvent to EventMessageNew.
                // (As we want get message object).
                EventMessageNew updateMessage = (EventMessageNew)updateEvent;

                // Getting values.
                string text = updateMessage.message.Text;
                long from = updateMessage.message.FromId;

                // Sending message to user.
                Methods.MessagesSend(_Api, text, from);
            }
        }

        // There will be class EchoUser that handles user longpoll api.
        // I will add this later, as there is no working user longpoll api in library for now.
        // class EchoUser { }
    }
}