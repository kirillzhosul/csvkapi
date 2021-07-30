#region Usings.

// JSON Parsing.
using Newtonsoft.Json.Linq;

#endregion

namespace vkapi.longpoll.json
{
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