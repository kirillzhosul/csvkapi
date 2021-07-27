﻿#region Usings.

// System.
using System.Collections.Generic;

// JSON Parsing.
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace vkapi
{
    namespace longpoll
    {
        /// Longpoll namespace. Includes working with User / Bots longpoll api.
        /// Use this is if you want process message and other events.

        namespace json
        {
            /// JSON namespace. Includes classes for JSON parsing.
            /// You should dont use this in your code.
            /// Except maybe rare situation when you want to add new event?

            public class JsonLongpollServer
            {
                /// Contains longpoll server response container.
                /// Holds response object.

                // Server response.
                [JsonProperty("response")]
                public JsonLongpollServerResponse Response { get; set; }
            }

            public class JsonLongpollServerResponse
            {
                /// Contains main longpoll server response.
                /// Holds server parameters: ts, key, server.

                // Key for request.
                [JsonProperty("key")]
                public string Key { get; set; }

                // TS for getting only latest update.
                [JsonProperty("ts")]
                public string Ts { get; set; }

                // Server URL.
                [JsonProperty("server")]
                public string Server { get; set; }
            }

            public class JsonLongpollUpdates
            {
                /// Contains longpoll update list response.
                /// Holds updates list, ts, and also raw / parsed fields.

                // TS for updating our ts.
                [JsonProperty("ts")]
                public string Ts { get; set; }

                // Updates list.
                [JsonProperty("updates")]
                public List<Dictionary<string, dynamic>> Updates { get; set; }

                // Raw response.
                public string raw;

                // Parsed updates.
                public List<JsonLongpollUpdate> parsed;
            }

            public class JsonLongpollUpdate
            {
                /// Contains longpoll update response.
                /// Holds type and update  object itself.

                // Type of update.
                public string type;

                // Update object itself.
                //Dictionary<string, dynamic> object_;
                public JObject object_;
            }
        }
    }
}