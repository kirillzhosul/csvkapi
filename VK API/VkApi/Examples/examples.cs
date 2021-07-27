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

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="accessToken">Access token from group</param>
            /// <param name="groupIndex">Group index</param>
            public EchoBot(string accessToken, int groupIndex)
            {
                // Creating API object.
                VkApiBotLongpoll api = new VkApiBotLongpoll(accessToken, groupIndex);

                // Subscribing on new messages (As there we may get any other events that we dont expect to use).
                api.EventSubscribeType(EventMessageNew.SubscriptionEventName);

                // Adding callback event.
                api.OnNewEvent += CallbackMessageNew;

                // Starting listening with our callback for message.
                api.ListenLongpoll();
            }

            /// </summary>
            /// Callback for messa new events.
            /// </summary>
            /// <param name="sender"> Sender (VkApi)</param>
            /// <param name="e"> Event arguments (LongpollEventArgs)</param>
            private void CallbackMessageNew(object sender, LongpollEventArgs e)
            {
                // Upcasting update event with type IEvent to EventMessageNew.
                // (As we want get message object).
                EventMessageNew.Message message = ((EventMessageNew)e.Update).message;

                // Sending message to user.
                Methods.MessagesSend((VkApi)sender, message.Text, message.FromId);
            }
        }

        public class BanBot
        {
            /// Bot that bans user on message ban from you.

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="accessToken">Access token from group</param>
            /// <param name="groupIndex">Group index</param>
            public BanBot(string accessToken, int groupIndex)
            {
                // Creating API object.
                VkApiBotLongpoll api = new VkApiBotLongpoll(accessToken, groupIndex);

                // Subscribing on our messages (As there we may get any other events that we dont expect to use).
                api.EventSubscribeType(EventMessageReply.SubscriptionEventName);

                // Adding callback event.
                api.OnNewEvent += CallbackMessageReply;

                // Starting listening.
                api.ListenLongpoll();
            }

            /// </summary>
            /// Callback for message reply event.
            /// </summary>
            /// <param name="sender"> Sender (VkApi)</param>
            /// <param name="e"> Event arguments (LongpollEventArgs)</param>
            private void CallbackMessageReply(object sender, LongpollEventArgs e)
            {
                // Upcasting update event with type IEvent to EventMessageReply.
                // (As we want get message object).
                EventMessageReply.Message message = ((EventMessageReply)e.Update).message;

                // Checking text.
                if (message.Text != "ban") return;

                // Sending message to user.
                Methods.MessagesSend((VkApi)sender, "[BanBot] Banned!", message.FromId);

                // Banning.
                Methods.GroupsBan((VkApi)sender, ((VkApiBotLongpoll)sender).groupIndex, message.FromId, null, 0, "Banned by BanBot", true);
            }
        }

        public class DebugAPIBot
        {
            /// Bot that logs all callbacks to console.

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="accessToken">Access token from group</param>
            /// <param name="groupIndex">Group index</param>
            public DebugAPIBot(string accessToken, int groupIndex)
            {
                // Creating API object.
                VkApiBotLongpoll api = new VkApiBotLongpoll(accessToken, groupIndex);
                Console.WriteLine("[Debug] Connected to API");

                // Adding callback event.
                api.OnNewEvent += CallbackEvent;

                // Starting listening longpoll.
                Console.WriteLine("[Debug] Starting longpoll...");
                api.ListenLongpoll();
            }

            /// </summary>
            /// Callback for new events.
            /// </summary>
            /// <param name="sender"> Sender (VkApi)</param>
            /// <param name="e"> Event arguments (LongpollEventArgs)</param>
            private void CallbackEvent(object sender, LongpollEventArgs e)
            {
                // Logging.
                Console.WriteLine($"[Debug] Got new event IEvent! Type - {((EventIBase)e.Update).update.type}. Object - {((EventIBase)e.Update).update.object_}");
            }
        }
        
        // There will be class EchoUser that handles user longpoll api.
        // I will add this later, as there is no working user longpoll api in library for now.
        // class EchoUser { }
    }
}