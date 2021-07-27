#region Usings.

// Methods for handling message sending.
using vkapi.methods;

// Longpoll for handling longpoll.
using vkapi.longpoll;

// Events for handling events. 
using vkapi.longpoll.events;

// Console write line for DebugBot.
using System;

#endregion

namespace vkapi
{
    namespace examples
    {
        /// Examples namespace. Includes examples classes.
        /// You should include this, and create any class that you want to use.
        /// This namespace is dont includes any for working with VK API!

        public class EchoBot
        {
            /// Bot that echo your message with same text.

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

        public class BanBot
        {
            /// Bot that bans user on message ban from you.

            // Fields.

            // API for using in callback methods.
            private readonly VkApiBotLongpoll _Api;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="accessToken">Access token from group</param>
            /// <param name="groupIndex">Group index</param>
            public BanBot(string accessToken, int groupIndex)
            {
                // Creating API object.
                _Api = new VkApiBotLongpoll(accessToken, groupIndex);

                // Subscribing on our messages (As there we may get any other events that we dont expect to use).
                _Api.EventSubscribeType(EventMessageReply.SubscriptionEventName);

                // Starting listening with our callback for message.
                _Api.ListenLongpoll(CallbackMessageEvent);
            }

            /// <summary>
            /// Callback for message event (As we subscibed only for new messages.
            /// </summary>
            /// <param name="updateEvent">Event given from library</param>
            private void CallbackMessageEvent(IEvent updateEvent)
            {
                if (updateEvent.GetType() == typeof(EventMessageReply)) // Or updateMessage.update.type == "message_reply" or updateMessage.update.type == EventMessageReply.SubscriptionEventName
                {
                    // Upcasting update event with type IEvent to EventMessageReply.
                    // (As we want get message object).
                    EventMessageReply updateMessage = (EventMessageReply)updateEvent;

                    // Getting values.
                    string text = updateMessage.message.Text;
                    long from = updateMessage.message.FromId;

                    // Checking text.
                    if (text != "ban") return;

                    // Sending message to user.
                    Methods.MessagesSend(_Api, "[BanBot] Banned!", from);

                    // Banning.
                    Methods.GroupsBan(_Api, _Api.groupIndex, from, null, 0, "Banned by BanBot", true);
                }
            }
        }

        public class DebugAPIBot
        {
            /// Bot that logs all callbacks to console.

            // Fields.

            // API for using in callback methods.
            private readonly VkApiBotLongpoll _Api;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="accessToken">Access token from group</param>
            /// <param name="groupIndex">Group index</param>
            public DebugAPIBot(string accessToken, int groupIndex)
            {
                // Creating API object.
                _Api = new VkApiBotLongpoll(accessToken, groupIndex);
                Console.WriteLine("[Debug] Connected to API");

                // Starting listening with our callback for message.
                Console.WriteLine("[Debug] Starting longpoll...");
                _Api.ListenLongpoll(CallbackMessageEvent);
            }

            /// <summary>
            /// Callback for message event (As we subscibed only for new messages.
            /// </summary>
            /// <param name="updateEvent">Event given from library</param>
            private void CallbackMessageEvent(IEvent updateEvent)
            {
                // Logging.
                Console.WriteLine("[Debug] Got new event IEvent!");

                if (updateEvent.GetType() == typeof(EventMessageReply)) // Or updateMessage.update.type == "message_reply" or updateMessage.update.type == EventMessageReply.SubscriptionEventName
                {
                    // Upcasting update event with type IEvent to EventMessageReply.
                    // (As we want get message object).
                    EventMessageReply updateMessage = (EventMessageReply)updateEvent;

                    // Debug.
                    Console.WriteLine($"[Debug] New event message reply! Object - {updateMessage.update.object_}");
                }

                if (updateEvent.GetType() == typeof(EventMessageNew)) // Or updateMessage.update.type == "message_new" or updateMessage.update.type == EventMessageNew.SubscriptionEventName
                {
                    // Upcasting update event with type IEvent to EventMessageNew.
                    // (As we want get message object).
                    EventMessageNew updateMessage = (EventMessageNew)updateEvent;

                    // Debug.
                    Console.WriteLine($"[Debug] New event message new! Object - {updateMessage.update.object_}");
                }
            }
        }
        
        // There will be class EchoUser that handles user longpoll api.
        // I will add this later, as there is no working user longpoll api in library for now.
        // class EchoUser { }
    }
}