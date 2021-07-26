#region Usings.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace vkapi
{
    class VkApiBotLongpoll : VkApi
    {

        // JSON.

        class JsonLongpollServer
        {
            // Fields.

            public JsonLongpollServerResponse response { get; set; }
        }

        class JsonLongpollServerResponse

        {
            // Fields.

            public string key { get; set; }

            public string ts { get; set; }

            public string server { get; set; }
        }

        class JsonLongpollUpdates
        {
            // Fields.

            public string ts { get; set; }

            public List<Dictionary<string, dynamic>> updates { get; set; }

            public string raw;

            public List<JsonLongpollUpdate> parsed;
        }

        class JsonLongpollUpdate
        {
            // Fields.

            public string type;

            public JObject object_;//Dictionary<string, dynamic> object_;
        }

        // Fields.

        // Group index.
        private int _groupIndex;

        // Longpoll server information.
        private Dictionary<string, string> _longpollServer = null;

        // Constructor.

        public VkApiBotLongpoll(string accessToken, int groupIndex) : base(accessToken)
        {
            // Group index.
            _groupIndex = groupIndex;

            // Ovveride virtual method.
            GenerateDefaultParameters();
        }

        // Longpoll.

        public void ListenLongpoll()
        {
            // If server is not set - getting server.
            if (_longpollServer == null) GetServer();

            while (true)
            {
                // While we running longpoll listener.

                // Getting updates.
                JsonLongpollUpdates updates = CheckUpdates();

                // If no updates - pass.
                if (updates.parsed.Count == 0) continue;

                foreach (JsonLongpollUpdate update in updates.parsed)
                {
                    // Debug message.
                    if (Debugger.IsAttached) Console.WriteLine($"[Debug] Got new event with type - {update.type}");

                    if (update.type == "message_new")
                    {
                        // Getting update message.
                        string message = update.object_.GetValue("message").ToString();

                        // Getting message text
                        string text = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(message)["text"];

                        Console.WriteLine(text);
                    }
                    else
                    {
                        Console.WriteLine($"Got unknown event with type {update.type}! = (");
                    }
                }

                // Updating TS.
                _longpollServer["ts"] = updates.ts;
            }
        }

        private JsonLongpollUpdates CheckUpdates()
        {
            // If server is not set - getting server.
            if (_longpollServer == null) GetServer();

            // Getting response.
            string _response = UrlGet($"{_longpollServer["url"]}?act=a_check&key={_longpollServer["key"]}&ts={_longpollServer["ts"]}&wait=25");

            // Getting updates object.
            JsonLongpollUpdates updates = JsonConvert.DeserializeObject<JsonLongpollUpdates>(_response);

            // Debug message.
            if (Debugger.IsAttached) Console.WriteLine($"[Debug] Updated longpoll and got response - {_response}");

            // Setting raw field.
            updates.raw = _response;

            // Parsing.
            updates.parsed = ParseUpdates(updates);

            // Returning updates.
            return updates;
        }

        private List<JsonLongpollUpdate> ParseUpdates(JsonLongpollUpdates updates)
        {
            // New list.
            List<JsonLongpollUpdate> updatesParsed = new List<JsonLongpollUpdate>();

            foreach (Dictionary<string, dynamic> update in updates.updates)
            {
                // New update.
                JsonLongpollUpdate updateParsed = new JsonLongpollUpdate();

                // Fields.
                updateParsed.type = Convert.ToString(update["type"]);
                updateParsed.object_ = update["object"];

                // Adding.
                updatesParsed.Add(updateParsed);
            }

            // Returning.
            return updatesParsed;
        }

        private void GetServer()
        {
            // Getting method response.
            string response = Method("groups.getLongPollServer");

            // Getting JSON longpoll object.
            JsonLongpollServer longpollServer = JsonConvert.DeserializeObject<JsonLongpollServer>(response);

            // Setting server.
            _longpollServer = new Dictionary<string, string>()
            {
                { "ts", longpollServer.response.ts },
                { "url", longpollServer.response.server },
                { "key", longpollServer.response.key },
            };
        }

        // Other.

        protected override void GenerateDefaultParameters()
        {
            // Generating default parameters.
            _urlDefaultParameters = $"v={_version}&access_token={_accessToken}&group_id={_groupIndex}";
        }
    }

    class VkApi
    {
        // Version for API requests.
        protected string _version = "5.131";

        // Access token.
        protected string _accessToken;

        // URLs.

        // Default GET parameters for method url.
        protected string _urlDefaultParameters;

        // Url to method.
        protected const string _urlMethod = "https://api.vk.com/method";

        // Constructor.

        public VkApi(string accessToken)
        {
            // Access token.
            _accessToken = accessToken;

            // Generating new default params.
            GenerateDefaultParameters();
        }

        // API.

        public string Method(string name)
        {
            // Returning response.
            return UrlGet($"{_urlMethod}/{name}?{_urlDefaultParameters}");
        }

        // Other.

        public void ChangeVersion(string version)
        {
            // Change version.
            _version = version;

            // Generating new default params.
            GenerateDefaultParameters();
        }

        protected virtual void GenerateDefaultParameters()
        {
            // Generating default parameters.
            _urlDefaultParameters = $"v={_version}&access_token={_accessToken}";
        }

        // System.

        protected string UrlGet(string url)
        {
            //using (var client = new HttpClient()){
            //    var result = await client.GetStringAsync(URL);}

            // Debug message.
            if (Debugger.IsAttached) Console.WriteLine($"[Debug] Called UrlGet() with url - {url}");

            // Getting request / response.
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            // Downloading and returning.
            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}
