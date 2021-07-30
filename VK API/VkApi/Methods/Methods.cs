#region Usings.

// System.
using System;
using System.Collections.Generic;

// JSON.
using Newtonsoft.Json;

#endregion

namespace vkapi.methods
{
    public class Methods
    {
        /// Methods class that implements better work with VkApi methods (You SHOULD use this instead of APIMethod if possible)

        #region Basic.

        #region Messages send (Overloads)

        /// <summary>
        /// Sends message to user or group.
        /// </summary>
        /// <param name="api">API Object</param>
        /// <param name="message">Message text</param>
        /// <param name="peer_id">Peer index where send to</param>
        public static string MessagesSend(VkApi api, string message, long peerId)
        {
            // Getting arguments for method.
            string[] arguments = {
                $"random_id=0",
                $"peer_id={peerId}",
                $"message={message}",
            };

            // Server JSON result.
            string serverJSON = api.APIMethod("messages.send", arguments);

            //  Returning server response.
            return serverJSON;
        }

        /// <summary>
        /// Sends message to user or group with attachment.
        /// </summary>
        /// <param name="api">API Object</param>
        /// <param name="message">Message text</param>
        /// <param name="peer_id">Peer index where send to</param>
        public static string MessagesSend(VkApi api, string message, long peerId, string attachment)
        {
            // Getting arguments for method.
            string[] arguments = {
                $"random_id=0",
                $"peer_id={peerId}",
                $"message={message}",
                $"attachment={attachment}",
            };

            // Server JSON result.
            string serverJSON = api.APIMethod("messages.send", arguments);

            //  Returning server response.
            return serverJSON;
        }

        /// <summary>
        /// Sends message to user or group with attachments.
        /// </summary>
        /// <param name="api">API Object</param>
        /// <param name="message">Message text</param>
        /// <param name="peer_id">Peer index where send to</param>
        public static string MessagesSend(VkApi api, string message, long peerId, string[] attachments) => MessagesSend(api, message, peerId, string.Join(",", attachments));
  
        #endregion

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
        public static string GroupsBan(VkApi api, long group_id, long owner_id, long? end_date, short reason, string comment, bool comment_visible)
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
                throw new ArgumentException("Reason argument is not valid (should be in range 0 - 4)");
            }

            // Getting arguments for method.
            string arguments = $"random_id=0&group_id={group_id}&owner_id={owner_id}&reason={reason}&comment={comment}&comment_visible={comment_visible}" + end_date == null ? "" : "&end_date={end_date}"; 

            // Sending message.
            return api.APIMethod("groups.ban", arguments);
        }

        #endregion

        #region System.

        /// <summary>
        /// Returns longpoll server data.
        /// </summary>
        /// <param name="api">API Object</param>
        /// <param name="type">Type of longpoll server (0 - groups, 1 - user)</param>
        /// <returns></returns>
        public static string GetLongpollServer(VkApi api, byte type)
        {
            // Final response.
            string response;

            switch (type)
            {
                case 0:
                    // Group server.

                    // Response.
                    response = api.APIMethod("groups.getLongPollServer");
                    break;
                case 1:
                    // User server.

                    // Response.
                    response = api.APIMethod("messages.getLongPollServer");
                    break;
                default:
                    // Exception.
                    throw new ArgumentException("Type is not valid, please select between 0 (group) and 1 (user)");
            }

            // Returning.
            return response;
        }

        /// <summary>
        /// Check is response error or not.
        /// </summary>
        /// <param name="JSON">Json from server</param>
        /// <returns>Is error or not</returns>
        public static void CheckResponseErrors(string JSON)
        {
            // Parsing.
            Dictionary<string, dynamic> parsedJSON = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(JSON);

            // Returning error if error.
            if (parsedJSON.ContainsKey("error"))
            {
                // Returning error.
                throw new MethodException(parsedJSON["error"].ToString());
            }
        }

        #endregion
    }
}