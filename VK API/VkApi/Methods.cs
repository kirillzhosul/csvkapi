#region Usings.

// System.
using System;

#endregion

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

            /// <summary>
            /// Bans user in group.
            /// </summary>
            /// <param name="api">API Object</param>
            /// <param name="group_id">Group index</param>
            /// <param name="owner_id">User index</param>
            /// <param name="end_date">End date in unixtime (or null if forever)</param>
            /// <param name="reason">Reason from 0 to 4 (See in body)</param>
            /// <param name="comment">Comment to user</param>
            /// <param name="comment_visible">Is comment visible to user not</param>
            public static void GroupsBan(VkApi api, long group_id, long owner_id, long? end_date, short reason, string comment, bool comment_visible)
            {
                if (reason < 0 || reason > 4)
                {
                    // If reason is not in range.

                    /*
                        0 — Other(by default);
                        1 — Spam;
                        2 — Harassment;
                        3 — Profanity;
                        4 — Irrelevant messages.
                    */

                    // Exception.
                    throw new Exception("Reason argument is not valid (should be in range 0 - 4)");
                }

                // Getting arguments for method.
                string arguments = $"random_id=0&group_id={group_id}&owner_id={owner_id}&reason={reason}&comment={comment}&comment_visible={comment_visible}" + end_date == null ? "" : "&end_date={end_date}"; 

                // Sending message.
                api.APIMethod("groups.ban", arguments);
            }
        }
    }
}
