#region Usings.

// JSON Parsing.
using Newtonsoft.Json;

#endregion

namespace vkapi.longpoll.json
{
    public class JsonLongpollServer
    {
        /// Contains longpoll server response container.
        /// Holds response object.

        // Server response.
        [JsonProperty("response")]
        public JsonLongpollServerResponse Response { get; set; }
    }
}