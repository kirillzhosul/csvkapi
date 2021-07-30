#region Usings.

// System.
using System.Collections.Generic;

// JSON Parsing.
using Newtonsoft.Json;

#endregion

namespace vkapi.longpoll.json
{
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
}