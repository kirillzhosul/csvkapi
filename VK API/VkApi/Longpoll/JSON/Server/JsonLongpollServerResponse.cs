#region Usings.

// JSON Parsing.
using Newtonsoft.Json;

#endregion

namespace vkapi.longpoll.json
{
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
}