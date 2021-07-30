#region Usings.

// Longpoll for handling longpoll.
using vkapi.longpoll;

// Events for handling events. 
using vkapi.longpoll.events;

// Console write line for DebugBot.
using System;

#endregion

namespace vkapi.examples
{
    public class DebugBot
    {
        /// Bot that logs all callbacks to console.

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="accessToken">Access token from group</param>
        /// <param name="groupIndex">Group index</param>
        public DebugBot(string accessToken, int groupIndex)
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
}