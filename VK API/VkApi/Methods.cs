namespace vkapi
{
    namespace methods
    {
        /// Methods namespace. Includes static methods for VkApi class.
        /// You should include this for more simple using of methods.
        /// You may use default VkApi class APIMethod implementation with 3 overloads.
        /// This class will give you opportunity to call methods like default methods/functions.

        public class Methods
        {
            /// <summary>
            /// Sends message to user or group.
            /// </summary>
            /// <param name="api">API Object</param>
            /// <param name="message">Message text</param>
            /// <param name="peer_id">Peer index where send to</param>
            public static void MessagesSend(VkApi api, string message, long peer_id)
            {
                // Getting arguments for method.
                string[] arguments = {
                    $"random_id=0",
                    $"peer_id={peer_id}",
                    $"message={message}",
                };

                // Sending message.
                api.APIMethod("messages.send", arguments);
            }
        }
    }
}
